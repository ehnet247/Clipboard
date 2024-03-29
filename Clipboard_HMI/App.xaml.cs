﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Clipboard_HMI.Views;
using Clipboard_HMI.ViewModels;
using Clipboard_HMI.Models;
using System.Diagnostics;

namespace Clipboard_HMI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Try to open the Settings
            int expressionsCount = 0;
            try
            {
                UserSettings.Default.Reload();
                if (UserSettings.Default.Expressions != null)
                    expressionsCount = UserSettings.Default.Expressions.Count;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            // If no expressions are stored, open the Settings view
            if (expressionsCount == 0)
            {
                try
                {
                    var viewConfig = new ViewConfig();
                    var viewModelConfig = new ViewModelConfig(viewConfig);
                    viewConfig.DataContext = viewModelConfig;
                    viewConfig.ShowDialog();
                    bool? result = viewConfig.DialogResult;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    ViewMain viewMain = new ViewMain();
                    ViewModelMain viewModel = new ViewModelMain(viewMain);
                    viewMain.DataContext = viewModel;
                    viewMain.Show();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            // Save the user settings
        }
    }
}
