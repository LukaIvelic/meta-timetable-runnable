using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Win32;
using Caliburn.Micro;
using System.Windows;
using Meta.View;
using Meta.Model;
using Microsoft.Extensions.Hosting;

namespace Meta.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentControlElement.Content = new UserControl1();
        }

    }
}
