using Othello.ViewModels.Base;
using Othello.Views.Components;
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

        public ObservableCollection <UCTile> Tiles { get; set; } = new ObservableCollection<UCTile>();
        

        public ObservableCollection <BoardPiece> Board { get; set; }

        public const int _boardSize = 8;

        public PlayerViewModel()
        {
            FillBoard();
        }
         
        private void FillBoard()
        {
            Board = new ObservableCollection<BoardPiece>();
           for (int x = 0; x < _boardSize; x++)
            {
                for (int y = 0; y < _boardSize; y++) 
                {
                    var piece = new BoardPiece();
                    Board.Add(piece); 
                }
            }
        }


    }
}
