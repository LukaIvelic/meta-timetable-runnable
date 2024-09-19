using Meta.Model.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Meta.View.ButtonStyle;

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for UserControl3.xaml
    /// </summary>
    public partial class UserControl3 : UserControl
    {
        public static string WindowName = "  •  Reminder list";
        public int column = -1, row = -1;
        public string dirpath = Directory.GetCurrentDirectory() +  @"\Reminders";
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger(); 

        public UserControl3()
        {
            eventLogger.LogEvent("A constructor called.", typeof(UserControl3));

            InitializeComponent();
            AddReminderToList();
        }

        public void AddReminderToList()
        {
            eventLogger.LogEvent("AddReminderToList method called.", typeof(UserControl3));

            FileInfo[] files = new DirectoryInfo(dirpath).GetFiles();

            foreach (FileInfo f in files)
            {
                string jsonRaw = File.ReadAllText(f.FullName);
                ReminderButton fileObj = JsonConvert.DeserializeObject<ReminderButton>(jsonRaw.ToString());

                var button = CreateReminder(fileObj);
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

                ReminderListGrid.Children.Add(myGrid);
            }
        }

        public Button CreateReminder(ReminderButton reminderButton)
        {
            eventLogger.LogEvent("CreateReminder method called.", typeof(UserControl3));

            Style buttonStyle = new Style();
            SolidColorBrush buttonBrush = Brushes.Black;

            switch (reminderButton.Priority)
            {
                case "Urgent":
                    buttonStyle = new ButtonStyle(1).ReturnStyle();
                    dynamic objectStyle = new ReminderButtonStyle().ReturnObject(1);
                    buttonBrush = objectStyle.Button_Foreground;
                    break;
                case "Important":
                    buttonStyle = new ButtonStyle(2).ReturnStyle();
                    objectStyle = new ReminderButtonStyle().ReturnObject(2);
                    buttonBrush = objectStyle.Button_Foreground;
                    break;
                case "Safe":
                    buttonStyle = new ButtonStyle(3).ReturnStyle();
                    objectStyle = new ReminderButtonStyle().ReturnObject(3);
                    buttonBrush = objectStyle.Button_Foreground;
                    break;
                case "None":
                    buttonStyle = new ButtonStyle(4).ReturnStyle();
                    objectStyle = new ReminderButtonStyle().ReturnObject(4);
                    buttonBrush = objectStyle.Button_Foreground;
                    break;
            }

            Label reminderTitle = new Label();
            reminderTitle.FontSize = 18;
            reminderTitle.FontWeight = FontWeights.DemiBold;
            reminderTitle.Content = reminderButton.Title;
            reminderTitle.Foreground = buttonBrush;

            Label reminderDate = new Label();
            reminderDate.FontSize = 18;
            reminderDate.FontWeight = FontWeights.Normal;
            reminderDate.Content = $"{reminderButton.Date} at {reminderButton.Time}";
            reminderDate.VerticalAlignment = VerticalAlignment.Center;
            reminderDate.HorizontalAlignment = HorizontalAlignment.Center;
            reminderDate.Foreground = buttonBrush;


            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            stackPanel.Children.Add(reminderTitle);
            stackPanel.Children.Add(reminderDate);

            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Children.Add(stackPanel);

            Button myButton = new Button();

            myButton.Content = wrapPanel;
            myButton.Style = buttonStyle;
            myButton.Click += OpenDetails;

            return myButton;
        }

        private void DeleteAllReminders(object sender, RoutedEventArgs e)
        {
            eventLogger.LogEvent("DeleteAllReminders method called.", typeof(UserControl3));

            string dirpath = Directory.GetCurrentDirectory() + @"\Reminders";

            if (!UserControl5.Delete)
            {
                foreach (string s in Directory.GetFiles(dirpath))
                {
                    File.Delete(s);
                }

                (Application.Current.MainWindow as MainWindow).ContentControlElement.Content = new UserControl3();
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
            mainWindow.ContentControlElement.Content = new UserControl3();
        }

        public void OpenDetails(object sender, RoutedEventArgs e)
        {
            eventLogger.LogEvent("OpenDetails method called.", typeof(UserControl3));

            var button = (Button)sender;
            string wantedFileName = $"mtreminder_{button.Uid}.json";
            string fullpath = Directory.GetCurrentDirectory() + $@"\Reminders\{wantedFileName}";

            string jsonRaw = File.ReadAllText(fullpath);
            var fileObj = JsonConvert.DeserializeObject<ReminderButton>(jsonRaw);

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentControlElement.Content = new UserControl6(fileObj, new ButtonStyle(5).ReturnStyle(), button.Uid);
        }
    }

    public class ButtonStyle
    {
        Style buttonStyle = new Style(typeof(Button));
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public ButtonStyle(int style)
        {
            try
            {
                eventLogger.LogEvent("0 parameter constructor called.", typeof(ButtonStyle));

                dynamic objectStyle = new ReminderButtonStyle().ReturnObject(style);
                eventLogger.LogEvent("ReminderButtonStyle struct SolidColorBrushConverter method called.", typeof(ButtonStyle));
                eventLogger.LogEvent("ReminderButtonStyle struct ReturnObject method called.", typeof(ButtonStyle));


                var template = new ControlTemplate(typeof(Button));
                var border = new FrameworkElementFactory(typeof(Border));
                border.SetValue(Border.BackgroundProperty, objectStyle.Button_Static_Background);
                border.SetValue(Border.BorderBrushProperty, objectStyle.Button_Static_Border);
                border.SetValue(Border.BorderThicknessProperty, new Thickness(1.25));

                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(5));

                border.Name = "border";

                var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                border.AppendChild(contentPresenter);
                template.VisualTree = border;

                var triggerMouseOver = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                triggerMouseOver.Setters.Add(
                    new Setter { Property = Control.BackgroundProperty, Value = (SolidColorBrush)objectStyle.Button_MouseOver_Background, TargetName = "border" }
                );
                triggerMouseOver.Setters.Add(
                    new Setter { Property = Button.BorderBrushProperty, Value = objectStyle.Button_MouseOver_Border, TargetName = "border" }
                );


                var triggerIsPressed = new Trigger { Property = Button.IsPressedProperty, Value = true };
                triggerIsPressed.Setters.Add(
                    new Setter { Property = Button.BackgroundProperty, Value = objectStyle.Button_Pressed_Background , TargetName= "border" }
                );
                triggerIsPressed.Setters.Add(
                    new Setter { Property = Button.BorderBrushProperty, Value = objectStyle.Button_PressedBorder , TargetName = "border" }
                );

                template.Triggers.Add(triggerMouseOver);
                template.Triggers.Add(triggerIsPressed);


                buttonStyle.Setters.Add(new Setter { Property = Control.TemplateProperty, Value = template });
                buttonStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = objectStyle.Button_Foreground });
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(ButtonStyle));
            }
        }

        public ButtonStyle(int style, int l, int t, int r, int b)
        {
            try
            {
                eventLogger.LogEvent("Parameter constructor called.", typeof(ButtonStyle));

                dynamic objectStyle = new ReminderButtonStyle().ReturnObject(style);
                eventLogger.LogEvent("ReminderButtonStyle struct SolidColorBrushConverter method called.", typeof(ButtonStyle));
                eventLogger.LogEvent("ReminderButtonStyle struct ReturnObject method called.", typeof(ButtonStyle));

                var template = new ControlTemplate(typeof(Button));
                var border = new FrameworkElementFactory(typeof(Border));
                border.SetValue(Border.BackgroundProperty, objectStyle.Button_Static_Background);
                border.SetValue(Border.BorderBrushProperty, objectStyle.Button_Static_Border);
                border.SetValue(Border.BorderThicknessProperty, new Thickness(1.25));

                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(l,t,r,b));

                border.Name = "border";

                var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
                contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                border.AppendChild(contentPresenter);
                template.VisualTree = border;

                var triggerMouseOver = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                triggerMouseOver.Setters.Add(
                    new Setter { Property = Control.BackgroundProperty, Value = (SolidColorBrush)objectStyle.Button_MouseOver_Background, TargetName = "border" }
                );
                triggerMouseOver.Setters.Add(
                    new Setter { Property = Button.BorderBrushProperty, Value = objectStyle.Button_MouseOver_Border, TargetName = "border" }
                );


                var triggerIsPressed = new Trigger { Property = Button.IsPressedProperty, Value = true };
                triggerIsPressed.Setters.Add(
                    new Setter { Property = Button.BackgroundProperty, Value = objectStyle.Button_Pressed_Background, TargetName = "border" }
                );
                triggerIsPressed.Setters.Add(
                    new Setter { Property = Button.BorderBrushProperty, Value = objectStyle.Button_PressedBorder, TargetName = "border" }
                );

                template.Triggers.Add(triggerMouseOver);
                template.Triggers.Add(triggerIsPressed);


                buttonStyle.Setters.Add(new Setter { Property = Control.TemplateProperty, Value = template });
                buttonStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = objectStyle.Button_Foreground });
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(ButtonStyle));
            }
        }

        public Style? ReturnStyle()
        {

            try
            {
                eventLogger.LogEvent("ReturnStyle method called.", typeof(ButtonStyle));

                return buttonStyle;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(ButtonStyle));
                return null;
            }
        }
    }

    public struct ReminderButtonStyle
    {
        public object ButtonStyle1;
        public object ButtonStyle2;
        public object ButtonStyle3;
        public object ButtonStyle4;
        public object ButtonStyle5;
        public object ButtonStyle6;
        public EventLogger eventLogger = new EventLogger();

        public ReminderButtonStyle()
        {

            try
            {
                eventLogger.LogEvent("ReminderButtonStyle struct used.", typeof(ReminderButtonStyle));

                #region ButtonStyle1
                ButtonStyle1 = new //Urgent
                {
                    Button_Static_Background = SolidColorBrushConverter("#FC7753"),
                    Button_Static_Border = SolidColorBrushConverter("#FC7753"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#FF6A40"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#FF6A40"),
                    Button_Pressed_Background = SolidColorBrushConverter("#D5522E"),
                    Button_PressedBorder = SolidColorBrushConverter("#D5522E"),

                    Button_Foreground = new SolidColorBrush(Colors.White)
                };
                #endregion
                #region ButtonStyle2
                ButtonStyle2 = new //Imporatant
                {
                    Button_Static_Background = SolidColorBrushConverter("#F7B32B"),
                    Button_Static_Border = SolidColorBrushConverter("#F7B32B"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#EEA615"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#EEA615"),
                    Button_Pressed_Background = SolidColorBrushConverter("#DC9811"),
                    Button_PressedBorder = SolidColorBrushConverter("#DC9811"),

                    Button_Foreground = new SolidColorBrush(Colors.White)
                };
                #endregion
                #region ButtonStyle3
                ButtonStyle3 = new //Safe
                {
                    Button_Static_Background = SolidColorBrushConverter("#5BC65D"),
                    Button_Static_Border = SolidColorBrushConverter("#5BC65D"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#55B357"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#55B357"),
                    Button_Pressed_Background = SolidColorBrushConverter("#4FA550"),
                    Button_PressedBorder = SolidColorBrushConverter("#4FA550"),

                    Button_Foreground = new SolidColorBrush(Colors.White)
                };
                #endregion
                #region ButtonStyle4
                ButtonStyle4 = new //None
                {
                    Button_Static_Background = SolidColorBrushConverter("#A8C5E2"),
                    Button_Static_Border = SolidColorBrushConverter("#A8C5E2"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#92BCE6"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#92BCE6"),
                    Button_Pressed_Background = SolidColorBrushConverter("#81AAD3"),
                    Button_PressedBorder = SolidColorBrushConverter("#81AAD3"),

                    Button_Foreground = new SolidColorBrush(Colors.White)
                };
                #endregion
                #region ButtonStyle5
                ButtonStyle5 = new //Default
                {
                    Button_Static_Background = SolidColorBrushConverter("#c0c0c0"),
                    Button_Static_Border = SolidColorBrushConverter("#c0c0c0"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#a9a9a9"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#a9a9a9"),
                    Button_Pressed_Background = SolidColorBrushConverter("#808080"),
                    Button_PressedBorder = SolidColorBrushConverter("#808080"),

                    Button_Foreground = new SolidColorBrush(Colors.Black)
                };
                #endregion

                #region ButtonStyle6
                ButtonStyle6 = new //Default
                {
                    Button_Static_Background = SolidColorBrushConverter("#2E5266"),
                    Button_Static_Border = SolidColorBrushConverter("#2E5266"),
                    Button_MouseOver_Background = SolidColorBrushConverter("#406B82"),
                    Button_MouseOver_Border = SolidColorBrushConverter("#406B82"),
                    Button_Pressed_Background = SolidColorBrushConverter("#1E5C7D"),
                    Button_PressedBorder = SolidColorBrushConverter("#1E5C7D"),

                    Button_Foreground = new SolidColorBrush(Colors.White)
                };
                #endregion
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(ButtonStyle));
            }
        }

        public SolidColorBrush? SolidColorBrushConverter(string hex)
        {

            try
            {
                Color color = (Color)ColorConverter.ConvertFromString(hex);
                SolidColorBrush brush = new SolidColorBrush(color);
                return brush;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(ButtonStyle));
                return null;
            }
        }

        public object ReturnObject(int style)
        {
            try
            {
                switch (style)
                {
                    case 1:
                        if (CustomizeUserControl.colorblindMode == "rg") return ButtonStyle4;
                        else return ButtonStyle1;
                    case 2:
                        return ButtonStyle2;
                    case 3:
                        if (CustomizeUserControl.colorblindMode == "rg") return ButtonStyle5;
                        else return ButtonStyle3; 
                    case 4:
                        if (CustomizeUserControl.colorblindMode == "by") return ButtonStyle1;
                        else return ButtonStyle4;
                    case 5:
                        return ButtonStyle5;
                    case 6:
                        return ButtonStyle6;
                    default:
                        return ButtonStyle1;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogStaticError(ex.ToString(), typeof(ButtonStyle));
                return ButtonStyle1;
            }
        }
    }

    public class ReminderButton
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
