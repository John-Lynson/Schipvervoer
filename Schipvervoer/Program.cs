using Schipvervoer.Logic;
using Schipvervoer.Models;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Voer de maximale gewichtscapaciteit van het schip in:");
        int maxWeight = int.Parse(Console.ReadLine());

        Ship ship = new Ship(maxWeight);

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
        algorithm.AllocateContainersToStacks(ship, containers);

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
