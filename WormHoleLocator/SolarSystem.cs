

using System.Collections.Generic;

namespace WindowsFormsApplication3
{
    public class SolarSystem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public string Region { get; set; }

        public string Constellation { get; set; }

        public string Effect { get; set; }

        public List<string> StaticSystems = new List<string>();

        public void ReLoad()
        {
            StaticSystems = new List<string>();
        }
    }
}
