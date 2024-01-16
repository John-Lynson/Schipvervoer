using System.Collections.Generic;
using System.Linq;
using Schipvervoer.Logic;

namespace Schipvervoer.Models
{
    public class Ship
    {
        public int MaxWeight { get; set; }
        public List<ContainerStack> ContainerStacks { get; private set; }

        public Ship(int maxWeight)
        {
            MaxWeight = maxWeight;
            ContainerStacks = new List<ContainerStack>();
        }

        public void AddContainerStack(ContainerStack stack)
        {
            ContainerStacks.Add(stack);
        }

        public bool IsOverloaded()
        {
            int totalWeight = ContainerStacks.Sum(stack => stack.TotalWeight());
            return totalWeight > MaxWeight;
        }

        public bool IsMinWeightMaintained()
        {
            int totalWeight = ContainerStacks.Sum(stack => stack.TotalWeight());
            return totalWeight >= MaxWeight * 0.5; // Minstens 50% van het MaxGewicht
        }

        public bool IsWeightDistributedProperly()
        {
            int halfIndex = ContainerStacks.Count / 2;
            int leftWeight = ContainerStacks.Take(halfIndex).Sum(stack => stack.TotalWeight());
            int rightWeight = ContainerStacks.Skip(halfIndex).Sum(stack => stack.TotalWeight());

            return Math.Abs(leftWeight - rightWeight) <= MaxWeight * 0.2; // Maximaal 20% verschil
        }

        // Overige methoden...
    }
}
