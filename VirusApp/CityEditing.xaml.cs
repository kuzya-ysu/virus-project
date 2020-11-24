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
    public partial class CityEditing : Window
    {
        public Country Country { get; set; }
        public int Index { get; set; }

        public CityEditing(Country country, int index)
        {
            InitializeComponent();
            Country = country;
            Index = index;
            NameInput.Text = Country.Cities[Index].Name;
            TotalInput.Text = Country.Cities[Index].People.Total.ToString();
            IllInput.Text = Country.Cities[Index].People.Ill.ToString();
            ImmuneInput.Text = Country.Cities[Index].People.Vaccinated.ToString();
            TrafficInput.Value = Country.Cities[Index].Traffic;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            bool correct = true;

            if (String.IsNullOrEmpty(NameInput.Text))
            {
                MessageBox.Show("Введите название");
                correct = false;
            }
            else if (!int.TryParse(TotalInput.Text, out int res) || int.Parse(TotalInput.Text) <= 0)
            {
                MessageBox.Show("Население должно быть целым положительным числом");
                TotalInput.Text = "0";
                correct = false;
            }
            else if (!int.TryParse(IllInput.Text, out int res2) || int.Parse(IllInput.Text) < 0) 
            {
                MessageBox.Show("Количество больных должно быть целым неотрицательным числом");
                IllInput.Text = "0";
                correct = false;
            }
            else if(!int.TryParse(ImmuneInput.Text, out int res3) || int.Parse(ImmuneInput.Text) < 0)
            {
                MessageBox.Show("Количество привитых должно быть целым неотрицательным числом");
                ImmuneInput.Text = "0";
                correct = false;
            }
            else if(int.Parse(IllInput.Text)+int.Parse(ImmuneInput.Text)>int.Parse(TotalInput.Text))
            {
                MessageBox.Show("Количество больных + количество привитых не может быть больше общего населения");
                correct = false;
            }
            if (correct)
            {
                Country.Cities[Index].Name = NameInput.Text;
                Country.Cities[Index].People.Total = int.Parse(TotalInput.Text);
                Country.Cities[Index].People.Ill = int.Parse(IllInput.Text);
                Country.Cities[Index].People.ImmuneSchedule[2] = (int)Math.Round(Country.Cities[Index].People.Ill * 0.25, MidpointRounding.AwayFromZero);
                Country.Cities[Index].People.ImmuneSchedule[3] = (int)Math.Round(Country.Cities[Index].People.Ill * 0.65);
                Country.Cities[Index].People.ImmuneSchedule[4] = Country.Cities[Index].People.Ill - (int)Math.Round(Country.Cities[Index].People.Ill * 0.25, MidpointRounding.AwayFromZero) - (int)Math.Round(Country.Cities[Index].People.Ill * 0.65);
                Country.Cities[Index].People.Vaccinated = int.Parse(ImmuneInput.Text);
                Country.Cities[Index].People.Immune = int.Parse(ImmuneInput.Text);
                Country.Cities[Index].Traffic = (int)TrafficInput.Value;
                var CitiesWindow = new CitiesSelection(Country);
                CitiesWindow.Show();
                Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var CitiesWindow = new CitiesSelection(Country);
            CitiesWindow.Show();
            Close();
        }
    }
}
