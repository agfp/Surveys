using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace ZeusDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var home = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Zeus");
            var filename = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];

            try
            {
                if (!Directory.Exists(home))
                {
                    Directory.CreateDirectory(home);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao criar a pasta " + home, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            if (!String.IsNullOrEmpty(filename))
            {
                if (File.Exists(filename))
                {
                    MainWindow main = new MainWindow(filename);
                    main.Show();
                    return;
                }
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
