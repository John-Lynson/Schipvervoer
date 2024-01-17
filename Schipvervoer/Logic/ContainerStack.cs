using System.Collections.Generic;
using System.Linq;
using Schipvervoer.Models;

namespace Schipvervoer.Logic
{
    public class ContainerStack
    {
        public const int MaxWeightOnTop = 120000; // Maximaal gewicht in kg
        public List<Container> Containers { get; private set; }

        public ContainerStack()
        {
            Containers = new List<Container>();
        }

        public bool CanPlaceOnTop(Container container)
        {
            if (container.IsValuable)
            {
                // Waardevolle containers alleen bovenop als stack leeg is of alleen waardevolle containers bevat
                return Containers.Count == 0 || Containers.All(c => c.IsValuable);
            }

            int totalWeightAbove = Containers.Sum(c => c.Weight);
            return totalWeightAbove + container.Weight <= 120000; // 120 ton
        }


        public void AddContainer(Container container)
        {
            if (!CanPlaceOnTop(container))
            {
                throw new InvalidOperationException($"Kan container (Gewicht: {container.Weight}, Waardevol: {container.IsValuable}, Gekoeld: {container.RequiresCooling}) niet bovenop plaatsen in stack met totaalgewicht {TotalWeight()}");
            }
            Containers.Add(container);
        }

        public int TotalWeight()
        {
            return Containers.Sum(c => c.Weight);
        }
    }
}