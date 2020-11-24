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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ClassLibrary;

namespace VirusApp
{
    public partial class ExitConfirmation : Window
    {
        Country Country;
        string fileName;
        Simulation simulation;

        public ExitConfirmation(Country country, Simulation simulation)
        {
            Country = country;
            fileName = Country.Name + " " + Country.DiseaseName +  " Неделя " + Country.Week.ToString() + ".dat";
            this.simulation = simulation;
            InitializeComponent();
        }

        static void SaveAsBinaryFormat(object objGraph, string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();

            using (Stream fStream = new FileStream(fileName,
                  FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, objGraph);
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
