using Schipvervoer.Logic;
using Schipvervoer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics; // Voeg deze namespace toe voor logging

class Program
{
    static void Main(string[] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener()); // Voeg een TraceListener toe voor logging
        Trace.AutoFlush = true;

        Console.WriteLine("Voer de maximale gewichtscapaciteit van het schip in:");
        int maxWeight = int.Parse(Console.ReadLine());
        Trace.WriteLine($"Maximale gewichtscapaciteit ingevoerd: {maxWeight}");

        // Voeg lengte en breedte toe (deze kunnen ook dynamisch worden opgevraagd)
        int length = 10; // Voorbeeldwaarde
        int width = 10;  // Voorbeeldwaarde
        Trace.WriteLine($"Schipgrootte ingesteld: Lengte {length}, Breedte {width}");
        Ship ship = new Ship(maxWeight, length, width);

        Console.WriteLine("Voer het aantal containers in:");
        int containerCount = int.Parse(Console.ReadLine());
        Trace.WriteLine($"Aantal containers ingevoerd: {containerCount}");

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
            Trace.WriteLine($"Container toegevoegd: Gewicht {weight}, Waardevol {isValuable}, Vereist Koeling {requiresCooling}");
        }

        ContainerAllocationAlgorithm algorithm = new ContainerAllocationAlgorithm();
        try
        {
            algorithm.AllocateContainers(ship, containers); // Zorg ervoor dat deze methode correct is geïmplementeerd
            Trace.WriteLine("Containers succesvol toegewezen.");
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Fout bij toewijzen van containers: {ex.Message}");
        }

        // Visualisatie van de indeling
        VisualizeShipLayout(ship);
    }

    static void VisualizeShipLayout(Ship ship)
    {
        Console.WriteLine("Indeling van het schip:");
        for (int i = 0; i < ship.Length; i++)
        {
            for (int j = 0; j < ship.Width; j++)
            {
                Console.WriteLine($"Stack op positie ({i}, {j}):");
                foreach (var container in ship.ShipLayout[i, j].Containers)
                {
                    Console.WriteLine($"- Gewicht: {container.Weight}, Waardevol: {container.IsValuable}, Gekoeld: {container.RequiresCooling}");
                }
            }
        }
    }
}
