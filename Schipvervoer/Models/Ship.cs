using Schipvervoer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; // Voeg deze namespace toe voor logging

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
            Trace.WriteLine($"Schip aangemaakt met MaxWeight: {MaxWeight}, Length: {Length}, Width: {Width}.");
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
            Trace.WriteLine("Stacks op het schip geïnitialiseerd.");
        }

        public bool IsOverloaded()
        {
            var overloaded = CalculateTotalWeight() > MaxWeight;
            Trace.WriteLine($"IsOverloaded check: {overloaded}.");
            return overloaded;
        }

        public bool IsMinWeightMaintained()
        {
            var minWeightMaintained = CalculateTotalWeight() >= MaxWeight * 0.5;
            Trace.WriteLine($"IsMinWeightMaintained check: {minWeightMaintained}.");
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
            Trace.WriteLine("Controleren of het gewicht goed verdeeld is.");

            int leftWeight = CalculateSideWeight(0, Width / 2);
            int rightWeight = CalculateSideWeight(Width / 2, Width);

            int tolerance = (int)(MaxWeight * 0.2);
            bool weightDistributedProperly = Math.Abs(leftWeight - rightWeight) <= tolerance;
            Trace.WriteLine($"Gewichtsverdeling: Links {leftWeight}, Rechts {rightWeight}, Tolerantie {tolerance}, Resultaat {weightDistributedProperly}");

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
            Trace.WriteLine($"Zijgewicht berekend: startWidth={startWidth}, endWidth={endWidth}, weight={sideWeight}.");
            return sideWeight;
        }

        public bool AreCoolingRequirementsMet()
        {
            for (int j = 0; j < Width; j++)
            {
                if (ShipLayout[0, j].ContainsCooling())
                {
                    Trace.WriteLine("Koelingsvereisten niet voldaan.");
                    return false;
                }
            }
            Trace.WriteLine("Koelingsvereisten voldaan.");
            return true;
        }

        public bool AreValuableContainersAccessible()
        {
            foreach (var stack in ShipLayout)
            {
                if (stack.ContainsValuable() && !stack.IsTopContainerValuable())
                {
                    Trace.WriteLine("Toegankelijkheid waardevolle containers niet voldaan.");
                    return false;
                }
            }
            Trace.WriteLine("Toegankelijkheid waardevolle containers voldaan.");
            return true;
        }

        // ... overige methoden ...
    }
}
