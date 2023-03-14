using ConversorDeDivisas.Model.api;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ConversorDeDivisas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Double tasasmxn = 0.0;
            Double importe;
            if (!string.IsNullOrEmpty(this.TextboxImporte.Text))
            {
                importe = Double.Parse(this.TextboxImporte.Text);
                RespuestaService respuesta = await DivisaServices.GetTasasConversion();
                if (!respuesta.Error)
                {
                    if (respuesta.Tasas != null && respuesta.Tasas.Rates != null
                        && respuesta.Tasas.Rates.TryGetValue("MXN", out tasasmxn))
                    {
                        if ((bool)RadioButtonPesos.IsChecked)
                        {
                            this.TextboxResultado.Text = String.Format("{0;#,##0.00}", importe, tasasmxn);
                        }
                        else
                        {
                            this.TextboxResultado.Text = String.Format("{0:#,##0.00}", tasasmxn * importe);
                        }
                        long tiempoActualizacion = respuesta.Tasas.Timestamp;


                    }
                }
            }
        }
    }
}
