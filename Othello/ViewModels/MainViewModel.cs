using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; } = new GameViewModel();
    }
}
