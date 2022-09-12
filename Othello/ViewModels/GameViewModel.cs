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

        private const int _gameBoardSize = 8;

        public string _PlayerBlackScore = "då";
        public string _PlayerWhiteScore = "hej";

        
        public GameViewModel()
        {
            FillBoard();
            UpdatePlayerBlackScore();
            UpdatePlayerWhiteScore();

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


        public void UpdatePlayerBlackScore()
        {
            int playerBlackScore = 0;

            foreach (var UCTile in BoardPieces)
            {
                if (UCTile.TypeOfTile == TileType.Black)
                {
                    playerBlackScore++;
                }
            }

            _PlayerBlackScore = playerBlackScore.ToString();
        }

        public void UpdatePlayerWhiteScore()
        {
            int playerWhiteScore = 0;

            foreach (var UCTile in BoardPieces)
            {
                if (UCTile.TypeOfTile == TileType.White)
                {
                    playerWhiteScore++;
                }
            }
            
            _PlayerWhiteScore = playerWhiteScore.ToString();
        }

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

    }
}
