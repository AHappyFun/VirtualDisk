using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    public class ICommand
    {
        /// <summary>
        /// 是否支持通配符
        /// </summary>
        public bool IsSupportWildcard = false;

        /// <summary>
        /// 是否支持真磁盘路径
        /// </summary>
        public bool IsSupportRealPath = false;

        /// <summary>
        /// 命令类型
        /// </summary>
        public CmdType cmdType;

        public string[] addParam;

        public ICommand()
        {
            cmdType = CmdType.None;
        }


        public virtual Disk Execute(Disk disk, string param)
        {
            return null;
        }

    }
}
