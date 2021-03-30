using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 符号链接
    /// </summary>
    [Serializable]
    class Symlink : Node, IDisposable
    {
        /// <summary>
        /// 链接的目标
        /// </summary>
        public Node target;

        public int targetIndex = 0;

        public Symlink(string name, Node parent, Disk disk) : base(name, parent,disk)
        {
            nodeType = 2;
        }

        public Symlink(string name, Node parent, Disk disk, Node target) : base(name, parent, disk)
        {
            nodeType = 2;
            this.target = target;
            this.targetIndex = target.index;
        }

        public void SetLinkTarget(Node target)
        {
            this.target = target;
            this.targetIndex = target.index;
        }

        public bool IsFileLink()
        {
            return target.nodeType != 2;
        }

        public override void ShowInfoType()
        {
            if (target.nodeType == 0)
                Console.Write("\t类型: FSylink");
            else if (target.nodeType == 1)
            {
                Console.Write("\t类型: DSylink");
            }
            else
            {
                Console.Write("\t类型: LSylink");
            }
        }

        public override void CopyData(Node src)
        {
            Symlink s = src as Symlink;
            this.target = s.target;
            this.targetIndex = s.targetIndex;
        }

        public override void ShowInfoPath()
        {
            base.ShowInfoPath();
            Console.Write("[{0}]", target.GetPath());
        }

        public void AddChild(Node n)
        {
            if (IsFileLink())
            {
                Console.WriteLine("不可向文件符号链接中加入结点");
                return;
            }
            else
            {
                if(target is Floder f)
                {
                    f.AddChild(n);
                }
            }        
        }

        /// <summary>
        /// 移除子节点n
        /// </summary>
        public void RemoveChild(Node n)
        {
            if (IsFileLink())
            {
                Console.WriteLine("不可从文件符号链接中删除结点");
                return;
            }
            else
            {
                if (target is Floder f)
                {
                    f.RemoveChild(n);
                }
            }
        }

        ~Symlink()
        {
            Dispose(false); //释放非托管内存
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
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Symlink() {
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
