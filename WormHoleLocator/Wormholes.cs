using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using log4net;

namespace WHLocator
{
    class Wormhole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Life { get; set; }
        public string MaxStableMass { get; set; }
        public string MaxJumpMass { get; set; }
    }

    class Wormholes
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Wormholes));

        private readonly List<Wormhole> _list = new List<Wormhole>();

        public Wormhole GetWormhole(string name)
        {
            Log.DebugFormat("[Wormholes.GetWormhole] name is {0}", name);

            return _list.FirstOrDefault(wormholeType => wormholeType.Id.Trim() == name.Trim());
        }

        public Wormholes()
        {
            using (var sr = new StreamReader(@"Data/WormholesList.csv"))
            {
                var reader = new CsvReader(sr);

                //CSVReader will now read the whole file into an enumerable
                var records = reader.GetRecords<Wormhole>();

                foreach (Wormhole record in records)
                {
                    Log.DebugFormat("[Wormholes.Wormholes] Read csv row. {0} {1}, {2}", record.Id, record.Name, record.Life);

                    record.MaxStableMass = record.MaxStableMass.Replace(":", "");

                    _list.Add(record);
                }
            }

            //GetAdditionalInfo();
        }

        private void GetAdditionalInfo()
        {
            var list = new List<Wormhole>();

            foreach (var wormholeType in _list)
            {
                var data = WHL.UiTools.Tools.ReadFile("http://www.ellatha.com/eve/wormholelistview.asp?key=Wormhole+" + wormholeType.Id, Log);

                var wormhole = new Wormhole
                {
                    Id = wormholeType.Id,
                    Name = wormholeType.Name,
                    Life = wormholeType.Life
                };


                var dataParts = data.Split(new[] { "Max Stable Mass</b>" }, StringSplitOptions.None)[1].Split(new[] { "&nbsp;kg" }, StringSplitOptions.None)[0];

                wormhole.MaxStableMass = dataParts;

                dataParts = data.Split(new[] { "<b>Max Jump Mass</b>:" }, StringSplitOptions.None)[1].Split(new[] { "&nbsp;kg" }, StringSplitOptions.None)[0];

                wormhole.MaxJumpMass = dataParts;

                list.Add(wormhole);
            }

            using (var sw = new StreamWriter(@"WormholesList.csv"))
            {
                var writer = new CsvWriter(sw);

                writer.WriteRecords(list);
            }
        }
    }
}
