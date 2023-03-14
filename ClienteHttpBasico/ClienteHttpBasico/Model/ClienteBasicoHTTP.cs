using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClienteHttpBasico.Model
{
    public class ClienteBasicoHTTP
    {
        public static async Task<HttpResponseMessage> ejecutarPeticion(string url, string metodo)
        {
            using (var httpClient = new HttpClient())
            {
                HttpRequestMessage request = null;
                HttpResponseMessage response = null;
                switch (metodo)
                {
                    case "GET":
                        request = new HttpRequestMessage(HttpMethod.Get, url);
                        response = await httpClient.SendAsync(request);
                        break;
                    case "HEAD":
                        request = new HttpRequestMessage(HttpMethod.Head, url);
                        response = await httpClient.SendAsync(request);
                        break;
                    case "OPTIONS":
                        request = new HttpRequestMessage(HttpMethod.Options, url);
                        response = await httpClient.SendAsync(request);
                        break;
                    case "POST":
                        break;
                    case "PUT":
                        break;
                    case "PATCH":
                        break;
                    case "DELETE":
                        break;
                }
                return response;
            }
        }
    }
}
