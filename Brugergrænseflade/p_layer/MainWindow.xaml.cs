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
        private LineSeries threshold;
        public Func<double, string> Formatter { get; set; }
        private bool FindNyPatientTrykket = false;
        DTO_EkgMåling ekgMåling;
        Stopwatch sw = new Stopwatch();
        int antalNyeMåinger = 0;
        private loginView loginref;
        private HentPatientInfo hentPinfo;
        public string medarbejderID;
        OpdaterLocalDB opdaterLocalDB;
        private string oprindeligCPR;
        UploadToOffDb UploadToOffDb = new UploadToOffDb();
        HentFraLocalDB hentNyeMålinger;
       

        /// <summary>
        /// Constructoren for mainwindow. sørger for at der bliver oprettet en graf
        /// </summary>
        /// <param name="LoginRef">referenceobjekt til Loginview</param>
        /// <param name="logicRef">referenceobjekt til l_layer</param>
        public MainWindow(loginView LoginRef, LoginRequest logicRef)
        {
            InitializeComponent();
            opdaterLocalDB = new OpdaterLocalDB();
           
            ekgLine = new LineSeries
            {
                Values = new ChartValues<double> { },
                PointGeometry = null,
                StrokeThickness = 1,
                Fill = Brushes.Transparent

            };
            threshold = new LineSeries
            {
                Values = new ChartValues<double> { },
                PointGeometry = null,
                StrokeThickness = 1

            };

            MyCollectionEkg = new SeriesCollection();

            MyCollectionEkg.Add(ekgLine);
            MyCollectionEkg.Add(threshold);
            Formatter = value => "";
            DataContext = this;

            //HentDataFraCSVfil();

            hentNyeMålinger = new HentFraLocalDB();

            antalNyeMåinger = hentNyeMålinger.HentantalAfNyeMålingerUKommentar();
            NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";
            hentPinfo = new HentPatientInfo();

        }
       
        /// <summary>
        /// Henter data fra en csv fil, lokal på computeren
        /// </summary>
        void HentDataFraCSVfil()
        {
            //Denne henter data fra en lokal fil og sætter det ind i databasen
            UploadNewDataFraLocalFile uploadNewDataFraLocalFile = new UploadNewDataFraLocalFile();

            // De er udkommenteret for at vi ikke overfylder vores database med samme måling 20 gange
            uploadNewDataFraLocalFile.HentDataFraFil(0);
            uploadNewDataFraLocalFile.HentDataFraFil(1);
            uploadNewDataFraLocalFile.HentDataFraFil(2);
            uploadNewDataFraLocalFile.HentDataFraFil(16);
            uploadNewDataFraLocalFile.HentDataFraFil(17);
            uploadNewDataFraLocalFile.HentDataFraFil(18);
            uploadNewDataFraLocalFile.HentDataFraFil(25);
        }


        /// <summary>
        /// Denne metode tilføjer alle punkter til grafen
        /// </summary>
        /// <param name="ekgMåling">objekt af DTO_ekgmåling - indeholder alle informationer til EKG-målingen</param>
        /// <param name="threshold">indikere at hvis det er over, er det en r-tak</param>
        private void TilføjPunkterTilGraf(DTO_EkgMåling ekgMåling,double threshold)
        {
            ekgLine.Values.Clear();
            if (ekgMåling.raa_data.Length != 0)
            {

                
                double højesteVærdi = ekgMåling.raa_data[0];
                double lavesteVærdi = ekgMåling.raa_data[0];
                this.threshold.Values.Clear();

                for (int i = Convert.ToInt32(ekgMåling.raa_data.Length*0.1); i < ekgMåling.raa_data.Length; i++)
                {

                    this.threshold.Values.Add(threshold);
                    ekgLine.Values.Add(ekgMåling.raa_data[i]);
                   
                    if (ekgMåling.raa_data[i] > højesteVærdi)
                    {
                        højesteVærdi = ekgMåling.raa_data[i];
                    }
                    if (ekgMåling.raa_data[i] < lavesteVærdi)
                    {
                        lavesteVærdi = ekgMåling.raa_data[i];
                    }
                    if (i > ekgMåling.raa_data.Length * 0.1 + 2500)
                    {
                        break;
                    }
                }
                
                højesteVærdi += 0.1;
                lavesteVærdi -= 0.1;
                // ekgLine.MaxHeight = højesteVærdi;
                yakse.MinValue = lavesteVærdi;
                if (højesteVærdi < lavesteVærdi + 2.5)
                {
                    yakse.MaxValue = lavesteVærdi + 2.5;
                }
                else
                    yakse.MaxValue = højesteVærdi;

            }
        }

        /// <summary>
        /// denne knap, genstarter programmet, som får brugeren tilbage til login-vinduet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogAfB_Click(object sender, RoutedEventArgs e)
        {
            //denne kode genstarter programmet, jeg leder lige efter en smartere måde, men det her gør sådanset hvad der skal ske
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// "vælg" knappen- opdatere henholdsvis listboxen med cpr og målinger, og grafen og patientinformationsboxen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CprB_Click(object sender, RoutedEventArgs e)
        {
            oprindeligCPR = cprTB.Text;
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
                    hentNyeMålinger.HentEnMålingFraLocalDB(Convert.ToInt32(CprLB.SelectedItem.ToString().Substring(CprLB.SelectedItem.ToString().Length - 2)));
                    ekgMåling = hentNyeMålinger.Hent1Måling(Convert.ToInt32(CprLB.SelectedItem.ToString().Substring(CprLB.SelectedItem.ToString().Length - 2)));
                    EKG_Analyser analyserEnMåling = new EKG_Analyser();

                    IndiSygdomTB.Text = analyserEnMåling.AnalyserEnMåling(ekgMåling);
                    TilføjPunkterTilGraf(ekgMåling,analyserEnMåling.threshold);

                    SPKommentar.Text = ekgMåling.kommentar;
                    cprTB.Text = ekgMåling.borger_cprnr;
                    MInfoTB.Text = "Måling taget af: " + ekgMåling.id_medarbejder;
                    MInfoTB.Text += "\n" + "Tidspunkt for måling: " + ekgMåling.start_tidspunkt;
                    MInfoTB.Text += "\n" + "Samplerate: " + ekgMåling.samplerate_hz + " Hz";
                    MInfoTB.Text += "\n" + "Baseline: " + analyserEnMåling.baseline.Key + " V";
                    //patientInfoTB.Text += "\n";
                    OpdaterCprB.IsEnabled = true;
                    cprTB.IsReadOnly = false;
                    SletEKGB.IsEnabled = true;


                }
            }


        }

        /// <summary>
        /// hvis brugeren har valgt i listboxen, opdatere den cpr, patientinformation og enabler vælg knappen. ellers unenabler den vælg knappen 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CprLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NulstilGUI();

            if (CprLB.SelectedIndex != -1)
            {
                cprTB.Text = CprLB.SelectedItem.ToString().Substring(5, 11);
                patientInfoTB.Text = hentPinfo.HentPinfo(cprTB.Text);
                cprB.IsEnabled = true;

            }
            else
            { cprB.IsEnabled = false; }
        }



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

        /// <summary>
        /// denne metode knytter en indtastet kommentar til den valgte måling. gør det muligt for brugeren at uploade til den offentlige database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TilføjKommentarB_Click(object sender, RoutedEventArgs e)
        {

            //bool kommentarVarTom = false;
            //if (string.IsNullOrEmpty(ekgMåling.kommentar))
            //{
            //    kommentarVarTom = true;
            //}
            string opdateText = "Kommentar ændret: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            ekgMåling.kommentar = SPKommentar.Text + "\n" + opdateText + "\n\n"; ;
            opdaterLocalDB.OpdaterKommentar(ekgMåling);
            SPKommentar.Text = ekgMåling.kommentar;
            TilføjKommentarB.IsEnabled = false;
            TilføjKommentarB.Content = "Kommentar tilføjet";
            UploadMålingB.IsEnabled = true;

            //if (kommentarVarTom)
            //{
            //    HentNyeMålingerHandler();

            //}


            antalNyeMåinger = hentNyeMålinger.HentAlleMålingerFraLocalDB();
            NyeMålingerTBL.Text = "Der er " + antalNyeMåinger + " nye målinger";

        }

        #region Hent nye målinger
        ICommand hentNyeMålingerCommand;
        /// <summary>
        /// klikevent fra hent nye målinger, en anden måde at køre den normale click_event
        /// </summary>
        public ICommand HentNyeMålingerCommand
        {
            get { return hentNyeMålingerCommand ?? (hentNyeMålingerCommand = new RelayCommand(HentNyeMålingerHandler, HentNyeMålingerHandlerCanExecute)); }
        }

        /// <summary>
        /// henter de nye målinger fra den lokale database
        /// </summary>
        public void HentNyeMålingerHandler()
        {
            FindNyPatientTrykket = false;
            CprLB.Items.Clear();

           
            foreach (var måling in hentNyeMålinger.HentMåleIdPåNyeMålingerUKommentar())
            {
                CprLB.Items.Add(måling);

            }
        }

        /// <summary>
        /// tjekker om der er nye(ukommenterede) målinger i databasen
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// lukker appen, når der trykkes på krydset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// sker når vinduet er loadet. tilføjer medarbejderID'et, der bliver logget ind med i loginoplysninger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loginInfoTB.Text = "MedarbejderID: " + medarbejderID;

        }


        /// <summary>
        /// metoden opdatere CPR nummeret på den givne måling hvis brugeren vælger at bekræfte at opdatere. ellers fokusere den i cpr textboxen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                
                                CprLB.Items.Clear();
                                NulstilGUI();
                                break;
                            }
                        case MessageBoxResult.Cancel:
                            
                            cprTB.Text = oprindeligCPR;
                            cprTB.Focus();
                            break;
                    }


                }

                //patientInfoTB.Text = hentPinfo.hentPinfo(cprTB.Text);
            }
        }

        /// <summary>
        /// kaldes af listboxen bliver ændret,
        ///Ansvar: sætter mainvindue "tilbage til start" alt ser ud som, hvis man lige var logget på systemet
        /// </summary>
        private void NulstilGUI()
        {
            threshold.Values.Clear();
            ekgMåling = null;
            OpdaterCprB.IsEnabled = false;
            SletEKGB.IsEnabled = false;
            cprTB.IsReadOnly = true;
            ekgLine.Values.Clear();
            SPKommentar.Text = "";
            IndiSygdomTB.Text = "";
            TilføjKommentarL.Content = "Tilføj evt kommentar";
            SPKommentar.IsReadOnly = false;
            TilføjKommentarB.IsEnabled = false;
            UploadMålingB.IsEnabled = false;
            MInfoTB.Text = "";
            cprTB.Text = "";
            patientInfoTB.Text = "";

        }
        /// <summary>
        /// kaldes af tryk på upload måling
        ///Ansvar: uploader måling til den offentlige database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadMålingB_Click(object sender, RoutedEventArgs e)
        {
            bool uploaded = UploadToOffDb.UploadToOff(ekgMåling);
            if (uploaded == true)
            {
                MessageBox.Show("Den valgte måling er blevet uploadet til databasen");
                CprLB.Items.Clear();
                NulstilGUI();
            }
            else
            { MessageBox.Show("Der gik noget galt da den valgte måling skulle uploades til databasen. Prøv igen eller tjek din database-forbindelse"); }
        }

        /// <summary>
        /// kaldes af tryk på slet EKG knap
        /// Ansvar: giver brugeren et valg, om vedkommende ønsker at slette den valgte måling.
        /// udkom: sletter i den lokale database, hvis der trykkes ok.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SletEKGB_Click(object sender, RoutedEventArgs e)
        {
            if (ekgMåling != null)
            {
                MessageBoxResult result = (MessageBox.Show("Bekræft sletning af ekgmåling", "Bekræft", MessageBoxButton.OKCancel));
                switch (result)
                {
                    case MessageBoxResult.OK:
                        {
                            opdaterLocalDB.DeleteEKG(ekgMåling);
                            CprLB.Items.Clear();
                            NulstilGUI();
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
        }

        /// <summary>
        /// kaldes hvis der kommer ændringer i textboxen
        /// Ansvar gør knappen tilføj kommentar enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// metoden kaldes når der er ændringer i cpr-textboxen
        /// ansvar: når antallet af karaktere i cprtextboxen er 11, enables opdaterCPR knappen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cprTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cprTB.Text.Length == 11)
            {
                OpdaterCprB.IsEnabled = true;

            }
            else OpdaterCprB.IsEnabled = false;
        }
    }
}
