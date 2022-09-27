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
        public bool IsSoundOn { get; set; } = true;

        public ICommand TurnSoundOffCommand { get; set; }
        public ICommand TurnSoundOnCommand { get; set; }


        public ICommand RulesInGameCommand { get; }
        public ICommand TileClickedCommand { get; }
        public int PlayerBlackScore { get; set; } = 0;
        public int PlayerWhiteScore { get; set; } = 0;

        public GameViewModel()
        {
            FillBoard();
            SetOppositeColor(CurrentPlayer);
            UpdateScore();
            ShowPossibleMoves();
            TileClickedCommand = new RelayCommand(execute: b => PlaceTile(b), predicate: b => IsPossibleMove(b));
            RulesInGameCommand = new RelayCommand(page => ChangeRulesScrollVisibility());
            TurnSoundOffCommand = new RelayCommand(execute: b => TurnSoundOff());
            TurnSoundOnCommand = new RelayCommand(execute: b => TurnSoundOn());

        }
        public Visibility Rules { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// Changes visibility of the Rules Scroll
        /// </summary>
        private void ChangeRulesScrollVisibility()
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
        /// Allows you to place a tile on the board depending on if there are possible moves or not
        /// </summary>
        /// <param name="b">Clicked position on board</param>
        private void PlaceTile(object b)
        {
            var tile = BoardPieces.First(t => t.Id == (int)b);
            tile.TypeOfTile = CurrentPlayer.TypeOfTile;
            MakeAMove(b);
            directionResults.Clear();
            PlayClickSound();
            UpdateScore();
            ChangePlayerTurn();
            IsGameOver();            
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

        /// <summary>
        /// Changes the BoardPieceType to show where you can place your piece
        /// </summary>
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

        /// <summary>
        /// Fills the gameboard with UCTiles where 2 white and 2 black pieces start
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
        /// Counts black and white tiles/pieces on the gameboard
        /// </summary>
        public void UpdateScore()
        {
            PlayerBlackScore = BoardPieces.Count(x => x.TypeOfTile == TileType.Black);
            PlayerWhiteScore = BoardPieces.Count(x => x.TypeOfTile == TileType.White);
        }


        /// <summary>
        /// Checks whether the game is over or not
        /// </summary>
        /// <returns>True if there are no more valid moves, otherwise false</returns>
        public bool IsGameOver()
        {
            if (CanPlayerMakeAMove())
            {
                return false;
            }
            else if (CanPlayerMakeAMove())
            {
                return false;
            }
            SetWinner();
            ShowWinner();
            return true;

        }
        /// <summary>
        /// Changes turn
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
            SetOppositeColor(CurrentPlayer);
        }
        /// <summary>
        /// Checks if a player can make a valid move
        /// </summary>
        /// <returns>True if there's a valid move, otherwise false</returns>
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
        /// Changes color value of OppositeColor
        /// </summary>
        /// <param name="CurrentPlayer">Player that is currently playing</param>
        /// <returns>The new color value of OppositeColor</returns>
        public TileType SetOppositeColor(UCTile CurrentPlayer)
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
        /// Makes the move for the current player's placed piece
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void MakeAMove (object b)
        {
            if (IsDirectionSouthPossible(b))
            {
                AddTilesDirectionSouth(b);
            }
            if (IsDirectionNorthPossible(b))
            {
                AddTilesDirectionNorth(b);
            }
            if (IsDirectionWestPossible(b))
            {
                AddTilesDirectionWest(b);
            }
            if (IsDirectionEastPossible(b))
            {
                AddTilesDirectionEast(b);
            }
            if(IsDirectionSouthWestPossible(b))
            {
                AddTilesDirectionSouthWest(b);
            }
            if (IsDirectionSouthEastPossible(b))
            {
                AddTilesDirectionSouthEast(b);
            }
            if (IsDirectionNorthWestPossible(b))
            {
                AddTilesDirectionNorthWest(b);
            }
            if (IsDirectionNorthEastPossible(b))
            {
                AddTilesDirectionNorthEast(b);
            }
            ChangeColorOfTiles();
        }


        /// <summary>
        /// Changes color of tiles within the list directionResults
        /// </summary>
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

        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionSouth(object b)
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

        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionSouthWest(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionSouthEast(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionNorthWest(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionNorthEast(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionNorth(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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
        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionWest(object b)
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


        /// <summary>
        /// Checks if move is valid in the given direction
        /// </summary>
        /// <param name="b"></param>
        /// <returns>True if valid, else false</returns>
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

        /// <summary>
        /// Adds affected tiles of the opposite color to the directionResults list
        /// </summary>
        /// <param name="b">UCTile Id</param>
        public void AddTilesDirectionEast(object b)
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

        /// <summary>
        /// Checks if there's a possible move
        /// </summary>
        /// <param name="b">UCTile Id</param>
        /// <returns>True if there's a possible move, else false</returns>
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

        /// <summary>
        /// Sets Winner by checking player's score
        /// </summary>
        public void SetWinner()
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

        /// <summary>
        /// Displays winner by creating an EndViewModel with right properties together with sound
        /// </summary>
        public void ShowWinner()
        {
            PlayWinSound(); 
            MainViewModel._instance.CurrentViewModel = new EndViewModel(Winner, PlayerBlackScore, PlayerWhiteScore);
        }       
    }
}
