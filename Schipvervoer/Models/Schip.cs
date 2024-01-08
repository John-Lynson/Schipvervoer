using System.Collections.Generic;
using Schipvervoer.Models;

public class Schip
{
    public int MaxGewicht { get; set; }
    public List<Container> Containers { get; set; }

    public Schip(int maxGewicht)
    {
        MaxGewicht = maxGewicht;
        Containers = new List<Container>();
    }

    public void VoegContainerToe(Container container)
    {
        // Logica om te controleren of de container toegevoegd kan worden
        // Bijvoorbeeld controle op maximaal gewicht, type container, etc.
    }

    // Methoden voor balans en gewichtscontroles
}
