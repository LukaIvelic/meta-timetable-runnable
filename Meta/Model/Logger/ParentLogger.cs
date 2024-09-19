using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Meta.Model.Logger
{
    public abstract class ParentLogger
    {
        public static string logPath = Directory.GetCurrentDirectory() + @"\logs";

        public ParentLogger() {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }

        public abstract void LogEvent(string message, Type location);

        public abstract void LogError(string message, Type location);

        public static async void LogStaticEvent(string message, Type location)
        {
            DateTime now = DateTime.Now;
            string directory = logPath + @"\events\events.txt";
            var lineCount = 0;
            using (var reader = File.OpenText(directory))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            string format = $"{now.ToString("ddd dd.MM.yyyy HH:mm:ss")} | [ EventLog number: {lineCount.ToString("D7")}] [ Event occured at: {location} with the message: {message} ]";
            format = File.ReadAllText(directory) + "\n" + format;

            using (FileStream fs = new FileStream(directory, FileMode.Open, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes(format));
            }

            return;
        }

        public static void LogStaticError(string message, Type location)
        {
            string directory = Directory.GetCurrentDirectory() + @$"\logs\errors\errors.txt";
            DateTime now = DateTime.Now;
            string format = $"{now.ToString("ddd dd.MM.yyyy HH:mm")} [ Event occured at: {location} with the message: {message.Remove('\n')}]";

            using (FileStream fs = new FileStream(directory, FileMode.Open, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes(format));
            }

            return;
        }
    }
}
