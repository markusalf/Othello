﻿using Othello.ViewModels.Base;
using Othello.Views.Components;
using Othello.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.ViewModels
{
    internal abstract class PlayerViewModel : BaseViewModel
    {
        public ObservableCollection <BlackTile> Tiles { get; set; } = new ObservableCollection<BlackTile>();
        public ObservableCollection <WhiteTile> WhiteTiles { get; set; } = new ObservableCollection<WhiteTile> ();

        public ObservableCollection <BoardPiece> Board { get; set; }

        public const int _boardSize = 8;

        public PlayerViewModel()
        {
            FillBoard();
        }
         
        private void FillBoard()
        {
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
