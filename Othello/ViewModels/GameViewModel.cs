using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Enums;
using System.Windows.Markup;

namespace Othello.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<UCTile> BoardPieces { get; private set; } = new ObservableCollection<UCTile>();
        public Player CurrentPlayer { get; set; }
        private const int _gameBoardSize = 8;



        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
            UpdatePlayerBlackScore();
            UpdatePlayerWhiteScore();

        }

        private void FillBoard()
        {
            for (var x = 0; x < _gameBoardSize; x++)
                for (var y = 0; y < _gameBoardSize; y++)
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
                    CurrentPlayer = Player.Black;
                }
        }


        public void UpdatePlayerBlackScore()
        {
            PlayerBlackScore = 0;

            foreach (var UCTile in BoardPieces)
            {
                if (UCTile.TypeOfTile == TileType.Black)
                {
                    PlayerBlackScore++;
                }
            }
        }

        public void UpdatePlayerWhiteScore()
        {
            PlayerWhiteScore = 0;

            foreach (var UCTile in BoardPieces)
            {
                if (UCTile.TypeOfTile == TileType.White)
                {
                    PlayerWhiteScore++;
                }
            }
        }

        public void ChangePlayerTurn()
        {
            if (CurrentPlayer == Player.Black)
            {
                CurrentPlayer = Player.White;
            }
            else if (CurrentPlayer == Player.White)
            {
                CurrentPlayer = Player.Black;
            }
        }

        /// <summary>
        /// Metod som kollar om spelarna kan göra ett giltigt drag
        /// </summary>
        /// <returns></returns>
        //public bool IsAvailableMove()
        //{

        //}


        public bool IsBoardPieceAvailable(UCTile Tile)
        {

            foreach (UCTile uCTile in BoardPieces)
            {
                if (uCTile.Coordinates == Tile.Coordinates)
                {
                    if (uCTile.TypeOfTile != TileType.Empty)
                    {
                        return false;
                    }
                    break;
                }

            }
            return true;
        }
        public bool IsPossibleMove(UCTile Tile)
        {
            int x = Tile.Coordinates.Item1;
            int y = Tile.Coordinates.Item2;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy < 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (x + dx < 0 || x + dx > 7 || y + dy < 0 || y + dy > 7)
                        continue;

                    foreach (UCTile tile in BoardPieces)
                    {
                        int thisx = tile.Coordinates.Item1;
                        int thisy = tile.Coordinates.Item2;
                        if (tile.Coordinates == (x + dx, y + dy) && tile.TypeOfTile != TileType.White)
                            continue;
                    }

                    int i = 2;
                    while (i <= 7)
                    {
                        if (x + i * dx < 0 || x + i * dx > 7 || y + i * dy < 0 || y + i * dy > 7)
                            break;

                        foreach (UCTile tile in BoardPieces)
                        {
                            int thisx = tile.Coordinates.Item1;
                            int thisy = tile.Coordinates.Item2;
                            if (tile.Coordinates == (x + i * dx, y + i * dy) && tile.TypeOfTile == TileType.Empty)
                                break;
                        }

                        foreach (UCTile tile in BoardPieces)
                        {
                            int thisx = tile.Coordinates.Item1;
                            int thisy = tile.Coordinates.Item2;
                            if (tile.Coordinates == (x + i * dx, y + i * dy) && tile.TypeOfTile == TileType.Black)
                            {
                                return true;
                            }

                        }
                        i++;
                    }

                }
            }
            return false;

        }
    }
}
