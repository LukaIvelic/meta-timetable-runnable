using Meta.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Meta.Model.Logger
{
    public class ErrorLogger : ParentLogger
    {
        private string fullPath = logPath + @"\errors\errors.txt";

        public ErrorLogger() : base() { 
            if(!Directory.Exists(fullPath.Replace("\\errors.txt", "")))
            {
                Directory.CreateDirectory(fullPath.Replace("\\errors.txt", ""));
            }
            if(!File.Exists(fullPath))
            {
                FileStream fs = File.Create(fullPath);
                fs.Close();
            }
        }

        public override async void LogError(string message, Type eventLocation)
        {
            if (!UserControl5.ErrorLogger) return;

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

        public override void LogEvent(string message, Type location)
        {
            throw new NotImplementedException();
        }
    }
}
