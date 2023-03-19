using ConversorDeDivisas.Model.obj;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConversorDeDivisas.Model.api
{
    public class DivisasServices
    {

        private static readonly string
        URL_BASE = "https://openexchangerates.org/api/";
        private static readonly string
        APP_ID = "e77f99c02f404d34a3631b67223d85e5";

        public static async Task<RespuestaService> getTasasConversion()
        {
            RespuestaService res = new RespuestaService();
            using (var httpClient = new HttpClient())
            {

                HttpRequestMessage request;
                HttpResponseMessage response;
                try
                {
                    string url = string.Format("{0}latest.json?app_id={1}", URL_BASE, APP_ID);

                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    response = await httpClient.SendAsync(request);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TasasConversion tasas =
                            await response.Content.ReadFromJsonAsync<TasasConversion>();

                            if (tasas != null && tasas.Disclaimer != null && tasas.Rates != null)
                            {
                                res.Error = false;
                                res.Mensaje = "OK";
                                res.Tasas = tasas;
                            }
                            else
                            {
                                res.Error = true;
                                res.Mensaje = "No se deserializar la respuesta en JSON...";
                            }
                        }
                        else
                        {
                            res.Error = true;
                            res.Mensaje = String.Format("Error: {0} - {1}",
                            (int)response.StatusCode, response.StatusCode);
                        }
                    }
                    else
                    {
                        res.Error = true;
                        res.Mensaje = "No se puede obtener respuesta del servicio web";
                    }
                }
                catch (Exception ex)
                {
                    res.Error = true;
                    res.Mensaje = ex.Message;
                }
                return res;
            }
        }

        public static async Task<RespuestaService> getMonedasConversion()
        {
            RespuestaService res = new RespuestaService();
            using (var httpClient = new HttpClient())
            {

                HttpRequestMessage request;
                HttpResponseMessage response;
                try
                {
                    string url = string.Format("{0}currencies.json?app_id={1}", URL_BASE, APP_ID);

                    request = new HttpRequestMessage(HttpMethod.Get, url);
                    response = await httpClient.SendAsync(request);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            JObject currenciesJson = JObject.Parse(json);
                            res.Monedas = new List<Moneda>();
                            foreach (JProperty currency in currenciesJson.Properties())
                            {
                                Moneda newCurrency = new Moneda()
                                {
                                    Codigo = currency.Name,
                                    Nombre = (string)currency.Value
                                };
                                res.Monedas.Add(newCurrency);
                            }

                        }
                        else
                        {
                            res.Error = true;
                            res.Mensaje = String.Format("Error: {0} - {1}",
                            (int)response.StatusCode, response.StatusCode);
                        }
                    }
                    else
                    {
                        res.Error = true;
                        res.Mensaje = "No se puede obtener respuesta del servicio web";
                    }
                }
                catch (Exception ex)
                {
                    res.Error = true;
                    res.Mensaje = ex.Message;
                }
                return res;
            }
        }
    }
}
