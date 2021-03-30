using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    [Serializable]
    abstract public class Node
    {
        /// <summary>
        /// 结点名
        /// </summary>
        public string name;

        /// <summary>
        /// 结点类型 0文件 1文件夹 2符号链接
        /// </summary>
        public int nodeType = -1;

        /// <summary>
        ///  文件修改时间
        /// </summary>
        protected DateTime date = DateTime.MinValue;

        /// <summary>
        /// 父节点
        /// </summary>
        public Node parent;

        public int parentIndex;

        [NonSerialized]
        protected Disk disk; //当前属于的磁盘

        /// <summary>
        /// 结点的全局索引
        /// </summary>
        public int index = 0;

        /// <summary>
        /// 当前节点的绝对路径
        /// </summary>
        protected string fullPath;

        public Node(string name, Node parent, Disk disk)
        {
            date = DateTime.Now;
            this.disk = disk;
            this.name = name;
            this.parent = parent;
            if(parent!=null)
                this.parentIndex = parent.index;
            if(parent is Floder f)
            {
                f.AddChild(this);
            }
        }

        /// <summary>
        ///  通过结点得出磁盘绝对路径
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            if(parent != null)
            {
                StringBuilder sb = new StringBuilder();
                GetCurrPath(this, ref sb);
                fullPath = sb.ToString();
                return fullPath;
            }
            else
            {
                fullPath = name;
                return fullPath;
            }          
        }

        void GetCurrPath(Node n, ref StringBuilder sb)
        {
            if(n.nodeType == 1)
                sb.Insert(0, "\\");
            sb.Insert(0, n.name);
            if (n.parent != null)
            {
                GetCurrPath(n.parent, ref sb);
            }
        }

        public void SetParent(Node p)
        {
            if(p.nodeType == 1)
                this.parent = p;
            else
                Console.WriteLine("父节点必须是文件夹类型");
        }

        public void SetDisk(Disk d)
        {
            this.disk = d;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetDate()
        {
            this.date = DateTime.Now;
        }

        public virtual void ShowInfo()
        {
            Console.Write("{0}", date.ToString());
            ShowInfoType();

            ShowDetailInfo();
            Console.Write("\t文件名: {0}", name);

            ShowInfoPath();
            Console.WriteLine("\tindex: {0}", index);
        }

        public virtual void ShowInfoType()
        {

        }

        public virtual void ShowInfoPath()
        {
            Console.Write("\t文件路径:{0}", GetPath());
        }

        public virtual void ShowDetailInfo()
        {
            Console.Write("\t");
        }

        public virtual void CopyData(Node src)
        {

        }

    }
}
