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
using System.Windows.Navigation;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using ClassLibrary;

namespace VirusApp
{
    public partial class CitiesSelection : Window
    {
        public Country Country { get; set; }
        public City City { get; set; }

        public CitiesSelection(Country country)
        {
            InitializeComponent();
            Country = country;
            for (int i = 0; i < Country.CitiesCount; i++)
            {
                var SP = new StackPanel
                {
                    Name = "City" + i.ToString(),
                    Orientation = Orientation.Horizontal,
                    Height = 70
                };
                var CityName = new Label
                {
                    Name = "City" + i.ToString() + "Name",
                    Width = 300,
                    VerticalAlignment = VerticalAlignment.Center
                };
                if (Country.Cities[i].Name == "")
                    CityName.Content = "Название города " + (i + 1).ToString();
                else if (Country.Cities[i].Name.Length > 17)
                    CityName.Content = Country.Cities[i].Name. Substring(0,17) + "...";
                else
                    CityName.Content = Country.Cities[i].Name;
                CityName.Style = this.Resources["LabelStyle"] as Style;
                SP.Children.Add(CityName);
                var EmptySpace = new Label
                {
                    Width = 100
                };
                SP.Children.Add(EmptySpace);
                string template = XamlWriter.Save(EditButtonTemplate.Template);
                StringReader stringReader = new StringReader(template);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                var EditButton = new Button
                {
                    Name = "City" + i.ToString() + "Edit",
                    Width = 60,
                    Height = 60,
                    Template = (ControlTemplate)XamlReader.Load(xmlReader)
                };
                EditButton.Click += CityEdit_Click;
                SP.Children.Add(EditButton);
                Cities.Children.Add(SP);
                if (i != Country.CitiesCount - 1)
                {
                    var BlackSpace = new Label
                    {
                        Style = this.Resources["Label2Style"] as Style
                    };
                    Cities.Children.Add(BlackSpace);
                }
            }
            
        }

        private void CityEdit_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as Button;
            int CityNumber = (int)char.GetNumericValue(Button.Name, 4);
            var CityEdit = new CityEditing(Country, CityNumber);
            CityEdit.Show();
            Close();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CountrySettings CountrySettings = new CountrySettings(Country);
            CountrySettings.Show();
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            bool correct = true;
            for (int i = 0; i < Country.CitiesCount; i++)
            {
                if (string.IsNullOrEmpty(Country.Cities[i].Name))
                {
                    MessageBox.Show("Введите информацию обо всех городах");
                    correct = false;
                    break;
                }
            }
            if (correct)
            {
                Simulation Simulation = new Simulation(Country);
                Simulation.Show();
                Close();
            }
        }
    }
}
