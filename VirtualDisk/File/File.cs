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
    class File : Node, IDisposable
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
            Dispose(false); //释放非托管内存
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



        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if(binData != null)
                    {
                        binData.Clear();
                    }
                    if(strData!= null)
                    {
                        strData.Clear();
                        strData = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~File() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
