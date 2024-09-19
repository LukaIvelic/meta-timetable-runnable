using Meta.Model.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for UserControl4.xaml
    /// </summary>
    public partial class UserControl4 : UserControl
    {
        public static string WindowName = "  •  Plan list";
        public int column = -1, row = -1;
        public string dirpath = Directory.GetCurrentDirectory() + @"\Plans";
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public UserControl4()
        {
            try
            {
                eventLogger.LogEvent("A constructor called.", typeof(UserControl4));

                InitializeComponent();
                AddPlanToList();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl4));
            }
        }

        public void AddPlanToList()
        {
            try
            {
                eventLogger.LogEvent("AddPlanToList method called.", typeof(UserControl4));

                FileInfo[] files = new DirectoryInfo(dirpath).GetFiles();

                foreach (FileInfo f in files)
                {
                    string jsonRaw = File.ReadAllText(f.FullName);
                    PlanButton fileObj = JsonConvert.DeserializeObject<PlanButton>(jsonRaw);

                    var button = CreatePlan(fileObj);
                    button.Uid = fileObj.Uid;

                    Grid myGrid = new Grid();
                    myGrid.Children.Add(button);
                    myGrid.Margin = new Thickness(5);

                    row++;

                    if(row % 5 == 0)
                    {
                        column++;
                        row = 0;
                    }

                    Grid.SetColumn(myGrid, column);
                    Grid.SetRow(myGrid, row);

                    PlanListGrid.Children.Add(myGrid);
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl4));
            }
        }

        public Button? CreatePlan(PlanButton planButton)
        {
            try
            {
                eventLogger.LogEvent("CreatePlan method called.", typeof(UserControl4));

                Style buttonStyle = new Style();
                SolidColorBrush buttonBrush = Brushes.Black;

                buttonStyle = new ButtonStyle(6).ReturnStyle();
                dynamic objectStyle = new ReminderButtonStyle().ReturnObject(6);
                buttonBrush = objectStyle.Button_Foreground;

                Label planTitle = new Label();
                planTitle.FontSize = 18;
                planTitle.FontWeight = FontWeights.DemiBold;
                planTitle.Content = planButton.Title;
                planTitle.Foreground = buttonBrush;

                Label planDate = new Label();
                planDate.FontSize = 18;
                planDate.FontWeight = FontWeights.Normal;
                planDate.Content = $"{planButton.Date}";
                planDate.VerticalAlignment = VerticalAlignment.Center;
                planDate.HorizontalAlignment = HorizontalAlignment.Center;
                planDate.Foreground = buttonBrush;


                StackPanel stackPanel = new StackPanel();
                stackPanel.VerticalAlignment = VerticalAlignment.Center;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

                stackPanel.Children.Add(planTitle);
                stackPanel.Children.Add(planDate);

                WrapPanel wrapPanel = new WrapPanel();
                wrapPanel.Children.Add(stackPanel);

                Button myButton = new Button();

                myButton.Content = wrapPanel;
                myButton.Style = buttonStyle;
                myButton.Click += OpenDetails;

                return myButton;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl4));
                return null;
            }
        }

        public void DeleteAllPlans(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("DeleteAllPlans method called.", typeof(UserControl4));

                string dirpath = Directory.GetCurrentDirectory() + @"\Plans";

                if (!UserControl5.Delete)
                {
                    foreach (string s in Directory.GetFiles(dirpath))
                    {
                        File.Delete(s);
                    }

                    (Application.Current.MainWindow as MainWindow).ContentControlElement.Content = new UserControl4();
                    return;
                }

                MessageBoxResult r = MessageBox.Show("Are you sure you want to delete the file?", "Delete file?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (r == MessageBoxResult.Yes)
                {
                    foreach (string s in Directory.GetFiles(dirpath))
                    {
                        File.Delete(s);
                    }
                }
                else
                {
                    return;
                }

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ContentControlElement.Content = new UserControl4();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl4));
            }
        }

        public void OpenDetails(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("OpenDetails method called.", typeof(UserControl4));

                var button = (Button)sender;
                string wantedFileName = $"mtplan_{button.Uid}.json";
                string fullpath = Directory.GetCurrentDirectory() + $@"\Plans\{wantedFileName}";

                string jsonRaw = File.ReadAllText(fullpath);
                var fileObj = JsonConvert.DeserializeObject<PlanButton>(jsonRaw);

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ContentControlElement.Content = new UserControl7(fileObj, new ButtonStyle(6).ReturnStyle(), button.Uid);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl4));
            }
        }
    }

    public class PlanButton
    {
        public string Uid { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string MainEvent { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Hour> Morning { get; set; }
        public Dictionary<string, Hour> Day { get; set; }
        public Dictionary<string, Hour> Night { get; set; }
    }
}
