using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    class CopyCommand : ICommand
    {
        public CopyCommand()
        {
            cmdType = CmdType.Copy;

            //支持通配符和真正磁盘路径
            IsSupportRealPath = true;
            IsSupportWildcard = true;
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
                    bool isRealFile = paths[0].First() == '@';
                    Node n1, n2;
                    //n1为拷贝前结点，真路径就创建一个
                    if (isRealFile)  //真实源路径，
                    {
                        string[] namelist1 = CmdStrTool.SplitPathToNameList(paths[0]);
                        string newname = namelist1.Last(); //最后一个是name
                        n1 = disk.CreateNode(0, newname, disk.root);
                        n1 = RealDiskTool.Instance.CopyRealFileToNode(paths[0].Substring(1), n1, disk);
                    }
                    else
                    {
                        string[] namelist1 = CmdStrTool.SplitPathToNameList(paths[0]);
                        n1 = disk.NameListToNode(namelist1, IsSupportWildcard);
                    }
                    //n2为目标目录，需要存在
                    string[] namelist2 = CmdStrTool.SplitPathToNameList(paths[1]);
                    n2 = disk.NameListToNode(namelist2, IsSupportWildcard);

                    if (n1 == null || n2 == null || n2.nodeType != 1)
                    {
                        CmdStrTool.ShowTips(2);
                    }
                    else
                    {
                        if (isRealFile)
                            disk.MoveNode(n1, n2, cover);
                        else
                            disk.CopyNode(n1, n2, cover);                     
                    }
                }
            }
            return null;
        }


    }
}
