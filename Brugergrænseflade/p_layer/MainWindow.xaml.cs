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

            uploadNewDataFraLocalFile.HentDataFraFil();
            uploadNewDataFraLocalFile.UploadDateTilLocalDB();

            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            //hentNyeMålinger.HentEnMålingFraLocalDB(16);
            hentNyeMålinger.HentAlleMålingerFraLocalDB();

            ekg_Analyse = new EKG_Analyser();
            
            ekg_Analyse.AnalyserEnMåling(hentNyeMålinger.Hent1Måling(uploadNewDataFraLocalFile.sidsteMålingUpladede));

            DummyTilføjPunkterTilGraf();
            

            cpreks = new List<cprEksempel>();
            cpreks.Add(new cprEksempel("210397-1554", 1));
            cpreks.Add(new cprEksempel("345678-1554", 2));

            CprLB.Items.Add(cpreks[0]);
            CprLB.Items.Add(cpreks[1]);
            //CurrentcprEksempel = cpreks[0];
        }


        #region DummyOpstartAnalyse

       
        /// <summary>
        /// Denne metode tilføjer alle punkter til grafen
        /// Denne metode er midlertidig
        /// </summary>
        private void DummyTilføjPunkterTilGraf()
        {
            int i = 0;

            foreach (double item in ekg_Analyse.Måling.raa_data)
            {
                testLine.Values.Add(item);
                i++;
                if (i > 2000)
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
    }
}
