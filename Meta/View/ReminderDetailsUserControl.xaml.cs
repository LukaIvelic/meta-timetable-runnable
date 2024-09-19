using Meta.Model.Logger;
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
    /// Interaction logic for UserControl6.xaml
    /// </summary>
    public partial class UserControl6 : UserControl
    {
        private ReminderButton _reminderContents;
        private Style _buttonStyle;
        private string _reminderUid;
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public UserControl6()
        {
            try
            {
                eventLogger.LogEvent("0 parameter contructor called.", typeof(UserControl6));

                InitializeComponent();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl6));
            }
        }

        public UserControl6(ReminderButton fileObj, Style buttonStyle, string uid)
        {
            try
            {
                eventLogger.LogEvent("Parameter contructor called.", typeof(UserControl6));

                InitializeComponent();

                _reminderContents = fileObj;
                _buttonStyle = buttonStyle;
                _reminderUid = uid;

                WriteOutAllContent();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl6));
            }
        }

        private void WriteOutAllContent()
        {
            try
            {
                eventLogger.LogEvent("WriteOutAllContent method called.", typeof(UserControl6));

                #region TextBlock
                TextBlock detailsTitile = new TextBlock();
                detailsTitile.Text = _reminderContents.Title;
                detailsTitile.VerticalAlignment = VerticalAlignment.Top;
                detailsTitile.HorizontalAlignment= HorizontalAlignment.Left;
                detailsTitile.FontSize = 25;
                detailsTitile.FontWeight = FontWeights.SemiBold;
                detailsTitile.Margin = new Thickness(0,80,0,0);
                #endregion

                #region Border1
                Border border1 = new Border();
                border1.BorderThickness = new Thickness(1.25,0,0,0);

                switch (_reminderContents.Priority)
                {
                    case "Urgent":
                        border1.BorderBrush = new ReminderButtonStyle().SolidColorBrushConverter("#FC7753");
                        break;
                    case "Important":
                        border1.BorderBrush = new ReminderButtonStyle().SolidColorBrushConverter("#F7B32B");
                        break;
                    case "Safe":
                        border1.BorderBrush = new ReminderButtonStyle().SolidColorBrushConverter("#5BC65D");
                        break;
                    case "None":
                        border1.BorderBrush = new ReminderButtonStyle().SolidColorBrushConverter("#A8C5E2");
                        break;
                }

                border1.Height = 400;
                #endregion

                #region MainGrid
                Grid mainGrid = new Grid();
                mainGrid.Height = 450;
                mainGrid.Margin = new Thickness(17,0,0,0);
                #endregion

                #region Stackpanel
                StackPanel stackPanel = new StackPanel();
                Style stackpanelStyle = new Style(typeof(StackPanel));
                stackpanelStyle.Setters.Add(new Setter { Property = Control.MarginProperty, Value = new Thickness(10, 10, 0, 20) } );
                stackPanel.Resources.Add(typeof(StackPanel), stackpanelStyle);
                #endregion

                #region Grid1
                Label priorityLabel = new Label();
                priorityLabel.Content = "Priority";
                priorityLabel.VerticalAlignment= VerticalAlignment.Top;
                priorityLabel.HorizontalAlignment = HorizontalAlignment.Left;
                priorityLabel.FontSize = 19;
                priorityLabel.FontWeight = FontWeights.SemiBold;

                Label priorityContent = new Label();
                priorityContent.Content = _reminderContents.Priority;
                priorityContent.VerticalAlignment= VerticalAlignment.Top;
                priorityContent.HorizontalAlignment = HorizontalAlignment.Left;
                priorityContent.FontSize = 19;
                priorityContent.FontWeight = FontWeights.SemiBold;
                priorityContent.Foreground = border1.BorderBrush;

                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Orientation = Orientation.Horizontal;
                stackPanel1.Children.Add( priorityLabel );
                stackPanel1.Children.Add( priorityContent );

                Grid myGrid1 = new Grid();
                myGrid1.Children.Add(stackPanel1);
                #endregion

                #region Grid2
                Label descriptionLabel = new Label();
                descriptionLabel.Content = "Description";
                descriptionLabel.VerticalAlignment = VerticalAlignment.Top;
                descriptionLabel.HorizontalAlignment = HorizontalAlignment.Left;
                descriptionLabel.FontSize = 19;
                descriptionLabel.FontWeight = FontWeights.SemiBold;

                TextBox descriptionContent = new TextBox();
                descriptionContent.Text = _reminderContents.Description;
                descriptionContent.TextWrapping = TextWrapping.Wrap;
                descriptionContent.VerticalAlignment = VerticalAlignment.Top;
                descriptionContent.HorizontalAlignment = HorizontalAlignment.Left;
                descriptionContent.FontSize = 15;
                descriptionContent.FontWeight = FontWeights.SemiBold;
                descriptionContent.BorderThickness = new Thickness(0);
                descriptionContent.Padding = new Thickness(5,0,300,0);
                descriptionContent.TextAlignment = TextAlignment.Justify;

                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Children.Add(descriptionLabel);
                stackPanel2.Children.Add(descriptionContent);

                Grid myGrid2 = new Grid();
                myGrid2.Children.Add(stackPanel2);
                #endregion

                #region Grid3
                Label dateLabel = new Label();
                dateLabel.Content = "Set date";
                dateLabel.VerticalAlignment = VerticalAlignment.Top;
                dateLabel.HorizontalAlignment = HorizontalAlignment.Left;
                dateLabel.FontSize = 19;
                dateLabel.FontWeight = FontWeights.SemiBold;

                TextBox dateContent = new TextBox();
                dateContent.Text = $"{_reminderContents.Date} at {_reminderContents.Time}";
                dateContent.TextWrapping = TextWrapping.Wrap;
                dateContent.VerticalAlignment = VerticalAlignment.Top;
                dateContent.HorizontalAlignment = HorizontalAlignment.Left;
                dateContent.FontSize = 15;
                dateContent.FontWeight = FontWeights.SemiBold;
                dateContent.BorderThickness = new Thickness(0);
                dateContent.Padding = new Thickness(5, 0, 300, 0);
                dateContent.TextAlignment = TextAlignment.Justify;

                StackPanel stackPanel3 = new StackPanel();
                stackPanel3.Children.Add(dateLabel);
                stackPanel3.Children.Add(dateContent);

                Grid myGrid3 = new Grid();
                myGrid3.Children.Add(stackPanel3);
                #endregion

                #region Grid4
                Label dateUntilLabel = new Label();
                dateUntilLabel.Content = "Days until set date";
                dateUntilLabel.VerticalAlignment = VerticalAlignment.Top;
                dateUntilLabel.HorizontalAlignment = HorizontalAlignment.Left;
                dateUntilLabel.FontSize = 19;
                dateUntilLabel.FontWeight = FontWeights.SemiBold;

                TextBox dateUntilContent = new TextBox();
                dateUntilContent.Text = $"{DateTime.Parse(_reminderContents.Date).DayOfYear-DateTime.Now.DayOfYear}";
                dateUntilContent.TextWrapping = TextWrapping.Wrap;
                dateUntilContent.VerticalAlignment = VerticalAlignment.Top;
                dateUntilContent.HorizontalAlignment = HorizontalAlignment.Left;
                dateUntilContent.FontSize = 15;
                dateUntilContent.FontWeight = FontWeights.SemiBold;
                dateUntilContent.BorderThickness = new Thickness(0);
                dateUntilContent.Padding = new Thickness(5, 0, 300, 0);
                dateUntilContent.TextAlignment = TextAlignment.Justify;

                StackPanel stackPanel4 = new StackPanel();
                stackPanel4.Children.Add(dateUntilLabel);
                stackPanel4.Children.Add(dateUntilContent);

                Grid myGrid4 = new Grid();
                myGrid4.Children.Add(stackPanel4);
                #endregion

                #region Grid5
                Button myButton = new Button();
                myButton.Content = "Delete";
                myButton.Style = _buttonStyle;
                myButton.FontSize= 15;
                myButton.Height = 40;
                myButton.Width = 100;
                myButton.Margin = new Thickness(10,15,0,0);
                myButton.HorizontalAlignment = HorizontalAlignment.Left;
                myButton.Click += DeleteReminder;

                Grid myGrid5 = new Grid();
                myGrid5.Children.Add(myButton);
                #endregion

                stackPanel.Children.Add(myGrid1);
                stackPanel.Children.Add(myGrid2);
                stackPanel.Children.Add(myGrid3);
                stackPanel.Children.Add(myGrid4);
                stackPanel.Children.Add(myGrid5);
                mainGrid.Children.Add(stackPanel);

                DetailsGrid.Children.Add(detailsTitile);
                DetailsGrid.Children.Add(border1);
                DetailsGrid.Children.Add(mainGrid);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl6));
            }
        }

        private void DeleteReminder(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("DeleteReminder method called.", typeof(UserControl6));

                string fullpath = Directory.GetCurrentDirectory() + $@"\Reminders\mtreminder_{_reminderUid}.json";

                if (!UserControl5.Delete)
                {
                    File.Delete(fullpath);
                    ReturnToSelection(sender, e);
                    return;
                }

                MessageBoxResult r = MessageBox.Show("Are you sure you want to delete the file?", "Delete file?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (r == MessageBoxResult.Yes)
                {
                    File.Delete(fullpath);
                    ReturnToSelection(sender, e);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl6));
            }
        }

        private void ReturnToSelection(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ReturnToSelection method called.", typeof(UserControl6));

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ContentControlElement.Content = new UserControl3();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl6));
            }
        }
    }
}
