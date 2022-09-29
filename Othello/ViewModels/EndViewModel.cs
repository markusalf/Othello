using Othello.Enums;
using Othello.ViewModels.Base;

namespace Othello.ViewModels
{
    internal class EndViewModel : BaseViewModel
    {
        public Player Winner { get; set; } = new Player();
        public string Message { get; set; }

        public EndViewModel(Player winner, int playerBlackScore, int playerWhiteScore)
        {
            Winner = winner;
            Message = $"Player {Winner} Wins!\nBlack:{playerBlackScore} White:{playerWhiteScore}";
        } 
    }
}
