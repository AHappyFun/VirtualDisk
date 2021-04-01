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

        public Disk ExecuteCmd(string cmdStr, Disk disk)
        {
            string cmdParam = String.Empty;
            ICommand cmd = null;
            //-------
            string[] ar = cmdStr.ToLower().Trim().Split(new char[] { ' ' }, 2);
            string tmp = ar[0];
            if (ar.Length > 1)
            {
                cmdParam = ar[1];
            }
            else
            {
                cmdParam = "";
            }

            if (string.IsNullOrEmpty(tmp) || tmp == "")
            {
                CmdStrTool.ShowTips(1);
                return null;
            }
            else
            {
                switch (tmp)
                {
                    case "dir":
                        cmd = new DirCommand();
                        break;
                    case "md":
                        cmd = new MdCommand();
                        break;
                    case "cd":
                        cmd = new CdCommand();
                        break;
                    case "copy":
                        cmd = new CopyCommand();
                        break;
                    case "del":
                        cmd = new DelCommand();
                        break;
                    case "rd":
                        cmd = new RdCommand();
                        break;
                    case "ren":
                        cmd = new RenCommand();
                        break;
                    case "move":
                        cmd = new MoveCommand();
                        break;
                    case "mklink":
                        cmd = new MklinkCommand();
                        break;
                    case "save":
                        cmd = new SaveCommand();
                        break;
                    case "load":
                        cmd = new LoadCommand();
                        break;
                    case "cls":
                        cmd = new ClsCommand();
                        break;
                    default:
                        CmdStrTool.ShowTips(1);
                        break;
                }               
            }

            if (cmd != null)
            {
                return cmd.Execute(disk, cmdParam);
            }
            else
            {
                return null;
            }
        }
    }
}
