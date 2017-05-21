using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters;
using Foray.Lib.Model;

namespace Foray.Lib
{
    public interface ISchemaFactory 
    {
        Schema Create();
    }
}
