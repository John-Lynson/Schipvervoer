using Schipvervoer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Schipvervoer.Logic
{
    public class ContainerStack
    {
        public const int MaxWeightOnTop = 120000; 
        public List<Container> Containers { get; private set; }

        public ContainerStack()
        {
            Containers = new List<Container>();
        }

        public bool CanPlaceOnTop(Container container)
        {
            if (container.IsValuable)
            {
                bool canPlace = Containers.Count == 0 || Containers.All(c => c.IsValuable);
                return canPlace;
            }

            int totalWeightAbove = Containers.Sum(c => c.Weight);
            bool canPlaceRegular = totalWeightAbove + container.Weight <= MaxWeightOnTop;

            return canPlaceRegular;
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

        public bool ContainsCooling()
        {
            bool containsCooling = Containers.Any(c => c.RequiresCooling);
            return containsCooling;
        }

        public bool ContainsValuable()
        {
            bool containsValuable = Containers.Any(c => c.IsValuable);
            return containsValuable;
        }

        public bool IsTopContainerValuable()
        {
            bool isTopValuable = Containers.LastOrDefault()?.IsValuable ?? false;
            return isTopValuable;
        }
    }
}
