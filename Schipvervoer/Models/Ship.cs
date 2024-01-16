using Schipvervoer.Logic;
using System.Collections.Generic;
using System.Linq;

namespace Schipvervoer.Models
{
    public class Ship
    {
        public int MaxWeight { get; set; }
        public List<ContainerStack> ContainerStacks { get; private set; }
        private int _cachedTotalWeight = -1;

        public Ship(int maxWeight)
        {
            MaxWeight = maxWeight;
            ContainerStacks = new List<ContainerStack>();
        }

        public void AddContainerStack(ContainerStack stack)
        {
            ContainerStacks.Add(stack);
            UpdateTotalWeight(); // Update het totale gewicht wanneer een nieuwe stack wordt toegevoegd
        }

        private void UpdateTotalWeight()
        {
            _cachedTotalWeight = ContainerStacks.Sum(stack => stack.TotalWeight());
        }

        public bool IsOverloaded()
        {
            return _cachedTotalWeight > MaxWeight;
        }

        public bool IsMinWeightMaintained()
        {
            return _cachedTotalWeight >= MaxWeight * 0.5; // Minstens 50% van het MaxGewicht
        }

        public bool IsWeightDistributedProperly()
        {
            int totalWeight = ContainerStacks.Sum(stack => stack.TotalWeight());
            int targetWeightPerSide = totalWeight / 2;
            int tolerance = (int)(MaxWeight * 0.2);

            int leftWeight = 0;
            int rightWeight = 0;

            foreach (var stack in ContainerStacks)
            {
                if (Math.Abs((leftWeight + stack.TotalWeight()) - rightWeight) <=
                    Math.Abs(leftWeight - (rightWeight + stack.TotalWeight())))
                {
                    leftWeight += stack.TotalWeight();
                }
                else
                {
                    rightWeight += stack.TotalWeight();
                }
            }

            return Math.Abs(leftWeight - rightWeight) <= tolerance;
        }

        // ... overige methoden ...
    }
}
