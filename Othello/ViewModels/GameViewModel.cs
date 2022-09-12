using Othello.Models;
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
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Tile> BoardPieces { get; private set; } = new ObservableCollection<Tile>();
        public ObservableCollection<Tile> Player1 { get; private set; } = new ObservableCollection<Tile>();
        public ObservableCollection<Tile> Player2 { get; private set; } = new ObservableCollection<Tile>();
        private const int _gameBoardSize = 8;

        public GameViewModel()
        {
            FillBoard();
            DisplayPlayer1Score();
            DisplayPlayer2Score();

        }

        private void FillBoard()
        {
            for(var x = 0; x < _gameBoardSize; x++)
            for(var y = 0; y < _gameBoardSize; y++)
                {
                    if (x == 3 && y == 3)
                    {
                        BoardPieces.Add(new Tile
                        {
                            Coordinates = (x, y),
                            SquareColor = "Green",
                            EllipseColor = "Black"
                        });
                    }
                    else if (x == 4 && y == 3)
                    {
                        BoardPieces.Add(new Tile
                        {
                            Coordinates = (x, y),
                            SquareColor = "Green",
                            EllipseColor = "White"
                        });
                    }
                    else if (x == 4 && y == 4)
                    {
                        BoardPieces.Add(new Tile
                        {
                            Coordinates = (x, y),
                            SquareColor = "Green",
                            EllipseColor = "Black"
                        });
                    }
                    else if (x == 3 && y == 4)
                    {
                        BoardPieces.Add(new Tile
                        {
                            Coordinates = (x, y),
                            SquareColor = "Green",
                            EllipseColor = "White"
                        });
                    }
                    else
                    {
                        BoardPieces.Add(new Tile
                        {
                            Coordinates = (x, y),
                            SquareColor = "Green",
                            EllipseColor = "#00FFFFFF"
                        });
                    }
                }
        }


        private int DisplayPlayer2Score()
        {
            foreach (var boardPiece in BoardPieces)
            {
                if (boardPiece.EllipseColor == "Black")
                {
                    Player2.Add(boardPiece);
                }
            }
            return Player2.Count;
        }

        private int DisplayPlayer1Score()
        {
            foreach (var boardPiece in BoardPieces)
            {
                if (boardPiece.EllipseColor == "White")
                {
                    Player1.Add(boardPiece);
                }
            }
            return Player1.Count;
        }

    }
}
