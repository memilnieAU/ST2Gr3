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
using LiveCharts;
using LiveCharts.Wpf;
using l_layer;
using DTOs;

namespace p_layer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //i stedet for at hide mainwindow har vi i App.xaml sat StartupUri="loginView.xaml", dette gør at programmet starter i loginView og derefter opretter mainWindow
        public SeriesCollection MyCollectionBS { get; set; }
        private LineSeries bsLine;
        private LineSeries testLine;
        List<cprEksempel> cpreks;


        HentNyeMålingerFraLocalDB hentNyeMålinger;
        EKG_Analyser ekg_Analyse;


        public MainWindow(loginView LoginRef, Logic1 logicRef)
        {
            InitializeComponent();
            testLine = new LineSeries
            {
                Values = new ChartValues<double> { },
                PointGeometry = null,
                StrokeThickness = 0.2
            };


            /* bsLine = new LineSeries
             {
                 Values = new ChartValues<double> { },
                 PointGeometry = null,
                 StrokeThickness = 0.2
             };
             */
            MyCollectionBS = new SeriesCollection();
            //MyCollectionBS.Add(bsLine);
            MyCollectionBS.Add(testLine);
            DataContext = this;


            
            //Denne henter data fra en lokal fil og sætter det ind i databasen
            UploadNewDataFraLocalFile uploadNewDataFraLocalFile = new UploadNewDataFraLocalFile();

            //uploadNewDataFraLocalFile.HentDataFraFil(1);
            //uploadNewDataFraLocalFile.HentDataFraFil(2);
            //uploadNewDataFraLocalFile.HentDataFraFil(16);
            //uploadNewDataFraLocalFile.HentDataFraFil(17);
            //uploadNewDataFraLocalFile.HentDataFraFil(18);
            

            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            //hentNyeMålinger.HentEnMålingFraLocalDB(16);
            hentNyeMålinger.HentAlleMålingerFraLocalDB();

            //ekg_Analyse = new EKG_Analyser();

            //DTOs.DTO_EkgMåling ekgMåling = hentNyeMålinger.Hent1Måling(uploadNewDataFraLocalFile.sidsteMålingUpladede);
            
            //IndiSygdomTB.Text = ekg_Analyse.AnalyserEnMåling(ekgMåling);
            //DummyTilføjPunkterTilGraf(ekgMåling);
            
            foreach (var item in hentNyeMålinger.AllSampels)
            {
                CprLB.Items.Add("cpr:" +  item.borger_cprnr +" MåleId: " + item.id_måling);
            }

            cpreks = new List<cprEksempel>();
            cpreks.Add(new cprEksempel("210397-1554", 1));
            cpreks.Add(new cprEksempel("345678-1554", 2));

            CprLB.Items.Add(cpreks[0].CPR);
            CprLB.Items.Add(cpreks[1].CPR);
            //CurrentcprEksempel = cpreks[0];
        }


        #region DummyOpstartAnalyse

       
        /// <summary>
        /// Denne metode tilføjer alle punkter til grafen
        /// Denne metode er midlertidig
        /// </summary>
        private void DummyTilføjPunkterTilGraf(DTO_EkgMåling ekgMåling)
        {
            testLine.Values.Clear();
            int i = 0;

            foreach (double item in ekgMåling.raa_data)
            {
                testLine.Values.Add(item);
                i++;
                if (i > 1500)
                {
                    break;
                }
            }

        }


        #endregion

        private void LogAfB_Click(object sender, RoutedEventArgs e)
        {
            //denne kode genstarter programmet, jeg leder lige efter en smartere måde, men det her gør sådanset hvad der skal ske
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }


        DTO_EkgMåling ekgMåling;
        private void CprB_Click(object sender, RoutedEventArgs e)
        {
            DummyTilføjPunkterTilGraf(ekgMåling);
            
        }

        private void CprLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ekgMåling = hentNyeMålinger.Hent1Måling(Convert.ToInt32(CprLB.SelectedItem.ToString().Substring(CprLB.SelectedItem.ToString().Length - 2)));
            EKG_Analyser analyserEnMåling = new EKG_Analyser();

            IndiSygdomTB.Text = analyserEnMåling.AnalyserEnMåling(ekgMåling);

        }
    }
}
