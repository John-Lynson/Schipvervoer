using Schipvervoer.Logic;
using Schipvervoer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics; // Voeg deze namespace toe voor logging

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("Voer de maximale gewichtscapaciteit van het schip in:");
        int maxWeight = int.Parse(Console.ReadLine());


        int length = 8;
        int width = 4;
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
        try
        {
            algorithm.AllocateContainers(ship, containers);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Fout bij toewijzen van containers: {ex.Message}");
        }

        VisualizeShipLayout(ship);
    }

    static void VisualizeShipLayout(Ship ship)
    {
        Console.WriteLine("Indeling van het schip:");
        Console.WriteLine("Legenda: # = Waardevol, @ = Gekoeld, * = Regulier, [Leeg] = Geen Container");
        Console.WriteLine();

        // Header voor kolomnummers
        Console.Write("       ");
        for (int col = 0; col < ship.Width; col++)
        {
            Console.Write($" {col + 1,6} ");
        }
        Console.WriteLine();

        int[] weightPerRow = new int[ship.Length];
        int halfWidth = ship.Width / 2;

        for (int i = 0; i < ship.Length; i++)
        {
            Console.Write($"Rij {i + 1} "); // Rij nummering, startend bij 1
            for (int j = 0; j < ship.Width; j++)
            {
                var stack = ship.ShipLayout[i, j];
                int stackCount = stack.Containers.Count;
                weightPerRow[i] += stack.TotalWeight();

                if (stackCount == 0)
                {
                    Console.Write(" [Leeg] ");
                    continue;
                }

                var topContainer = stack.Containers.LastOrDefault();
                char symbol = topContainer.IsValuable ? '#' : (topContainer.RequiresCooling ? '@' : '*');
                Console.ForegroundColor = topContainer.IsValuable ? ConsoleColor.Red : (topContainer.RequiresCooling ? ConsoleColor.Blue : ConsoleColor.Gray);
                Console.Write($" [{symbol}{stackCount}] ");
                Console.ResetColor();
            }
            Console.WriteLine(); // Nieuwe regel na elke rij
        }

        // Gewichtsinfo aan het einde
        Console.WriteLine("\nGewicht per rij:");
        for (int i = 0; i < ship.Length; i++)
        {
            Console.WriteLine($"Rij {i + 1}: {weightPerRow[i]} kg");
        }

        // Gewichtsinfo per kant
        Console.WriteLine("\nGewicht per kant:");
        int leftWeight = 0, rightWeight = 0;
        for (int i = 0; i < ship.Length; i++)
        {
            for (int j = 0; j < halfWidth; j++)
            {
                leftWeight += ship.ShipLayout[i, j].TotalWeight();
            }
            for (int j = halfWidth; j < ship.Width; j++)
            {
                rightWeight += ship.ShipLayout[i, j].TotalWeight();
            }
        }
        Console.WriteLine($"Links: {leftWeight} kg, Rechts: {rightWeight} kg");
    }
}



