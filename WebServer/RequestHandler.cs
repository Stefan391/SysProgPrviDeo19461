using SysProgPrviDeo19461.Caching;
using SysProgPrviDeo19461.Const;
using SysProgPrviDeo19461.FileManager;
using SysProgPrviDeo19461.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SysProgPrviDeo19461.WebServer
{
    public class RequestHandler
    {
        public static void HandleRequest(object? state)
        {
            HttpListenerContext? context = null;
            try
            {
                Logger.Log("Obradjivanje zahteva");

                if (state == null)
                    throw new Exception("Greska prilikom obrade");

                context = (HttpListenerContext)state;
                Obrada(context);
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
                if(context != null)
                    SendResponse(context, $"{ex.Message}", true);
            }
        }

        public static void Obrada(HttpListenerContext context)
        {
            string nazivFajla = context?.Request?.Url?.AbsolutePath.TrimStart('/') ?? "";

            if (string.IsNullOrWhiteSpace(nazivFajla))
                throw new Exception("Naziv fajla je obavezan");

            Logger.Log($"Obradjivanje za fajl: {nazivFajla}");

            string odgovor = CacheManger.GetItem(nazivFajla);
            if (!string.IsNullOrWhiteSpace(odgovor))
            {
                SendResponse(context!, odgovor);
                Logger.Log($"Uspesno vracanje odgovora iz kesa: {odgovor}");
                return;
            }

            string fajl = FileManager.FileManager.FindFile(AppSettings.RootFolder, nazivFajla);
            
            if (string.IsNullOrWhiteSpace(fajl))
                throw new Exception("Trazeni fajl ne postoji.");

            int avg = FileManager.FileManager.FindAverageWordLength(fajl);
            if(avg == 0)
                throw new Exception("Fajl koji je poslat je prazan");

            CacheManger.SetItem(nazivFajla, avg.ToString());

            Logger.Log($"Uspesna obrada, slanje odgovora: {avg}");
            SendResponse(context!, $"{avg}");
        }

        private static void SendResponse(HttpListenerContext context, string response, bool isError = false)
        {
            try
            {
                if (isError)
                    context.Response.StatusCode = (short)HttpStatusCode.BadRequest;

                byte[] buffer = Encoding.UTF8.GetBytes(response);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Logger.Error($"{ex.Message}");
            }
            finally
            {
                context.Response.OutputStream.Close();
            }
        }
    }
}
