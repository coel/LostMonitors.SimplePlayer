using System;
using LostMonitors.Core;

namespace LostMonitors.SimplePlayer
{
    public class Simpleton : IPlayer
    {
        public void Play(BoardState currentState, Turn theirMove, Func<Turn, Destination, Card> draw)
        {
            throw new NotImplementedException();
        }

        public string GetFriendlyName()
        {
            return "Simply the simpleton";
        }
    }
}