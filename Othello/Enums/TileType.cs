using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Enums
{
    public enum TileType
    {
        Black, // Visar svarta tiles
        White, // Visar vita tiles
        PossibleMoveMarker, // Visar vart det går att lägga sin tile.
        NotPossibleMoveMarker, // Visar vart det inte går att lägga sin tile.
        FlankedTileMarker, // Visar vilka tiles som erövras vid ett visst drag.
    }
}
