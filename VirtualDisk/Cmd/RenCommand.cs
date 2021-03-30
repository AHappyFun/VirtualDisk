using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 重命名存在的结点
    /// </summary>
    class RenCommand : ICommand
    {
        public RenCommand()
        {
            cmdType = CmdType.Ren;
            addParam = new string[]  //不支持附加参数
            {
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
                if (paths.Length !=2) //参数数不对
                    CmdStrTool.ShowTips(1);
                else
                {
                    string[] namelist = CmdStrTool.SplitPathToNameList(paths[0]);
                    Node n = disk.NameListToNode(namelist, IsSupportWildcard);
                    if (n != null)
                    {
                        RenameNode(n, paths[1]);
                    }
                    else
                    {
                        CmdStrTool.ShowTips(2);
                    }               
                }
            }

            return null;
        }

        void RenameNode(Node n ,string name)
        {
            //先看父节点有没有已经存在这个name
            if(n.parent != null && n.parent is Floder f)
            {
                Node has = f.GetChildByName(name);
                if(has != null)  //父节点已经存在这个儿子
                {
                    CmdStrTool.ShowTips(4);
                }
                else
                {
                    //name参数对不对
                    if (CmdStrTool.NameIsGood(name))
                        CmdStrTool.ShowTips(5);
                    else
                    {
                        Console.WriteLine(n.GetPath() + "-->" + name);
                        n.SetName(name);
                        n.SetDate();
                    }
                }
            }
            else
            {
                CmdStrTool.ShowTips(3);
            }
        }
    }
}
