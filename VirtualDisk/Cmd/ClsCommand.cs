using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    class ClsCommand : ICommand
    {
        public ClsCommand()
        {
            cmdType = CmdType.Cls;
        }

        public override Disk Execute(Disk disk, string param)
        {
            Console.Clear();
            //再打印出当前路径

            return null;
        }
    }
}
