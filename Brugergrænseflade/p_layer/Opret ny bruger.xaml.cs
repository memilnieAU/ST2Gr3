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
    /// Interaction logic for Opret_ny_bruger.xaml
    /// </summary>
    public partial class Opret_ny_bruger : Window
    {
        private LoginRequest logicObj;

        public Opret_ny_bruger()
        {
            InitializeComponent();
        }

        private void OpretNyBrugerB_Click(object sender, RoutedEventArgs e)
        {
            logicObj = new LoginRequest();
            if (MedarbejderIDTB.Text.Length != 4)
            {
                MessageBox.Show("MedarbejderID skal være fire karaktere");
            }
            else if (PasswordPW.Password.Length != 4)
            {
                MessageBox.Show("Password skal være fire karaktere");
            }
            else if (PasswordPW.Password != gentagPasswordPW.Password)
                {
                    MessageBox.Show("De to password skal være ens");
                }
            
            else if (MedarbejderIDTB.Text.Length == 4 && PasswordPW.Password.Length ==4 && PasswordPW.Password==gentagPasswordPW.Password)
            {
                logicObj.registerNewUser(MedarbejderIDTB.Text, PasswordPW.Password);
                loginView lWRef = new loginView();
                lWRef.Show();
                this.Close();
            }
            
           
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void AlleredeOprettetB_Click(object sender, RoutedEventArgs e)
        {
            loginView lWRef = new loginView();
            lWRef.Show();
            this.Close();
        }
    }
}
