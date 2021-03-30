using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 创建目录的命令
    /// </summary>
    class MdCommand : ICommand
    {
        public MdCommand()
        {
            cmdType = CmdType.Md;
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
                if(paths.Length > 0)
                {
                    for (int i = 0; i < paths.Length; i++)  //创建多个目录
                    {
                        string[] namelist = CmdStrTool.SplitPathToNameList(paths[i]);
                        disk.CreateNodesWithNameList(namelist);

                         Console.WriteLine("已创建{0}{1}", disk.current.GetPath(), GetStrNames(namelist));
                        
                    }
                }
            }
        

            return null;
        }

        string GetStrNames(string[] names)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                sb.Append(names[i]);
                if(i < names.Length - 1)
                    sb.Append("\\");
            }
            sb.Replace("_", " ");
            return sb.ToString();
        }


    }
}
