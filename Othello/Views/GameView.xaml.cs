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

        private void ItemsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) // testa koordinaterna med klick som när spelpjäser ska läggas ut. Klick på t.ex. 2 = x 120, y 180
        {
            Point clickedPosition = e.GetPosition(this);
            Point boardCoordinates = getCoordinates(clickedPosition);
            int x = (int)clickedPosition.X;
            int y = (int)clickedPosition.Y;
        }

        /// <summary>
        /// "Get" coordinates for each square in board
        /// </summary>
        /// <param name="clickedPosition"></param>
        /// <returns></returns>

        private Point getCoordinates(Point clickedPosition)
        {
            int pieceSize = 60;

            double x = clickedPosition.X;
            double y = clickedPosition.Y;

            x = Math.Floor(x / pieceSize) * pieceSize;
            y = Math.Floor(y / pieceSize) * pieceSize;

            return new Point(x, y);
        }
    }
}
