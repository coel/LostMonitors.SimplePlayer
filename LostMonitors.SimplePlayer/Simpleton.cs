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

            // Simply play the lowest card and discard it if it can't be played (value too high or would start more than MaxExpeditions expeditions)
            var myTurn = new Turn(yourCards.First());
            myTurn.Discard = !CanPlay(currentState, myTurn.Card);

            if (myTurn.Discard)
            {
                // If it looks we're running out of remaining cards, try to play cards rather than discarding if possible
                var playableCards = yourCards.Where(x => CanPlay(currentState, x)).ToList();
                if (currentState.CardsRemaining <= playableCards.Count() * 2)
                {
                    myTurn = new Turn(playableCards.First());
                }
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

        private static bool CanPlay(BoardState currentState, Card card)
        {
            var yourExpedition = currentState.YourExpeditions[card.Destination].OrderBy(x => x.Value);

            var expeditionsRunning = currentState.YourExpeditions.Count(x => x.Value.Any());
            if (!yourExpedition.Any() && expeditionsRunning >= MaxExpeditions)
            {
                return false;
            }

            if (yourExpedition.Any() && card.Value < yourExpedition.Last().Value)
            {
                return false;
            }
            return true;
        }

        public string GetFriendlyName()
        {
            return "Simply the simpleton";
        }
    }
}