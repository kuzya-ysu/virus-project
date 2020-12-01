using System.Windows;

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