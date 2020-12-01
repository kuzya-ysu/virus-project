using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ClassLibrary;

namespace VirusApp
{
    public partial class ExitConfirmation : Window
    {
        Country Country { get; set; }
        string fileName;
        Simulation simulation;

        public ExitConfirmation(Country country, Simulation simulation)
        {
            Country = country;
            fileName = Country.Name + " " + Country.DiseaseName +  " Неделя " + Country.Week.ToString() + ".dat";
            this.simulation = simulation;
            InitializeComponent();
        }

        static void SaveAsBinaryFormat(object simulation, string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            using (Stream fStream = new FileStream(fileName,
                  FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, simulation);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
            simulation.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveAsBinaryFormat(Country, fileName);
            Menu menu = new Menu();
            menu.Show();
            Close();
            simulation.Close();
        }
    }
}