using Othello.Commands;
using Othello.ViewModels.Base;
using Othello.Views;
using Othello.Views.GameTiles;
using Othello.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Othello.ViewModels
{
    internal sealed class MainViewModel : BaseViewModel
    {
        public static MainViewModel? _instance;
        public BaseViewModel? CurrentViewModel { get; set; } = new StartViewModel();
        public static MainViewModel Instance { get => _instance ?? (_instance = new MainViewModel()); }
        public ICommand StartGameCommand { get; }
        public ICommand RulesPageCommand { get; }
        public ICommand StartPageCommand { get; }
        public ICommand QuitGameCommand { get; }


        private MainViewModel()
        {
            StartGameCommand = new RelayCommand(page => StartGame());
            RulesPageCommand = new RelayCommand(page => RulesPage());
            StartPageCommand = new RelayCommand(page => StartPage());
            QuitGameCommand = new RelayCommand(page => QuitPage());           
            

        }
        

        private void QuitPage()
        {
            Application.Current.Shutdown();
        }



        private void StartGame()
        {
            CurrentViewModel = new GameViewModel();
        }

        private void RulesPage()
        {
            CurrentViewModel = new RulesViewModel();
        }
        private void StartPage()
        {
            CurrentViewModel = new StartViewModel();
        }
    }
}
