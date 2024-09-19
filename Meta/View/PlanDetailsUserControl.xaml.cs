using Meta.Model.Logger;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
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
using Microsoft.VisualBasic.FileIO;

namespace Meta.View
{
    /// <summary>
    /// Interaction logic for UserControl7.xaml
    /// </summary>
    public partial class UserControl7 : UserControl
    {
        private PlanButton _planContents;
        private Style _buttonStyle;
        private string _planUid;
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public UserControl7()
        {
            try
            {
                eventLogger.LogEvent("0 parameter constructor called.", typeof(UserControl7));
                InitializeComponent();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl7));
            }
        }

        public UserControl7(PlanButton fileObj, Style buttonStyle, string uid)
        {
            try
            {
                eventLogger.LogEvent("Parameter constructor called.", typeof(UserControl7));

                InitializeComponent();

                _planContents= fileObj;
                _buttonStyle= buttonStyle;
                _planUid= uid;

                WriteOutAllContent();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl7));
            }
        }

        private void WriteOutAllContent()
        {
            try
            {
                eventLogger.LogEvent("WriteOutAllContent method called.", typeof(UserControl7));

                #region MainGrid
                Grid mainGrid = new Grid();
                mainGrid.Background = Brushes.White;
                mainGrid.Margin = new Thickness(50);
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                mainGrid.RowDefinitions.Add(new RowDefinition());
                #endregion

                #region SubGrid1
                Grid subGrid1 = new Grid();
                Grid.SetRow(subGrid1, 0);
                subGrid1.Background = Brushes.White;
                #endregion

                #region TextBlock
                TextBlock textBlockTitle = new TextBlock();
                textBlockTitle.VerticalAlignment = VerticalAlignment.Center;
                textBlockTitle.HorizontalAlignment = HorizontalAlignment.Left;
                textBlockTitle.FontSize = 25;
                textBlockTitle.FontWeight = FontWeights.SemiBold;
                textBlockTitle.Text = $"{(_planContents.Title == string.Empty ? "Not set" : _planContents.Title)}  •  {(_planContents.MainEvent == string.Empty ? "Not set" : _planContents.MainEvent)}  •  Set for {_planContents.Date}";
                #endregion

                #region SubGrid2
                Grid subGrid2 = new Grid();
                Grid.SetRow(subGrid2, 1);
                #endregion

                #region StackPanel
                StackPanel stackPanel = new StackPanel();
                #endregion

                #region TextBlock
                TextBlock textBlockDescription = new TextBlock();
                textBlockDescription.VerticalAlignment = VerticalAlignment.Center;
                textBlockDescription.HorizontalAlignment = HorizontalAlignment.Left;
                textBlockDescription.FontSize = 19;
                textBlockDescription.FontWeight = FontWeights.SemiBold;
                textBlockDescription.Text = "Decription";
                #endregion

                #region TextBox
                TextBox textBox = new TextBox();
                textBox.Padding = new Thickness(0, 0, 300, 20);
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.Margin = new Thickness(0, 10, 0, 0);
                textBox.VerticalAlignment = VerticalAlignment.Center;
                textBox.HorizontalAlignment = HorizontalAlignment.Left;
                textBox.FontSize = 15;
                textBox.FontWeight = FontWeights.SemiBold;
                textBox.BorderThickness = new Thickness(0);
                textBox.Height = 70;
                textBox.Text = (_planContents.Description == null ? "Not set" : _planContents.Description);
                #endregion

                #region PlanGrid
                Grid planGrid = new Grid();
                for (int i = 0; i < 4; i++) planGrid.RowDefinitions.Add(new RowDefinition());
                #endregion

                #region 4PlanGrids
                Grid planGrid1 = new Grid();
                Grid planGrid2 = new Grid();
                Grid planGrid3 = new Grid();
                Grid planGrid4 = new Grid();

                Grid.SetRowSpan(planGrid1, 1);
                Grid.SetRowSpan(planGrid2, 2);
                Grid.SetRowSpan(planGrid3, 3);
                Grid.SetRowSpan(planGrid4, 4);
                #endregion

                #region Times
                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Margin = new Thickness(0, 5, 0, 0);
                stackPanel1.Orientation = Orientation.Horizontal;

                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Margin = new Thickness(0, 5, 0, 0);
                stackPanel2.Orientation = Orientation.Horizontal;

                StackPanel stackPanel3 = new StackPanel();
                stackPanel3.Margin = new Thickness(0, 5, 0, 0);
                stackPanel3.Orientation = Orientation.Horizontal;

                StackPanel stackPanel4 = new StackPanel();
                stackPanel4.Margin = new Thickness(0, 5, 0, 0);
                stackPanel4.Orientation = Orientation.Horizontal;

                List<Hour> timeList = new List<Hour>();
                foreach (Hour o in _planContents.Morning.Values)
                {
                    timeList.Add(o);
                }
                foreach (Hour o in _planContents.Day.Values)
                {
                    timeList.Add(o);
                }
                foreach (Hour o in _planContents.Night.Values)
                {
                    timeList.Add(o);
                }

                int currRow = 1;
                for (int i = 0; i < timeList.Count; i++)
                {

                    Grid timeGrid = new Grid();
                    timeGrid.Height = 85;
                    timeGrid.Width = 200;
                    timeGrid.Margin = new Thickness(0, 0, 5, 0);

                    Border timeBorder = new Border();

                    switch (timeList[i].Priority)
                    {
                        case "Urgent":
                            dynamic objectStyle = new ReminderButtonStyle().ButtonStyle1;

                            if(CustomizeUserControl.colorblindMode == "rg") objectStyle = new ReminderButtonStyle().ButtonStyle4;

                            timeBorder.Background = objectStyle.Button_Static_Background;

                            break;
                        case "Important":
                            objectStyle = new ReminderButtonStyle().ButtonStyle2;
                            timeBorder.Background = objectStyle.Button_Static_Background;
                            break;
                        case "Safe":
                            objectStyle = new ReminderButtonStyle().ButtonStyle3;

                            if (CustomizeUserControl.colorblindMode == "rg") objectStyle = new ReminderButtonStyle().ButtonStyle5;

                            timeBorder.Background = objectStyle.Button_Static_Background;
                            break;
                        case "None":

                            objectStyle = new ReminderButtonStyle().ButtonStyle4;

                            if (CustomizeUserControl.colorblindMode == "by") objectStyle = new ReminderButtonStyle().ButtonStyle1;

                            timeBorder.Background = objectStyle.Button_Static_Background;
                            break;
                    }

                    timeBorder.CornerRadius = new CornerRadius(5);

                    Label timeLabel = new Label();
                    timeLabel.Foreground = Brushes.White;
                    timeLabel.FontSize = 19;
                    timeLabel.VerticalAlignment = VerticalAlignment.Center;
                    timeLabel.Margin = new Thickness(5);
                    timeLabel.Content = $"{6 + i:D2}:00";

                    TextBlock timeBlock = new TextBlock();
                    timeBlock.MaxHeight = 60;
                    timeBlock.MaxWidth = 80;
                    timeBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    timeBlock.VerticalAlignment = VerticalAlignment.Center;
                    timeBlock.Margin = new Thickness(0, -5, 0, 0);
                    timeBlock.TextWrapping = TextWrapping.Wrap;
                    //timeBlock.Background = Brushes.Black;
                    timeBlock.Foreground = Brushes.White;
                    timeBlock.TextAlignment = TextAlignment.Center;
                    timeBlock.Padding = new Thickness(1);
                    timeBlock.Text = (timeList[i].Event == null ? "Not set" : timeList[i].Event);
                    timeBlock.FontWeight = (timeList[i].Event == null ? FontWeights.Normal : FontWeights.Bold);

                    timeGrid.Children.Add(timeBorder);
                    timeGrid.Children.Add(timeLabel);
                    timeGrid.Children.Add(timeBlock);

                    if (i < 5)
                    {
                        stackPanel1.Children.Add(timeGrid);
                    }
                    else if (i < 10)
                    {
                        stackPanel2.Children.Add(timeGrid);

                    }
                    else if (i < 15)
                    {
                        stackPanel3.Children.Add(timeGrid);

                    }
                    else if (i < 20)
                    {
                        stackPanel4.Children.Add(timeGrid);
                    }
                }

                #endregion

                #region DeleteButton & BackButton
                Button deleteButton = new Button();
                deleteButton.Style = new ButtonStyle(5).ReturnStyle();
                deleteButton.Content = "Delete";
                deleteButton.FontSize = 15;
                deleteButton.Height = 40;
                deleteButton.Width = 100;
                deleteButton.HorizontalAlignment = HorizontalAlignment.Right;
                deleteButton.VerticalAlignment = VerticalAlignment.Center;
                deleteButton.Margin = new Thickness(0,0,100,0);
                deleteButton.Uid = _planContents.Uid;
                deleteButton.Foreground = Brushes.White;
                deleteButton.Click += DeletePlan;

                Button backButton = new Button();
                backButton.Style = new ButtonStyle(5).ReturnStyle();
                backButton.Content = "Back";
                backButton.FontSize = 15;
                backButton.Height = 40;
                backButton.Width = 100;
                backButton.HorizontalAlignment = HorizontalAlignment.Right;
                backButton.VerticalAlignment = VerticalAlignment.Bottom;
                backButton.Foreground = Brushes.White;
                backButton.Click += (object s, RoutedEventArgs e) => {
                    var mw = (MainWindow)Application.Current.MainWindow;
                    mw.ContentControlElement.Content = new UserControl4();
                };
                #endregion

                subGrid1.Children.Add(textBlockTitle);

                stackPanel.Children.Add(textBlockDescription);
                stackPanel.Children.Add(textBox);

                planGrid1.Children.Add(stackPanel1);
                planGrid2.Children.Add(stackPanel2);
                planGrid3.Children.Add(stackPanel3);
                planGrid4.Children.Add(stackPanel4);

                stackPanel.Children.Add(planGrid1);
                stackPanel.Children.Add(planGrid2);
                stackPanel.Children.Add(planGrid3);
                stackPanel.Children.Add(planGrid4);

                subGrid2.Children.Add(stackPanel);

                mainGrid.Children.Add(subGrid1);
                mainGrid.Children.Add(subGrid2);
                mainGrid.Children.Add(deleteButton);

                PlanContentGrid.Children.Add(mainGrid);
                PlanContentGrid.Children.Add(backButton);
            }
            catch(Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl7));
            }
        }

        private void DeletePlan(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("DeletePlan method called.", typeof(UserControl7));

                string filename = @$"{Directory.GetCurrentDirectory()}\Plans\mtplan_{(sender as Button).Uid}.json";

                if (!UserControl5.Delete)
                {
                    File.Delete(filename);
                    ReturnToSelection(sender, e);
                    return;
                }

                MessageBoxResult r = MessageBox.Show("Are you sure you want to delete the file?", "Delete file?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (r == MessageBoxResult.Yes)
                {
                    File.Delete(filename);
                    ReturnToSelection(sender, e);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl7));
            }
        }

        private void ReturnToSelection(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("ReturnToSelection method called.", typeof(UserControl7));

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ContentControlElement.Content = new UserControl4();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(UserControl7));
            }
        }
    }
}

/*
 <Grid.RowDefinitions>
            <RowDefinition Height="100"/>
            <RowDefinition/>
        </Grid.RowDefinitions>

        <Grid Grid.Row="0" Background="White">
            <TextBlock Text="Plan dana za maturu iz matematike  •  Main Event here" VerticalAlignment="Center" HorizontalAlignment="Left" FontSize="25" FontWeight="SemiBold"></TextBlock>
        </Grid>

        <Grid Grid.Row="1">
            <StackPanel>
                <TextBlock Text="Description" VerticalAlignment="Center" HorizontalAlignment="Left" FontSize="19" FontWeight="SemiBold"></TextBlock>
                <TextBox Padding="0,0,300,0" TextWrapping="Wrap" Margin="0,10,0,0" HorizontalAlignment="Left" VerticalAlignment="Center" FontSize="15" BorderThickness="0" FontWeight="SemiBold" Height="100"></TextBox>

                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition/>
                        <RowDefinition/>
                        <RowDefinition/>
                        <RowDefinition/>
                    </Grid.RowDefinitions>
                </Grid>

                <Grid Grid.Row="0">
                    <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                        <StackPanel.Resources>
                            <Style TargetType="Grid">
                                <Setter Property="Margin" Value="0,0,5,0"/>
                            </Style>
                        </StackPanel.Resources>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">07:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-5,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">08:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">09:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">10:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">11:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                    </StackPanel>
                </Grid>

                <Grid Grid.Row="1">
                    <StackPanel Orientation="Horizontal" Margin="0,10,0,0">
                        <StackPanel.Resources>
                            <Style TargetType="Grid">
                                <Setter Property="Margin" Value="0,0,5,0"/>
                            </Style>
                        </StackPanel.Resources>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">12:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">13:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">14:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">15:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">16:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                    </StackPanel>
                </Grid>

                <Grid Grid.Row="2">
                    <StackPanel Orientation="Horizontal" Margin="0,10,0,0">
                        <StackPanel.Resources>
                            <Style TargetType="Grid">
                                <Setter Property="Margin" Value="0,0,5,0"/>
                            </Style>
                        </StackPanel.Resources>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">17:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">18:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">19:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">20:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">21:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                    </StackPanel>
                </Grid>
                
                <Grid Grid.Row="3">
                    <StackPanel Orientation="Horizontal" Margin="0,10,0,0">
                        <StackPanel.Resources>
                            <Style TargetType="Grid">
                                <Setter Property="Margin" Value="0,0,5,0"/>
                            </Style>
                        </StackPanel.Resources>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">22:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">05:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                        <Grid Height="85" Width="200">
                            <Border Background="#A8C5E2" CornerRadius="5"></Border>
                            <Label Foreground="White" FontSize="19" VerticalAlignment="Center" Margin="5">06:00</Label>
                            <TextBlock Height="60" Width="125" HorizontalAlignment="Right" Margin="0,-10,0,0" TextWrapping="Wrap" Background="Transparent" Foreground="White" TextAlignment="Left" Padding="1" Text="Ljubav ono ljubav ovo, kroz ovo sve da prolazim, u noc me zovu ulice, ja nigde"></TextBlock>
                        </Grid>
                    </StackPanel>
                </Grid>
            </StackPanel>
        </Grid>
 */