using System.Collections.Generic;

namespace DiscordBot.Data.Interfaces
{
    public interface IPaginate<T>
    {
        int From { get; }
        int Index { get; }
        int Size { get; }
        int Count { get; }
        int Pages { get; }
        IList<T> Items { get; }
        bool HasPrevious { get; }
        bool HasNext { get; }
    }
}