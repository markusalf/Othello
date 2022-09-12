using Othello.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Models
{
    public class Tile : INotifyPropertyChanged
    {
        public (int, int) Coordinates { get; set; }

        private string _squareColor;
        public string SquareColor
        {
            get => _squareColor;
            set 
            {
                _squareColor = value;
                OnPropertyChanged();
            }
        }

        private string _ellipseColor;
        public string EllipseColor
        {
            get => _ellipseColor;
            set 
            {
                _ellipseColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
