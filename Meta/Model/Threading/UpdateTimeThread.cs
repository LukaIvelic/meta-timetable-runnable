using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Meta.View;
using System.Globalization;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.IO;

namespace Meta.Model.Threading
{
    internal class UpdateTimeThread
    {
        private MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        public static DependencyProperty DateTimePartProperty = DependencyProperty.Register("DateTimePart", typeof(string), typeof(UpdateTimeThread), new PropertyMetadata(""));

        private string dateTime;
        public string DatetimePart { 
            get { return dateTime; }    
            set { dateTime = value; }
        }

        public UpdateTimeThread() {
            
        }   

        private Thread _thread;

        private void UpdateTime()
        {
            while (true)
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(UserControl5.filename));
                string format = settings.Format;

                string date = DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("en-GB"));
                string time = DateTime.Now.ToString($"{(format == "12" ? "hh" : "HH")}:mm", CultureInfo.CreateSpecificCulture("en-GB"));

                if (!UserControl5.TimeNav) time = "";
                if (!UserControl5.DateNav) date = "";

                string dateTime = $"{date}\n{time}";
                _mainWindow.Dispatcher.Invoke(new Action(() =>
                {
                    _mainWindow.TimeNow.Text = dateTime;
                }));
            }
        }

        public void StartThread()
        {
            _thread = new Thread(UpdateTime);
            _thread.Start();
        }

        public void StopThread()
        {
            _thread.Abort();
        }
    }
}