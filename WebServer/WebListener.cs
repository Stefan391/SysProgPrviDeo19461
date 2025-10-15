using SysProgPrviDeo19461.Const;
using SysProgPrviDeo19461.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProgPrviDeo19461.WebServer
{
    public class WebListener
    {
        private static HttpListener listener = new HttpListener();
        public void Listen()
        {
            listener.Prefixes.Add(AppSettings.ListenerUrl);
            listener.Start();
            Logger.Log($"Server slusa na adresi: {AppSettings.ListenerUrl}");
            
            while (true)
            {
                var context = listener.GetContext();
                ThreadPool.QueueUserWorkItem(RequestHandler.HandleRequest, context);
            }
        }
    }
}
