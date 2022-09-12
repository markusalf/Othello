using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.ViewModels
{
    internal abstract class PlayerViewModel : BaseViewModel
    {

        //public ObservableCollection <UCTile> Tiles { get; set; } = new ObservableCollection<UCTile>();

        //public const int _boardSize = 8;

        //public PlayerViewModel()
        //{
        //    FillTiles();
        //}
         
        //private void FillTiles()
        //{
        //    Tiles = new ObservableCollection<UCTile>();
        //   for (int x = 0; x < _boardSize; x++)
        //    {
        //        for (int y = 0; y < _boardSize; y++) 
        //        {
        //            var tile = new UCTile();
        //            Tiles.Add(tile); 
        //        }
        //    }
        //}
    }
}
