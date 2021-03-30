using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    //1.切换当前结点为新文件夹结点
    //2.切换符号链接
    //3.各种异常情况
    class CdCommand : ICommand
    {
        public CdCommand()
        {
            cmdType = CmdType.Cd;
            IsSupportWildcard = true; //支持通配符
            addParam = new string[]  //不支持附加参数
            {
            };
        }

        public override Disk Execute(Disk disk, string param)
        {
            string path = "";
            string addPar = "";
            if(param == ".")
            {
                Console.WriteLine(disk.current.GetPath() + ">");
                return null;
            }
            else if(param == "/" || param == "\\")
            {
                disk.current = disk.root as Floder;
                Console.WriteLine(disk.current.GetPath() + ">");
                return null;
            }

            CmdStrTool.SplitParam(param, addParam, out path, out addPar);

            if(path == "")
            {
                Console.WriteLine(disk.current.GetPath()+">");
                return null;
            }
            else
            {
                string[] paths = CmdStrTool.SplitPathParamToPathList(path);
                if (paths.Length != 1) //多个路径命令不正确
                    CmdStrTool.ShowTips(2);
                else
                {
                    string[] namelist = CmdStrTool.SplitPathToNameList(paths[0]);
                    Node n = disk.NameListToNode(namelist, IsSupportWildcard);
                    SetCurrentDir(n);
                }
            }

            void SetCurrentDir(Node n)
            {
                if (n != null && n is Floder f)   //正常情况
                {
                    disk.current = f as Floder;
                    Console.WriteLine(disk.current.GetPath() + ">");
                }
                else if (n != null && n is Symlink s)  //找到的是个符号链接
                {
                    Node tar = s.target;
                    SetCurrentDir(tar);
                }
                else   //没找到结点或找到了个文件不能cd
                {
                    CmdStrTool.ShowTips(2);
                }
            }

            return null;
        }
    }
}
