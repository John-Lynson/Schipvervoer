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
            // Implementeer de logica om de container op de juiste plek te plaatsen
            foreach (var stack in ship.ContainerStacks)
            {
                if (stack.CanPlaceOnTop(container))
                {
                    stack.AddContainer(container);
                    return true;
                }
            }
            return false; // Geen geschikte plaats gevonden
        }
    }
}
