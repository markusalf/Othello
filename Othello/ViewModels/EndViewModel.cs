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
        public Player Winner { get; set; } = new Player();
        public string Message { get; set; }
        public EndViewModel()
        {

        }

        public EndViewModel(Player winner)
        {
            Winner = winner;
            Message = $"Player {Winner} Wins!";
        } 
    }
}
