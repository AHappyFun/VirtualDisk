using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 删除文件命令，不删除文件夹
    /// </summary>
    class DelCommand : ICommand
    {
        public DelCommand()
        {
            cmdType = CmdType.Del;
            addParam = new string[]  //不支持附加参数
            {
                 "/s",    //删除目录下文件以及子目录下所有文件
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
                bool alldel = addPar == "/s";
                if (paths.Length > 0)
                {
                    for (int i = 0; i < paths.Length; i++)
                    {
                        string[] namelist = CmdStrTool.SplitPathToNameList(paths[i]);
                        Node n = disk.NameListToNode(namelist, IsSupportWildcard);
                        if (n != null)
                        {
                            List<Node> dels = new List<Node>();
                            Del(DelNode(n, alldel,ref dels));
                        }
                        else
                        {
                            CmdStrTool.ShowTips(2);
                        }
                    }

                }
            }

            List<Node> DelNode(Node d, bool alldel, ref List<Node> dels)
            {           
                if (d is File fl)
                {
                    dels.Add(fl);
                }
                else if (d is Floder f)
                {

                    foreach (Node item in f.childs)
                    {
                        if (item is Floder)
                        {
                            if (alldel)
                                DelNode(item, alldel, ref dels);
                            else
                                continue;
                        }
                        else
                            dels.Add(item);
                    }

                }
                else if (d is Symlink s)
                {
                    dels.Add(s);
                }
                return dels;
            }

            void Del(List<Node> indexs)
            {
                for (int i = 0; i < indexs.Count; i++)
                {
                    disk.RemoveNode(indexs[i]);
                }
            }

            GC.Collect(); //手动gc

            return null;
        }


    }
}
