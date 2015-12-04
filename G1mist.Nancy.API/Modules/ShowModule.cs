using System;
using System.Collections.Generic;
using System.Linq;
using G1mist.Nancy.API.Model.DTOs;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using Nancy;

namespace G1mist.Nancy.API.Modules
{
    public class ShowModule : NancyModule
    {
        public ShowModule()
            : base("/show")
        {
            Get["/index"] = _ => View["index.html"];

            Get["/temperature"] = p => View["Temperature.html"];

            Get["/voltage"] = p => View["Voltage.html"];

            Get["/electrical"] = p => View["Electrical.html"];

            Get["/efficiency"] = p => View["Efficiency.html"];

            Get["/Lighting"] = p => View["Lighting.html"];

            Get["/control"] = p => View["Control.html"];

            Get["/saving"] = p => View["Saving.html"];

            Get["/login"] = p => View["login.html"];
        }
    }
}