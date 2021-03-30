using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    public class CmdCreater
    {
        private static CmdCreater _instance;
        public static CmdCreater Instance
        {
            get {
                _instance = new CmdCreater();
                return _instance;
            }
        }

        public ICommand CreateCommand(CmdType type, Disk disk)
        {
            switch (type)
            {
                case CmdType.Dir:
                    return new DirCommand();
               case CmdType.Md:
                    return new MdCommand();
               case CmdType.Cd:
                    return new CdCommand();
               case CmdType.Copy:
                    return new CopyCommand();
               case CmdType.Del:
                    return new DelCommand();
               case CmdType.Rd:
                    return new RdCommand();
               case CmdType.Ren:
                    return new RenCommand();
               case CmdType.Move:
                    return new MoveCommand();
               case CmdType.Mklink:
                    return new MklinkCommand();
               case CmdType.Save:
                    return new SaveCommand();
               case CmdType.Load:
                    return new LoadCommand();
               case CmdType.Cls:
                    return new ClsCommand();
               case CmdType.None:
                   return null;
            }
            return null;
        }
    }
}
