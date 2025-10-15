using SysProgPrviDeo19461.Logging;
using SysProgPrviDeo19461.WebServer;

namespace SysProgPrviDeo19461
{
    public class Program
    {
        static void Main(string[] args)
        {
            WebListener server = new WebListener();
            Logger.Log("Pokretanje servera.");
            server.Listen();
            Logger.Log("Server je pokrenut.");
        }
    }
}
