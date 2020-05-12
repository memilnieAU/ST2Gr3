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
using System.Windows.Shapes;
using l_layer;

namespace p_layer
{
    /// <summary>
    /// Interaction logic for loginView.xaml
    /// </summary>
    public partial class loginView : Window
    {
        private LoginRequest logicObj;
        //private string MedarbejderID;
        private MainWindow mainWRef;
        //private Opret_ny_bruger onbRef;
        public loginView()
        {
            InitializeComponent();
            
        }

        private void LoginB_Click(object sender, RoutedEventArgs e)
        {
            logicObj = new LoginRequest();

            if (logicObj.checkLogin(MedarbejderIDTB.Text, PasswordPW.Password) == true)
            {
                MainWindow mainWRef = new MainWindow(this, logicObj);
                mainWRef.medarbejderID= MedarbejderIDTB.Text;
                this.Close();
                mainWRef.Show();
            }
            else
            {
                MessageBox.Show("Enten Brugernavn eller password er forkert. Prøv igen\n (BrugerNavn == \"1234\" && pw == \"1234\")");
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void OpretNyBrugerB_Click(object sender, RoutedEventArgs e)
        {
            
            Opret_ny_bruger onbRef = new Opret_ny_bruger();
            onbRef.Show();
            this.Close();
        }
    }
}
