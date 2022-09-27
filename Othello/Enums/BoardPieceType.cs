using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Enums
{
    public enum BoardPieceType
    {
        /// <summary>
        /// Shows where tile can be placed
        /// </summary>
        PossibleMoveMarker,
        /// <summary>
        /// Shows where tile cannot be placed
        /// </summary>
        NotPossibleMoveMarker,
    }
}
