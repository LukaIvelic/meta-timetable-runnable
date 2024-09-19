using Meta.Model.Logger;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
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
#pragma warning disable CS0252
namespace Meta.View
{
    /// <summary>
    /// Interaction logic for TimeSelector.xaml
    /// </summary>
    public partial class TimeSelector : UserControl
    {
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public TimeSelector()
        {
            try
            {
                eventLogger.LogEvent("A 0 parameter constructor called.", typeof(TimeSelector));

                InitializeComponent();
                DataContext = this;

                HourPart = DateTime.Now.Hour.ToString("D2");
                MinutePart = DateTime.Now.Minute.ToString("D2");
            
                TextBlockHour.Text = HourPart;
                TextBlockMinute.Text = MinutePart;
            }
            catch(Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }
        }

        private bool? _updateUiCtor = false;
        public TimeSelector(bool? updateUi)
        {
            try
            {
                eventLogger.LogEvent("Parameter constructor called.", typeof(TimeSelector));

                InitializeComponent();
                DataContext = this;

                HourPart = DateTime.Now.Hour.ToString("D2");
                MinutePart = DateTime.Now.Minute.ToString("D2");

                TextBlockHour.Text = HourPart;
                TextBlockMinute.Text = MinutePart;

                _updateUiCtor= updateUi;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }
        }

        public static string HourPart { get; set; }
        public static string MinutePart { get; set; }
        public static string FullTime { get; set; }
        public static string TimePartValue { get; set; }

        #region Update related
        public void Update(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Update method called.", typeof(TimeSelector));

                adjustTime(sender);
                updateProperties(true);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }
        }

        public void UpdateBackground()
        {
            try
            {
                eventLogger.LogEvent("UpdateBackground method called.", typeof(TimeSelector));

                updateProperties(true);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }
        }

        private void updateProperties(bool? updateUi)
        {
            try
            {
                eventLogger.LogEvent("updateProperties method called.", typeof(TimeSelector));

                HourPart = TextBlockHour.Text;
                MinutePart = TextBlockMinute.Text;
                TimePartValue = TimePart.Content.ToString();

                if (updateUi == null || _updateUiCtor == null) return;

                UserControl1 uc = FindVisualChild<UserControl1>(Application.Current.MainWindow);
                eventLogger.LogEvent("FindVisualChild method called.", typeof(TimeSelector));


                uc.ReminderDateTimeFormat.Content = $"{DateSelector.ReturnDate()} at {ReturnTime()}";
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }
        }
        #endregion

        public int hourLowerBound = 0, minuteLowerBound = 0;
        public int hourUpperBound = 23, minuteUpperBound = 59;

        #region Generic type methods
        private T? adjustTime <T> (T sender)
        {
            try
            {
                eventLogger.LogEvent("adjustTime<T> method called.", typeof(TimeSelector));

                if (TimeFormat.Content == "12H") hourUpperBound = 12;
                if (TimeFormat.Content == "24H") hourUpperBound = 23;

                Comparer<T> comparer = Comparer<T>.Default;
                Type senderType = sender.GetType();
                dynamic buttonInstance = 0;

                if (senderType != typeof(Button) && senderType != typeof(RepeatButton))
                {
                    MessageBox.Show((senderType != typeof(Button) || senderType != typeof(RepeatButton)).ToString());
                    return sender;
                }
                else if(senderType == typeof(Button))
                {
                    buttonInstance = sender as Button;
                }
                else
                {
                    buttonInstance = sender as RepeatButton;
                }

                string TextBlockHourValue = TextBlockHour.Text;
                string TextBlockMinuteValue = TextBlockMinute.Text;

                int hourValue = int.Parse(TextBlockHourValue);
                int minuteValue = int.Parse(TextBlockMinuteValue);

                TimeFormatPart timeFormatPart = new TimeFormatPart();

                switch (buttonInstance.Uid)
                {
                    case "RadioButtonHourPartDecrement":
                        hourValue = constrain(hourLowerBound, hourUpperBound, --hourValue);
                        TextBlockHour.Text = hourValue.ToString("D2");
                        break;
                    case "RadioButtonHourPartIncrement":
                        hourValue = constrain(hourLowerBound, hourUpperBound, ++hourValue);
                        TextBlockHour.Text = hourValue.ToString("D2");
                        break;
                    case "RadioButtonMinutePartDecrement":
                        minuteValue = constrain(minuteLowerBound, minuteUpperBound, --minuteValue);
                        TextBlockMinute.Text = minuteValue.ToString("D2");
                        break;
                    case "RadioButtonMinutePartIncrement":
                        minuteValue = constrain(minuteLowerBound, minuteUpperBound, ++minuteValue);
                        TextBlockMinute.Text = minuteValue.ToString("D2");
                        break;
                    case "ButtonTimeFormat":
                        var toPart = "";
                        if (TimePart.Content == "AM" || TimePart.Content == "PM") toPart = "";
                        else toPart = "PM";

                        (int Hours, int Minutes, string Format, string Part) time = timeFormatPart.TransformFormat(hourValue, minuteValue, TimePart.Content.ToString(), toPart);

                        TextBlockHour.Text = time.Hours.ToString("D2");
                        TextBlockMinute.Text = time.Minutes.ToString("D2");
                        TimeFormat.Content = time.Format;
                        TimePart.Content = time.Part;

                        break;
                    case "ButtonTimePart":
                        if(TimePart.Content == "AM")
                        {
                            TimePart.Content = "PM";
                        }
                        else
                        {
                            TimePart.Content = "AM";
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
            }

            return sender;
        }

        private T constrain <T> (T minValue, T maxValue, T currentValue)
        {
            try
            {
                eventLogger.LogEvent("constrain<T> method called.", typeof(TimeSelector));

                Comparer<T> comparer = Comparer<T>.Default;

                if(comparer.Compare(currentValue, minValue) < 0 )
                {
                    return minValue;
                }
                else if(comparer.Compare(currentValue, maxValue) > 0)
                {
                    return maxValue;
                }
                else
                {
                    return currentValue;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
                return currentValue;
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
                ErrorLogger.LogStaticError(ex.ToString(), typeof(TimeSelector));
                return null;
            }
        }
        #endregion

        public struct TimeFormatPart
        {
            public EventLogger eventLogger = new EventLogger();
            public ErrorLogger errorLogger = new ErrorLogger();

            public TimeFormatPart()
            {
                try
                {
                    eventLogger.LogEvent("TimeFormatPart struct used.", typeof(TimeFormatPart));    
                }
                catch (Exception ex)
                {
                    errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
                }
            }

            public const string Format24 = "24H";
            public const string Format12 = "12H";

            public const string PartAM = "AM";
            public const string PartPM = "PM";
            public const string PartEmpty = "";

            public (int, int, string, string) TransformFormat(int hours, int minutes, string currentPart, string toPart)
            {
                try
                {
                    eventLogger.LogEvent("TimeFormatPart struct TransformFormat method called.", typeof(TimeFormatPart));

                    (int Hours, int Minutes, string Format, string Part) returnTuple = (1, 2, "24H", "");

                    returnTuple.Minutes = minutes;

                    switch (toPart)
                    {
                        case PartAM:
                            returnTuple.Format = Format12;
                            returnTuple.Part = PartAM;

                            if (currentPart == PartEmpty)
                            {
                                if ((hours %= 12) == 0)
                                {
                                    hours = 12;
                                }
                            }

                            returnTuple.Hours = hours;
                            break;
                        case PartPM:
                            returnTuple.Format = Format12;
                            returnTuple.Part = PartPM;

                            if (currentPart == PartEmpty)
                            {
                                if ((hours %= 12) == 0)
                                {
                                    hours = 0;
                                }
                            }

                            returnTuple.Hours = hours;
                            break;
                        case PartEmpty:
                            returnTuple.Format = Format24;
                            returnTuple.Part = PartEmpty;

                            if (currentPart == PartPM)
                            {
                                hours = hours + 12;
                            }

                            if ((hours %= 24) == 0)
                            {
                                hours = 0;
                            }

                            returnTuple.Hours = hours;
                            break;
                    }

                    return returnTuple;
                }
                catch (Exception ex)
                {
                    errorLogger.LogError(ex.ToString(), typeof(TimeSelector));
                    return (0, 0, string.Empty, string.Empty);
                }
            }
        }

        public static string ReturnTime()
        {
            try
            {
                EventLogger.LogStaticEvent("ReturnTime method called.", typeof(TimeSelector));

                return $"{HourPart}:{MinutePart} {TimePartValue}";
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(TimeSelector));
                return string.Empty;
            }
        }
    }
}
