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
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Markup;
using System.Xml;
using ClassLibrary;

namespace VirusApp
{
    public partial class FileChoice : Window
    {
        Country Country;
        string fileName;
        List<string> list=new List<string>();
        public FileChoice()
        {
            InitializeComponent();
            list = (from a in Directory.GetFiles(Directory.GetCurrentDirectory().ToString(), "*.dat") select System.IO.Path.GetFileName(a)).ToList();
            if (list.Count == 0)
            {
                var noFiles = new Label
                {
                    Style = this.Resources["LabelStyle"] as Style,
                    Content = "У вас нет сохранённых симуляций",
                    Width = 494,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                ListOfFiles.Children.Add(noFiles);
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var fileLine = new StackPanel
                    {
                        MinHeight = 70,
                        Orientation = Orientation.Horizontal
                    };
                    var fileName = new TextBlock
                    {
                        Width = 300,
                        Style = this.Resources["TextBlockStyle"] as Style,
                        Text = list[i].Substring(0, list[i].Length - 4),
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };
                    fileLine.Children.Add(fileName);
                    var EmptySpace = new Label
                    {
                        Width = 55
                    };
                    fileLine.Children.Add(EmptySpace);
                    string template = XamlWriter.Save(LoadTemplate.Template);
                    StringReader stringReader = new StringReader(template);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    var okButton = new Button
                    {
                        Name = "File" + i.ToString(),
                        Height = 50,
                        Width = 50,
                        VerticalAlignment = VerticalAlignment.Center,
                        Template = (ControlTemplate)XamlReader.Load(xmlReader)
                    };
                    okButton.Click += OkButton_Click;
                    fileLine.Children.Add(okButton);


                    var EmptySpace2 = new Label
                    {
                        Width = 10
                    };
                    fileLine.Children.Add(EmptySpace2);
                    string template2 = XamlWriter.Save(DeleteTemplate.Template);
                    StringReader stringReader2 = new StringReader(template2);
                    XmlReader xmlReader2 = XmlReader.Create(stringReader2);
                    var deleteButton = new Button
                    {
                        Name = "File" + i.ToString(),
                        Template = (ControlTemplate)XamlReader.Load(xmlReader2),
                        Height = 50,
                        Width = 50,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    deleteButton.Click += DeleteButton_Click;
                    fileLine.Children.Add(deleteButton);

                    ListOfFiles.Children.Add(fileLine);

                }
            }
        }

        public void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as Button;
            int fileNumber = (int)char.GetNumericValue(Button.Name, 4);
            fileName = list[fileNumber];
            Country=LoadFromBinaryFile(fileName);
            var simulation = new Simulation(Country);
            simulation.Show();
            Close();

        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as Button;
            int fileNumber = (int)char.GetNumericValue(Button.Name, 4);
            fileName = list[fileNumber];
            var stackPanel = ListOfFiles.Children[fileNumber];
            ListOfFiles.Children[fileNumber].Visibility=Visibility.Collapsed;
            File.Delete(fileName);
        }

        static Country LoadFromBinaryFile(string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                var country = (Country)binFormat.Deserialize(fStream);
                return country;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var menu = new Menu();
            menu.Show();
            Close();
        }
    }
}
