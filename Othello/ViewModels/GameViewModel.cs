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
        public UCTile CurrentPlayer { get; set; } = new UCTile();
        TileType oppositeColor;
        private const int _gameBoardSize = 8;



        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
            UpdatePlayerBlackScore();
            UpdatePlayerWhiteScore();
            OppositeColor(CurrentPlayer);
            IsPossibleMove(BoardPieces[20]);
            ChangeTileType(BoardPieces[20]);
            CanPlayerMakeAMove(CurrentPlayer);
        }

        public void ChangeTileType(UCTile tile)
        {
            tile.TypeOfTile = CurrentPlayer.TypeOfTile;
        }


        private void FillBoard()
        {
            for (var y = 0; y < _gameBoardSize; y++)
                for (var x = 0; x < _gameBoardSize; x++)
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

            CurrentPlayer.TypeOfTile = TileType.Black;
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
            if (CurrentPlayer.TypeOfTile == TileType.Black)
            {
                CurrentPlayer.TypeOfTile = TileType.White;
            }
            else if (CurrentPlayer.TypeOfTile == TileType.White)
            {
                CurrentPlayer.TypeOfTile = TileType.Black;
            }
            OppositeColor(CurrentPlayer);
        }

        public bool CanPlayerMakeAMove(UCTile CurrentPlayer)
        {
            ChangePlayerTurn();
            foreach (UCTile tile in BoardPieces)
            {
                IsPossibleMove(tile);
                return true;
            }
            ChangePlayerTurn();
            return false;
        }


        public bool IsBoardPieceAvailable(UCTile Tile)
        {
            if (Tile.TypeOfTile != TileType.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public TileType OppositeColor(UCTile CurrentPlayer)
        {
            if(CurrentPlayer.TypeOfTile == TileType.Black)
            {
                oppositeColor = TileType.White;
            }
            else if(CurrentPlayer.TypeOfTile == TileType.White)
            {
                oppositeColor = TileType.Black;
            }
            return oppositeColor;
        }

        public bool IsPossibleMove(UCTile Tile)
        {
            TileType currentColor = CurrentPlayer.TypeOfTile;
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

                    
                    if (BoardPieces.Any(piece => piece.Coordinates == (x + dx, y + dy) && piece.TypeOfTile != oppositeColor))
                        continue;
  
                    int i = 2;
                    while (i <= 7)
                    {
                        if (x + i * dx < 0 || x + i * dx > 7 || y + i * dy < 0 || y + i * dy > 7)
                            break;


                        if (BoardPieces.Any(piece => piece.Coordinates == (x + i * dx, y + i * dy) && piece.TypeOfTile == TileType.Empty))
                            break;
                            

                        if (BoardPieces.Any(piece => piece.Coordinates == (x + i * dx, y + i * dy) && piece.TypeOfTile == currentColor))
                        {
                                return true;
                        }
                        
                       i++;                           
                                                                      
                           
                    }

                }
            }
            return false;

        }
    }
}
