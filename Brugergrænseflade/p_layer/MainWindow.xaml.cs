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
            MyCollectionBS = new SeriesCollection();

            MyCollectionBS.Add(bsLine);
            DataContext = this;

           //DummyOpstartAnalyse();
           //DummyTilføjPunkterTilGraf();
        }


        #region DummyOpstartAnalyse

        EKG_HentDummyData eKG_HentDummyData;

        private void DummyOpstartAnalyse()
        {
            eKG_HentDummyData = new EKG_HentDummyData();
        }

        private void DummyTilføjPunkterTilGraf()
        {
            Dictionary<string, double> måling = eKG_HentDummyData.GetOneSampel(0);
            foreach (double item in måling.Values)
            {
                bsLine.Values.Add(item);
            }
           //eKG_HentDummyData.GetOneSampel(0);
        }

        #endregion
    }
}
