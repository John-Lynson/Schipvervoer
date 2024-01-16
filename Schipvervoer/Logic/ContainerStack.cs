using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schipvervoer.Models;

namespace Schipvervoer.Logic
{
    public class ContainerStack
    {
        public List<Container> Containers { get; private set; }

        public ContainerStack()
        {
            Containers = new List<Container>();
        }

        public bool CanPlaceOnTop(Container container)
        {
            // Controleer of de container bovenop geplaatst kan worden.
            // Bijvoorbeeld, waardevolle containers mogen niet bedekt worden.

            if (container.IsValuable)
            {
                // Waardevolle containers kunnen alleen bovenop een lege stack of bovenop een andere waardevolle container.
                return Containers.Count == 0 || Containers.All(c => c.IsValuable);
            }
            else
            {
                // Voor reguliere containers, controleer andere condities (zoals gewichtsbeperkingen).
                return true; // Dit moet verder uitgewerkt worden op basis van specifieke regels.
            }
        }

        // Overige methoden...
    }
}
