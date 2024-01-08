using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schipvervoer.Models
{
    public class Container
    {
        public int Gewicht { get; set; }
        public bool IsWaardeVol { get; set; }
        public bool MoetGekoeldWorden { get; set; }

        public Container(int gewicht, bool isWaardeVol, bool moetGekoeldWorden)
        {
            Gewicht = gewicht;
            IsWaardeVol = isWaardeVol;
            MoetGekoeldWorden = moetGekoeldWorden;
        }
    }
}
