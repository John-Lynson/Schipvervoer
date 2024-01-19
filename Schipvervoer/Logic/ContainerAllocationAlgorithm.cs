using Schipvervoer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Schipvervoer.Logic
{
    public class ContainerAllocationAlgorithm
    {
        public void AllocateContainers(Ship ship, List<Container> containers)
        {
            var sortedContainers = SortContainers(containers, ship);

            foreach (var container in sortedContainers)
            {
                if (!TryPlaceContainer(ship, container))
                {
                    throw new InvalidOperationException("Kan container niet plaatsen");
                }
            }

            if (!ship.IsWeightDistributedProperly() || !ship.IsMinWeightMaintained())
            {
                throw new InvalidOperationException("Kan containers niet correct plaatsen: gewichtsverdeling of minimale gewichtseis niet voldaan");
            }
        }

        private List<Container> SortContainers(List<Container> containers, Ship ship)
        {
            Trace.WriteLine("Begin sortering van containers.");
            var coolingContainers = containers.Where(c => c.RequiresCooling)
                                              .OrderByDescending(c => c.Weight);

            var valuableContainers = containers.Where(c => c.IsValuable && !c.RequiresCooling)
                                               .OrderByDescending(c => c.Weight);

            var regularContainers = containers.Where(c => !c.IsValuable && !c.RequiresCooling)
                                              .OrderBy(c => OptimizedWeightOrder(c, ship));

            var sortedList = coolingContainers.Concat(valuableContainers)
                                              .Concat(regularContainers)
                                              .ToList();
            Trace.WriteLine("Sortering van containers voltooid.");
            return sortedList;
        }

        private int OptimizedWeightOrder(Container container, Ship ship)
        {
            int leftWeight = ship.CalculateSideWeight(0, ship.Width / 2);
            int rightWeight = ship.CalculateSideWeight(ship.Width / 2, ship.Width);
            int imbalance = Math.Abs(leftWeight - rightWeight);

            bool shouldPlaceOnLeft = leftWeight > rightWeight;

            if ((shouldPlaceOnLeft && container.Weight > 0) || (!shouldPlaceOnLeft && container.Weight < 0))
            {
                return imbalance + container.Weight;
            }
            else
            {
                return imbalance - container.Weight;
            }
        }

        private bool TryPlaceContainer(Ship ship, Container container)
        {

            if (container.RequiresCooling)
            {
                return TryPlaceCoolingContainer(ship, container);
            }

            if (container.IsValuable)
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    for (int j = 0; j < ship.Width; j++)
                    {
                        if (CanPlaceContainerInStack(ship.ShipLayout[i, j], container) && ship.ShipLayout[i, j].Containers.Count == 0)
                        {
                            ship.ShipLayout[i, j].AddContainer(container);
                            return true;
                        }
                    }
                }
            }

            return PlaceRegularContainer(ship, container);
        }

        private bool TryPlaceCoolingContainer(Ship ship, Container container)
        {
            for (int i = 0; i < ship.Width; i++)
            {
                if (CanPlaceContainerInStack(ship.ShipLayout[0, i], container))
                {
                    ship.ShipLayout[0, i].AddContainer(container);
                    return true;
                }
            }
            return false;
        }

        private bool PlaceRegularContainer(Ship ship, Container container)
        {
            bool placeOnLeftSide = ship.CalculateSideWeight(0, ship.Width / 2) < ship.CalculateSideWeight(ship.Width / 2, ship.Width);

            int startWidth = placeOnLeftSide ? 0 : ship.Width / 2;
            int endWidth = placeOnLeftSide ? ship.Width / 2 : ship.Width;

            for (int i = 0; i < ship.Length; i++)
            {
                for (int j = startWidth; j < endWidth; j++)
                {
                    if (CanPlaceContainerInStack(ship.ShipLayout[i, j], container))
                    {
                        ship.ShipLayout[i, j].AddContainer(container);
                        return true;
                    }
                }
            }

            for (int i = 0; i < ship.Length; i++)
            {
                for (int j = (placeOnLeftSide ? ship.Width / 2 : 0); j < (placeOnLeftSide ? ship.Width : ship.Width / 2); j++)
                {
                    if (CanPlaceContainerInStack(ship.ShipLayout[i, j], container))
                    {
                        ship.ShipLayout[i, j].AddContainer(container);
                        return true;
                    }
                }
            }

            return false;
        }


        private bool CanPlaceContainerInStack(ContainerStack stack, Container container)
        {
            bool canPlace = stack.CanPlaceOnTop(container);
            return canPlace;
        }
    }
}
