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
        private LineSeries Line1;
        List<cprEksempel> cpreks;
        private loginView loginref;
        private hentPatientinformationer hentPinfo;
        public string medarbejderID;
        OpdaterLocalDB opdaterLocalDB;
        private string gammelCpr;

        HentNyeMålingerFraLocalDB hentNyeMålinger;
        EKG_Analyser ekg_Analyse;


        public MainWindow(loginView LoginRef, LoginRequest logicRef)
        {
            InitializeComponent();
            opdaterLocalDB = new OpdaterLocalDB();
            //TODO få tilføjet medarbejderID'et her og evt et navn, hvis dette skal med og tilføjes i databasen
            ekgLine = new LineSeries
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

            MyCollectionEkg = new SeriesCollection();

            MyCollectionEkg.Add(ekgLine);

            Formatter = value => "";
            DataContext = this;



            //Denne henter data fra en lokal fil og sætter det ind i databasen
            UploadNewDataFraLocalFile uploadNewDataFraLocalFile = new UploadNewDataFraLocalFile();

            // De er udkommenteret for at vi ikke overfylder vores database med samme måling 20 gange
           uploadNewDataFraLocalFile.HentDataFraFil(1);
           uploadNewDataFraLocalFile.HentDataFraFil(2);
           uploadNewDataFraLocalFile.HentDataFraFil(16);
           uploadNewDataFraLocalFile.HentDataFraFil(17);
           uploadNewDataFraLocalFile.HentDataFraFil(18);
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();

            antalNyeMåinger = hentNyeMålinger.HentAlleMålingerFraLocalDB();
            NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";
            hentPinfo = new hentPatientinformationer();

        }
        public Func<double, string> Formatter { get; set; }


        #region DummyOpstartAnalyse


        /// <summary>
        /// Denne metode tilføjer alle punkter til grafen
        /// Denne metode er midlertidig
        /// </summary>
        private void DummyTilføjPunkterTilGraf(DTO_EkgMåling ekgMåling)
        {
            ekgLine.Values.Clear();
            int i = 0;


            foreach (double item in ekgMåling.raa_data)
            {
                ekgLine.Values.Add(item);
                i++;
                if (i > 2500)
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
            gammelCpr = cprTB.Text;
            if (CprLB.SelectedIndex != -1)
            {
                if (FindNyPatientTrykket == true)
                {
                    string cpr = (CprLB.SelectedItem.ToString().Substring(5));

                    CprLB.Items.Clear();

                    foreach (int item in hentNyeMålinger.HentMåleIdUdfracpr(cpr))
                    {
                        CprLB.Items.Add("Cpr: " + cpr + " MåleId: " + item);
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
                    MInfoTB.Text = "Måling taget af: " + ekgMåling.id_medarbejder;
                    MInfoTB.Text += "\n" + "Tidspunkt for måling: " + ekgMåling.start_tidspunkt;
                    MInfoTB.Text += "\n";
                    //patientInfoTB.Text += "\n";
                    OpdaterCprB.IsEnabled = true;
                    cprTB.IsReadOnly = false;
                    SletEKGB.IsEnabled = true;


                }
            }


        }

        private void CprLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ekgMåling = null;
            OpdaterCprB.IsEnabled = false;
            cprTB.IsReadOnly = true;
            ekgLine.Values.Clear();
            SPKommentar.Text = "";
            IndiSygdomTB.Text = "";
            TilføjKommentarL.Content = "Tilføj evt kommentar";
            SPKommentar.IsReadOnly = false;
            TilføjKommentarB.IsEnabled = false;
            UploadMålingB.IsEnabled = false;



            if (CprLB.SelectedIndex != -1)
            {
                cprTB.Text = CprLB.SelectedItem.ToString().Substring(5, 11);
                patientInfoTB.Text = hentPinfo.hentPinfo(cprTB.Text);
                cprB.IsEnabled = true;

            }
            else
            { cprB.IsEnabled = false; }
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


            foreach (string cprNR in hentNyeMålinger.HentAlleCprNr())
            {
                CprLB.Items.Add("Cpr: " + cprNR);
            }

        }

        private void TilføjKommentarB_Click(object sender, RoutedEventArgs e)
        {

            //if (string.IsNullOrEmpty(SPKommentar.Text) == false)
            {
                ekgMåling.kommentar = SPKommentar.Text;
                opdaterLocalDB.OpdaterKommentar(ekgMåling);
                TilføjKommentarB.IsEnabled = false;
                TilføjKommentarB.Content = "Kommentar tilføjet";


            }
            UploadMålingB.IsEnabled = true;

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

            foreach (var måling in hentNyeMålinger.HentMåleIdPåNyeMålingerUKommentar())
            {
                CprLB.Items.Add(måling);

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
                antalNyeMåinger = hentNyeMålinger.HentantalAfNyeMålingerUKommentar();
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





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loginInfoTB.Text = "MedarbejderID: " + medarbejderID;

        }

        private void OpdaterCprB_Click(object sender, RoutedEventArgs e)
        {
            if (ekgMåling != null)
            {

                if (cprTB.Text != ekgMåling.borger_cprnr)
                {
                    MessageBoxResult result = (MessageBox.Show("Bekræft ændring af cprnummer", "Bekræft", MessageBoxButton.OKCancel));
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            {
                                ekgMåling.borger_cprnr = cprTB.Text;
                                opdaterLocalDB.OpdaterCpr(ekgMåling);
                                CprLB.SelectedItem = "test";
                                break;
                            }
                        case MessageBoxResult.Cancel:
                            cprTB.Text = gammelCpr;
                            cprTB.Focus();
                            break;
                    }


                }

                patientInfoTB.Text = hentPinfo.hentPinfo(cprTB.Text);
            }
        }
        UploadToOffDb UploadToOffDb = new UploadToOffDb();
        private void UploadMålingB_Click(object sender, RoutedEventArgs e)
        {
            bool uploaded = UploadToOffDb.uploadToOff(ekgMåling);
            if (uploaded == true)
            { MessageBox.Show("Den valgte måling er blevet uploadet til databasen"); }
            else
            { MessageBox.Show("Der gik noget galt da den valgte måling skulle uploades til databasen. Prøv igen eller tjek din database-forbindelse"); }
        }

        private void SletEKGB_Click(object sender, RoutedEventArgs e)
        {
            if (ekgMåling != null)
            {
                MessageBoxResult result = (MessageBox.Show("Bekræft sletning af ekgmåling", "Bekræft", MessageBoxButton.OKCancel));
                switch (result)
                {
                    case MessageBoxResult.OK:
                        {
                            opdaterLocalDB.deleteEKG(ekgMåling);
                            SletEKGB.IsEnabled = false;
                            ekgLine.Values.Clear();
                            break;

                        }
                    case MessageBoxResult.Cancel:
                        break;

                }
            }
        }

        private void SPKommentar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ekgMåling != null)
            {

                if (String.IsNullOrEmpty(SPKommentar.Text))
                {
                    TilføjKommentarB.IsEnabled = false;

                }
                else TilføjKommentarB.IsEnabled = true;

                if (string.IsNullOrEmpty(ekgMåling.kommentar))
                {
                    UploadMålingB.IsEnabled = false;
                }
                else UploadMålingB.IsEnabled = true;
            }
        }
    }
}
