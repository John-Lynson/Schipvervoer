using Schipvervoer.Logic;
using Schipvervoer.Models;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Voer de maximale gewichtscapaciteit van het schip in:");
        int maxWeight = int.Parse(Console.ReadLine());

        // Voeg lengte en breedte toe (deze kunnen ook dynamisch worden opgevraagd)
        int length = 10; // Voorbeeldwaarde
        int width = 10;  // Voorbeeldwaarde
        Ship ship = new Ship(maxWeight, length, width);

        Console.WriteLine("Voer het aantal containers in:");
        int containerCount = int.Parse(Console.ReadLine());

        List<Container> containers = new List<Container>();

        for (int i = 0; i < containerCount; i++)
        {
            Console.WriteLine($"Container {i + 1}: Voer gewicht, is waardevol (true/false), vereist koeling (true/false) in:");
            string[] inputs = Console.ReadLine().Split(',');
            int weight = int.Parse(inputs[0]);
            bool isValuable = bool.Parse(inputs[1]);
            bool requiresCooling = bool.Parse(inputs[2]);

            Container container = new Container(weight, isValuable, requiresCooling);
            containers.Add(container);
        }

        ContainerAllocationAlgorithm algorithm = new ContainerAllocationAlgorithm();
        algorithm.AllocateContainers(ship, containers); // Zorg ervoor dat deze methode correct is geïmplementeerd

        // Visualisatie van de indeling
        VisualizeShipLayout(ship);
    }

    static void VisualizeShipLayout(Ship ship)
    {
        Console.WriteLine("Indeling van het schip:");
        foreach (var stack in ship.ContainerStacks)
        {
            Console.WriteLine("Stack:");
            foreach (var container in stack.Containers)
            {
                Console.WriteLine($"- Gewicht: {container.Weight}, Waardevol: {container.IsValuable}, Gekoeld: {container.RequiresCooling}");
            }
        }
    }
}
