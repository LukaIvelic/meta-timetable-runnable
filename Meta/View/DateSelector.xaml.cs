using Meta.Model.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for DateSelector.xaml
    /// </summary>
    public partial class DateSelector : UserControl
    {
        public static DateTime _currentTime = DateTime.Now;
        public static string DayPart { get; set; }
        public static string MonthPart { get; set; }
        public static string YearPart { get; set; }

        private int _dayLowerBound = 1, _monthLowerBound = 1, _yearLowerBound = 1990;
        private int _dayUpperBound = 28, _monthUpperBound = 12, _yearUpperBound = 2050;

        private bool? _updateUI = null;
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public DateSelector()
        {
            try
            {
                eventLogger.LogEvent("0 parameter constructor called.", typeof(DateSelector));

                InitializeComponent();
                DataContext = this;

                DayPart = _currentTime.Day.ToString("D2");
                MonthPart = _currentTime.Month.ToString("D2");
                YearPart = _currentTime.Year.ToString("D2");

                TextBlockDays.Text = DayPart.ToString();
                TextBlockMonths.Text = MonthPart.ToString();
                TextBlockYears.Text = YearPart.ToString();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }

        public DateSelector(bool? updateUi)
        {
            try
            {
                eventLogger.LogEvent("Parameter constructor called.", typeof(DateSelector));

                InitializeComponent();
                DataContext = this;

                DayPart = _currentTime.Day.ToString("D2");
                MonthPart = _currentTime.Month.ToString("D2");
                YearPart = _currentTime.Year.ToString("D2");

                TextBlockDays.Text = DayPart.ToString();
                TextBlockMonths.Text = MonthPart.ToString();
                TextBlockYears.Text = YearPart.ToString();

                _updateUI = updateUi;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }

        #region Update related
        public void Update(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Update method called.", typeof(DateSelector));

                adjustDate(sender as RepeatButton);
                updateProperties(true);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }

        public void UpdateBackground()
        {
            try
            {
                eventLogger.LogEvent("UpdateBackground method called.", typeof(DateSelector));

                updateProperties(true);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }

        public void updateProperties(bool? updateUi)
        {
            try
            {
                eventLogger.LogEvent("updateProperties method called.", typeof(DateSelector));

                DayPart = TextBlockDays.Text;
                MonthPart= TextBlockMonths.Text;
                YearPart = TextBlockYears.Text;

                if (updateUi == null || _updateUI == null) return;

                UserControl1 uc = FindVisualChild<UserControl1>(Application.Current.MainWindow);
                eventLogger.LogEvent("FindVisualChild method called.", typeof(DateSelector));

                uc.ReminderDateTimeFormat.Content = $"{ReturnDate()} at {TimeSelector.ReturnTime()}";
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }
        #endregion

        #region Date related
        public void adjustDate (RepeatButton sender)
        {
            try
            {
                eventLogger.LogEvent("adjustDate method called.", typeof(DateSelector));

                string TextBlockDayValue = TextBlockDays.Text;
                string TextBlockMonthValue = TextBlockMonths.Text;
                string TextBlockYearValue = TextBlockYears.Text;

                int dayValue = int.Parse(TextBlockDayValue);
                int monthValue = int.Parse(TextBlockMonthValue);
                int yearValue = int.Parse(TextBlockYearValue);

                switch (sender.Uid)
                {
                    case "RadioButtonDayPartDecrement":
                        dayValue = constrain(_dayLowerBound, _dayUpperBound, --dayValue);
                        DayPart = dayValue.ToString("D2");
                        break;
                    case "RadioButtonDayPartIncrement":
                        dayValue = constrain(_dayLowerBound, _dayUpperBound, ++dayValue);
                        DayPart = dayValue.ToString("D2");
                        break;
                    case "RadioButtonMonthPartDecrement":
                        monthValue = constrain(_monthLowerBound, _monthUpperBound, --monthValue);
                        MonthPart = monthValue.ToString("D2");
                        break;
                    case "RadioButtonMonthPartIncrement":
                        monthValue = constrain(_monthLowerBound, _monthUpperBound, ++monthValue);
                        MonthPart = monthValue.ToString("D2");
                        break;
                    case "RadioButtonYearPartDecrement":
                        yearValue = constrain(_yearLowerBound, _yearUpperBound, --yearValue);
                        YearPart = yearValue.ToString("D2");
                        break;
                    case "RadioButtonYearPartIncrement":
                        yearValue = constrain(_yearLowerBound, _yearUpperBound, ++yearValue);
                        YearPart = yearValue.ToString("D2");
                        break;
                }

                _dayUpperBound = DateTime.DaysInMonth(yearValue, monthValue);
                dayValue = constrain(_dayLowerBound, _dayUpperBound, dayValue);

                TextBlockDays.Text = dayValue.ToString("D2");
                TextBlockMonths.Text = monthValue.ToString("D2");
                TextBlockYears.Text = yearValue.ToString("D2");
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
            }
        }

        public static string ReturnDate()
        {
            try
            {
                EventLogger.LogStaticEvent("ReturnDate method called.", typeof(DateSelector));
                return $"{DayPart}.{MonthPart}.{YearPart}.";
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(DateSelector));
                return string.Empty;
            }
        }
        #endregion

        #region Generic type methods
        private T constrain<T>(T minValue, T maxValue, T currentValue)
        {
            try
            {
                eventLogger.LogEvent("constrain<T> method called.", typeof(DateSelector));

                Comparer<T> comparer = Comparer<T>.Default;

                if (comparer.Compare(currentValue, minValue) < 0)
                {
                    return minValue;
                }
                else if (comparer.Compare(currentValue, maxValue) > 0)
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
                errorLogger.LogError(ex.ToString(), typeof(DateSelector));
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
                ErrorLogger.LogStaticError(ex.ToString(), typeof(DateSelector));
                return null;
            }
        }
        #endregion
    }
}
