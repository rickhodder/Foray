using System;

namespace Foray.Common
{
    public interface IStringReader : IDisposable
    {
        string CurrentLine { get; }
        int LineNumber { get; set; }
        string GetLine();
        bool IsDone();
    }
}