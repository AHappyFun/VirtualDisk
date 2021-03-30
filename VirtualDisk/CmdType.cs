using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    public enum CmdType
    {
        Dir = 0,
        Md = 1,
        Cd = 2,
        Copy = 3,
        Del = 4,
        Rd = 5,
        Ren = 6,
        Move = 7,
        Mklink = 8,
        Save = 9,
        Load = 10,
        Cls = 11,
        None = 99
    }
}
