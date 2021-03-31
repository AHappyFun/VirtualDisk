using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 文件
    /// </summary>
    [Serializable]
    class File : Node
    {
        /// <summary>
        /// 二进制文件数据
        /// </summary>
        List<byte[]> binData;

        /// <summary>
        /// 字符文件数据
        /// </summary>
        StringBuilder strData;

        public File(string name, Node parent, Disk disk) : base(name, parent, disk)
        {
            nodeType = 0;
        }

        public File(string name, Node parent, Disk disk, List<byte[]> data) : base(name, parent, disk)
        {
            nodeType = 0;
            this.binData = data;
        }

        ~File()
        {
            if (binData != null)
            {
                binData.Clear();
                binData = null;
            }
            if (strData != null)
            {
                strData.Clear();
                strData = null;
            }
        }

        public File(string name, Node parent, Disk disk, StringBuilder data) : base(name, parent, disk)
        {
            this.strData = data;
        }

        public void SetBinData(List<byte[]> data)
        {
            this.binData = data;
        }

        public void SetStrData(string data)
        {
            this.strData = new StringBuilder(data);
        }

        public override void CopyData(Node src)
        {
            File f = src as File;
            this.binData = f.binData;
            this.strData = f.strData;
        }

        public override void ShowInfoType()
        {
            Console.Write("\t类型: file");
        }

        public override void ShowDetailInfo()
        {
            Console.Write("\t{0}", GetFileLength());
        }

        string GetFileLength()
        {
            int length = 0; 
            if (strData != null)
                length+= strData.Length;
            if(binData!=null)
            {
                for (int i = 0; i < binData.Count; i++)
                {
                    length += binData[i].Length;
                }
            }
            return length.ToString();
        }

    }
}
