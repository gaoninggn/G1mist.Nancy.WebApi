using System;
using System.Collections.Generic;
using System.Linq;
using G1mist.Nancy.API.Model.DTOs;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using Nancy;

namespace G1mist.Nancy.API.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule(IBaseRepository<tb_gather> repository)
            : base("/api")
        {
            Get["/"] = _ => View["index.html"];

            Get["/temperature"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(5).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(5).ToList();

                voltagesA.Reverse();
                voltagesB.Reverse();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.temperature }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.temperature }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol); 
            };

            Get["/gettemperature"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(1).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(1).ToList();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.temperature }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.temperature }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol);
            };

            Get["/voltage"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(5).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(5).ToList();

                voltagesA.Reverse();
                voltagesB.Reverse();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.voltage }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.voltage }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol); 
            };

            Get["/getvoltage"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(1).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(1).ToList();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.voltage }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.voltage }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol);
            };

            Get["/electrical"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(5).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(5).ToList();

                voltagesA.Reverse();
                voltagesB.Reverse();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.electrical }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.electrical }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol); 
            };

            Get["/getelectrical"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(1).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(1).ToList();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = v.electrical }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = v.electrical }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol);
            };

            Get["/efficiency"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(5).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(5).ToList();

                voltagesA.Reverse();
                voltagesB.Reverse();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = Math.Round((v.electrical * v.voltage) / 1000, 2) }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = Math.Round((v.electrical * v.voltage) / 1000, 2) }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol); 
            };

            Get["/getefficiency"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(1).ToList();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(1).ToList();

                var dataA = voltagesA.Select(v => new data { x = long.Parse(v.time), y = Math.Round((v.electrical * v.voltage) / 1000, 2) }).ToList();
                var dataB = voltagesB.Select(v => new data { x = long.Parse(v.time), y = Math.Round((v.electrical * v.voltage) / 1000, 2) }).ToList();

                var listVol = new List<Voltage>
                {
                    new Voltage{name = "灯泡1",data = dataA},
                    new Voltage{name = "灯泡2",data = dataB}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listVol);
            };

            Get["/lumen"] = p =>
            {
                var voltagesA = repository.GetList(a => a.num == 1).OrderByDescending(a => a.time).Take(1).ToList().FirstOrDefault();
                var voltagesB = repository.GetList(a => a.num == 2).OrderByDescending(a => a.time).Take(1).ToList().FirstOrDefault();

                var listLuman = new List<LumanDto>
                {
                    new LumanDto {Num = voltagesA.num, Luman = voltagesA.lumen},
                    new LumanDto {Num = voltagesB.num, Luman = voltagesB.lumen}
                };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(listLuman);
            };
        }

        public long ConvertDateTimeInt(DateTime theDate)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = theDate.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
        }
    }
}