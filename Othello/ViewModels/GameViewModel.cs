using Othello.ViewModels.Base;
using Othello.Views.GameTiles;
using System;
using System.Media;
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
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using System.Windows.Controls;

namespace Othello.ViewModels
{
    internal class GameViewModel : BaseViewModel
    {
        public ObservableCollection<UCTile> BoardPieces { get; private set; } = new ObservableCollection<UCTile>();
        public UCTile CurrentPlayer { get; set; } = new UCTile();
        public Player Winner { get; set; } = new Player();
        TileType oppositeColor;
        List<Tuple<int, int>> directionResults = new List<Tuple<int, int>>();
        public int _gameBoardSize = 8;


        public Visibility SoundOn { get; set; } = Visibility.Collapsed;
        public Visibility SoundOff { get; set; } = Visibility.Visible;
        public bool IsSoundOn;

        public ICommand TurnSoundOffCommand { get; set; }
        public ICommand TurnSoundOnCommand { get; set; }


        public ICommand RulesInGameCommand { get; }
        public ICommand TileClickedCommand { get; }
        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
            OppositeColor(CurrentPlayer);
            UpdateScore();
            ShowPossibleMoves();
            TileClickedCommand = new RelayCommand(execute: b => PlaceTile(b), predicate: b => IsPossibleMove(b));
            RulesInGameCommand = new RelayCommand(page => OpenRulesScroll());
            TurnSoundOffCommand = new RelayCommand(execute: b => TurnSoundOff());
            TurnSoundOnCommand = new RelayCommand(execute: b => TurnSoundOn());

        }
        public Visibility Rules { get; set; } = Visibility.Collapsed;
        private void OpenRulesScroll()
        {
            if (Rules == Visibility.Collapsed)
            {
                Rules = Visibility.Visible;
            }
            else
            {
                Rules = Visibility.Collapsed;
            }
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
            directionResults.Clear();
            PlayClickSound();
            UpdateScore();
            ChangePlayerTurn();
            CheckIfGameOver();            
            ShowPossibleMoves();
        }

        #region Sounds


        private void TurnSoundOff()
        {
            IsSoundOn = false;
            SoundOff = Visibility.Collapsed;
            SoundOn = Visibility.Visible;
        }
        private void TurnSoundOn()
        {
            IsSoundOn = true;
            SoundOn = Visibility.Collapsed;
            SoundOff = Visibility.Visible;
        }

        private void PlayClickSound()
        {
            if (IsSoundOn)
            {
                var clickSound = new SoundPlayer(Properties.Resources.clickSound);
                clickSound.Play();
            }
        }

        private void PlayWinSound()
        {

            if (IsSoundOn)
            {
                var winSound = new SoundPlayer(Properties.Resources.winSound);
                winSound.Play();
            }
        } 
        #endregion


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
                    if (x == _gameBoardSize/2 - 1 && y == _gameBoardSize/2 - 1)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == _gameBoardSize/2 && y == _gameBoardSize/2 - 1)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.Black,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == _gameBoardSize/2 && y == _gameBoardSize/2)
                    {
                        BoardPieces.Add(new UCTile
                        {
                            Coordinates = (x, y),
                            TypeOfSquare = BoardPieceType.NotPossibleMoveMarker,
                            TypeOfTile = TileType.White,
                            Id = BoardPieces.Count
                        });
                    }
                    else if (x == _gameBoardSize/2 - 1 && y == _gameBoardSize/2)
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



        public bool CheckIfGameOver()
        {
            if (CanPlayerMakeAMove())
            {
                return false;
            }
            else
            {                
                if (CanPlayerMakeAMove())
                {
                    return false;
                }
            }
            FindWinner();
            ShowWinner();
            return true;

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
        public bool CanPlayerMakeAMove()
        {
            
            foreach (UCTile tile in BoardPieces)
            {
                if (IsPossibleMove(tile.Id)) { return true; }
                
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

            if (y + 1 <= _gameBoardSize - 1)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = y + 2; i <= _gameBoardSize - 1; i++)
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

            
            
                
                    for (int i = y + 1; i <= _gameBoardSize - 1; i++)
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

            if (y + 1 <= _gameBoardSize - 1 && x - 1 >= 0)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x - 1, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                var i = x - 2;
                var j = y + 2;

                    while (i >= 0 && j <= _gameBoardSize - 1)
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

            if (y + 1 <= _gameBoardSize - 1 && x - 1 >= 0)
            {
                var i = x - 1;
                var j = y + 1;
                while (i >= 0 && j <= _gameBoardSize - 1)
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

            if (y + 1 <= _gameBoardSize - 1 && x + 1 <= _gameBoardSize - 1)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y + 1) && piece.TypeOfTile == oppositeColor))
                {
                    var i = x + 2;
                    var j = y + 2;

                    while (i <= _gameBoardSize - 1 && j <= _gameBoardSize - 1)
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

            if (y + 1 <= _gameBoardSize - 1 && x + 1 <= _gameBoardSize - 1)
            {
                var i = x + 1;
                var j = y + 1;
                while (i <= _gameBoardSize - 1 && j <= _gameBoardSize - 1)
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

            if (y - 1 >= 0 && x + 1 <= _gameBoardSize - 1)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y - 1) && piece.TypeOfTile == oppositeColor))
                {
                    var i = x + 2;
                    var j = y - 2;

                    while (i <= _gameBoardSize - 1 && j >= 0)
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

            if (y - 1 >= 0 && x + 1 <= _gameBoardSize - 1)
            {
                var i = x + 1;
                var j = y - 1;
                while (i <= _gameBoardSize - 1 && j >= 0)
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

            if (y - 1 <= _gameBoardSize - 1)
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

            if (x - 1 <= _gameBoardSize - 1)
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

            if (x + 1 <= _gameBoardSize - 1)
            {
                if (BoardPieces.Any(piece => piece.Coordinates == (x + 1, y) && piece.TypeOfTile == oppositeColor))
                {
                    for (int i = x + 2; i <= _gameBoardSize - 1; i++)
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




            for (int i = x + 1; i <= _gameBoardSize - 1; i++)
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
            var tile = BoardPieces.First(t => t.Id == (int)b);

            if (IsDirectionSouthPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionNorthPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionEastPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionWestPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionNorthEastPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionNorthWestPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionSouthEastPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }
            else if (IsDirectionSouthWestPossible(b) && tile.TypeOfTile == TileType.Empty)
            {
                return true;
            }

            return false;

        }

        public void FindWinner()
        {

            if (PlayerBlackScore > PlayerWhiteScore)
            {
                Winner = Player.Black;
            }

            else if (PlayerWhiteScore > PlayerBlackScore)
            {
                Winner = Player.White;
            }
            else if (PlayerBlackScore == PlayerWhiteScore)
            {
                Winner = Player.None;
            }
        }

        public void ShowWinner()
        {
            PlayWinSound(); 
            MainViewModel._instance.CurrentViewModel = new EndViewModel(Winner, PlayerBlackScore, PlayerWhiteScore);
        }       

    }
}
