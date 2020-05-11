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
        private LineSeries Line1;
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
                StrokeThickness = 1,
                Fill = Brushes.Transparent

            };
            Line1 = new LineSeries
            {
                Values = new ChartValues<double> {1,1,1,1 },
                PointGeometry = null,
                StrokeThickness = 1

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
            
            Formatter = value => "";
            DataContext = this;



            //Denne henter data fra en lokal fil og sætter det ind i databasen
            UploadNewDataFraLocalFile uploadNewDataFraLocalFile = new UploadNewDataFraLocalFile();

            // De er udkommenteret for at vi ikke overfylder vores database med samme måling 20 gange
            //uploadNewDataFraLocalFile.HentDataFraFil(1);
            //uploadNewDataFraLocalFile.HentDataFraFil(2);
            //uploadNewDataFraLocalFile.HentDataFraFil(16);
            //uploadNewDataFraLocalFile.HentDataFraFil(17);
            //uploadNewDataFraLocalFile.HentDataFraFil(18);


        }
        public Func<double, string> Formatter { get; set; }
        //public SeriesCollection SeriesCollection1 { get; private set; }

        //private void SetUpChart()
        //{

        //    // Create an empty series collection.
        //    SeriesCollection1 = new SeriesCollection();

        //    // Setup the axis for the first chart
        //    ChartFile.AxisX.Add(new Axis
        //    {
        //        Title = "Time",
        //        Unit = TimeSpan.FromSeconds(1).Seconds,
        //        Separator = new LiveCharts.Wpf.Separator
        //        {
        //            IsEnabled = true
        //        },
        //        DisableAnimations = true

        //    });
        //}


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
                if (i > 1000)
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
            if (FindNyPatientTrykket == true)
            {
                string cpr = (CprLB.SelectedItem.ToString().Substring(5));

                CprLB.Items.Clear();
                //Henter data fra Den lokaleDB 
                hentNyeMålinger = new HentNyeMålingerFraLocalDB();
                hentNyeMålinger.HentAlleMålingerFraLocalDB();

                foreach (var måling in hentNyeMålinger.nyeMålinger)
                {
                    if (måling.borger_cprnr == cpr && måling.kommentar != null)
                    {
                        CprLB.Items.Add("Cpr: " + måling.borger_cprnr + " MåleId: " + måling.id_måling);
                    }
                }

                FindNyPatientTrykket = false;
            }
            else
            {
                ekgMåling = hentNyeMålinger.Hent1Måling(Convert.ToInt32(CprLB.SelectedItem.ToString().Substring(CprLB.SelectedItem.ToString().Length - 2)));
                EKG_Analyser analyserEnMåling = new EKG_Analyser();

                IndiSygdomTB.Text = analyserEnMåling.AnalyserEnMåling(ekgMåling);
                DummyTilføjPunkterTilGraf(ekgMåling);
                
                SPKommentar.Text = ekgMåling.kommentar;
            }

        }

        private void CprLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            testLine.Values.Clear();
            SPKommentar.Text = "";
            IndiSygdomTB.Text = "";
            TilføjKommentarL.Content = "Tilføj evt kommentar";
            SPKommentar.IsReadOnly = false;
        }


        private void CheckLocalDbB_Click(object sender, RoutedEventArgs e)
        {
            FindNyPatientTrykket = false;
            CprLB.Items.Clear();
            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            hentNyeMålinger.HentAlleMålingerFraLocalDB();

            foreach (var måling in hentNyeMålinger.nyeMålinger)
            {
                if (måling.kommentar == null)
                {
                    CprLB.Items.Add("Cpr: " + måling.borger_cprnr + " MåleId: " + måling.id_måling);
                }
            }
        }

        private bool FindNyPatientTrykket = false;
        /// <summary>
        /// Denne metode lister alle cprnummer, som fremgår i den lokale database, som har en kommentar tilknytttet 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientRegisterB_Click(object sender, RoutedEventArgs e)
        {
            FindNyPatientTrykket = true;
            CprLB.Items.Clear();
            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            hentNyeMålinger.HentAlleMålingerFraLocalDB();

            foreach (var måling in hentNyeMålinger.nyeMålinger)
            {
                //TODO Test linje som kan tilføje en kommentar til målingens kommentar
                //måling.kommentar = "Huske at slette denne linje kode";
                if (måling.kommentar != null)
                {
                    if (CprLB.Items.Contains("Cpr: " + måling.borger_cprnr) == false)
                    {
                        CprLB.Items.Add("Cpr: " + måling.borger_cprnr);
                    }
                }
            }
        }

        OpdaterLocalDB opdaterLocalDB;
        private void TilføjKommentarB_Click(object sender, RoutedEventArgs e)
        {
            opdaterLocalDB = new OpdaterLocalDB();
            if (SPKommentar.Text != "")
            {
                ekgMåling.kommentar = SPKommentar.Text;
                opdaterLocalDB.OpdaterKommentar(ekgMåling);

                TilføjKommentarL.Content = "Kommentar tilføjet";
                SPKommentar.IsReadOnly = true;
                
            }
            

        }
    }
}
