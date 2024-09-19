using Meta.View;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meta.Model.Logger
{
    public class EventLogger : ParentLogger
    {
        private string fullPath = logPath + @"\events\events.txt";

        public EventLogger() : base() {
            if (!Directory.Exists(fullPath.Replace("\\events.txt", "")))
            {
                Directory.CreateDirectory(fullPath.Replace("\\events.txt", ""));
            }
            if (!File.Exists(fullPath))
            {
                FileStream fs = File.Create(fullPath);
                fs.Close();
            }
        }

        public override async void LogEvent(string message, Type eventLocation)
        {
            if (!UserControl5.EventLogger) return;

            DateTime now = DateTime.Now;
            var lineCount = 0;
            using (var reader = File.OpenText(fullPath))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            string format = $"{now.ToString("ddd dd.MM.yyyy HH:mm:ss")} | [ EventLog number: {lineCount.ToString("D7")}] [ Event occured at: {eventLocation} with the message: {message} ]";
            format = File.ReadAllText(fullPath) + "\n" + format;

            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes(format));
            }

            return;
        }

        public override void LogError(string message, Type location)
        {
            throw new NotImplementedException();
        }
    }
}
