﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using UPOSS.Commands;
using UPOSS.Models;
using UPOSS.Services;
using UPOSS.State;

namespace UPOSS.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        APIService ObjAuthService;
        private string _UserPath;
        private string _BranchPath;

        public AuthViewModel()
        {
            ObjAuthService = new APIService();
            _UserPath = "user";
            _BranchPath = "branch";

            loginCommand = new RelayCommand(Login);
            exitCommand = new RelayCommand(Exit);

            InputUser = new User();
            SelectedBranch = Properties.Settings.Default.CurrentBranch;

            GetLoginBranchList();
        }

        #region Define
        public event EventHandler LoginCompleted;
        protected virtual void OnLoginCompleted(EventArgs e)
        {
            EventHandler handler = LoginCompleted;
            handler?.Invoke(this, e);
        }

        private ObservableCollection<string> activeBranchList;
        public ObservableCollection<string> ActiveBranchList
        {
            get { return activeBranchList; }
            set { activeBranchList = value; OnPropertyChanged("ActiveBranchList"); }
        }

        private User inputUser;
        public User InputUser
        {
            get { return inputUser; }
            set { inputUser = value; OnPropertyChanged("InputUser"); }
        }

        private string selectedBranch;
        public string SelectedBranch
        {
            get { return selectedBranch; }
            set { selectedBranch = value; OnPropertyChanged("SelectedBranch"); }
        }
        #endregion


        #region CustomOperation
        private async void GetLoginBranchList()
        {
            try
            {
                dynamic param = new { page = 0 };

                RootBranchObject Response = await ObjAuthService.PostAPI("getLoginBranchList", param, _BranchPath);

                if (Response.Status == "ok")
                {
                    ActiveBranchList = new ObservableCollection<string>(Response.Data.OrderBy(property => property.Name).Select(item => item.Name));
                }
                else
                {
                    MessageBox.Show(Response.Msg, "UPO$$");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "UPO$$");
            }
        }
        #endregion


        #region Login
        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get { return loginCommand; }
        }
        private async void Login()
        {
            try
            {
                dynamic param = new { username = InputUser.Username, password = InputUser.Password, branchName = SelectedBranch };

                RootUserObject Response = await ObjAuthService.PostAPI("login", param, _UserPath);

                if (Response.Status != "ok")
                {
                    MessageBox.Show(Response.Msg, "UPO$$");
                }
                else
                {
                    Properties.Settings.Default.CurrentUsername = Response.Data[0].Username;
                    Properties.Settings.Default.CurrentBranch = SelectedBranch;
                    Properties.Settings.Default.CurrentUserRole = Response.Data[0].Role;
                    Properties.Settings.Default.Save();

                    MessageBox.Show(Response.Msg, "UPO$$");

                    //change viewModel to Dashboard screen
                    OnLoginCompleted(EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "UPO$$");
            }
        }
        #endregion


        #region Exit
        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get { return exitCommand; }
        }
        private void Exit()
        {
            var msgBoxResult = MessageBox.Show("Do you want to exit UPO$$ ?", "UPO$$", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (msgBoxResult == MessageBoxResult.Yes)
            {
                // shut down
                Application.Current.Shutdown();
            }
        }
        #endregion
    }
}