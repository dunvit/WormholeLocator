using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace WHL.UiTools
{
    class Tools
    {
        public static string ReadFile(string urlAddress, ILog log)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                log.ErrorFormat("[UiTools.ReadFile] Read configuration file {0} is failed. Status = {1} ", urlAddress, response.StatusCode);
                return null;
            }

            var receiveStream = response.GetResponseStream();

            var readStream = response.CharacterSet == null ? new StreamReader(receiveStream) : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

            var data = readStream.ReadToEnd();

            response.Close();
            readStream.Close();

            return data;
        }

        public static Color GetColorBySolarSystem(string solarSystemName)
        {
            if (solarSystemName.Contains("hi-sec system")) return Color.LimeGreen;

            if (solarSystemName.Contains("low-sec system")) return Color.Chocolate;

            if (solarSystemName.Contains("deadly w-space system")) return Color.DarkRed;

            if (solarSystemName.Contains("Class 2")) return Color.DeepSkyBlue;

            if (solarSystemName.Contains("Class 3")) return Color.DeepSkyBlue;

            if (solarSystemName.Contains("Class 1")) return Color.DeepSkyBlue;

            if (solarSystemName.Contains("Class 4")) return Color.OrangeRed;

            if (solarSystemName.Contains("Class 1")) return Color.DeepSkyBlue;

            if (solarSystemName.Contains("Class 5")) return Color.Crimson;

            return Color.Bisque;
        }
    }
}
