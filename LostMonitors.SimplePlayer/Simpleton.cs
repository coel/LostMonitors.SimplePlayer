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

            // Simple play the lowest card and discard it if it can't be played (value too high or would start more than MaxExpeditions expeditions)
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
            
            // Take the first (if any) discard that can be played on existing expeditions
            foreach (var destination in currentState.Discards)
            {
                if (myTurn.Card.Destination == destination.Key)
                {
                    if (!myTurn.Discard && destination.Value.Value >= myTurn.Card.Value)
                    {
                        myTurn.Draw = destination.Key;
                        break;
                    }
                }
                else
                {
                    var expedition = currentState.YourExpeditions[destination.Key].OrderBy(x => x.Value);
                    if (expedition.Any() && destination.Value.Value >= expedition.Last().Value)
                    {
                        myTurn.Draw = destination.Key;
                        break;
                    }
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