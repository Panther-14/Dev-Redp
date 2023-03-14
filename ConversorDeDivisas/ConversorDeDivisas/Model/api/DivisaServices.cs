using ConversorDeDivisas.Model.obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConversorDeDivisas.Model.api
{
    public class DivisaServices
    {
        private static readonly string
           URL_BASE = "https://openexchangerates.org/api/";
        private static readonly string
            API_ID = "e77f99c02f404d34a3631b67223d85e5";

        public static async Task<RespuestaServices> GetTasasConversion()
        {
            RespuestaServices respuesta = new RespuestaServices();
            using (var httpClient = new HttpClient())
            {

                HttpRequestMessage request;
                HttpResponseMessage response;
                try
                {
                    string url = string.Format("{0}latest.json?app_id={1}", URL_BASE, API_ID);

                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    response = await httpClient.SendAsync(request);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            // string responseJson = await response.Content.ReadAsStringAsync();

                            TasasConversion tasas = await response.Content.ReadFromJsonAsync<TasasConversion>();

                            if (tasas != null && tasas.Disclaimer != null && tasas.Rates != null)
                            {
                                respuesta.Error = false;
                                respuesta.Mensaje = "OK";
                                respuesta.Tasas = tasas;

                            }
                            else
                            {
                                respuesta.Error = true;
                                respuesta.Mensaje = "No se deserealizo la respuesta JSON...";
                            }
                        }
                        else
                        {
                            respuesta.Error = true;
                            respuesta.Mensaje = string.Format("Error. {0} - {1}", (int)response.StatusCode, response.IsSuccessStatusCode);
                        }
                    }
                    else
                    {
                        respuesta.Error = true;
                        respuesta.Mensaje = "No se puede obtener respuesta del servicio web";
                    }

                }
                catch (Exception e)
                {
                    respuesta.Error = true;
                    respuesta.Mensaje = e.Message;

                }

            }
        }
    }
}
