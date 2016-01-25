using System;
using System.Linq;
using LostMonitors.Core;

namespace LostMonitors.SimplePlayer
{
    public class Dummy : IPlayer
    {
        public void Play(BoardState currentState, Turn theirMove, Func<Turn, Card> draw)
        {
            var yourCards = currentState.YourCards.OrderByDescending(x => x.Value);

            var myTurn = new Turn(yourCards.First()) {Discard = true};

            draw(myTurn);
        }

        public string GetFriendlyName()
        {
            return "Dummy McDumdum";
        }
    }
}