using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
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

    public partial class UserControl5 : UserControl
    {
        public static string WindowName = "  •  Settings";
        public static string dirname = Directory.GetCurrentDirectory() + @"\Settings";
        public static string filename = Directory.GetCurrentDirectory() + @"\Settings\settings.json";
        public static string windowSizeName = Directory.GetCurrentDirectory() + @"\Settings\size.json";

        public static string Language = string.Empty;
        public static string Format = string.Empty;
        public static bool Maximize;
        public static bool EventLogger;
        public static bool ErrorLogger;
        public static bool TimeNav;
        public static bool DateNav;
        public static bool Delete;
        public static bool Zen;
        public static bool Minimize;

        public UserControl5()
        {
            InitializeComponent();

            if(!Directory.Exists(dirname)) Directory.CreateDirectory(dirname);

            if (!File.Exists(filename))
            {
                FileStream fs = File.Create(filename);
                fs.Close();
            }

            if (!File.Exists(windowSizeName))
            {
                FileStream fs = File.Create(windowSizeName);
                fs.Close();
            }

            Settings fileObj = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));

            Language = fileObj.Language;
            Format = fileObj.Format;
            Maximize = fileObj.Maximize;
            EventLogger= fileObj.EventLogger;
            ErrorLogger = fileObj.ErrorLogger; 
            TimeNav = fileObj.TimeNav;
            DateNav = fileObj.DateNav;
            Delete = fileObj.Delete;
            Zen = fileObj.Zen;
            Minimize = fileObj.Minimize;

            LoadSettings();
            ApplyChanges((MainWindow)Application.Current.MainWindow);

            WindowProperties fileObj2 = JsonConvert.DeserializeObject<WindowProperties>(File.ReadAllText(windowSizeName));
            switch (fileObj2.Height)
            {
                case 900:
                    Cb1.IsSelected = true;
                    break;
                case 825:
                    Cb2.IsSelected = true;
                    break;
                case 700:
                    Cb3.IsSelected = true;
                    break;
                case 600:
                    Cb4.IsSelected = true;
                    break;
                case 400:
                    Cb5.IsSelected = true;
                    break;
            }
        }

        public void LoadSettings()
        {
            if (Language == "hrv") language_hrv.IsChecked = true;
            else language_eng.IsChecked = true;

            if (Format == "24") timeformat_24.IsChecked = true;
            else timeformat_12.IsChecked = true;

            if (Maximize == true) maximize_enable.IsChecked = true;
            else maximize_disable.IsChecked = true;

            if (EventLogger == true) event_enable.IsChecked = true;
            else event_disable.IsChecked = true;

            if (ErrorLogger == true) error_enable.IsChecked = true;
            else error_disable.IsChecked = true;

            if (TimeNav == true) time_enable.IsChecked = true;
            else time_disable.IsChecked = true;

            if (DateNav == true) day_enable.IsChecked = true;
            else day_disable.IsChecked = true;

            if (Delete == true) deleting_enable.IsChecked = true;
            else deleting_disable.IsChecked = true;

            if (Zen == true) zen_enable.IsChecked = true;
            else zen_disable.IsChecked = true;

            if (Minimize == true) minimize_enable.IsChecked = true;
            else minimize_disable.IsChecked = true;
        }

        public void ChangeSettings(object sender, RoutedEventArgs e)
        {
            string name = (sender as RadioButton).Name;

            switch (name)
            {
                case string lang when name.Contains("language"):
                    Language = lang.Contains("eng") ? "eng" : "hrv";
                    break;

                case string timeformat when name.Contains("timeformat"):
                    Format = timeformat.Contains("24") ? "24" : "12";
                    break;

                case string maximize when name.Contains("maximize"):
                    Maximize = maximize.Contains("enable") ? true : false;
                    break;

                case string logger when name.Contains("event"):
                    EventLogger = logger.Contains("enable") ? true : false;
                    break;

                case string logger when name.Contains("error"):
                    ErrorLogger = logger.Contains("enable") ? true : false;
                    break;

                case string display when name.Contains("day"):
                    DateNav = display.Contains("enable") ? true : false;
                    break;

                case string display when name.Contains("time"):
                    TimeNav = display.Contains("enable") ? true : false;
                    break;

                case string delete when name.Contains("deleting"):
                    Delete = delete.Contains("enable") ? true : false;
                    break;

                case string zen when name.Contains("zen"):
                    Zen = zen.Contains("enable") ? true : false;
                    break;

                case string minimize when name.Contains("minimize"):
                    Minimize = minimize.Contains("enable") ? true : false;
                    break;
            }

            var fileObj = new Settings {
                Language = Language,
                Format = Format,
                Maximize = Maximize,
                EventLogger = EventLogger,
                ErrorLogger = ErrorLogger,
                DateNav = DateNav,
                TimeNav = TimeNav,
                Delete = Delete,
                Zen = Zen,
                Minimize = Minimize
            };

            string jsonRaw = JsonConvert.SerializeObject(fileObj);
            File.WriteAllText(filename, jsonRaw);

            ApplyChanges(null);
        }

        public static void ApplyChanges(MainWindow? mainWindow)
        {
            ManageMaximizeButton();
            ManageZenMode();
            ManageWindowSize();
        }

        public static void ManageMaximizeButton()
        {
            var mw = (MainWindow)Application.Current.MainWindow;
            mw.MaximizeButtn.IsEnabled = Maximize;
        }

        public static void ManageZenMode()
        {
            var mw = (MainWindow)Application.Current.MainWindow;
            if (Zen == true)
            {
                mw.WindowStyle = WindowStyle.None;
                mw.WindowState = WindowState.Maximized;
                mw.DisableBlur();

                Maximize = true;
            }
            else
            {
                mw.WindowState = WindowState.Normal;
                mw.EnableBlur();

                Maximize = false;
            }
        }

        public static void ManageWindowSize()
        {
            WindowProperties fileObj = JsonConvert.DeserializeObject<WindowProperties>(File.ReadAllText(windowSizeName));

            var mw = (MainWindow)Application.Current.MainWindow;
            mw.Height = fileObj.Height;
            mw.Width = fileObj.Width;
        }

        public void ComboBoxItemClicked(object sender, RoutedEventArgs e)
        {
            int[,] size =
            {
                {900, 1800},
                {825, 1650},
                {700, 1400},
                {600, 1200},
                {400, 800},
            };

            string name = (sender as ComboBoxItem).Name;

            int height = 850, width = 1650;

            switch (name)
            {
                case "Cb1":
                    height = size[0, 0];
                    width = size[0, 1];
                    break;
                case "Cb2":
                    height = size[1, 0];
                    width = size[1, 1];
                    break;
                case "Cb3":
                    height = size[2, 0];
                    width = size[2, 1];
                    break;
                case "Cb4":
                    height = size[3, 0];
                    width = size[3, 1];
                    break;
                case "Cb5":
                    height = size[4, 0];
                    width = size[4, 1];
                    break;
            }

            var fileObj = new WindowProperties
            {
                Height = height,
                Width = width,
            };

            File.WriteAllText(windowSizeName, JsonConvert.SerializeObject(fileObj));

            ManageWindowSize();
        }
    
        
    }

    public class Settings
    {
        public string Language;
        public string Format;
        public bool Maximize;
        public bool EventLogger;
        public bool ErrorLogger;
        public bool TimeNav;
        public bool DateNav;
        public bool Delete;
        public bool Zen;
        public bool Minimize;
    }

    public class WindowProperties
    {
        public int Height;
        public int Width;
    }
}
