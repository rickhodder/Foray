using System.IO;
using System.Text;

namespace Foray.Common
{
    public class StringReader : IStringReader
    {
        StreamReader _reader;

        public string CurrentLine { get; private set; }
        public int LineNumber { get; set; }

        public StringReader(string content)
        {
            var ms = new MemoryStream();

            ms.Write(Encoding.Default.GetBytes(content.ToCharArray(), 0, content.Length), 0, content.Length);
            ms.Position = 0;
            _reader = new StreamReader(ms);
        }

        public string GetLine()
        {
            CurrentLine = "";
            LineNumber++;

            while (!_reader.EndOfStream)
            {

                char peek = (char)_reader.Peek();
                if (peek == ' ')
                {
                    _reader.Read();
                    continue;
                }

                if (peek == '\r' || peek == '\n')
                {
                    // skip over it
                    _reader.Read();

                    peek = (char)_reader.Peek();

                    if (peek == '\n')
                    {
                        // skip over it
                        _reader.Read();
                    }
                    break;
                }
                else
                {
                    CurrentLine += (char)_reader.Read();
                }
            }


            return CurrentLine;
        }

        public bool IsDone()
        {
            return _reader.EndOfStream;
        }

        public void Dispose()
        {
            _reader?.Close();
            _reader = null;
        }
    }
}