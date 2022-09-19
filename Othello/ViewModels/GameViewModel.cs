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
using System.Windows.Input;
using Othello.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Othello.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<UCTile> BoardPieces { get; private set; } = new ObservableCollection<UCTile>();
        public UCTile CurrentPlayer { get; set; } = new UCTile();
        TileType oppositeColor;
        List<Tuple<int, int>> directionResults = new List<Tuple<int, int>>();
        private const int _gameBoardSize = 8;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand TileClickedCommand { get; }
        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
            ChangePlayerTurn();
            OppositeColor(CurrentPlayer);
            UpdateScore();
            ShowPossibleMoves();
            TileClickedCommand = new RelayCommand(execute: b => PlaceTile(b), predicate: b => IsPossibleMove(b));       
        }


        /// <summary>
        /// Lägger ut en Tile på spelbrädan, uppdaterar resultatet, byter spelares tur och visar möjliga drag för den nya spelaren.
        /// </summary>
        /// <param name="x"></param>
        private void PlaceTile(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            tile.TypeOfTile = CurrentPlayer.TypeOfTile;
            MakeAMove(b);
            ChangePlayerTurn();
            directionResults.Clear();
            ShowPossibleMoves();
            UpdateScore();
        }

        private void ShowPossibleMoves()
        {

            foreach (var UCTile in BoardPieces)
            {

                var b = UCTile.Id;
                if (IsPossibleMove(b) && UCTile.TypeOfTile == TileType.Empty)
                {
                    UCTile.TypeOfSquare = BoardPieceType.PossibleMoveMarker;
                }
                else
                {
                    UCTile.TypeOfSquare = BoardPieceType.NotPossibleMoveMarker;
                }
            }
        }

        public void ChangeTileType(UCTile tile)
        {
            tile.TypeOfTile = CurrentPlayer.TypeOfTile;
        }

        /// <summary>
        /// Fyller boarden med UCTile och lägger till 2 svarta och 2 vita.
        /// </summary>
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
                            TypeOfTile = TileType.White,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 4 && y == 3)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 4 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 3 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black,
                            Id = BoardPieces.Count
                        });
                    }                    
                    else
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Empty,
                            Id = BoardPieces.Count
                        });
                    }
                }

            CurrentPlayer.TypeOfTile = TileType.White;
        }


        /// <summary>
        /// Räknar svarta/vita brickor på spelbrädan för att visa resultat
        /// </summary>
        public void UpdateScore()
        {
        PlayerBlackScore = BoardPieces.Count(x => x.TypeOfTile == TileType.Black);
        PlayerWhiteScore = BoardPieces.Count(x => x.TypeOfTile == TileType.White);
        }
        /// <summary>
        /// Byter vilken spelares tur det är.
        /// </summary>
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
        /// <summary>
        /// Kollar om spelaren kan göra ett giltigt drag.
        /// </summary>
        /// <param name="CurrentPlayer"></param>
        /// <returns>Sant om spelaren kan göra ett giltigt drag. Falskt om den inte kan samt ändrar då till andra spelarens tur</returns>
        //public bool CanPlayerMakeAMove(UCTile CurrentPlayer)
        //{
        //    ChangePlayerTurn();
        //    foreach (UCTile tile in BoardPieces)
        //    {
        //        IsPossibleMove(tile);
        //        return true;
        //    }
        //    ChangePlayerTurn();
        //    return false;
        //}

        /// <summary>
        /// Kollar om brickan spelaren vill lägga sin tile på är tom och tillgänglig.
        /// </summary>
        /// <param name="Tile"></param>
        /// <returns>Sant om rutan är tom och spelaren kan lägga ut. Falskt om den inte kan lägga ut.</returns>
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
        /// <summary>
        /// Ändrar vilken färg/tiletype oppositecolor är.
        /// </summary>
        /// <param name="CurrentPlayer"></param>
        /// <returns>Den färg/tiletype som oppositecolor är.</returns>
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

        public void MakeAMove (object b)
        {
            if (IsDirectionSouthPossible(b))
            {
                ChangeTilesDirectionSouth(b);
            }
            if (IsDirectionNorthPossible(b))
            {
                ChangeTilesDirectionNorth(b);
            }
            if (IsDirectionWestPossible(b))
            {
                ChangeTilesDirectionWest(b);
            }
            if (IsDirectionEastPossible(b))
            {
                ChangeTilesDirectionEast(b);
            }
            if(IsDirectionSouthWestPossible(b))
            {
                ChangeTilesDirectionSouthWest(b);
            }
            if (IsDirectionSouthEastPossible(b))
            {
                ChangeTilesDirectionSouthEast(b);
            }
            if (IsDirectionNorthWestPossible(b))
            {
                ChangeTilesDirectionNorthWest(b);
            }
            if (IsDirectionNorthEastPossible(b))
            {
                ChangeTilesDirectionNorthEast(b);
            }

            ChangeColorOfTiles();
        }

        public void ChangeColorOfTiles()
        {
            foreach (var affectedcoordinates in directionResults)
            {
                foreach (UCTile tile in BoardPieces)
                {
                    if (tile.Coordinates.Item1 == affectedcoordinates.Item1 && tile.Coordinates.Item2 == affectedcoordinates.Item2)
                    {
                        tile.TypeOfTile = CurrentPlayer.TypeOfTile;
                    }
                }
            }
            
        }

        public bool IsDirectionSouthPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y + 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = y + 2; i <= 7; i++)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }
                        else if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == oppositeColor))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

           
        public void ChangeTilesDirectionSouth(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;

            
            
                
                    for (int i = y + 1; i <= 7; i++)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == oppositeColor))
                        {
                            directionResults.Add(Tuple.Create(x, i));
                        }
                        else
                        {                            
                            break;
                        }
                        
                    }                            
            
        }
        public bool IsDirectionSouthWestPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y + 1 <= 7 && x - 1 >= 0)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x - 1, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                var i = x - 2;
                var j = y + 2;

                    while (i >= 0 && j <= 7)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }

                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                        {
                            i--;
                            j++;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeTilesDirectionSouthWest(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;

            if (y + 1 <= 7 && x - 1 >= 0)
            {
                var i = x - 1;
                var j = y + 1;
                while (i >= 0 && j <= 7)
                {
                    if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                    {
                        directionResults.Add(Tuple.Create(i, j));
                        i--;
                        j++;
                    }

                    else
                    {
                        break;
                    }
                }
            }

        }


        public bool IsDirectionSouthEastPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y + 1 <= 7 && x + 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                    var i = x + 2;
                    var j = y + 2;

                    while (i <= 7 && j <= 7)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }

                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                        {
                            i++;
                            j++;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeTilesDirectionSouthEast(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;

            if (y + 1 <= 7 && x + 1 <= 7)
            {
                var i = x + 1;
                var j = y + 1;
                while (i <= 7 && j <= 7)
                {
                    if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                    {
                        directionResults.Add(Tuple.Create(i, j));
                        i++;
                        j++;
                    }

                    else
                    {
                        break;
                    }
                }
            }

        }

        public bool IsDirectionNorthWestPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y - 1 >= 0 && x - 1 >= 0)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x - 1, y - 1) && piece.TypeOfTile == oppositeColor))
                {
                    var i = x - 2;
                    var j = y - 2;

                    while (i >= 0 && j >= 0)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }

                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                        {
                            i--;
                            j--;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeTilesDirectionNorthWest(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;

            if (y - 1 >= 0 && x - 1 >= 0)
            {
                var i = x - 1;
                var j = y - 1;
                while (i >= 0 && j >= 0)
                {
                    if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                    {
                        directionResults.Add(Tuple.Create(i, j));
                        i--;
                        j--;
                    }

                    else
                    {
                        break;
                    }
                }
            }

        }

        public bool IsDirectionNorthEastPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y - 1 >= 0 && x + 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y - 1) && piece.TypeOfTile == oppositeColor))
                {
                    var i = x + 2;
                    var j = y - 2;

                    while (i <= 7 && j >= 0)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }

                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                        {
                            i++;
                            j--;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        public void ChangeTilesDirectionNorthEast(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;

            if (y - 1 >= 0 && x + 1 <= 7)
            {
                var i = x + 1;
                var j = y - 1;
                while (i <= 7 && j >= 0)
                {
                    if (BoardPieces.Any(piece => piece.Coordinates == (i, j) && piece.TypeOfTile == oppositeColor))
                    {
                        directionResults.Add(Tuple.Create(i, j));
                        i++;
                        j--;
                    }

                    else
                    {
                        break;
                    }
                }
            }

        }


        public bool IsDirectionNorthPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (y - 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x, y - 1) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = y - 2; i >= 0; i--)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }
                        else if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == oppositeColor))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }
        public void ChangeTilesDirectionNorth(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;




            for (int i = y - 1; i >= 0; i--)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x, i) && piece.TypeOfTile == oppositeColor))
                {
                    directionResults.Add(Tuple.Create(x, i));
                }
                else
                {
                    break;
                }

            }

        }

        public bool IsDirectionWestPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (x - 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x - 1, y) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = x - 2; i >= 0; i--)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }
                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == oppositeColor))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }
        public void ChangeTilesDirectionWest(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;




            for (int i = x - 1; i >= 0; i--)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == oppositeColor))
                {
                    directionResults.Add(Tuple.Create(i, y));
                }
                else
                {
                    break;
                }

            }

        }

        public bool IsDirectionEastPossible(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            if (x + 1 <= 7)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = x + 2; i <= 7; i++)
                    {
                        if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == currentColor))
                        {
                            return true;
                        }
                        else if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == oppositeColor))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }


        public void ChangeTilesDirectionEast(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;
            TileType currentColor = CurrentPlayer.TypeOfTile;




            for (int i = x + 1; i <= 7; i++)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (i, y) && piece.TypeOfTile == oppositeColor))
                {
                    directionResults.Add(Tuple.Create(i, y));
                }
                else
                {
                    break;
                }

            }

        }

        public bool IsPossibleMove(object b)
        {
            if (IsDirectionSouthPossible(b))
            {
                return true;
            }
            else if (IsDirectionNorthPossible(b))
            {
                return true;
            }
            else if (IsDirectionEastPossible(b))
            {
                return true;
            }
            else if (IsDirectionWestPossible(b))
            {
                return true;
            }
            else if (IsDirectionNorthEastPossible(b))
            {
                return true;
            }
            else if (IsDirectionNorthWestPossible(b))
            {
                return true;
            }
            else if (IsDirectionSouthEastPossible(b))
            {
                return true;
            }
            else if (IsDirectionSouthWestPossible(b))
            {
                return true;
            }

            return false;

        }
    }
}
