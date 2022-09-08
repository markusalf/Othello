using Othello.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public TileType CurrentTileType
        {
            get { return (TileType)GetValue(CurrentTileTypeProperty); }
            set { SetValue(CurrentTileTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTileTypeProperty =
            DependencyProperty.Register("CurrentTileType", typeof(TileType), typeof(UCTile), new PropertyMetadata(0));


        public UCTile()
        {
            InitializeComponent();
        }
    }
}
