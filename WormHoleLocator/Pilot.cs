using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WindowsFormsApplication3;
using log4net;
using log4net.Repository.Hierarchy;
using WHLocator.Infrastructure;

namespace WHLocator
{
    public class Pilot
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CrestAuthorization));
        public long Id { get; set; }

        public string Name { get; set; }

        public Image Portrait { get; set; }

        public SolarSystem Location { get; set; }

        public CrestAuthorization CrestData { get; set; }

        private DateTime LastTokenUpdate;

        public void Initialization(string token)
        {
            Log.DebugFormat("[Pilot.Initialization] starting for token = {0}", token);

            CrestData = new CrestAuthorization(token);

            dynamic data = CrestData.ObtainingCharacterData();

            Id = data.CharacterID;
            Name = data.CharacterName;

            LoadLocationInfo();

            LoadCharacterInfo();

            LastTokenUpdate = DateTime.Now;
        }

        private void LoadCharacterInfo()
        {
            Log.DebugFormat("[Pilot.LoadCharacterInfo] starting for Id = {0}", Id);

            dynamic characterInfo = CrestData.GetCharacterInfo(Id);

            var portraitAddress = characterInfo.SelectToken("portrait.64x64.href").Value;

            var request = WebRequest.Create(portraitAddress);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                Portrait = Bitmap.FromStream(stream);
            }
        }

        private void LoadLocationInfo()
        {
            Log.DebugFormat("[Pilot.LoadLocationInfo] starting for Id = {0}", Id);

            try
            {
                if (Location == null)
                {
                    Location = new SolarSystem();
                }

                dynamic locationInfo = CrestData.GetLocation(Id);

                if (Location.Id != locationInfo.SelectToken("solarSystem.id").Value)
                {
                    Location.Id = locationInfo.SelectToken("solarSystem.id").Value;
                    Location.Name = locationInfo.SelectToken("solarSystem.name").Value.ToString();

                    if (Location.Name.Contains("J"))
                    {
                        GetAllWormholeInformation(Location.Name.Replace("J", ""));
                    }
                    else
                    {
                        Location.ReLoad();
                        Location.Class = "";
                        Location.Effect = "";
                    }
                }

            }
            catch (Exception ex)
            {
                Log.ErrorFormat("[Pilot.LoadLocationInfo] pilot Id = {0} not login in game.", Id);

                Location.Id = 0;
                Location.Name = "unknown";
            }
            
        }

        private void GetAllWormholeInformation(string wormholeId)
        {
            Log.DebugFormat("[Pilot.GetAllWormholeInformation] starting for wormholeId = {0}", wormholeId);

            try
            {

                Location.ReLoad();

                var data = WHL.UiTools.Tools.ReadFile("http://superpute.com/system/J" + wormholeId, Log);

                var dataParts = data.Split(new[] { "WH Static</a><br />" }, StringSplitOptions.None)[1].Split(new[] { "</table>" }, StringSplitOptions.None)[0];

                var m1 = Regex.Matches(dataParts, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

                foreach (Match m in m1)
                {
                    var value = m.Groups[1].Value;

                    var staticWormholeCode = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);

                    Location.StaticSystems.Add(staticWormholeCode);
                }

                dataParts = data.Split(new[] { "Class" }, StringSplitOptions.None)[1].Split(new[] { "WH Effect" }, StringSplitOptions.None)[0];

                m1 = Regex.Matches(dataParts, @"(<font.*?>.*?</font>)", RegexOptions.Singleline);

                foreach (Match m in m1)
                {
                    var value = m.Groups[1].Value;

                    var systemClass = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline).Replace("&nbsp;","");

                    Location.Class = systemClass;
                }

                dataParts = data.Split(new[] { "WH Effect" }, StringSplitOptions.None)[1].Split(new[] { "</tr>" }, StringSplitOptions.None)[0];

                m1 = Regex.Matches(dataParts, @"(<td.*?>.*?</td>)", RegexOptions.Singleline);

                foreach (Match m in m1)
                {
                    var value = m.Groups[1].Value;

                    var systemEffect = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline).Replace("&nbsp;", "");

                    Location.Effect = systemEffect;
                }

                data = data.Replace(Environment.NewLine, "").Replace(" ","");

                dataParts = data.Replace(Environment.NewLine, "").Split(new[] { "<td>Constellation" }, StringSplitOptions.None)[1].Split(new[] { "</tr>" }, StringSplitOptions.None)[0];

                m1 = Regex.Matches(dataParts, @"(<td.*?>.*?</td>)", RegexOptions.Singleline);

                foreach (Match m in m1)
                {
                    var value = m.Groups[1].Value;

                    var systemEffect = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline).Replace("&nbsp;", "");

                    Location.Constellation = systemEffect;
                }

                dataParts = data.Split(new[] { "<td>Region" }, StringSplitOptions.None)[1].Split(new[] { "</tr>" }, StringSplitOptions.None)[0];

                m1 = Regex.Matches(dataParts, @"(<td.*?>.*?</td>)", RegexOptions.Singleline);

                foreach (Match m in m1)
                {
                    var value = m.Groups[1].Value;

                    var systemEffect = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline).Replace("&nbsp;", "");

                    Location.Region = systemEffect;
                }

                //Constellation
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("[Pilot.GetAllWormholeInformation] Critical error {0}", ex);
            }

            
        }


        public void RefreshInfo()
        {
            Log.DebugFormat("[Pilot.RefreshInfo] starting for Id = {0}", Id);

            var span = DateTime.Now - LastTokenUpdate;
            var ms = (int)span.TotalMilliseconds;

            if (ms > CrestData.ExpiresIn + 200)
            {
                CrestData.Refresh();

                LastTokenUpdate = DateTime.Now;

                Log.DebugFormat("[Pilot.RefreshInfo] set LastTokenUpdate for Id = {0}", Id);
            }

            LoadLocationInfo();
        }

        
    }
}
