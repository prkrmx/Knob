using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnobUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

            ThemeManager.AddAppTheme("DarkTheme", new Uri("pack://application:,,,/KnobUI;component/Style/DarkTheme.xaml"));
            ThemeManager.AddAppTheme("LightTheme", new Uri("pack://application:,,,/KnobUI;component/Style/LightTheme.xaml"));


            base.OnStartup(e);

        }
    }
}
