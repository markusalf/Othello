using Othello.Commands;
using Othello.ViewModels.Base;
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

        /// <summary>
        /// Shuts down the application
        /// </summary>
        private void QuitPage()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Changes page to GameView
        /// </summary>
        private void StartGame()
        {
            CurrentViewModel = new GameViewModel();
        }

        /// <summary>
        /// Changes page to RulesView
        /// </summary>
        private void RulesPage()
        {
            CurrentViewModel = new RulesViewModel();
        }

        /// <summary>
        /// Changes page to StartView
        /// </summary>
        private void StartPage()
        {
            CurrentViewModel = new StartViewModel();
        }
    }
}
