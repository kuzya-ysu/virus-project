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
using ClassLibrary;

namespace VirusApp
{
    public partial class CountrySettings : Window
    {
        Country country;
        public CountrySettings()
        {
            InitializeComponent();
        }

        public CountrySettings(Country country)
        {
            InitializeComponent();
            MoneyInput.Text = country.Fund.ToString();
            VaccineInput.Text = country.Cost.ToString();
            PeriodInput.Value = country.Months;
            MonthInput.SelectedIndex = country.Month-1;
            CitiesCountInput.Value = country.CitiesCount;
            CountryNameInput.Text = country.Name;
            DiseaseNameInput.Text = country.DiseaseName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            bool correct = true;

            if (string.IsNullOrEmpty(CountryNameInput.Text.ToString()) || string.IsNullOrEmpty(DiseaseNameInput.Text.ToString()))
            {
                MessageBox.Show("Введите название");
                correct = false;
            }
            else if (MonthInput.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите месяц");
                correct = false;
            }
            else if (!int.TryParse(MoneyInput.Text, out int result) || int.Parse(MoneyInput.Text) <= 0)
            {
                MessageBox.Show("Размер денежного фонда должен быть целым положительным числом");
                MoneyInput.Text = "0";
                correct = false;
            }
            else if (!int.TryParse(VaccineInput.Text, out int result2) || int.Parse(VaccineInput.Text) <= 0)
            {
                MessageBox.Show("Стоимость прививки должна быть целым положительным числом");
                VaccineInput.Text = "0";
                correct = false;
            }
            
            
            if (correct) 
            {
                country = new Country(CountryNameInput.Text, DiseaseNameInput.Text, (int)PeriodInput.Value, MonthInput.SelectedIndex, int.Parse(MoneyInput.Text), int.Parse(VaccineInput.Text), (int)CitiesCountInput.Value);
                var city = new City("", new Population(0, 0, 0, country.Weeks), 1);
                for (int i = 0; i < CitiesCountInput.Value; i++)
                {
                    country.Cities[i] = new City("", new Population(0, 0, 0, country.Weeks), 1);
                }
                var CitiesWindow = new CitiesSelection(country);
                CitiesWindow.Show();
                Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }

        private void MonthInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
