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
        public MainWindow(loginView LoginRef, Logic1 logicRef)
        {
            InitializeComponent();



            bsLine = new LineSeries();
            bsLine.Values = new ChartValues<double> { };
            bsLine.PointGeometry = null;
            bsLine.StrokeThickness = 0.2;
            
            MyCollectionBS = new SeriesCollection();
            MyCollectionBS.Add(bsLine);
            DataContext = this;

            DummyOpstartAnalyse();
            DummyTilføjPunkterTilGraf();
            AnalyserData();
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
                bsLine.Values.Add(item);
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

        #endregion
    }
}
