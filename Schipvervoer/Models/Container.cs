using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schipvervoer.Logic;
using System.Threading.Tasks;

namespace Schipvervoer.Models
{
    public class Container
    {
        public int Weight { get; set; }
        public bool IsValuable { get; set; }
        public bool RequiresCooling { get; set; }

        public Container(int weight, bool isValuable, bool requiresCooling)
        {
            Weight = weight;
            IsValuable = isValuable;
            RequiresCooling = requiresCooling;
        }
    }
}
