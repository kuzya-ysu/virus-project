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
using System.IO;
using ClassLibrary;

namespace VirusApp
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            CountrySettings CountrySettings = new CountrySettings();
            CountrySettings.Show();
            Close();
        }

        private void InfButton_Click(object sender, RoutedEventArgs e)
        {
            Inform inform = new Inform();
            inform.Show();
            Close();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            var fileChoice = new FileChoice();
            fileChoice.Show();
            Close();
        }
    }
}
