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
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using ClassLibrary;

namespace VirusApp
{ 
    public partial class Simulation : Window
    {

        public Country Country { get; set; }
        public Dictionary<int, string> Months { get; set; }
        public string fileName;
        public int[] Changes { get; set; }
        
        public SolidColorBrush greenBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1ac8b6"));
        public SolidColorBrush redBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0036"));
        public SolidColorBrush purpleBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b8b4ed"));
        public SolidColorBrush darkBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252061"));

        public Simulation(Country country)
        {
            InitializeComponent();
            Country = country;
            Changes = new int[Country.CitiesCount];
            Binding FundBind = new Binding
            {
                Source = Country,
                Path = new PropertyPath("Fund"),
                Mode = BindingMode.OneWay
            };

            Binding NewFundBind = new Binding
            {
                Source = Country,
                Path = new PropertyPath("NewFund"),
                Mode = BindingMode.OneWay
            };

            if (Country.Name.Length > 17)
                CountryName.Content = Country.Name.Substring(0, 14) + "...";
            else
                CountryName.Content = Country.Name;
            if (Country.DiseaseName.Length > 22)
                DiseaseName.Content = Country.DiseaseName.Substring(0, 19) + "...";
            else
                DiseaseName.Content = Country.DiseaseName;

            FundInfo.SetBinding(TextBlock.TextProperty, FundBind);
            NewFundInfo.SetBinding(TextBlock.TextProperty, NewFundBind);
            CostInfo.Text = Country.Cost.ToString();

            Binding WeekBind = new Binding
            {
                Source = Country,
                Path = new PropertyPath("Week"),
                Mode = BindingMode.OneWay
            };

            Week.SetBinding(Label.ContentProperty, WeekBind);

            Months = new Dictionary<int, string> { { 0, "Январь" }, { 1, "Февраль" }, { 2, "Март" }, { 3, "Апрель" }, { 4, "Май" }, { 5, "Июнь" }, { 6, "Июль" }, { 7, "Август" }, { 8, "Сентябрь" }, { 9, "Октябрь" }, { 10, "Ноябрь" }, { 11, "Декабрь" } };
            
            Month.Content = "(" + Months[Country.Month] + ")";
            int width = 1260 / Country.CitiesCount;
            for (int i = 0; i < Country.CitiesCount; i++)
            {
                var CityStackPanel = new StackPanel
                {
                    Width = width,
                    Orientation = Orientation.Horizontal
                };


                Rectangle rectanglePurple = new Rectangle
                {
                    Width = width * 0.4,
                    Height = 500,
                    Fill = purpleBrush,
                    Stroke = darkBrush
                };
                Canvas.SetLeft(rectanglePurple, width*(0.3+i));
                Canvas.SetTop(rectanglePurple, 95);
                MainCanvas.Children.Add(rectanglePurple);

                Rectangle rectangleRed = new Rectangle
                {
                    Width = width * 0.4 - 2,
                    Height = Math.Ceiling((double)Country.Cities[i].People.Ill / Country.Cities[i].People.Total * 500),
                    Fill = redBrush
                };
                Canvas.SetLeft(rectangleRed, width * (0.3 + i) + 1);
                Canvas.SetTop(rectangleRed, 95 + 500 - rectangleRed.Height);
                MainCanvas.Children.Add(rectangleRed);

                Rectangle rectangleGreen = new Rectangle
                {
                    Width = width * 0.4 - 2,
                    Height = (double)Country.Cities[i].People.Immune / Country.Cities[i].People.Total * 500,
                    Fill = greenBrush
                };
                Canvas.SetLeft(rectangleGreen, width * (0.3 + i) + 1);
                Canvas.SetTop(rectangleGreen, 95 + 500 - rectangleRed.Height - rectangleGreen.Height);
                MainCanvas.Children.Add(rectangleGreen);

                Cities.Children.Add(CityStackPanel);
                var CityName = new TextBlock
                {
                    Width = width - 45,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                if (Country.Cities[i].Name.Length > 13)
                    CityName.Text = Country.Cities[i].Name.Substring(0, 10) + "...";
                else
                    CityName.Text = Country.Cities[i].Name;
                var EmptySpace = new Label
                {
                    Width = 5
                };
                CityStackPanel.Children.Add(CityName);
                CityStackPanel.Children.Add(EmptySpace);
                string template = XamlWriter.Save(CountryInfo.Template);
                StringReader stringReader = new StringReader(template);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                var infButton = new Button
                {
                    Template = (ControlTemplate)XamlReader.Load(xmlReader),
                    Height = 20,
                    Width = 20
                };

                var ToolTipContent = new Grid();
                ToolTipContent.RowDefinitions.Add(new RowDefinition());
                ToolTipContent.RowDefinitions.Add(new RowDefinition());
                ToolTipContent.RowDefinitions.Add(new RowDefinition());
                ToolTipContent.RowDefinitions.Add(new RowDefinition());
                ToolTipContent.RowDefinitions.Add(new RowDefinition());
                ToolTipContent.ColumnDefinitions.Add(new ColumnDefinition());
                #region
                var row0 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                row0.SetValue(Grid.RowProperty, 0);
                row0.SetValue(Grid.ColumnProperty, 0);
                var Total = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = "Население: " + Country.Cities[i].People.Total.ToString()
                };
                row0.Children.Add(Total);
                ToolTipContent.Children.Add(row0);
                #endregion
                #region
                var row1 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                row1.SetValue(Grid.RowProperty, 1);
                row1.SetValue(Grid.ColumnProperty, 0);
                var Ill = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = "Больные: "
                };
                row1.Children.Add(Ill);
                Binding IllBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("Ill"),
                    Mode = BindingMode.OneWay
                };
                var part10 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part10.SetBinding(TextBlock.TextProperty, IllBind);
                //IllCity.SetBinding(TextBlock.TextProperty, IllBind);
                row1.Children.Add(part10);
                var part11 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = " (+"
                };
                row1.Children.Add(part11);
                Binding NewIllBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("NewIll"),
                    Mode = BindingMode.OneWay
                };
                var part12 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part12.SetBinding(TextBlock.TextProperty, NewIllBind);
                //NewIllCity.SetBinding(TextBlock.TextProperty, NewIllBind);
                row1.Children.Add(part12);
                var part13 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = ")"
                };
                row1.Children.Add(part13);
                ToolTipContent.Children.Add(row1);
                #endregion
                #region
                var row2 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                row2.SetValue(Grid.RowProperty, 2);
                row2.SetValue(Grid.ColumnProperty, 0);
                var Recovered = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = "Выздоровевшие: "
                };
                row2.Children.Add(Recovered);
                Binding RecoveredBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("Recovered"),
                    Mode = BindingMode.OneWay
                };
                var part20 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part20.SetBinding(TextBlock.TextProperty, RecoveredBind);
                row2.Children.Add(part20);
                var part21 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = " (+"
                };
                row2.Children.Add(part21);
                Binding NewRecoveredBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("NewRecovered"),
                    Mode = BindingMode.OneWay
                };
                var part22 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part22.SetBinding(TextBlock.TextProperty, NewRecoveredBind);
                row2.Children.Add(part22);
                var part23 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = ")"
                };
                row2.Children.Add(part23);
                ToolTipContent.Children.Add(row2);
                #endregion
                #region
                var row3 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                row3.SetValue(Grid.RowProperty, 3);
                row3.SetValue(Grid.ColumnProperty, 0);
                var Vaccinated = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = "Вакцинированные: "
                };
                row3.Children.Add(Vaccinated);
                Binding VaccinatedBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("Vaccinated"),
                    Mode = BindingMode.OneWay
                };
                var part30 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part30.SetBinding(TextBlock.TextProperty, VaccinatedBind);
                row3.Children.Add(part30);
                var part31 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = " (+"
                };
                row3.Children.Add(part31);
                Binding NewVaccinatedBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("NewVaccinated"),
                    Mode = BindingMode.OneWay
                };
                var part32 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style
                };
                part32.SetBinding(TextBlock.TextProperty, NewVaccinatedBind);
                row3.Children.Add(part32);
                var part33 = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = ")"
                };
                row3.Children.Add(part33);
                ToolTipContent.Children.Add(row3);
                #endregion
                #region
                var row4 = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                row4.SetValue(Grid.RowProperty, 4);
                row4.SetValue(Grid.ColumnProperty, 0);
                var Traffic = new TextBlock
                {
                    Style = this.Resources["TextBlockStyle"] as Style,
                    Text = "Насыщенность транспорта: " + Country.Cities[i].Traffic.ToString()
                };
                row4.Children.Add(Traffic);
                ToolTipContent.Children.Add(row4);
                #endregion
                var OurToolTip = new ToolTip
                {
                    Content = ToolTipContent,
                    Placement = System.Windows.Controls.Primitives.PlacementMode.Top
                };
                infButton.ToolTip = OurToolTip;
                CityStackPanel.Children.Add(infButton);
                infButton.VerticalAlignment = VerticalAlignment.Top;
                Binding ImmuneBind = new Binding
                {
                    Source = Country.Cities[i].People,
                    Path = new PropertyPath("Immune"),
                    Mode = BindingMode.OneWay
                };
                //ImmuneCity.SetBinding(TextBlock.TextProperty, ImmuneBind);
                var label2 = new Label
                {
                    Width = 20
                };
                CityStackPanel.Children.Add(label2);
            }
            if (Country.Week == Country.Weeks)
            {
                NextWeekButton.Visibility = Visibility.Hidden;
                VaccinateButton.Visibility = Visibility.Hidden;
            }
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var exitConfirmation = new ExitConfirmation(Country, this);
            exitConfirmation.ShowDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            fileName = Country.Name + " " + Country.DiseaseName + " Неделя " + Country.Week.ToString() + ".dat";
            SaveAsBinaryFormat(Country, fileName);
            MessageBox.Show("Сохранение успешно выполнено");
        }

        private void VaccinateButton_Click(object sender, RoutedEventArgs e)
        {
            var Vaccination = new Vaccination(Country, Changes);
            Vaccination.ShowDialog();
        }

        private void NextWeekButton_Click(object sender, RoutedEventArgs e)
        {
            int width = 1260 / Country.CitiesCount;

            if (Country.Week < Country.Weeks)
            {
                Country.WeeklyUpdate();
                for (int i = 0; i < Country.CitiesCount; i++)
                {
                    Changes[i] = 0;

                    Rectangle rectanglePurple = new Rectangle
                    {
                        Width = width * 0.4,
                        Height = 500,
                        Fill = purpleBrush,
                        Stroke = darkBrush
                    };
                    Canvas.SetLeft(rectanglePurple, width * (0.3 + i));
                    Canvas.SetTop(rectanglePurple, 95);
                    MainCanvas.Children.Add(rectanglePurple);

                    Rectangle rectangleRed2 = new Rectangle
                    {
                        Width = width * 0.4 - 2,
                        Height = Math.Ceiling((double)Country.Cities[i].People.Ill / Country.Cities[i].People.Total * 500),
                        Fill = redBrush
                    };
                    Canvas.SetLeft(rectangleRed2, width * (0.3 + i) + 1);
                    Canvas.SetTop(rectangleRed2, 95 + 500 - rectangleRed2.Height);
                    MainCanvas.Children.Add(rectangleRed2);

                    Rectangle rectangleGreen2 = new Rectangle
                    {
                        Width = width * 0.4 - 2,
                        Height = (double)Country.Cities[i].People.Immune / Country.Cities[i].People.Total * 500,
                        Fill = greenBrush
                    };
                    Canvas.SetLeft(rectangleGreen2, width * (0.3 + i) + 1);
                    Canvas.SetTop(rectangleGreen2, 95 + 500 - rectangleRed2.Height - rectangleGreen2.Height);
                    MainCanvas.Children.Add(rectangleGreen2);
                }
                Month.Content = "(" + Months[Country.Month] + ")";
                if (Country.NewFund >= 0)
                    FundSign.Text = " (+";
                else
                    FundSign.Text = " (";

                if (Country.Week == Country.Weeks)
                {
                    MessageBox.Show("Поздравляем, вы завершили моделирование!");
                    NextWeekButton.Visibility = Visibility.Hidden;
                    VaccinateButton.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
