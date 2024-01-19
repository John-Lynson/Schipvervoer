using Schipvervoer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; 

namespace Schipvervoer.Models
{
    public class Ship
    {
        public int MaxWeight { get; set; }
        public ContainerStack[,] ShipLayout { get; private set; }
        public int Length { get; set; }
        public int Width { get; set; }

        public Ship(int maxWeight, int length, int width)
        {
            MaxWeight = maxWeight;
            Length = length;
            Width = width;
            ShipLayout = new ContainerStack[length, width];
            InitializeStacks();
        }

        private void InitializeStacks()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ShipLayout[i, j] = new ContainerStack();
                }
            }
        }

        public bool IsOverloaded()
        {
            var overloaded = CalculateTotalWeight() > MaxWeight;
            return overloaded;
        }

        public bool IsMinWeightMaintained()
        {
            var minWeightMaintained = CalculateTotalWeight() >= MaxWeight * 0.5;
            return minWeightMaintained;
        }

        private int CalculateTotalWeight()
        {
            int totalWeight = 0;
            foreach (var stack in ShipLayout)
            {
                totalWeight += stack.TotalWeight();
            }
            Trace.WriteLine($"Totaalgewicht berekend: {totalWeight}.");
            return totalWeight;
        }

        public bool IsWeightDistributedProperly()
        {

            int leftWeight = CalculateSideWeight(0, Width / 2);
            int rightWeight = CalculateSideWeight(Width / 2, Width);

            int tolerance = (int)(MaxWeight * 0.2);
            bool weightDistributedProperly = Math.Abs(leftWeight - rightWeight) <= tolerance;

            return weightDistributedProperly;
        }


        public int CalculateSideWeight(int startWidth, int endWidth)
        {
            int sideWeight = 0;
            for (int i = 0; i < Length; i++)
            {
                for (int j = startWidth; j < endWidth; j++)
                {
                    sideWeight += ShipLayout[i, j].TotalWeight();
                }
            }
            return sideWeight;
        }

        public bool AreCoolingRequirementsMet()
        {
            for (int i = 1; i < Length; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (ShipLayout[i, j].ContainsCooling())
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public bool AreValuableContainersAccessible()
        {
            foreach (var stack in ShipLayout)
            {
                if (stack.ContainsValuable() && !stack.IsTopContainerValuable())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
