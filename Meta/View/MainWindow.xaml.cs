using Meta.Model;
using Meta.Model.Logger;
using Meta.Model.Threading;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Meta.View
{

    public partial class MainWindow : Window
    {
        #region Blur Effect enums and structs
        internal enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }
        #endregion
        #region DLL Imports
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        #endregion
        #region INotifyCollectionChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            eventLogger.LogEvent("OnPropertyChanged method called.", typeof(MainWindow));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public SolidColorBrush BackgroundColor
        {
            get { return Brushes.White; }
            set { OnPropertyChanged(nameof(BackgroundColor)); }
        }
        public Dictionary<Type, string> correspondingNames = new Dictionary<Type, string>();
        Point lastMousePosition;
        public EventLogger eventLogger = new EventLogger();
        public ErrorLogger errorLogger = new ErrorLogger();

        public MainWindow()
        {
            try
            {
                eventLogger.LogEvent("A constructor called.", typeof(MainWindow));

                InitializeComponent();

                DataContext = this;

                MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

                correspondingNames.Add(typeof(UserControl1), "  •  Create reminder");
                correspondingNames.Add(typeof(UserControl2), "  •  Create plan");
                correspondingNames.Add(typeof(UserControl3), "  •  Reminder list");
                correspondingNames.Add(typeof(UserControl4), "  •  Plan list");
                correspondingNames.Add(typeof(UserControl5), "  •  Settings");
                correspondingNames.Add(typeof(CustomizeUserControl), "  •  Customize");

                ContentControlElement.Content = new UserControl1();
                MetaWindowName.Text =(CustomizeUserControl.Content == true ? "Meta Timetable" + correspondingNames[ContentControlElement.Content.GetType()] : "");

                new CustomizeUserControl();

                if (CustomizeUserControl.Location == "location_cs") WindowStartupLocation = WindowStartupLocation.CenterScreen;
                else WindowStartupLocation = WindowStartupLocation.CenterOwner;

                Panel fileObj = JsonConvert.DeserializeObject<Panel>(File.ReadAllText(CustomizeUserControl.panelFilename));

                Color hexToColor(string hex)
                {
                    return (Color)ColorConverter.ConvertFromString(hex);
                }

                NavigationPanel.Color = hexToColor(fileObj.hexCode);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        public void StrartBackgroundProcess(object sender, EventArgs e)
        {
            try
            {
                eventLogger.LogEvent("StartBackgroundProcess method called.", typeof(MainWindow));

                EnableBlur(sender, e as RoutedEventArgs);
                new UserControl5();
                Settings fileObj = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(UserControl5.filename));

                if (fileObj.Minimize) WindowState = WindowState.Minimized;
                else WindowState = WindowState.Normal;

                try
                {
                    UpdateTimeThread _thread = new UpdateTimeThread();
                    _thread.StartThread();
                }
                catch (Exception ex) {
                    errorLogger.LogError(ex.ToString(), typeof(MainWindow));
                }

                void Window_Loaded()
                {
                    try
                    {
                        eventLogger.LogEvent("Window_Loaded local method called.", typeof(MainWindow));

                        int nWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
                        int nHieght = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

                        this.LayoutTransform = new ScaleTransform(nWidth / 2560, nHieght / 1440);
                    }
                    catch (Exception ex)
                    {
                        errorLogger.LogError(ex.ToString(), typeof(MainWindow));
                    }
                }
                Window_Loaded();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        #region Window-related
        public void EnableBlur(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("EnableBlur method called.", typeof(MainWindow));

                var windowHelper = new WindowInteropHelper(this);

                var accent = new AccentPolicy();
                accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

                var accentStructSize = Marshal.SizeOf(accent);

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData();
                data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;

                SetWindowCompositionAttribute(windowHelper.Handle, ref data);

                Marshal.FreeHGlobal(accentPtr);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        public void EnableBlur()
        {
            try
            {
                eventLogger.LogEvent("EnableBlur method called.", typeof(MainWindow));

                var windowHelper = new WindowInteropHelper(this);

                var accent = new AccentPolicy();
                accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

                var accentStructSize = Marshal.SizeOf(accent);

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData();
                data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;

                SetWindowCompositionAttribute(windowHelper.Handle, ref data);

                Marshal.FreeHGlobal(accentPtr);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        public void DisableBlur()
        {
            try
            {
                eventLogger.LogEvent("DisableBlur method called.", typeof(MainWindow));

                var windowHelper = new WindowInteropHelper(this);

                var accent = new AccentPolicy();
                accent.AccentState = AccentState.ACCENT_DISABLED;

                var accentStructSize = Marshal.SizeOf(accent);

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData();
                data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;

                SetWindowCompositionAttribute(windowHelper.Handle, ref data);

                Marshal.FreeHGlobal(accentPtr);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        public void Drag(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        public void TurnToPageModel(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("TurnToPageModel method called.", typeof(MainWindow));

                var btn = (Button)sender;

                switch(btn.Uid)
                {
                    case "1":
                        ContentControlElement.Content = new UserControl1();
                        break;
                    case "2":
                        ContentControlElement.Content = new UserControl2();
                        break;
                    case "3":
                        ContentControlElement.Content = new UserControl3();
                        break;
                    case "4":
                        ContentControlElement.Content = new UserControl4();
                        break;
                    case "5":
                        ContentControlElement.Content = new UserControl5();
                        break;
                    case "6":
                        ContentControlElement.Content = new CustomizeUserControl();
                        break;
                };

                MetaWindowName.Text = (CustomizeUserControl.Content == true ? "Meta Timetable" + correspondingNames[ContentControlElement.Content.GetType()] : "");
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }
        #endregion

        #region Window Control Buttons
        private void Shutdown(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Shutdown method called.", typeof(MainWindow));

                Environment.Exit(0);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Minimize method called.", typeof(MainWindow));

                WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Maximize method called.", typeof(MainWindow));

                if (WindowState == WindowState.Normal)
                {
                    WindowState = WindowState.Maximized;
                    BorderThickness = new Thickness(0);
                    SidebarOptionsStackPanel.VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    WindowState = WindowState.Normal;
                    BorderThickness = new Thickness(0);
                    SidebarOptionsStackPanel.VerticalAlignment = VerticalAlignment.Center;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }
        #endregion

        #region Mouse-related
        private void CustomResizeGrip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                eventLogger.LogEvent("CustomResizeGrip_MouseLeftButtonDown method called.", typeof(MainWindow));

                Mouse.Capture(CustomResizeGrip);
                lastMousePosition = e.GetPosition(this);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        private void CustomResizeGrip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            try
            {
                eventLogger.LogEvent("CustomResizeGrip_MouseLeftButtonUp method called.", typeof(MainWindow));

                Mouse.Capture(null);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                eventLogger.LogEvent("Window_MouseMove method called.", typeof(MainWindow));

                if (CustomResizeGrip.IsMouseCaptured)
                {
                    Point currentMousePosition = e.GetPosition(this);
                    double deltaX = currentMousePosition.X - lastMousePosition.X;
                    double deltaY = currentMousePosition.Y - lastMousePosition.Y;

                    Width += deltaX;
                    Height += deltaY;

                    lastMousePosition = currentMousePosition;
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex.ToString(), typeof(MainWindow));
            }
        }
        #endregion
    }
}
