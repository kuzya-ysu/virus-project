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
using ClassLibrary;

namespace VirusApp
{
    public partial class Vaccination : Window
    {
        public Country Country { get; set; }
        public City City { get; set; }
        public int[] Changes;

        public Vaccination(Country country, int[] changes)
        {
            InitializeComponent();
            Country = country;
            Fund.Content = Country.Fund;
            Changes = changes;

            for (int i = 0; i < Country.CitiesCount; i++)
            {
                var SP = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Height = 70
                };
                var CityName = new Label
                {
                    Width = 260,
                    VerticalAlignment = VerticalAlignment.Center
                };
                if (Country.Cities[i].Name.Length > 17)
                    CityName.Content = Country.Cities[i].Name.Substring(0, 17) + "...";
                else
                    CityName.Content = Country.Cities[i].Name;
                CityName.Style = this.Resources["LabelStyle"] as Style;
                SP.Children.Add(CityName);
                var EmptySpace = new Label
                {
                    Width = 70
                };
                SP.Children.Add(EmptySpace);
                var Vaccinating = new Xceed.Wpf.Toolkit.IntegerUpDown
                {
                    Style = this.Resources["UpDownStyle"] as Style,
                    Name = "City" + (i).ToString() + "Vaccinated",
                    //Maximum = Country.Cities[i].People.Total - Country.Cities[i].People.Immune - Country.Cities[i].People.Ill,
                    Value = Changes[i]

                };
                Vaccinating.ValueChanged += Vaccinating_ValueChanged;
                SP.Children.Add(Vaccinating);
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

        private void Vaccinating_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Xceed.Wpf.Toolkit.IntegerUpDown a = sender as Xceed.Wpf.Toolkit.IntegerUpDown;
            int index = int.Parse(a.Name[4].ToString());
            if (!string.IsNullOrEmpty(a.Text))
            {
                Country.Fund -= (int)a.Value * Country.Cost;
                Country.Fund += Changes[index] * Country.Cost;
                Changes[index] = (int)a.Value;
                Fund.Content = Country.Fund;
            }
        }

        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            bool correct = true;
            if (Country.Fund < 0)
            {
                MessageBox.Show("Не хватает денег");
                correct = false;
            }
            for (int i=0;i<Country.CitiesCount;i++)
            {
                var SP = (StackPanel)Cities.Children[2*i];
                var upDown = (Xceed.Wpf.Toolkit.IntegerUpDown)SP.Children[2];
                var value = upDown.Value;
                if (value > Country.Cities[i].People.Total - Country.Cities[i].People.Immune - Country.Cities[i].People.Ill) 
                {
                    MessageBox.Show("Нельзя сделать больше прививок, чем количество здоровых людей без иммунитета в городе");
                    correct = false;
                    upDown.Value = Country.Cities[i].People.Total - Country.Cities[i].People.Immune - Country.Cities[i].People.Ill;
                    break;
                }
            }
            if(correct)
            {
                for (int i = 0; i < Country.CitiesCount; i++)
                {
                    Country.Cities[i].People.NewVaccinated += Changes[i];
                }
                Close();
            }
        }
    }
}
