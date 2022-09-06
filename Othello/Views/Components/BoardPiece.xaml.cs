using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Othello.Views.Components
{
    /// <summary>
    /// Interaction logic for BoardPiece.xaml
    /// </summary>
    public partial class BoardPiece : UserControl
    {
        public BoardPiece()
        {
            InitializeComponent();
        }
 
        public int X
        {
            get { return (int)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(int), typeof(BoardPiece), new PropertyMetadata(0));

        public int Y
        {
            get { return (int)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(int), typeof(BoardPiece), new PropertyMetadata(0));

        /// <summary>
        /// Unique Id's for each piece(square) of the board
        /// </summary>
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(BoardPiece), new PropertyMetadata(0));
    }
}
