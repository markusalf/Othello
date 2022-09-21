using Othello.Enums;
using Othello.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.ViewModels
{
    internal class EndViewModel : BaseViewModel
    {
        public string Winner { get; set; } = "";

        public EndViewModel()
        {

        }
        public EndViewModel(string winner)
        {
            Winner = winner;
        }
    }
}
