using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    class MoveCommand : ICommand
    {
        public MoveCommand()
        {
            cmdType = CmdType.Move;
            addParam = new string[]  //不支持附加参数
            {
                "/y",    //遇到同名默认覆盖
            };
        }

        public override Disk Execute(Disk disk, string param)
        {
            string path = "";
            string addPar = "";
            CmdStrTool.SplitParam(param, addParam, out path, out addPar);
            if (string.IsNullOrEmpty(path))
            {
                CmdStrTool.ShowTips(1);
                return null;
            }
            else
            {
                string[] paths = CmdStrTool.SplitPathParamToPathList(path);
                if (paths.Length != 2) //参数不对
                    CmdStrTool.ShowTips(1);
                else
                {
                    bool cover = addPar == "/y";
                    Node n1, n2;
                    //n1为移动前结点，需要存在             
                   string[] namelist1 = CmdStrTool.SplitPathToNameList(paths[0]);
                   n1 = disk.NameListToNode(namelist1, IsSupportWildcard);

                    //n2为目标目录，需要存在
                    string[] namelist2 = CmdStrTool.SplitPathToNameList(paths[1]);
                    n2 = disk.NameListToNode(namelist2, IsSupportWildcard);

                    if (n1 == null || n2 == null || n2.nodeType != 1)
                    {
                        CmdStrTool.ShowTips(2);
                    }
                    else
                    {
                        disk.MoveNode(n1, n2, cover);
                        Console.WriteLine("移动：{0}--->{1}", n1.name, n2.GetPath());
                    }
                }
            }
            return null;
        }
    }
}
