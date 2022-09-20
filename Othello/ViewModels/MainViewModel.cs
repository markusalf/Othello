using Othello.Commands;
using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Othello.ViewModels
{
    internal sealed class MainViewModel : BaseViewModel
    {
        public static MainViewModel? _instance;
        public BaseViewModel? CurrentViewModel { get; set; } = new StartViewModel();
        public static MainViewModel Instance { get => _instance ?? (_instance = new MainViewModel()); }
        public ICommand ChangePageCommand { get; }
        public ICommand StartGameCommand { get; }

        private MainViewModel()
        {
            ChangePageCommand = new RelayCommand(page=> ChangePage((BaseViewModel)page));
            StartGameCommand = new RelayCommand(page=> StartGame());
        }

        private void ChangePage(BaseViewModel page)
        {

            CurrentViewModel = page;
        }
        
        private void StartGame()
        {
            CurrentViewModel = new GameViewModel();
        }
    }
}
