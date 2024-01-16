using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Voorbeeld: Vind de juiste ContainerStack op het schip om de container te plaatsen.
            foreach (var stack in ship.ContainerStacks)
            {
                if (stack.CanPlaceOnTop(container))
                {
                    // Voeg hier de logica toe om de container aan de stack toe te voegen.
                    stack.AddContainer(container);
                    return true;
                }
            }

            return false; // Geen geschikte plaats gevonden
        }
    }
}
