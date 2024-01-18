using Schipvervoer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Schipvervoer.Logic
{
    public class ContainerStack
    {
        public const int MaxWeightOnTop = 120000; // Maximaal gewicht in kg
        public List<Container> Containers { get; private set; }

        public ContainerStack()
        {
            Containers = new List<Container>();
            Trace.WriteLine("Nieuwe ContainerStack aangemaakt.");
        }

        public bool CanPlaceOnTop(Container container)
        {
            Trace.WriteLine($"Controleren of container met gewicht {container.Weight} bovenop geplaatst kan worden.");
            if (container.IsValuable)
            {
                bool canPlace = Containers.Count == 0 || Containers.All(c => c.IsValuable);
                Trace.WriteLine($"Container is waardevol. Kan bovenop plaatsen: {canPlace}");
                return canPlace;
            }

            int totalWeightAbove = Containers.Sum(c => c.Weight);
            bool canPlaceRegular = totalWeightAbove + container.Weight <= MaxWeightOnTop;
            Trace.WriteLine($"Container is regulier. Totale gewicht na plaatsing zou zijn: {totalWeightAbove + container.Weight}. Kan plaatsen: {canPlaceRegular}");

            return canPlaceRegular;
        }

        public void AddContainer(Container container)
        {
            Trace.WriteLine($"Proberen container met gewicht {container.Weight} toe te voegen.");
            if (!CanPlaceOnTop(container))
            {
                Trace.WriteLine($"Kan container niet bovenop plaatsen. Gooit InvalidOperationException.");
                throw new InvalidOperationException($"Kan container (Gewicht: {container.Weight}, Waardevol: {container.IsValuable}, Gekoeld: {container.RequiresCooling}) niet bovenop plaatsen in stack met totaalgewicht {TotalWeight()}");
            }

            Containers.Add(container);
            Trace.WriteLine($"Container toegevoegd. Nieuw totaalgewicht van stack: {TotalWeight()}");
        }

        public int TotalWeight()
        {
            return Containers.Sum(c => c.Weight);
        }

        public bool ContainsCooling()
        {
            bool containsCooling = Containers.Any(c => c.RequiresCooling);
            Trace.WriteLine($"Stack bevat gekoelde container: {containsCooling}");
            return containsCooling;
        }

        public bool ContainsValuable()
        {
            bool containsValuable = Containers.Any(c => c.IsValuable);
            Trace.WriteLine($"Stack bevat waardevolle container: {containsValuable}");
            return containsValuable;
        }

        public bool IsTopContainerValuable()
        {
            bool isTopValuable = Containers.LastOrDefault()?.IsValuable ?? false;
            Trace.WriteLine($"Bovenste container in stack is waardevol: {isTopValuable}");
            return isTopValuable;
        }
    }
}
