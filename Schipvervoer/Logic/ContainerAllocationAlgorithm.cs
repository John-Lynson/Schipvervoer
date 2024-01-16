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
                    // Behandel het geval waar een container niet geplaatst kan worden
                    throw new InvalidOperationException("Kan container niet plaatsen");
                }
            }
        }

        private List<Container> SortContainers(List<Container> containers)
        {
            // Sorteer eerst gekoelde, dan waardevolle, dan overige containers
            return containers
                .OrderByDescending(c => c.RequiresCooling)
                .ThenByDescending(c => c.IsValuable)
                .ThenBy(c => c.Weight)
                .ToList();
        }

        private bool TryPlaceContainer(Ship ship, Container container)
        {
            // Zoek naar een bestaande stack waar de container bovenop kan
            var suitableStack = ship.ContainerStacks.FirstOrDefault(stack => stack.CanPlaceOnTop(container));
            if (suitableStack != null)
            {
                suitableStack.AddContainer(container);
                return true;
            }

            return false; // Geen geschikte plaats gevonden
        }

        public void AllocateContainersToStacks(Ship ship, List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (!TryPlaceContainer(ship, container))
                {
                    var newStack = new ContainerStack();
                    newStack.AddContainer(container);
                    ship.AddContainerStack(newStack);
                }
            }

            if (!ship.IsWeightDistributedProperly() || !ship.IsMinWeightMaintained())
            {
                throw new InvalidOperationException("Kan containers niet correct plaatsen");
            }
        }
    }
}
