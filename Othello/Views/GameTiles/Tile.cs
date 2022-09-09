using Othello.Enums;
using Othello.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Othello.Views.GameTiles
{
    public class Tile : UserControl
    {
        public Tile()
        {
            Coordinates = new ObservableCollection<System.Drawing.Point>();
            
        }

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }



        public TileType CurrentTileType
        {
            get { return (TileType)GetValue(CurrentTileTypeProperty); }
            set { SetValue(CurrentTileTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTileTypeProperty =

        DependencyProperty.Register("CurrentTileType", typeof(TileType), typeof(UCTile), new PropertyMetadata(null));



        public BoardPieceType CurrentBoardPieceType
        {
            get { return (BoardPieceType)GetValue(CurrentBoadPieceTypeProperty); }
            set { SetValue(CurrentBoadPieceTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentBoardPieceType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentBoadPieceTypeProperty =
            DependencyProperty.Register("CurrentBoardPieceType", typeof(BoardPieceType), typeof(UCTile), new PropertyMetadata(null));




        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(Tile), new PropertyMetadata(0,
                new PropertyChangedCallback(OnSizeSet)));

        public static void OnSizeSet(DependencyObject t, DependencyPropertyChangedEventArgs e)
        {
            var tile = t as Tile;
            tile.Width = (int)e.NewValue * 75;
            tile.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(Tile), new PropertyMetadata(0));

        public ObservableCollection<System.Drawing.Point> Coordinates
        {
            get { return (ObservableCollection<System.Drawing.Point>)GetValue(CoordinatesProperty); }
            set { SetValue(CoordinatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoordinatesProperty =
            DependencyProperty.Register("Coordinates", typeof(ObservableCollection<System.Drawing.Point>), typeof(Tile), new PropertyMetadata(null));

        public int X
        {
            get { return (int)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(int), typeof(Tile), new PropertyMetadata(0));

        public int Y
        {
            get { return (int)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(int), typeof(Tile), new PropertyMetadata(0));

        //public ObservableCollection<System.Drawing.Point> Coordinate { get; private set; } = new ObservableCollection<System.Drawing.Point>();

        public void SetCoordinate(System.Drawing.Point startPoint)
        {
            int x = startPoint.X;
            int y = startPoint.Y;
            var point = new System.Drawing.Point(x, y);
            Coordinates.Add(point);
        }
    }
}
