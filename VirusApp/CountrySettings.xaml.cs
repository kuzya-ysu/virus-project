using System.Windows;
using System.Text.RegularExpressions;
using ClassLibrary;

namespace VirusApp
{
    public partial class CountrySettings : Window
    {
        private Country Country;

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
            MonthInput.SelectedIndex = country.Month - 1;
            CitiesCountInput.Value = country.CitiesCount;
            CountryNameInput.Text = country.Name;
            DiseaseNameInput.Text = country.DiseaseName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            bool correct = true;
            string namePattern = @"^[a-zA-Zа-яА-ЯёЁ]+( ?-? ?[a-zA-Zа-яА-ЯёЁ]+)*$";
            string deseasePattern = @"^[a-zA-Zа-яА-ЯёЁ0-9]+( ?-? ?[a-zA-Zа-яА-ЯёЁ0-9]+)*$";
            Regex nameRegex = new Regex(namePattern);
            Regex deseaseRegex = new Regex(deseasePattern);

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
            else if (!nameRegex.IsMatch(CountryNameInput.Text.ToString()))
            {
                MessageBox.Show("Некорректное название страны");
                correct = false;
            }
            else if (!deseaseRegex.IsMatch(DiseaseNameInput.Text.ToString()))
            {
                MessageBox.Show("Некорректное название заболевания");
                correct = false;
            }

            if (correct)
            {
                Country = new Country(CountryNameInput.Text, DiseaseNameInput.Text, (int)PeriodInput.Value, MonthInput.SelectedIndex, int.Parse(MoneyInput.Text), int.Parse(VaccineInput.Text), (int)CitiesCountInput.Value);
                for (int i = 0; i < CitiesCountInput.Value; i++)
                {
                    Country.Cities[i] = new City("", new Population(0, 0, 0, Country.Weeks), 1);
                }
                var CitiesWindow = new CitiesSelection(Country);
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
    }
}