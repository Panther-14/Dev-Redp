using ConversorDeDivisas.Model.obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversorDeDivisas.Model.api
{
    public class RespuestaService
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public TasasConversion Tasas { get; set; }
        public List<Moneda> Monedas { get; set; }
    }
}
