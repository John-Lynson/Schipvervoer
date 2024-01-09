using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schipvervoer.Models;

namespace Schipvervoer.Logic
{
    public class IndelingsAlgoritme
    {
        public void IndeelContainers(Schip schip, List<Container> containers)
        {
            // Sorteer containers: eerst gekoeld, dan waardevol, dan overige
            var gesorteerdeContainers = containers
                .OrderByDescending(c => c.MoetGekoeldWorden)
                .ThenByDescending(c => c.IsWaardeVol)
                .ToList();

            foreach (var container in gesorteerdeContainers)
            {
                // Voeg toe aan het schip, controleer op gewicht en type
                schip.VoegContainerToe(container);
            }
        }
    }
}