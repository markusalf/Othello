using Othello.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Othello.Converters
{
    internal class RectangleTileTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is BoardPieceType && value != null)
            {
                var boardPieceType = (BoardPieceType)value;
                return boardPieceType switch
                {
                    
                    BoardPieceType.PossibleMoveMarker => new SolidColorBrush(Colors.LightSeaGreen),
                    BoardPieceType.NotPossibleMoveMarker => new SolidColorBrush(Colors.Green),
                    _ => new SolidColorBrush(Colors.Green),
                };
            }
            return new SolidColorBrush(Colors.Green);   
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
