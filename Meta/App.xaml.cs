using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Meta.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Meta.View;
using Meta.ViewModel;
using System.IO;
using Newtonsoft.Json;

namespace Meta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }

        public void ShutdownProp(object sender, ExitEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
