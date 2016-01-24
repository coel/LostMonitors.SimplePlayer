using System;
using System.Linq;
using LostMonitors.Core;

namespace LostMonitors.SimplePlayer
{
    public class Simpleton : IPlayer
    {
        private const int MaxExpeditions = 3;

        public void Play(BoardState currentState, Turn theirMove, Func<Turn, Card> draw)
        {
            var yourCards = currentState.YourCards.OrderBy(x => x.Value);
            var myTurn = new Turn(yourCards.First());

            var yourExpedition = currentState.YourExpeditions[myTurn.Card.Destination].OrderBy(x => x.Value);

            var expeditionsRunning = currentState.YourExpeditions.Count(x => x.Value.Any());
            if (!yourExpedition.Any() && expeditionsRunning >= MaxExpeditions)
            {
                myTurn.Discard = true;
            }

            if (yourExpedition.Any() && myTurn.Card.Value < yourExpedition.Last().Value)
            {
                myTurn.Discard = true;
            }

            draw(myTurn);
        }

        public string GetFriendlyName()
        {
            return "Simply the simpleton";
        }
    }
}