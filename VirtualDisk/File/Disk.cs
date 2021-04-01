using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 管理文件、文件夹、符号链接的结点，对结点的各种操作
    /// </summary>
    public class Disk
    {
        /// <summary>
        /// 磁盘根结点
        /// </summary>
        public Floder root;

        /// <summary>
        /// 当前结点
        /// </summary>
        public Floder current;

        /// <summary>
        /// 所有结点
        /// </summary>
        public List<Node> allNodes;


        public int nodeCount;

        public Disk()
        {
            allNodes = new List<Node>();
        }

        public void SetRoot(Node root)
        {
            Floder f = root as Floder;
            f.index = 0;
            allNodes.Add(f);
            this.root = f;
            this.current = this.root;
            nodeCount++;
        }

        public Disk(Node root)
        {
            Floder f = root as Floder;
            allNodes = new List<Node>();
            f.index = 0;
            allNodes.Add(f);
            this.root = f;
            this.current = this.root;
            nodeCount++;
        }

        void CreateNodeIndex(Node n)
        {
            n.index = nodeCount;
            allNodes.Add(n);
            nodeCount++;
        }

        public void Clear()
        {
            root = null;
            current = null;
            allNodes.Clear();
            nodeCount = 0;
        }

        public Node CreateNode(int nodeType, string name, Node parent)
        {
            Node n;
            switch (nodeType)
            {
                case 0:
                    n = new File(name, parent, this);
                    CreateNodeIndex(n);
                    return n;
                case 1:
                    n = new Floder(name, parent, this);
                    CreateNodeIndex(n);
                    return n;
                case 2:
                    n = new Symlink(name, parent, this);
                    CreateNodeIndex(n);
                    return n;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 将n结点加入到desNode下面
        /// </summary>
        public void AddNode(Node n, Node desNode, bool cover = false)
        {
            if(n == null || desNode == null)
            {
                Console.WriteLine("AddNode 参数为null");
                return;
            }
            if(desNode.nodeType == 1)   //目标是文件夹
            {
                if (desNode is Floder f)
                {
                    f.AddChild(n, cover);
                }
            }
            else if(desNode.nodeType == 2) //目标是符号链接
            {
                if(desNode is Symlink link)
                {
                    link.AddChild(n);
                }
            }
            else
            {
                Console.WriteLine("不可以给文件下创建结点");
                return;
            }
          
        }

        /// <summary>
        /// 删除结点n
        /// </summary>
        public void RemoveNode(Node n)
        {
            if (n == null) return;
            
            if(n.parent != null)   //有父节点 用父节点的remove
            {
                if(n.parent is Floder f)
                {
                    f.RemoveChild(n);
                    allNodes.Remove(n);
                    nodeCount--;
                    Console.WriteLine("已删除结点：" + n.GetPath());
                    n = null;
                }
            }
            else  //无父节点
            {
                Console.WriteLine("不可以移除根路径");
            }
        }

        /// <summary>
        /// 将n结点移动到desNode下面
        /// </summary>
        public void MoveNode(Node n, Node destNode, bool cover)
        {
            if (n == null || destNode == null)
            {
                Console.WriteLine("moveNode 参数为null");
                return;
            }

            //先从父节点移除，再移动进去
            if(n.parent != null)
            {
                if(n.parent is Floder f)
                {
                    f.RemoveChild(n);
                }
                AddNode(n, destNode, cover);
            }
            else if(n.index != root.index)
            {
                AddNode(n, destNode, cover);
            }
            else
            {
                Console.WriteLine("不可移动根结点");
            }
            Console.WriteLine("移动：{0}--->{1}", n.GetPath(), destNode.GetPath());
        }

        /// <summary>
        /// 拷贝结点到destNode下
        /// </summary>
        public void CopyNode(Node n, Node destNode, bool cover)
        {
            if (n == null || destNode == null)
            {
                Console.WriteLine("CopyNode 参数为null");
                return;
            }
            Node nn = CreateNode(n.nodeType, n.name, null);
            nn.CopyData(n);
            switch (n.nodeType)
            {
                case 0:
                    //Node n0 = CreateNode(n.nodeType, n.name, destNode);
                    //n0CopyData(n0);
                    break;
                case 1:
                    List<Node> allchilds = new List<Node>();
                    GetAllNodes(ref allchilds, n);
                    CreateNodesTemp(allchilds, nn);
                    break;
                case 2:
                    //Node n2 = CreateNode(n.nodeType, n.name, destNode);
                    //n2.CopyData(n0);
                    break;
            }
            MoveNode(nn, destNode, cover);
            Console.WriteLine("拷贝：{0}--->{1}",n.GetPath(), destNode.GetPath());
        }

        void CreateNodesTemp(List<Node> nodes, Node parent)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Node n;
                switch (nodes[i].nodeType)
                {                  
                    case 0:
                        n = CreateNode(0, nodes[i].name, parent);
                        break;
                    case 1:
                        n = CreateNode(1, nodes[i].name, nodes[i].parent);                
                        break;
                    case 2:
                        n = CreateNode(1, nodes[i].name, parent);
                        break;
                    default:
                        n = null;
                        break;
                }
                n.CopyData(nodes[i]);
            }
        }

        /// <summary>
        /// 保存磁盘到真磁盘中
        /// </summary>
        public bool SaveToDisk(string path)
        {
            return RealDiskTool.Instance.SerializeDisk(allNodes, path);
        }

        /// <summary>
        /// 从真磁盘加载，初始化磁盘
        /// </summary>
        public bool LoadFromDisk(string path)
        {
            return false;
        }



        //-------------外部调用------------------

        /// <summary>
        /// 通过namelist转换成结点
        /// </summary>
        public Node NameListToNode(string[] names, bool isMatch = false)
        {
            if (names.Length < 1) return null;

            if(names[0] == root.name)   //绝对路径
            {
                return FindFloderWithName(root, names, true, isMatch);
            }
            else //相对路径
            {
                return FindFloderWithName(current, names, false, isMatch);
            }                      
        }

        /// <summary>
        /// 用namelist找文件夹
        /// </summary>
        Node FindFloderWithName(Node start, string[] names, bool isFullPath, bool isMatch = false)
        {
            Floder fo = start as Floder;
            if (fo == null) return null;

            int tmp = isFullPath ? 1 : 0;
            int count = names.Length;
            if(count == 1&& isFullPath) //绝对路径只有一个结点
            {
                return fo;
            }

            Node n = fo.GetChildByName(names[tmp], isMatch);
            if (names.Length > tmp + 1)
            {
                for (int i = tmp + 1; i < names.Length; i++)
                {
                    if (i == names.Length - 1)  //最后一个结点
                    {
                        if (n != null)
                        {
                            if (n is Floder f)  //找到文件夹继续找   如果是最后一个结点了，返回这个文件或者文件夹
                            {
                                n = f.GetChildByName(names[i], isMatch);
                                return n;
                            }
                            else if(n is Symlink s && s.target != null)  //找到符号链接 看目标是不是文件夹
                            {
                                n = SymlinkFinalReturnNode(s);
                                if(n is Floder f1)
                                {
                                    n = f1.GetChildByName(names[i], isMatch);
                                    return n;
                                }
                            }
                        }
                        else
                            return null;
                    }
                    else          //不是最后一个
                    {
                        if (n != null)
                        {
                            if (n is Floder f)  //找到文件夹继续找
                            {
                                n = f.GetChildByName(names[i], isMatch);
                            }
                            else   //找到个文件，不正常情况，路径错误
                            {
                                Console.WriteLine("路径不存在");
                                return null;
                            }
                        }
                        else    //没找到
                        {
                            return null;
                        }
                    }
                }
            }
            else  //如果只有1个结点
            {
                if (n != null)
                    return n;   //如果是最后一个结点了，返回这个文件或者文件夹
                else
                    return null;
            }

            return null;
        }

        Node SymlinkFinalReturnNode(Node n)
        {
            if(n != null && n is Symlink s)
            {
                return SymlinkFinalReturnNode(s.target);
            }
            else if(n != null && n is Floder f)
            {
                return f;
            }
            else
            {
                return n;
            }
        }

        /// <summary>
        /// 通过namelist创建结点
        /// </summary>
        public void CreateNodesWithNameList(string[] names)
        {
            if (names.Length < 1) return;

            if (names[0] == root.name)   //绝对路径
            {
                CreateFloder(names, 1, root, names.Length - 1);
            }
            else //相对路径
            {
                CreateFloder(names, 0, current, names.Length-1);
            }          
        }

        void CreateFloder(string[] names, int index, Floder f, int max)
        {
            if (index > max) return;
            if (f == null) return;

            Node n = f.GetChildByName(names[index]);

            if (n != null)
            {
                if(n is Floder childF)
                {
                    index++;
                    CreateFloder(names, index, childF, max);
                }
                else if(n is Symlink s)
                {
                    if(s.target.nodeType == 1)
                    {
                        Floder tarF = s.target as Floder;
                        index++;
                        CreateFloder(names, index, tarF , max);
                    }
                }
                else
                {
                    Console.WriteLine("不可在文件下创建目录");
                    return;
                }
            }
            else
            {
                Floder ff = CreateNode(1, names[index], f) as Floder;
                index++;
                CreateFloder(names, index, ff, max);
            }
        }



        /// <summary>
        /// 从一个目录获得所有子结点(不包括n)，返回list
        /// </summary>
        void GetAllNodes(ref List<Node> nodes, Node n)
        {
            if (n.nodeType != 1)
            {
                nodes.Add(n);
                return;
            }
            Floder f = n as Floder;
            if (f != null)
            {
                if(f.index != n.index)
                    nodes.Add(f);
                for (int i = 0; i < f.childs.Count; i++)
                {
                    GetAllNodes(ref nodes, f.childs[i]);
                }
            }
        }
    }
   
}
