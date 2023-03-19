using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorDeDivisas.Model.obj
{
    public class TasasConversion
    {
        public string Disclaimer { get; set; }
        public string License { get; set; }
        public long Timestamp { get; set; }
        public string Base { get; set; }
        public Dictionary<string, Double> Rates { get; set; }
    }
}
