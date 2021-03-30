using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 列出当前目录下的目录和文件
    /// </summary>
    class DirCommand : ICommand
    {
        public DirCommand()
        {
            cmdType = CmdType.Dir;
            addParam = new string[]  
            {
                "/ad",      //只输出子目录
                "/s"        //输出子目录及所有目录文件
            };
        }

        public override Disk Execute(Disk disk, string param)
        {

            string path = "";
            string addPar = "";
            CmdStrTool.SplitParam(param, addParam, out path, out addPar);

            bool onlyDir = addPar == "/ad";
            bool allChilds = addPar == "/s";

            if (string.IsNullOrEmpty(path))
            {
                if (onlyDir)
                    ShowChildsFloderInfo(disk.current);
                else if (allChilds)
                    ShowNodeInfo(disk.current);
                else
                    ShowChildsInfo(disk.current);
            }
            else
            {


                string[] paths = CmdStrTool.SplitPathParamToPathList(path);
                if (paths.Length == 1)   //必须一个路径
                {
                    string[] namelist = CmdStrTool.SplitPathToNameList(paths[0]);
                    Node n = disk.NameListToNode(namelist, IsSupportWildcard);
                    if (n == null)
                    {
                        CmdStrTool.ShowTips(2);
                        return null;
                    }

                    if (allChilds)
                    {
                        ShowNodeInfo(n);
                    }
                    else if (onlyDir)
                    {
                        ShowChildsFloderInfo(n);
                    }
                    else
                    {
                        ShowChildsInfo(n);
                    }

                }
                else
                {
                    CmdStrTool.ShowTips(2);
                }
            }



            return null;
        }

        /// <summary>
        /// 遍历所有文件信息
        /// </summary>
        void ShowNodeInfo(Node n)
        {
            n.ShowInfo();
            if(n.nodeType == 1 && n is Floder f)
            {
                for (int i = 0; i < f.childs.Count; i++)
                {
                    ShowNodeInfo(f.childs[i]);
                }
            }
        }

        /// <summary>
        /// 子目录信息
        /// </summary>
        void ShowChildsFloderInfo(Node n)
        {
            n.ShowInfo();
            Floder f;
            if(n.nodeType == 1)
            {
                f = n as Floder;
            }
            else
            {
                f = null;
            }
            if (f == null) return;

            for (int i = 0; i < f.childs.Count; i++)
            {
                if(f.childs[i].nodeType == 1)
                {
                    f.childs[i].ShowInfo();
                }
            }
        }

        /// <summary>
        /// 子目录和子文件
        /// </summary>
        /// <param name="n"></param>
        void ShowChildsInfo(Node n)
        {
            n.ShowInfo();
            Floder f;
            if (n.nodeType == 1)
            {
                f = n as Floder;
            }
            else
            {
                f = null;
            }
            if (f == null) return;

            for (int i = 0; i < f.childs.Count; i++)
            {
                 f.childs[i].ShowInfo();
            }
        }
    }
}
