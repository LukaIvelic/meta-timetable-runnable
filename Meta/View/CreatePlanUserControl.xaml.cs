using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Meta.View.UserControl1;
using Meta.Model.Logger;

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public static string WindowName = "  •  Create plan";
        public DateSelector dateSelector = new DateSelector();
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public UserControl2()
        {
            try
            {
                eventLogger.LogEvent("A constructor called.", typeof(UserControl2));

                InitializeComponent();

                dateSelector = new DateSelector(null);
                dateSelector.Height = 40;
                dateSelector.Width = 290;
                dateSelector.HorizontalAlignment = HorizontalAlignment.Left;
                dateSelector.Margin = new Thickness(10, 0, 0, 0);
                dateSelector.RadioButtonDayPartDecrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                dateSelector.RadioButtonDayPartIncrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                dateSelector.RadioButtonMonthPartDecrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                dateSelector.RadioButtonMonthPartIncrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                dateSelector.RadioButtonYearPartDecrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                dateSelector.RadioButtonYearPartIncrement.Click += (object sender, RoutedEventArgs e) => { dateSelector.updateProperties(null); ChangeDate(sender, e); };
                DateStackPanel.Children.Add(dateSelector);

                object planFolder = CreatePlanFolder(true, false);

                CultureInfo cultureInfo = new CultureInfo("en-US");
                PlanMainDate.Content = DateTime.Now.ToString("dddd, dd.MM.yyyy", cultureInfo);
            }
            catch(Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
            }
        }

        private void ChangeTime(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ChangeTime method called.", typeof(UserControl2));

                GridMorning.Visibility = Visibility.Collapsed;
                GridDay.Visibility = Visibility.Collapsed;
                GridNight.Visibility = Visibility.Collapsed;

                switch ((sender as Button).Uid)
                {
                    case "Morning":
                        GridMorning.Visibility = Visibility.Visible;
                        break;
                    case "Day":
                        GridDay.Visibility = Visibility.Visible;
                        break;
                    case "Night":
                        GridNight.Visibility = Visibility.Visible;
                        break;
                }
            }
            catch(Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
            }
            
        }

        private void ChangeDate(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ChangeDate method called.", typeof(UserControl2));

                string timeString = $"{dateSelector.TextBlockDays.Text}.{dateSelector.TextBlockMonths.Text}.{dateSelector.TextBlockYears.Text}";
                DayOfWeek dayName = DateTime.Parse(timeString).DayOfWeek;
                PlanMainDate.Content = $"{dayName.ToString("G")}, {timeString}";
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
            }
        }

        private void CreatePlan(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("CreatePlan method called.", typeof(UserControl2));

                Dictionary<string, Hour> map = new Dictionary<string, Hour>();
                Grid[] gridArray = { GridMorning, GridDay, GridNight };

                int count = 5;
                foreach (Grid g in gridArray)
                {
                    for (int i = 0; i < g.Children.Count; i++)
                    {
                        PlanUserControl pUCMorning = (PlanUserControl)g.Children[i];

                        Hour myHour = new Hour();
                        myHour.Priority = pUCMorning.Priority;
                        myHour.Event = pUCMorning.Text;

                        map.Add($"_{(count):D2}00", myHour);
                        count++;
                    }
                }

                PlanStructure plan = new PlanStructure();
                plan.Title = PlanTitle.Text;

                DateSelector dateSelector = FindVisualChild<DateSelector>(this);
                eventLogger.LogEvent("FindVisualChild method called.", typeof(UserControl2));

                plan.Date = $"{DateSelector.DayPart}.{DateSelector.MonthPart}.{DateSelector.YearPart}";

                plan.Description = PlanDescription.Text;
                plan.MainEvent = PlanMainEvent.Text;

                plan.Morning = map.Where(t => t.Key.CompareTo("_0500") >= 0 && t.Key.CompareTo("_1000") <= 0).ToDictionary(x => x.Key, x => x.Value);
                plan.Day = map.Where(t => t.Key.CompareTo("_1100") >= 0 && t.Key.CompareTo("_1600") <= 0).ToDictionary(x => x.Key, x => x.Value);
                plan.Night = map.Where(t => t.Key.CompareTo("_1700") >= 0 && t.Key.CompareTo("_2200") <= 0).ToDictionary(x => x.Key, x => x.Value);
                plan.Uid = (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Plans").Length+1).ToString("D3");

                (string Directory, string FileName, string? Fullpath) planFolder = CreatePlanFolder(true, true);

                string jsonRaw = JsonConvert.SerializeObject(plan);
                using (FileStream fs = new FileStream(planFolder.Fullpath, FileMode.Open, FileAccess.Write))
                {
                    fs.Write(Encoding.UTF8.GetBytes(jsonRaw));
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
            }
        }

        private void ResetPlan(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ResetPlan method called.", typeof(UserControl2));
                return;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
            }
        }

        private (string Directory, string FileName, string? Fullpath) CreatePlanFolder(bool createPath, bool createFile)
        {
            try
            {
                eventLogger.LogEvent("CreatePlanFolder method called.", typeof(UserControl2));

                string dirpath = Directory.GetCurrentDirectory() + @"\Plans";

                if (createPath)
                {
                    if (!Directory.Exists(dirpath))
                    {
                        Directory.CreateDirectory(dirpath);
                    }
                }

                string nameFormat = "mtplan_";
                int numberOfFiles = Directory.GetFiles(dirpath).Length + 1;
                string? pathToFile = null;

                if (createFile)
                {
                    pathToFile = @$"{dirpath}\{nameFormat}{numberOfFiles.ToString("D3")}.json";
                    FileStream fs = File.Create(pathToFile);
                    fs.Close();
                }
                return (dirpath, $"{nameFormat}{numberOfFiles}.json", pathToFile);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl2));
                return (string.Empty, string.Empty, null);
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            try
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    if (child is T result)
                    {
                        return result;
                    }
                    else
                    {
                        T foundChild = FindVisualChild<T>(child);
                        if (foundChild != null)
                        {
                            return foundChild;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(UserControl2));
                return null;

            }
        }
   
    }

    public class Hour
    {
        public string Priority { get; set; }
        public string Event { get; set; }
    }

    public class PlanStructure
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
