using System.Collections.Generic;

namespace Selene.Core
{
    public interface IContext
    {
        object[] Arguments { get; set; }
    }
}