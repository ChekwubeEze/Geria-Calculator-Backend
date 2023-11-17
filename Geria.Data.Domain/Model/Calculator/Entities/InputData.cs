using Geria.Data.Domain.Infrastruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Data.Domain.Model.Calculator.Entities
{
    public class InputData :BaseEntity
    {
        public int FirstNumber { get; set; }
        public int LastNumber { get; set; }
        public string Sign { get; set; }
        public decimal Result { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
