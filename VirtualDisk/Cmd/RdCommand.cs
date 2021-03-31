using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 删除空文件夹, 加了/s就和del一样会删文件
    /// </summary>
    class RdCommand : ICommand
    {
        public RdCommand()
        {
            cmdType = CmdType.Rd;
            addParam = new string[]  //不支持附加参数
            {
                  "/s",    //删除结点目录以及子目录下所有目录
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
                        if(n.index == disk.current.index)
                        {
                            Console.WriteLine("无法删除正在使用的当前路径");
                            return null;
                        }
                        if (n != null)
                        {
                            List<Node> dels = new List<Node>();
                            Rd(RdNode(n, alldel, ref dels), alldel);
                        }
                        else
                        {
                            CmdStrTool.ShowTips(2);
                        }
        
                    }

                }
            }

            List<Node> RdNode(Node d, bool alldel, ref List<Node> dels)
            {
                if (d is File fl)
                {
                    if(alldel)
                        dels.Add(fl);
                }
                else if (d is Floder f)
                {
                    if(!alldel && f.childs.Count != 0)
                    {
                        Console.WriteLine("目录非空");
                        return dels;
                    }
                    dels.Add(f);
                    foreach (Node item in f.childs)
                    {
                        if (item is Floder)
                        {
                            if (alldel)
                                RdNode(item, alldel, ref dels);
                            else
                                dels.Add(item);
                        }
                        else if (alldel)
                            dels.Add(item);
                    }
                }
                else if (d is Symlink s)
                {
                    if(alldel)
                        dels.Add(s);
                }
                return dels;
            }

            void Rd(List<Node> indexs, bool alldel)
            {
                for (int i = 0; i < indexs.Count; i++)
                {
                    if(alldel)
                        disk.RemoveNode(indexs[i]);
                    else if(indexs[i] is Floder f)
                    {
                        if (f.childs.Count == 0)
                        {
                            disk.RemoveNode(f);
                        }
                        else
                        {
                            Console.WriteLine("目录非空");
                            return;
                        }
                    }
                }
            }

            GC.Collect(); //手动gc

            return null;
        }
    }
}
