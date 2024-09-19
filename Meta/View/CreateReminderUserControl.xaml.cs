using Meta.Model.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Meta.View.TimeSelector;

namespace Meta.View
{
    public partial class UserControl1 : UserControl
    {
        private ReminderStructure reminderStructure = new ReminderStructure();
        public string LeftOverChars = "330";
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public UserControl1()
        {
            try
            {
                eventLogger.LogEvent("A constructor called.", typeof(UserControl1));

                InitializeComponent();
                DataContext = this;

                DateSelector dateSelector = new DateSelector(true);
                dateSelector.Width = 300;
                dateSelector.Height = 40;
                DateStackPanel.Children.Add(dateSelector);

                TimeSelector timeSelector = new TimeSelector();
                timeSelector.Width = 300;
                timeSelector.Height = 40;
                TimeStackPanel.Children.Add(timeSelector);

                var dir = CreateReminderFolder(true, false);
                reminderStructure.Priority = Priority.PriorityLevel_3;
                TextBlockDescription.Text = $"Description  •  {int.Parse(LeftOverChars) - ReminderDescription.Text.Length}/330";
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl1));
            }
        }

        #region Methods
        public void RadioButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ReminderButtonClicked method called.", typeof(UserControl1));


                var radioButton = (RadioButton)sender;
                var priority = new Priority(this);
                priority.SetPriorityLevel(radioButton.Uid);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl1));
            }
        }

        public struct Priority
        {
            public SolidColorBrush BackgroundColor { get; set; }
            public SolidColorBrush BorderColor { get; set; }
            public SolidColorBrush ForegroundColor { get; set; }

            public const string PriorityLevel_1 = "Urgent";
            public const string PriorityLevel_2 = "Important";
            public const string PriorityLevel_3 = "Safe";
            public const string PriorityLevel_4 = "None";

            private UserControl1 userControl1;

            public Priority(UserControl1 uc1)
            {
                try
                {
                    EventLogger.LogStaticEvent("Priority struct used.", typeof(Priority));

                    this.userControl1 = uc1;
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogStaticError(ex.ToString(), typeof(UserControl1));
                }
            }

            public void SetPriorityLevel(string prioLevel)
            {
                try
                {
                    EventLogger.LogStaticEvent("SetPriorityLevel local method called.", typeof(Priority));

                    var border = userControl1.ReminderBorder;
                    var label1 = userControl1.ReminderPreviewTitle;
                    var label2 = userControl1.ReminderDateTimeFormat;

                    switch (prioLevel)
                    {
                        case PriorityLevel_1:
                            BackgroundColor = new ReminderButtonStyle().SolidColorBrushConverter("#FC7753");
                            BorderColor = BackgroundColor;
                            ForegroundColor = Brushes.White;

                            userControl1.reminderStructure.Priority = (string)Priority.PriorityLevel_1;

                            break;
                        case PriorityLevel_2:
                            BackgroundColor = new ReminderButtonStyle().SolidColorBrushConverter("#F7B32B");
                            BorderColor = BackgroundColor;
                            ForegroundColor = Brushes.White;

                            userControl1.reminderStructure.Priority = (string)Priority.PriorityLevel_2;

                            break;
                        case PriorityLevel_3:
                            BackgroundColor = new ReminderButtonStyle().SolidColorBrushConverter("#5BC65D");
                            BorderColor = BackgroundColor;
                            ForegroundColor = Brushes.White;

                            userControl1.reminderStructure.Priority = (string)Priority.PriorityLevel_3;

                            break;
                        case PriorityLevel_4:
                            BackgroundColor = new ReminderButtonStyle().SolidColorBrushConverter("#A8C5E2");
                            BorderColor = BackgroundColor;
                            ForegroundColor = Brushes.White;

                            userControl1.reminderStructure.Priority = (string)Priority.PriorityLevel_4;

                            break;
                    }

                    border.BorderBrush = BorderColor;
                    border.Background = BackgroundColor;
                    label1.Foreground = ForegroundColor;
                    label2.Foreground = ForegroundColor;
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogStaticError(ex.ToString(), typeof(UserControl1));
                }
            }
        }

        public void Update(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Update method called.", typeof(UserControl1));

                TextBlockDescription.Text = $"Description  •  {int.Parse(LeftOverChars) - ReminderDescription.Text.Length}/330";
                ReminderDateTimeFormat.Content = $"{DateSelector.ReturnDate()} at {TimeSelector.ReturnTime()}";

                if ((sender as TextBox).Uid == "Title")
                {
                    ReminderPreviewTitle.Content = (sender as TextBox).Text;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl1));
            }
        }

        public void CreateReminder(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("CreateReminder method called.", typeof(UserControl1));

                var dir = CreateReminderFolder(true, true);
                var timeSelector = FindVisualChild<TimeSelector>(Application.Current.MainWindow);
                eventLogger.LogEvent("FindVisualChild method called.", typeof(UserControl1));
                var dateSelector = FindVisualChild<DateSelector>(Application.Current.MainWindow);
                eventLogger.LogEvent("FindVisualChild method called.", typeof(UserControl1));

                reminderStructure.Uid = (Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Reminders").Length).ToString("D3");
                reminderStructure.Time = (string)$"{timeSelector.TextBlockHour.Text}:{timeSelector.TextBlockMinute.Text} {timeSelector.TimePart.Content}";
                reminderStructure.Date = (string)$"{DateSelector.ReturnDate()}";
                reminderStructure.Title = (string)ReminderPreviewTitle.Content;
                reminderStructure.Description = (string)ReminderDescription.Text;
                reminderStructure.Adjust_background_to_time_of_day = (bool)BackgroundCheckBox.IsChecked;
                reminderStructure.Include_description = (bool)DescriptionCheckBox.IsChecked;
                reminderStructure.Send_desktop_notifications = (bool)NotificationsCheckBox.IsChecked;

                string jsonRaw = JsonConvert.SerializeObject(reminderStructure);
                using (FileStream fs = new FileStream(dir.Fullpath, FileMode.Open, FileAccess.Write))
                {
                    fs.Write(Encoding.UTF8.GetBytes(jsonRaw));
                }

                timeSelector.TextBlockHour.Text = DateTime.Now.Hour.ToString("D2");
                timeSelector.TextBlockMinute.Text = DateTime.Now.Minute.ToString("D2");
                timeSelector.TimeFormat.Content = TimeFormatPart.Format24;
                timeSelector.TimePart.Content = TimeFormatPart.PartEmpty;

                dateSelector.TextBlockDays.Text = DateTime.Now.Day.ToString("D2");
                dateSelector.TextBlockMonths.Text = DateTime.Now.Month.ToString("D2");
                dateSelector.TextBlockYears.Text = DateTime.Now.Year.ToString("D2");

                ReminderTitle.Text = string.Empty;
                ReminderDescription.Text = string.Empty;

                foreach (RadioButton r in RadioButtonStackPanel.Children)
                {
                    if(r.Uid == "Safe")
                    {
                        r.IsChecked = true;
                        new Priority(this).SetPriorityLevel(Priority.PriorityLevel_3);
                    }
                    else
                    {
                        r.IsChecked = false;
                    }
                }

                new View.UserControl3().AddReminderToList();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl1));
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
                ErrorLogger.LogStaticError(ex.ToString(), typeof(UserControl1));
                return null;
            }
        }

        private (string Directory, string FileName, string? Fullpath) CreateReminderFolder(bool createPath, bool createFile)
        {
            try
            {
                eventLogger.LogEvent("CreateReminderFolder method called.", typeof(UserControl1));

                string dirpath = Directory.GetCurrentDirectory() + @"\Reminders";

                if (createPath)
                {
                    if (!Directory.Exists(dirpath))
                    {
                        Directory.CreateDirectory(dirpath);
                    }
                }

                string nameFormat = "mtreminder_";
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
                ErrorLogger.LogStaticError(ex.ToString(), typeof(UserControl1));
                return (string.Empty, string.Empty, null);
            }
        }
        #endregion
    }

    public class ReminderStructure
    {
        public string Uid { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public bool Adjust_background_to_time_of_day { get; set; }
        public bool Include_description { get; set; }
        public bool Send_desktop_notifications { get; set; }
    }


}
