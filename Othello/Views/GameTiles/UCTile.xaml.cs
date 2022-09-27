using Othello.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Unique id for UCTiles
        /// </summary>
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(UCTile), new PropertyMetadata(0));


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
