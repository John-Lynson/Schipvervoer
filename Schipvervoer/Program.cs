using Schipvervoer.Logic;
using System;
using System.Collections.Generic;
using Schipvervoer.Models;

class Program
{
    static void Main(string[] args)
    {
        // Voorbeeld: Maak een schip aan
        Schip schip = new Schip(100000); // MaxGewicht als voorbeeld

        // Voeg containers toe (voorbeeld)
        schip.VoegContainerToe(new Container(30000, false, false));
        schip.VoegContainerToe(new Container(4000, true, false));

        // Maak een instantie van de indelingsalgoritme
        IndelingsAlgoritme algoritme = new IndelingsAlgoritme();

        // Roep de indelingsmethode aan
        algoritme.IndeelContainers(schip, schip.Containers);

        // Voeg hier je logica toe om de indeling te tonen of te testen
    }
}
