using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EjemploHTTP
{
    /// <summary>
    /// Interaction logic for ClienteHTTP_WPF.xaml
    /// </summary>
    public partial class ClienteHTTP_WPF : Window
    {
        public ClienteHTTP_WPF()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txt_url.Text)) 
            {
                this.bdr_indicador.BorderBrush = null;
                this.txt_headers.Text = "";
                this.txt_body.Text= "";
                this.lbl_statuscode.Content = "";
                try
                {
                    if(!txt_url.Text.ToLower().StartsWith("http") && 
                        !txt_url.Text.ToLower().StartsWith("https"))
                    {
                        txt_url.Text = "https://" + txt_url.Text;
                    }

                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Blue);
                    this.lbl_statuscode.Content = "Procesando peticion";

                    HttpResponseMessage response =
                        await Model.ClienteBasicoHTTP.ejecutarPeticion(txt_url.Text, cbx_metodo.Text);
                    HttpStatusCode statusCode = response.StatusCode;
                    this.lbl_statuscode.Content = "" + (int)statusCode +
                        " - " + statusCode.ToString();
                    if(response.IsSuccessStatusCode)
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
                    this.txt_body.Text = await response.Content.ReadAsStringAsync();

                    this.webBrowser1.NavigateToString(this.txt_body.Text);
                }
                catch(Exception ex) 
                {
                    this.bdr_indicador.BorderBrush = new SolidColorBrush(Colors.Red);
                    this.lbl_statuscode.Content = ex.Message;
                }
            }
            else
            {
                MessageBox.Show("Debes ingresar la URL a consultar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
