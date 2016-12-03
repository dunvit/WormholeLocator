using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using log4net;

namespace WHLocator
{
    class WormholeType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Life { get; set; }
    }

    class Wormholes
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Wormholes));

        private readonly List<WormholeType> _list = new List<WormholeType>();

        public WormholeType GetWormhole(string name)
        {
            Log.DebugFormat("[Wormholes.GetWormhole] name is {0}", name);

            return _list.FirstOrDefault(wormholeType => wormholeType.Id.Trim() == name.Trim());
        }

        public Wormholes()
        {
            using (var sr = new StreamReader(@"Data/Wormholes.csv"))
            {
                var reader = new CsvReader(sr);

                //CSVReader will now read the whole file into an enumerable
                var records = reader.GetRecords<WormholeType>();

                foreach (WormholeType record in records)
                {
                    Log.DebugFormat("[Wormholes.Wormholes] Read csv row. {0} {1}, {2}", record.Id, record.Name, record.Life);

                    _list.Add(record);
                }
            }
        }
    }
}
