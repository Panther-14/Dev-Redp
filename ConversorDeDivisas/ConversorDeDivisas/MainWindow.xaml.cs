using ConversorDeDivisas.Model.api;
using ConversorDeDivisas.Model.obj;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            CargarComponentes();
        }

        private async void CargarComponentes()
        {
            RespuestaService res = await DivisasServices.getMonedasConversion();
            
            cb_D.ItemsSource = res.Monedas;
            cb_A.ItemsSource = res.Monedas;
        }
        
        private async void btn_convertir_Click(object sender, RoutedEventArgs e)
        {
            Double tasamxn = 0.0;
            Double importe;
            if (!string.IsNullOrEmpty(this.txt_importe.Text))
            {
                importe = Double.Parse(this.txt_importe.Text);
                RespuestaService res = await DivisasServices.getTasasConversion();
                if (!res.Error)
                {
                    if (res.Tasas != null && res.Tasas.Rates != null &&
                    res.Tasas.Rates.TryGetValue("MXN", out tasamxn))
                    {
                        //-----------RESULTADO CONVERSION------------//
                        if (rdb_mxn_usd.IsChecked == true)
                        {
                            //-----------MXN A USD-------------------//
                            this.lbl_resultado.Content = String.Format("{0:#,##0.00}",
                            importe / tasamxn);
                        }
                        else
                        {
                            //-----------USD A MXN-------------------//
                            this.lbl_resultado.Content = String.Format("{0:#,##0.00}",
                            importe * tasamxn);
                        }
                        //-----------FECHA ACTUALIZACION-------------//
                        long tiempoactualizacion = res.Tasas.Timestamp;
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                        this.lbl_fecha.Content = new DateTime(1970, 1, 1, 0, 0, 0,
                        DateTimeKind.Utc)
                        .AddSeconds(tiempoactualizacion).ToString("dd/MM/yyyy HH:mm:ss");
                        //-------------TASA CONVERSION---------------//
                        this.lbl_tasa.Content = String.Format("{0:#,##0.0000}", tasamxn);
                    }
                    else
                    {
                        MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Introduce un importe válido para hacer la conversión",
                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void btn_convertir_Click2(object sender, RoutedEventArgs e)
        {
            /*
             * <ComboBox.ItemTemplate>
                            <DataTemplate>
                                    <TextBlock Text="{Binding Nombre}"/>
                            </DataTemplate>
                        </ComboBox.ItemTemplate>
             */
            Double fromTasa = 0.0;
            Double toTasa = 0.0;
            Double importe;
            if (!string.IsNullOrEmpty(this.txt_importe2.Text))
            {
                importe = Double.Parse(this.txt_importe2.Text);
                RespuestaService res = await DivisasServices.getTasasConversion();
                if (!res.Error)
                {
                    Moneda fromMoneda = (Moneda)cb_D.SelectedItem;
                    Moneda toMoneda = (Moneda)cb_A.SelectedItem;
                    if (res.Tasas != null && res.Tasas.Rates != null &&
                    res.Tasas.Rates.TryGetValue(fromMoneda.Codigo, out fromTasa) && res.Tasas.Rates.TryGetValue(toMoneda.Codigo, out toTasa))
                    {
                        //-----------RESULTADO CONVERSION------------//
                        double rate = toTasa/ fromTasa;
                        double convertedAmount = importe * rate;
                        this.lbl_resultado2.Content = convertedAmount;
                        //-----------FECHA ACTUALIZACION-------------//
                        long tiempoactualizacion = res.Tasas.Timestamp;
                        DateTimeOffset.Now.ToUnixTimeSeconds();
                        this.lbl_fecha2.Content = new DateTime(1970, 1, 1, 0, 0, 0,
                        DateTimeKind.Utc)
                        .AddSeconds(tiempoactualizacion).ToString("dd/MM/yyyy HH:mm:ss");
                        //-------------TASA CONVERSION---------------//
                        this.lbl_tasa2.Content = String.Format("{0:#,##0.0000}", fromTasa);
                    }
                    else
                    {
                        MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(res.Mensaje, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Introduce un importe válido para hacer la conversión",
                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int auxA = cb_A.SelectedIndex;
            int auxD = cb_D.SelectedIndex;

            cb_A.SelectedIndex = auxD;
            cb_D.SelectedIndex = auxA;
        }
    }
}
