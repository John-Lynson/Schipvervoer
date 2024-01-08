using Schipvervoer.Logic;
using System;
using System.Collections.Generic;
using Schipvervoer.Models;

class Program
{
    static void Main(string[] args)
    {
        // Creëer een schip (voorbeeld)
        Schip schip = new Schip(100000); // MaxGewicht als voorbeeld

        // Voeg voorbeeldcontainers toe
        schip.VoegContainerToe(new Container(30000, false, false));
        schip.VoegContainerToe(new Container(4000, true, false));
        // Voeg meer containers toe zoals nodig

        // Indelen
        IndelingsAlgoritme algoritme = new IndelingsAlgoritme();
        algoritme.IndeelContainers(schip, schip.Containers);

        // Visualisatie van de indeling
        foreach (var container in schip.Containers)
        {
            Console.WriteLine($"Container Gewicht: {container.Gewicht}, Waardevol: {container.IsWaardeVol}, Gekoeld: {container.MoetGekoeldWorden}");
        }
    }
}
