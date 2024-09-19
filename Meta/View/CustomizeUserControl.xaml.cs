using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Packaging;
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
    /// Interaction logic for CustomizeUserControl.xaml
    /// </summary>
    public partial class CustomizeUserControl : UserControl
    {
        public static string dirpath = Directory.GetCurrentDirectory() + @"\Customize";
        public static string filename = dirpath + @"\customize.json";
        public static string panelFilename = dirpath + @"\panel.json";
        public static string colorblindFilename = dirpath + @"\colorblind.json";

        public static bool Content;
        public static bool Icons;
        public static string Location;
        public static bool CloseButton;
        public static bool EnhancedBorders;

        public static string colorblindMode = "nn";

        public CustomizeUserControl()
        {
            InitializeComponent();

            if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);

            if (!File.Exists(filename)) File.Create(filename).Close();

            if (!File.Exists(panelFilename)) File.Create(panelFilename).Close();

            if (!File.Exists(colorblindFilename)) File.Create(colorblindFilename).Close();

            LoadSettings();
        }

        public void LoadSettings()
        {
            Customize fileObj = JsonConvert.DeserializeObject<Customize>(File.ReadAllText(filename));

            Content = fileObj.Content;
            Icons = fileObj.Icons;
            Location = fileObj.Location;
            CloseButton = fileObj.CloseButton;
            EnhancedBorders = fileObj.EnhancedBorders;

            if (fileObj.Content == true) content_enable.IsChecked = true;
            else content_disable.IsChecked = true;

            if (fileObj.Icons == true) icon_enable.IsChecked = true;
            else icon_disable.IsChecked = true;

            if (fileObj.Location.Contains("cs")) location_cs.IsChecked = true;
            else location_co.IsChecked = true;

            if (fileObj.CloseButton == true) close_enable.IsChecked = true;
            else close_disable.IsChecked = true;

            if (fileObj.EnhancedBorders == true) border_enable.IsChecked = true;
            else border_disable.IsChecked = true;

            ApplyChanges();
        }

        public void ChangeSettings(object sender, RoutedEventArgs e)
        {
            string name = (sender as RadioButton).Name;

            switch (name)
            {
                case string content when name.Contains("content"):
                    if(content.Contains("enable")) Content = true;
                    else Content = false;
                    break;
                case string icons when name.Contains("icon"):
                    if (icons.Contains("enable")) Icons = true;
                    else Icons = false;
                    break;
                case string location when name.Contains("location"):
                    if (location.Contains("cs")) Location = "location_cs";
                    else Location = "location_co";
                    break;
                case string closeBtn when name.Contains("close"):
                    if (closeBtn.Contains("enable")) CloseButton = true;
                    else CloseButton = false;
                    break;
                case string borders when name.Contains("border"):
                    if (borders.Contains("enable")) EnhancedBorders = true;
                    else EnhancedBorders = false;
                    break;
            }

            var fileObj = new Customize
            {
                Content = Content,
                Icons = Icons,
                Location = Location,
                CloseButton = CloseButton,
                EnhancedBorders = EnhancedBorders,
            };

            string jsonRaw = JsonConvert.SerializeObject(fileObj);
            File.WriteAllText(filename, jsonRaw);

            ApplyChanges();
        }

        public void ApplyChanges()
        {
            ManageContent();
            ManageIcons();

            LoadNavigationPanelColor();

            LoadColorblindness();

            ManageCloseButton();
            ManageWindowBorders();
        }

        public void ManageContent()
        {
            var mw = (Application.Current.MainWindow as MainWindow);
            mw.MetaWindowName.Text = (CustomizeUserControl.Content == true ? "Meta Timetable" + mw.correspondingNames[mw.ContentControlElement.Content.GetType()] : "");
        }

        public void ManageIcons()
        {
            var mw = (Application.Current.MainWindow as MainWindow);
            Grid[] icons = { mw.CreateReminderIcon, mw.CreatePlanIcon, mw.ReminderListIcon, mw.PlanListIcon, mw.SettingsIcon, mw.CustomizeIcon };
            
            if(Icons == true)
            {
                foreach(Grid icon in icons)
                {
                    icon.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach (Grid icon in icons)
                {
                    icon.Visibility = Visibility.Hidden;
                }
            }
        }

        public void ManageCloseButton()
        {
            var mw = (Application.Current.MainWindow as MainWindow);

            if (CustomizeUserControl.CloseButton)
            {
                mw.ShutdownButton.Background = Brushes.Firebrick;
                mw.ShutdownButton.Foreground = Brushes.White;
            }
            else
            {
                mw.ShutdownButton.Background = Brushes.Transparent;
                mw.ShutdownButton.Foreground = Brushes.Black;
            }
        }

        public void ManageWindowBorders()
        {
            var mw = (Application.Current.MainWindow as MainWindow);
            if (EnhancedBorders)
            {
                mw.WindowBorder.BorderThickness = new Thickness(10);
                mw.WindowBorder.BorderBrush = Brushes.Gray;
            }
            else
            {
                mw.WindowBorder.BorderThickness = new Thickness(0.25);
                mw.WindowBorder.BorderBrush = Brushes.Gray;
            }
        }

        #region Navigation Pane
        public void LoadNavigationPanelColor()
        {
            Panel fileObj = JsonConvert.DeserializeObject<Panel>(File.ReadAllText(panelFilename));

            Color hexToColor(string hex)
            {
                return (Color)ColorConverter.ConvertFromString(hex);
            }

            var mw = (Application.Current.MainWindow as MainWindow);
            mw.NavigationPanel.Color = hexToColor(fileObj.hexCode);

            switch (fileObj.hexCode)
            {
                case "#000000":
                    black.IsSelected = true;
                    break;
                case "#A8C5E2":
                    blue.IsSelected = true;
                    break;
                case "#5BC65D":
                    green.IsSelected = true;
                    break;
                case "#F7B32B":
                    orange.IsSelected = true;
                    break;
                case "#FC7753":
                    red.IsSelected = true;
                    break;
            }
        }

        public void ManageNavigationPanel(object sender, RoutedEventArgs e)
        {
            string name = (sender as ComboBoxItem).Name;
            var mw = (Application.Current.MainWindow as MainWindow);

            Color hexToColor(string hex)
            {
                return (Color)ColorConverter.ConvertFromString(hex);
            }

            string currentHexCode = string.Empty;

            switch (name)
            {
                case "black":
                    mw.NavigationPanel.Color = hexToColor("#000000");
                    currentHexCode = "#000000";
                    break;
                case "blue":
                    mw.NavigationPanel.Color = hexToColor("#A8C5E2");
                    currentHexCode = "#A8C5E2";
                    break;
                case "green":
                    mw.NavigationPanel.Color = hexToColor("#5BC65D");
                    currentHexCode = "#5BC65D";
                    break;
                case "orange":
                    mw.NavigationPanel.Color = hexToColor("#F7B32B");
                    currentHexCode = "#F7B32B";
                    break;
                case "red":
                    mw.NavigationPanel.Color = hexToColor("#FC7753");
                    currentHexCode = "#FC7753";
                    break;
            }

            var fileObj = new Panel
            {
                hexCode = currentHexCode
            };

            string jsonRaw = JsonConvert.SerializeObject(fileObj);
            File.WriteAllText(panelFilename, jsonRaw);
        }
        #endregion

        public void LoadColorblindness()
        {
            ColorblindMode fileObj = JsonConvert.DeserializeObject<ColorblindMode>(File.ReadAllText(colorblindFilename));

            switch (fileObj.mode)
            {
                case "nn":
                    colorblindMode = "nn";
                    ColorBlind1.IsSelected = true;
                    break;
                case "rg":
                    colorblindMode = "rg";
                    ColorBlind2.IsSelected = true;
                    break;
                case "by":
                    colorblindMode = "by";
                    ColorBlind3.IsSelected = true;
                    break;
            }
        }

        public void ManageColorblindness(object sender, RoutedEventArgs e)
        {
            string name = (sender as ComboBoxItem).Name;
            var mw = (Application.Current.MainWindow as MainWindow);

            switch (name)
            {
                case "ColorBlind1":
                    colorblindMode = "nn";
                    break;
                case "ColorBlind2":
                    colorblindMode = "rg";
                    break;
                case "ColorBlind3":
                    colorblindMode = "by";
                    break;
            }

            var fileObj = new ColorblindMode
            {
                mode = colorblindMode
            };

            string jsonRaw = JsonConvert.SerializeObject(fileObj);
            File.WriteAllText(colorblindFilename, jsonRaw);
        }
    }

    public class Customize{
        public bool Content;
        public bool Icons;
        public string Location = string.Empty;
        public bool CloseButton;
        public bool EnhancedBorders;
    }

    public class Panel
    {
        public string hexCode;
    }

    public class ColorblindMode
    {
        public string mode;
    }
}
