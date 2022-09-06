using Othello.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Models
{
    internal class Tile : BaseViewModel
    {
        public Tile(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Id for tile Black or White
        /// </summary>
        public int Id { get; }
        public ObservableCollection<Point> Coordinate { get; private set; } = new ObservableCollection<Point>();

        public void SetCoordinate(Point starPoint)
        {
            int x = starPoint.X;
            int y = starPoint.Y;
            Point point = new Point(x, y);
            Coordinate.Add(point);
        }
    }
}
