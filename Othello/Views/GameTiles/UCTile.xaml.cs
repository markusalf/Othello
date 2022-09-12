using Othello.Enums;
using Othello.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello.Views.GameTiles
{
    /// <summary>
    /// Interaction logic for UCTile.xaml
    /// </summary>
    public partial class UCTile : UserControl
    {
        public UCTile()
        {
            InitializeComponent();
        }

        public (int, int) Coordinates { get; set; }

        //private string _squareColor;
        //public string SquareColor
        //{
        //    get => _squareColor;
        //    set
        //    {
        //        _squareColor = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _ellipseColor;
        //public string EllipseColor
        //{
        //    get => _ellipseColor;
        //    set
        //    {
        //        _ellipseColor = value;
        //        OnPropertyChanged();
        //    }
        //}

        public TileType TypeOfTile
        {
            get { return (TileType)GetValue(TypeOfTileProperty); }
            set { SetValue(TypeOfTileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTileProperty =
            DependencyProperty.Register("TypeOfTile", typeof(TileType), typeof(UCTile), new PropertyMetadata(TileType.Black));

        public BoardPieceType TypeOfSquare
        {
            get { return (BoardPieceType)GetValue(TypeOfSquareProperty); }
            set { SetValue(TypeOfSquareProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfSquareProperty =
            DependencyProperty.Register("TypeOfSquare", typeof(BoardPieceType), typeof(UCTile), new PropertyMetadata(BoardPieceType.NotPossibleMoveMarker));

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
