using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Flags]
    public enum Roles
    {
        Create = 1 << 0,
        Read = 1 << 1,
        Update = 1 << 2,
        Delete = 1 << 3
    }
}
