using System.Collections.Generic;
using Schipvervoer.Models;
using Schipvervoer.Logic;

namespace Schipvervoer.Models
{
    public class Ship
    {
        public int MaxWeight { get; set; }
        public List<Container> Containers { get; set; }

        public Ship(int maxWeight)
        {
            MaxWeight = maxWeight;
            Containers = new List<Container>();
        }

        public bool IsOverloaded()
        {
            int totalWeight = Containers.Sum(c => c.Weight);
            return totalWeight > MaxWeight;
        }

        public void AddContainer(Container container)
        {
            // Voeg de container toe aan de lijst
            // Aanname: Alle veiligheidscontroles zijn al uitgevoerd
            Containers.Add(container);
            // Voeg hier eventueel extra logica toe voor balanceren
        }

        public bool CanSafelyAddContainer(Container container)
        {
            // Implementeer logica om te controleren of de container veilig toegevoegd kan worden
            // Controleer op maximale gewicht en speciale regels voor waardevolle en gekoelde containers

            int projectedTotalWeight = Containers.Sum(c => c.Weight) + container.Weight;
            return projectedTotalWeight <= MaxWeight; // Eenvoudige gewichtscontrole
                                                      // Voeg hier verdere controles toe...
        }

        public void PlaceContainerStacks(List<ContainerStack> containerStacks)
        {
            // Implement logic to place container stacks on the ship
        }

        public bool IsMinWeightMaintained()
        {
            int totalWeight = Containers.Sum(c => c.Weight);
            return totalWeight >= MaxWeight * 0.5; // Minstens 50% van het MaxGewicht
        }

        public bool IsWeightDistributedProperly()
        {
            // Implementeer logica voor gewichtsverdeling
            // Zorg ervoor dat de gewichtsverdeling binnen 20% verschil blijft
        }
    }
}
