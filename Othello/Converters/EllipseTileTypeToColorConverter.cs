﻿using Othello.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Othello.Converters
{
    internal class EllipseTileTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TileType && value != null)
            {
                var tileType = (TileType)value;
                return tileType switch
                {
                    TileType.Black => new SolidColorBrush(Colors.Black),
                    TileType.White => new SolidColorBrush(Colors.White),
                    TileType.PossibleMoveMarker => new SolidColorBrush(Colors.Green),
                    TileType.NotPossibleMoveMarker => new SolidColorBrush(Colors.Green),
                    TileType.FlankedTileMarker => new SolidColorBrush(Colors.DimGray),
                    _ => new SolidColorBrush(Colors.Magenta),
                };
            }
            return new SolidColorBrush(Colors.Pink);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}