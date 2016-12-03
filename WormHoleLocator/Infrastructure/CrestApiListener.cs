using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace WHLocator.Infrastructure
{
    class CrestApiListener
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(frmMain));

        public void ListenLocalhost(frmMain.DelegateStartProcess StartPilotAuthorizeFlow)
        {
            var web = new HttpListener();

            const string url = "http://localhost";
            const string port = "8080";
            var prefix = string.Format("{0}:{1}/", url, port);

            //NetAclChecker.AddAddress(prefix);

            web.Prefixes.Add(prefix);

            Log.DebugFormat("Listening new ..");

            web.Start();

            while (true)
            {
                var context = web.GetContext();

                Task.Run(() =>
                {
                    var code = "";

                    Log.DebugFormat("Get new request.");

                    foreach (var key in context.Request.QueryString.Keys.Cast<object>().Where(key => key.ToString() == "code"))
                    {
                        code = context.Request.QueryString[key.ToString()];
                    }

                    using (var writer = new StreamWriter(context.Response.OutputStream))
                    {
                        writer.WriteLine("Wormhole Locator authorize complete. Close this tab and return to application.");
                    }
                    context.Response.OutputStream.Close();

                    if (string.IsNullOrEmpty(code) == false)
                    {
                        StartPilotAuthorizeFlow(code);
                    }

                });



            }

        }
    }
}
