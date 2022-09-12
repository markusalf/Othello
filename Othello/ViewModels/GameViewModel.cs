using Othello.Models;
using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Enums;

namespace Othello.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<UCTile> BoardPieces { get; private set; } = new ObservableCollection<UCTile>();

        private const int _gameBoardSize = 8;

        public GameViewModel()
        {
            FillBoard();
            //DisplayPlayer1Score();
            //DisplayPlayer2Score();

        }

        private void FillBoard()
        {
            for(var x = 0; x < _gameBoardSize; x++)
            for(var y = 0; y < _gameBoardSize; y++)
                {
                    if (x == 3 && y == 3)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black
                        });
                    }
                    else if (x == 4 && y == 3)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White
                        });
                    }
                    else if (x == 4 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black
                        });
                    }
                    else if (x == 3 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White
                        });
                    }
                    else
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Empty
                        });
                    }
                }
        }
        //private int DisplayPlayer1Score()
        //{
        //    foreach (var boardPiece in BoardPieces)
        //    {
        //        if (boardPiece.EllipseColor == "Black")
        //        {
        //            Player1Black.Add(boardPiece);
        //        }
        //    }
        //    return Player1Black.Count;
        //}

        //private int DisplayPlayer2Score()
        //{
        //    foreach (var boardPiece in BoardPieces)
        //    {
        //        if (boardPiece.EllipseColor == "White")
        //        {
        //            Player2White.Add(boardPiece);
        //        }
        //    }
        //    return Player2White.Count;
        //}

    }
}
