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
        public int Length { get; set; }
        public int Width { get; set; }

        public Ship(int maxWeight, int length, int width)
        {
            MaxWeight = maxWeight;
            Length = length;
            Width = width;
            ContainerStacks = new List<ContainerStack>();
            InitializeStacks();
        }

        private void InitializeStacks()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ContainerStacks.Add(new ContainerStack());
                }
            }
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

        public bool AreCoolingRequirementsMet()
        {
            // Aannemende dat de eerste rij van containerstacks overeenkomt met de eerste 'Width' stacks in de lijst
            for (int i = 0; i < Width; i++)
            {
                foreach (var container in ContainerStacks[i].Containers)
                {
                    if (container.RequiresCooling && i >= Width)
                    {
                        // Een gekoelde container bevindt zich niet in de eerste rij
                        return false;
                    }
                }
            }

            // Alle gekoelde containers bevinden zich in de eerste rij
            return true;
        }

        public bool AreValuableContainersAccessible()
        {
            foreach (var stack in ContainerStacks)
            {
                for (int i = 0; i < stack.Containers.Count; i++)
                {
                    if (stack.Containers[i].IsValuable && i != 0)
                    {
                        // Een waardevolle container heeft andere containers erbovenop
                        return false;
                    }
                }
            }

            // Alle waardevolle containers zijn toegankelijk (geen andere containers erbovenop)
            return true;
        }


        // ... overige methoden ...
    }
}
