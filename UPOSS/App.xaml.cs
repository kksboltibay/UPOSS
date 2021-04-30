﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UPOSS.Services;
using UPOSS.State;
using UPOSS.ViewModels;

using Squirrel;

namespace UPOSS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        UpdateManager manager;

        protected override void OnStartup(StartupEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();
            base.OnStartup(e);

            CheckAvailableUpdate();
        }

        private async void CheckAvailableUpdate()
        {
            manager = await UpdateManager.GitHubUpdateManager(@"https://github.com/kksboltibay/UPOSS-Frontend");

            //CurrentVersionTextBox.Text = manager.CurrentlyInstalledVersion().ToString();

            var updateInfo = await manager.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
            {
                var update = MessageBox.Show("There is an update available", "UPO$$", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (update == MessageBoxResult.Yes)
                {
                    await manager.UpdateApp();

                    MessageBox.Show("Updated succesfuly!");
                }
            }
        }
    }
}