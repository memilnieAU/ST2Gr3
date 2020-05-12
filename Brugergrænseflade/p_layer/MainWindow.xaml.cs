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
using MyCommands;
using System.Diagnostics;

namespace p_layer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //i stedet for at hide mainwindow har vi i App.xaml sat StartupUri="loginView.xaml", dette gør at programmet starter i loginView og derefter opretter mainWindow
        public SeriesCollection MyCollectionEkg { get; set; }
        private LineSeries ekgLine;
        private LineSeries testLine;
        private LineSeries Line1;
        List<cprEksempel> cpreks;
        private loginView loginref;
        public string medarbejderID;

        HentNyeMålingerFraLocalDB hentNyeMålinger;
        EKG_Analyser ekg_Analyse;


        public MainWindow(loginView LoginRef, LoginRequest logicRef)
        {
            InitializeComponent();
            //TODO få tilføjet medarbejderID'et her og evt et navn, hvis dette skal med og tilføjes i databasen
            testLine = new LineSeries
            {
                Values = new ChartValues<double> { },
                PointGeometry = null,
                StrokeThickness = 1,
                Fill = Brushes.Transparent

            };
            Line1 = new LineSeries
            {
                Values = new ChartValues<double> { 1, 1, 1, 1 },
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
            MyCollectionEkg = new SeriesCollection();


            //MyCollectionBS.Add(bsLine);
            MyCollectionEkg.Add(testLine);

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
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            antalNyeMåinger = hentNyeMålinger.HentAlleMålingerFraLocalDB();
            NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";
            
        }
        public Func<double, string> Formatter { get; set; }
        

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
                    if (måling.borger_cprnr == cpr && måling.kommentar.Length > 0)
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
                cprTB.Text = ekgMåling.borger_cprnr;
                patientInfoTB.Text = "Måling taget af: " + ekgMåling.id_medarbejder;
                patientInfoTB.Text += "\n" + "Tidspunkt for måling: " + ekgMåling.start_tidspunkt;
                patientInfoTB.Text += "\n";
                //patientInfoTB.Text += "\n";
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
                if (string.IsNullOrEmpty(måling.kommentar) || string.IsNullOrWhiteSpace(måling.kommentar))
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
            //if (string.IsNullOrEmpty(SPKommentar.Text) == false)
            {
                ekgMåling.kommentar = SPKommentar.Text;
                opdaterLocalDB.OpdaterKommentar(ekgMåling);

                TilføjKommentarL.Content = "Kommentar tilføjet";
                SPKommentar.IsReadOnly = true;

            }

            antalNyeMåinger = hentNyeMålinger.HentAlleMålingerFraLocalDB();
            NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";

        }

        #region Hent nye målinger

        ICommand hentNyeMålingerCommand;
        public ICommand HentNyeMålingerCommand
        {
            get { return hentNyeMålingerCommand ?? (hentNyeMålingerCommand = new RelayCommand(HentNyeMålingerHandler, HentNyeMålingerHandlerCanExecute)); }
        }


        public void HentNyeMålingerHandler()
        {
            FindNyPatientTrykket = false;
            CprLB.Items.Clear();
            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            hentNyeMålinger.HentAlleMålingerFraLocalDB();

            foreach (var måling in hentNyeMålinger.nyeMålinger)
            {
                if (string.IsNullOrEmpty(måling.kommentar) || string.IsNullOrWhiteSpace(måling.kommentar))
                {
                    CprLB.Items.Add("Cpr: " + måling.borger_cprnr + " MåleId: " + måling.id_måling);
                }
            }
        }

        Stopwatch sw = new Stopwatch();
        int antalNyeMåinger = 0;
        public bool HentNyeMålingerHandlerCanExecute()
        {
            sw.Start();
            if (sw.ElapsedMilliseconds > 500)
            {
                sw.Stop();
                sw.Reset();
                antalNyeMåinger = hentNyeMålinger.HentAlleMålingerFraLocalDB();
                NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";
            }
            if (antalNyeMåinger > 0)
            {
                return true;
            }
            return false;
        }
        #endregion 
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loginInfoTB.Text = "MedarbejderID: " + medarbejderID;

        }
    }
}
