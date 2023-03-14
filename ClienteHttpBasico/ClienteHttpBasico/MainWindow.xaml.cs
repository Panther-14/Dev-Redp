using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using HeyRed.Mime;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Security.Policy;

namespace ClienteHttpBasico
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpResponseMessage response;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_url.Text))
            {
                this.bdr_indicador.BorderBrush = null;
                this.txt_headers.Text = "";
                this.txt_body.Text = "";
                this.lbl_statuscode.Content = "";
                try
                {
                    if (!txt_url.Text.ToLower().StartsWith("http") &&
                        !txt_url.Text.ToLower().StartsWith("https"))
                    {
                        txt_url.Text = "https://" + txt_url.Text;
                    }

                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Blue);
                    this.lbl_statuscode.Content = "Procesando peticion";

                    response =
                        await Model.ClienteBasicoHTTP.ejecutarPeticion(txt_url.Text, cbx_metodo.Text);
                    HttpStatusCode statusCode = response.StatusCode;
                    this.lbl_statuscode.Content = "" + (int)statusCode +
                        " - " + statusCode.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                    this.txt_headers.Text += "***** Generales y de respuesta: *****\r\n\r\n";
                    this.txt_headers.Text += response.Headers.ToString();
                    this.txt_headers.Text += "\r\n";
                    this.txt_headers.Text += "***** De entidad: *****\r\n\r\n";
                    this.txt_headers.Text += response.Content.Headers.ToString();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    if ((bool)btnRaw.IsChecked)
                    {
                        this.txt_body.Text = responseBody;
                        this.webBrowser1.NavigateToString(this.txt_body.Text);
                    }
                    else if ((bool)btnPretty.IsChecked) {
                        if (response.Content.Headers.ContentType.MediaType.StartsWith("text/html"))
                        {
                            this.txt_body.Text = responseBody;
                            this.webBrowser1.NavigateToString(this.txt_body.Text);
                        }
                        else if (response.Content.Headers.ContentType.MediaType.StartsWith("image/"))
                        {
                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.StreamSource = new MemoryStream(response.Content.ReadAsByteArrayAsync().Result);
                            image.EndInit();


                            this.txt_body.Background = new ImageBrush(image);
                        }
                        else if (response.Content.Headers.ContentType.MediaType.StartsWith("application/json"))
                        {
                            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
                            this.txt_body.Text = JsonConvert.SerializeObject(jsonResponse, Formatting.Indented); ;
                        }
                        else if (response.Content.Headers.ContentType.MediaType.StartsWith("video/mp4"))
                        {
                            byte[] videoBytes = await response.Content.ReadAsByteArrayAsync();
                            MemoryStream stream = new MemoryStream(videoBytes);
                            MediaElement mediaElement = new MediaElement();
                            mediaElement.Source = new Uri(stream.ToString());
                            gridChido.Children.Add(mediaElement);
                            mediaElement.Play();

                            this.txt_body.Text = "No puedo reproducir videos, no se hacerlo, me enseñas 😖";
                        }
                        else
                        {
                            this.txt_body.Text = "Formato no soportado";
                        }
                    }

                    string extension = MimeTypesMap.GetExtension(response.Content.Headers.ContentType.MediaType);
                    lbType.Content = "Tipo: " + extension;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Red);
                    this.lbl_statuscode.Content = ex.Message;
                }
            }
            else
            {
                MessageBox.Show("Debes ingresar la URL a consultar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JSON files (*.json)|*.json|HTML files (*.html)|*.html|Image files (*.png;*.jpeg;*.gif)|*.png;*.jpeg;*.gif|Video files (*.mp4;*.avi)|*.mp4;*.avi";

            if (saveDialog.ShowDialog() != true)
            {
                return;
            }

            byte[] content = await response.Content.ReadAsByteArrayAsync();

            File.WriteAllBytes(saveDialog.FileName, content);
        }

    }
}
