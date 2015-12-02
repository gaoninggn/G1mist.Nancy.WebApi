using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace G1mist.Nancy.API.Model.DTOs
{
    public class Voltage
    {
        public string name { get; set; }

        public List<data> data { get; set; }

        public Voltage()
        {
            data = new List<data>();
        }
    }
}