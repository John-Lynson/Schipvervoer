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
            Trace.WriteLine("Start allocatie van containers.");
            var sortedContainers = SortContainers(containers, ship);

            foreach (var container in sortedContainers)
            {
                if (!TryPlaceContainer(ship, container))
                {
                    Trace.WriteLine("Kon container niet plaatsen, gooit InvalidOperationException.");
                    throw new InvalidOperationException("Kan container niet plaatsen");
                }
            }

            if (!ship.IsWeightDistributedProperly() || !ship.IsMinWeightMaintained())
            {
                Trace.WriteLine("Gewichtsverdeling of minimale gewichtseis niet voldaan, gooit InvalidOperationException.");
                throw new InvalidOperationException("Kan containers niet correct plaatsen: gewichtsverdeling of minimale gewichtseis niet voldaan");
            }
            Trace.WriteLine("Allocatie van containers voltooid.");
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
            Trace.WriteLine($"Proberen container te plaatsen: Gewicht={container.Weight}, Waardevol={container.IsValuable}, Gekoeld={container.RequiresCooling}");
            if (container.RequiresCooling)
            {
                for (int i = 0; i < ship.Width; i++)
                {
                    if (CanPlaceContainerInStack(ship.ShipLayout[0, i], container))
                    {
                        ship.ShipLayout[0, i].AddContainer(container);
                        Trace.WriteLine($"Gekoelde container geplaatst in rij 0, kolom {i}.");
                        return true;
                    }
                }
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
                            Trace.WriteLine($"Waardevolle container geplaatst in rij {i}, kolom {j}.");
                            return true;
                        }
                    }
                }
            }

            if (PlaceRegularContainer(ship, container))
            {
                Trace.WriteLine($"Reguliere container geplaatst.");
                return true;
            }

            Trace.WriteLine($"Kon geen geschikte plaats vinden voor container met gewicht {container.Weight}.");
            return false;
        }

        private bool PlaceRegularContainer(Ship ship, Container container)
        {
            // Bepaal de kant van het schip die momenteel lichter is
            bool placeOnLeftSide = ship.CalculateSideWeight(0, ship.Width / 2) < ship.CalculateSideWeight(ship.Width / 2, ship.Width);

            // Start bij de lichtere kant voor het plaatsen van de container
            int startWidth = placeOnLeftSide ? 0 : ship.Width / 2;
            int endWidth = placeOnLeftSide ? ship.Width / 2 : ship.Width;

            for (int i = 0; i < ship.Length; i++)
            {
                for (int j = startWidth; j < endWidth; j++)
                {
                    if (CanPlaceContainerInStack(ship.ShipLayout[i, j], container))
                    {
                        ship.ShipLayout[i, j].AddContainer(container);
                        Trace.WriteLine($"Reguliere container geplaatst in rij {i}, kolom {j}.");
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
                        Trace.WriteLine($"Reguliere container geplaatst in rij {i}, kolom {j}.");
                        return true;
                    }
                }
            }

            return false;
        }


        private bool CanPlaceContainerInStack(ContainerStack stack, Container container)
        {
            bool canPlace = stack.CanPlaceOnTop(container);
            Trace.WriteLine($"Kan container met gewicht {container.Weight} plaatsen in stack: {canPlace}.");
            return canPlace;
        }

        // ... Rest van de methoden, indien aanwezig ...
    }
}
