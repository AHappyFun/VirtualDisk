using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    class MklinkCommand : ICommand
    {
        public MklinkCommand()
        {
            cmdType = CmdType.Mklink;
            addParam = new string[]  //不支持附加参数
            {
                "/d", //创建目录符号链接，不写/d默认创建文件符号链接
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
                    string[] namelist1 = CmdStrTool.SplitPathToNameList(paths[0]);
                    string newname = namelist1.Last(); //最后一个是新name
                    Node n1;  //n1为新建文件的父节点
                    if (namelist1.Length > 1)
                    {
                        string[] namelist = new string[namelist1.Length - 1];
                        Array.Copy(namelist1, namelist, namelist1.Length - 1);

                        n1 = disk.NameListToNode(namelist, IsSupportWildcard);
                    }
                    else
                    {

                         n1 = disk.current;
                    }

                    //n2为link的目标结点
                    string[] namelist2 = CmdStrTool.SplitPathToNameList(paths[1]);
                    Node n2 = disk.NameListToNode(namelist2, IsSupportWildcard);
                    if (n1 == null || n2 == null)
                    {
                        CmdStrTool.ShowTips(2);                
                    }
                    else
                    {
                        Symlink s = disk.CreateNode(2, newname, n1) as Symlink;
                        s.SetLinkTarget(n2);
                        Console.WriteLine("创建链接{0}--->{1}", s.GetPath(), n2.GetPath());
                    }
                }
            }


            return null;
        }

    }
}
