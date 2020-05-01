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


            //Henter data fra Den lokaleDB 
            hentNyeMålinger = new HentNyeMålingerFraLocalDB();
            
            AnalyserData(7/*Her kan man skrive det id som man gerne vil bruge*/);
            DummyTilføjPunkterTilGraf();
            

            cpreks = new List<cprEksempel>();
            cpreks.Add(new cprEksempel("210397-1554", 1));
            cpreks.Add(new cprEksempel("345678-1554", 2));

            CprLB.Items.Add(cpreks[0]);
            CprLB.Items.Add(cpreks[1]);
            //CurrentcprEksempel = cpreks[0];
        }


        #region DummyOpstartAnalyse


        
        double[] råMåling;
        EKG_Analyser ekg_Analyse;
        /// <summary>
        /// Denne metode vil analysere en given måling 
        /// </summary>
        /// <param name="id"></param>
        private void AnalyserData(int id)
        {
            råMåling = hentNyeMålinger.Hent1Måling(id).raa_data;
            ekg_Analyse = new EKG_Analyser(råMåling);

        }
        /// <summary>
        /// Denne metode tilføjer alle punkter til grafen
        /// Denne metode er midlertidig
        /// </summary>
        private void DummyTilføjPunkterTilGraf()
        {
            int i = 0;

            foreach (double item in råMåling)
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
    }
}
