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

namespace Othello.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<UCTile> BoardPieces { get; private set; } = new ObservableCollection<UCTile>();
        public UCTile CurrentPlayer { get; set; } = new UCTile();
        TileType oppositeColor;
        private const int _gameBoardSize = 8;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand TileClickedCommand { get; }
        public ICommand CurrentTurn { get; }

        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
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
            ChangePlayerTurn();
            UpdateScore();
            ShowPossibleMoves();
        }

        private void ShowPossibleMoves()
        {

            foreach (var UCTile in BoardPieces)
            {

                var b = UCTile.Id;
                if (IsPossibleMove(b))
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
                            TypeOfTile = TileType.Black,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 4 && y == 3)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 4 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == 3 && y == 4)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White,
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

            CurrentPlayer.TypeOfTile = TileType.Black;
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
        /// <summary>
        /// Går igenom brädet runt den tile/rutan man vill lägga ut för att se om det är ett möjligt drag genom att man flankerar motståndarens tile/tiles.
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Sant om det är ett möjligt drag. Falskt om det inte är ett möjligt drag.</returns>
        public bool IsPossibleMove(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            TileType currentColor = CurrentPlayer.TypeOfTile;
            

            int x = tile.Coordinates.Item1;
            int y = tile.Coordinates.Item2;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Uppdatera i gränssnittet vilken spelares tur det är att lägga sin bricka
        /// </summary>
        private UCTile currentPlayerTurn;
        public UCTile CurrentPlayerTurn
        {
            get { return currentPlayerTurn; }
            set 
            {
                currentPlayerTurn = value;
                NotifyPropertyChanged(nameof(CurrentPlayer));
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
