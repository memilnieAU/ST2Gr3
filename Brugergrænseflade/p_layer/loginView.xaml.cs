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
        private Logic logicObj;
        private MainWindow mainWRef;
        public loginView()
        {
            InitializeComponent();
        }
        
        private void LoginB_Click(object sender, RoutedEventArgs e)
        {
            logicObj = new Logic();
                      
                if (logicObj.checkLogin(BrugernavnTB.Text, PasswordPW.Password) == true)
                {

                this.Hide();
                MainWindow mainWRef = new MainWindow (this, logicObj);
                mainWRef.Show();
            }
                else
                {
                    MessageBox.Show("Enten Brugernavn eller password er forkert. Prøv igen");
                }

            
        }

      
    }
}
