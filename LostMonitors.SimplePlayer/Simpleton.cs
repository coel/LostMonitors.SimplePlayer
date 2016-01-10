using System;
using System.Linq;
using LostMonitors.Core;

namespace LostMonitors.SimplePlayer
{
    public class Simpleton : IPlayer
    {
        public void Play(BoardState currentState, Turn theirMove, Func<Turn, Card> draw)
        {
            var yourCards = currentState.YourCards.OrderBy(x => x.Value);
            var myTurn = new Turn(yourCards.First()) { Discard = true };

            foreach (var card in yourCards)
            {
                var yourExpedition = currentState.YourExpeditions[card.Destination];
                if (!yourExpedition.Any() || card.Value > yourExpedition.Last().Value)
                {
                    myTurn = new Turn(card);
                    break;
                }
            }

            draw(myTurn);
        }

        public string GetFriendlyName()
        {
            return "Simply the simpleton";
        }
    }
}