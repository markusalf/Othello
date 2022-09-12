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

namespace Othello.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
        }

        //private void boardPiece_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Point clickedPosition = e.GetPosition(GameBoard);
        //    Point boardPieceCo = GetCoordinates(clickedPosition);
        //}

        ///// <summary>
        ///// "Get" coordinates for each square in board
        ///// </summary>
        ///// <param name="clickedPosition"></param>
        ///// <returns></returns>

        //private Point GetCoordinates(Point clickedPosition)
        //{
        //    int pieceSize = 75;

        //    double x = clickedPosition.X;
        //    double y = clickedPosition.Y;

        //    x = Math.Floor(x / pieceSize) * pieceSize;
        //    y = Math.Floor(y / pieceSize) * pieceSize;
                                 
        //    return new Point(x, y);
        //}
    }  
}
