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
    class Symlink : Node
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

        ~Symlink()
        {
            target = null;
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


    }
}
