using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 从本地加载重建虚拟磁盘
    /// </summary>
    class LoadCommand : ICommand
    {
        public LoadCommand()
        {
            cmdType = CmdType.Load;
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
                    if (paths[0].First() == '@')
                    {
                        string real = paths[0].Substring(1);
                        Disk newDisk = RealDiskTool.Instance.DeserializeDisk(real);
                        if (newDisk != null)
                        {
                            disk.Clear(); //先归一
                            //disk = newDisk;
                            return newDisk;
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
