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

            //UploadTestData();
            //DummyTilføjPunkterTilGraf();
            DownloadTestData();
            DummyOpstartAnalyse();
           // AnalyserData();

            cpreks = new List<cprEksempel>();
            cpreks.Add(new cprEksempel("210397-1554", 1));
            cpreks.Add(new cprEksempel("345678-1554", 2));

            CprLB.Items.Add(cpreks[0]);
            CprLB.Items.Add(cpreks[1]);
            //CurrentcprEksempel = cpreks[0];
        }


        #region DummyOpstartAnalyse

        EKG_HentDummyData eKG_HentDummyData;

        private void DummyOpstartAnalyse()
        {
            //Denne oprrettelse henter automatisk data fra en bestemt fil
            eKG_HentDummyData = new EKG_HentDummyData();
        }
        Dictionary<string, double> råMåling;
        private void DummyTilføjPunkterTilGraf()
        {
            råMåling = eKG_HentDummyData.GetOneSampel(0);
            int i = 0;

            foreach (double item in råMåling.Values)
            {
                testLine.Values.Add(item);
                i++;
                if (i > 1000)
                {
                    break;
                }
            }
            //eKG_HentDummyData.GetOneSampel(0);
        }
        EKG_Analyser ekg_Analyse;
        private void AnalyserData()
        {
            ekg_Analyse = new EKG_Analyser(råMåling);

        }
       private static Local_UploadEKG local_uploadEKG;

        private void UploadTestData()
        {
            local_uploadEKG = new Local_UploadEKG();
            //local_UploadEKG.uploadNewEKG(råMåling.Values.ToArray());
            local_uploadEKG.uploadNewEKG(råMåling.Values.ToArray());
            
        }
        Local_DownloadEKG local_DownloadEKG;
            double[] testmåling;
        private void DownloadTestData()
        {
            local_DownloadEKG = new Local_DownloadEKG();
            testmåling = local_DownloadEKG.hentData(3);
            int i = 0;

            foreach (double item in testmåling)
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
    }
}
