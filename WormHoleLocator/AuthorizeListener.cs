using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WHLocator
{
    public class AuthorizeListener
    {
        private bool isRunning = true;

        public void Stop()
        {
            isRunning = false;
        }

        public void Run()
        {
            var thread = new Thread(() =>
            {
                var web = new HttpListener();

                web.Prefixes.Add("http://localhost:8080/");

                Console.WriteLine("Listening new ..");

                web.Start();

                while (isRunning)
                {
                    var context = web.GetContext();

                    Task.Run(() =>
                    {
                        Console.WriteLine(context.Request.Url.AbsolutePath);

                        foreach (var key in context.Request.QueryString.Keys)
                        {
                            Console.WriteLine(key + "=" + context.Request.QueryString[key.ToString()]);
                        }



                        using (var writer = new StreamWriter(context.Response.OutputStream))
                        {
                            writer.WriteLine("Wormhole Locator authorize complete. Return to application.");
                        }
                        context.Response.OutputStream.Close();

                    });



                }

                web.Stop();

            });

            thread.IsBackground = true;

            thread.Start();
        }
    }
}
