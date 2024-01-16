using System.Collections.Generic;
using System.Linq;
using Schipvervoer.Models;

namespace Schipvervoer.Logic
{
    public class ContainerStack
    {
        private const int MaxWeightOnTop = 120000; // Maximaal gewicht in kg
        public List<Container> Containers { get; private set; }

        public ContainerStack()
        {
            Containers = new List<Container>();
        }

        public bool CanPlaceOnTop(Container container)
        {
            // Controleer of de container bovenop geplaatst kan worden
            if (container.IsValuable)
            {
                // Waardevolle containers kunnen alleen bovenop een lege stack of bovenop een andere waardevolle container
                return Containers.Count == 0 || Containers.All(c => c.IsValuable);
            }
            else
            {
                // Voor reguliere containers, controleer of het totale gewicht niet overschreden wordt
                int totalWeightAbove = Containers.Sum(c => c.Weight);
                return totalWeightAbove + container.Weight <= MaxWeightOnTop;
            }
        }

        public void AddContainer(Container container)
        {
            if (!CanPlaceOnTop(container))
            {
                throw new InvalidOperationException("Kan container niet bovenop plaatsen");
            }
            Containers.Add(container);
        }

        public int TotalWeight()
        {
            return Containers.Sum(c => c.Weight);
        }
    }
}