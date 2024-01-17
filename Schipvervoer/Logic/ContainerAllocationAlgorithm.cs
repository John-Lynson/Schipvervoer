using System;
using System.Collections.Generic;
using System.Linq;
using Schipvervoer.Models;

namespace Schipvervoer.Logic
{
    public class ContainerAllocationAlgorithm
    {
        public void AllocateContainers(Ship ship, List<Container> containers)
        {
            var sortedContainers = SortContainers(containers);

            foreach (var container in sortedContainers)
            {
                if (!TryPlaceContainer(ship, container))
                {
                    throw new InvalidOperationException($"Kan container (Gewicht: {container.Weight}, Waardevol: {container.IsValuable}, Gekoeld: {container.RequiresCooling}) niet plaatsen.");
                }
            }

            if (!ship.IsWeightDistributedProperly() || !ship.IsMinWeightMaintained())
            {
                throw new InvalidOperationException("Kan containers niet correct plaatsen: gewichtsverdeling of minimale gewichtseis niet voldaan.");
            }
        }

        private List<Container> SortContainers(List<Container> containers)
        {
            return containers
                .OrderByDescending(c => c.RequiresCooling)
                .ThenByDescending(c => c.IsValuable)
                .ThenBy(c => c.Weight)
                .ToList();
        }

        private bool TryPlaceContainer(Ship ship, Container container)
        {
            // Eerst proberen we de waardevolle containers te plaatsen
            if (container.IsValuable)
            {
                foreach (var stack in ship.ContainerStacks)
                {
                    if (stack.Containers.Count == 0 || stack.Containers.All(c => c.IsValuable))
                    {
                        stack.AddContainer(container);
                        return true;
                    }
                }
            }

            // Controleer op gekoelde containers
            if (container.RequiresCooling)
            {
                for (int i = 0; i < ship.Width; i++)
                {
                    if (CanPlaceContainerInStack(ship.ContainerStacks[i], container))
                    {
                        ship.ContainerStacks[i].AddContainer(container);
                        return true;
                    }
                }
            }
            else // Voor niet-gekoelde, niet-waardevolle containers
            {
                foreach (var stack in ship.ContainerStacks)
                {
                    if (CanPlaceContainerInStack(stack, container))
                    {
                        stack.AddContainer(container);
                        return true;
                    }
                }
            }

            // Als er geen geschikte stack is gevonden
            return false;
        }


        private bool CanPlaceContainerInStack(ContainerStack stack, Container container)
        {
            // Controleer of de container waardevol is en of de stack leeg is of alleen waardevolle containers bevat
            if (container.IsValuable && (stack.Containers.Count == 0 || stack.Containers.All(c => c.IsValuable)))
            {
                return true;
            }

            // Controleer het totale gewicht op de stack
            int totalWeightAbove = stack.TotalWeight();
            if (totalWeightAbove + container.Weight <= ContainerStack.MaxWeightOnTop)
            {
                return true;
            }

            // Als de container niet waardevol is, maar de stack heeft waardevolle containers, plaats dan niet
            if (stack.Containers.Any(c => c.IsValuable))
            {
                return false;
            }

            return false;
        }


        public void AllocateContainersToStacks(Ship ship, List<Container> containers)
        {
            foreach (var container in containers)
            {
                TryPlaceContainer(ship, container);
            }

            if (!ship.IsWeightDistributedProperly() || !ship.IsMinWeightMaintained())
            {
                throw new InvalidOperationException("Kan containers niet correct plaatsen");
            }
        }
    }
}
