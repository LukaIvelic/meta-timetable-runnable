using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Meta.Model.Logger;

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for PlanUserControl.xaml
    /// </summary>
    public partial class PlanUserControl : UserControl
    {
        public string BackgroundBorder { get; set; }
        public string Hour { get; set; }
        public  string Text { get; set; }
        public string Priority { get; set; }

        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public PlanUserControl()
        {
            try
            {
                eventLogger.LogEvent("A constructor called.", typeof(PlanUserControl));

                InitializeComponent();

                Priority = "Safe";
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(PlanUserControl));
            }
        }

        private void HidePriorities(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("HidePriorities method called.", typeof(PlanUserControl));

                if (Grid_2.Visibility == Visibility.Visible)
                {
                    Grid_2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Grid_2.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(PlanUserControl));
            }
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Update method called.", typeof(PlanUserControl));

                PlanBorder.Background = new ReminderButtonStyle().SolidColorBrushConverter("#5BC65D");
                HourLabel.Content = Hour;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(PlanUserControl));
            }
        }

        private void SetPriority(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("SetPriority method called.", typeof(PlanUserControl));

                Style buttonStyle = new Style();

                switch ((sender as RadioButton).Uid)
                {
                    case "Urgent":
                        PlanBorder.Background = new ReminderButtonStyle().SolidColorBrushConverter("#FC7753");
                        buttonStyle = new ButtonStyle(1, 0, 5, 5, 0).ReturnStyle();
                        break;
                    case "Important":
                        PlanBorder.Background = new ReminderButtonStyle().SolidColorBrushConverter("#F7B32B");
                        buttonStyle = new ButtonStyle(2, 0, 5, 5, 0).ReturnStyle();
                        break;
                    case "Safe":
                        PlanBorder.Background = new ReminderButtonStyle().SolidColorBrushConverter("#5BC65D");
                        buttonStyle = new ButtonStyle(3, 0, 5, 5, 0).ReturnStyle();
                        break;
                    case "None":
                        PlanBorder.Background = new ReminderButtonStyle().SolidColorBrushConverter("#A8C5E2");
                        buttonStyle = new ButtonStyle(4, 0, 5, 5, 0).ReturnStyle();
                        break;
                }

                PriorityButton.Style = buttonStyle;
                Priority = (sender as RadioButton).Uid;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(PlanUserControl));
            }
        }

        private void UpdateText(object sender, KeyEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("UpdateText method called.", typeof(PlanUserControl));

                Text = PlanTextBox.Text;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(PlanUserControl));
            }
        }
    }
}
