using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Identity
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}
