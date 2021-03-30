using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 序列化保存虚拟磁盘至真实磁盘中
    /// </summary>
    class SaveCommand : ICommand
    {
        public SaveCommand()
        {
            cmdType = CmdType.Save;
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
                if (paths.Length != 1) //多个路径命令不正确
                {
                    CmdStrTool.ShowTips(2);
                    return null;
                }
                else
                {
                    if (paths[0].Contains("@"))
                    {
                        string real = paths[0].Substring(1);
                        if (disk.SaveToDisk(real))
                        {
                            Console.WriteLine("已保存虚拟磁盘文件至: " + real);
                        }
                    }
                    else
                    {
                        CmdStrTool.ShowTips(2);
                        return null;
                    }

                }
            }

            return null;
        }
    }
}
