using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 文件夹
    /// </summary>
    [Serializable]
    public class Floder : Node, IDisposable
    {
        /// <summary>
        /// 储存所有子结点
        /// </summary>
        public List<Node> childs;

        public Floder(string name, Node parent, Disk disk) : base(name, parent, disk)
        {
            nodeType = 1;
            childs = new List<Node>();
        }

        ~Floder()
        {
            Dispose(false); //释放非托管内存
        }

        public override void CopyData(Node src)
        {
           // this.childs = 
        }

        public void AddChild(Node n, bool cover = false)
        {
            if (childs == null || n == null)
            {
                return;
            }
            Node tmp = GetChildByName(n.name);
            if (tmp == null)  //当前不存在重名结点
            {
                childs.Add(n);
                n.parent = this;
            }
            else                 //有重名结点
            {
                if (n.nodeType == tmp.nodeType) //类型一致
                {
                    if (cover) //默认覆盖
                    {
                        //--删掉之前的
                        childs.Remove(tmp);
                        tmp = null;

                        childs.Add(n);
                        n.parent = this;
                    }
                    else
                    {
                        Console.WriteLine("当前已存在{0}，是否替换文件Y/N:", n.name);
                        string y = Console.ReadLine().ToLower().Trim();
                        if (y[0] == 'y')
                        {
                            //--删掉之前的
                            childs.Remove(tmp);
                            tmp = null;

                            childs.Add(n);
                            n.parent = this;

                        }
                        else if (y[0] == 'n')
                        {
                            //保留不做处理
                            return;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("存在同名且类型不同的结点，不可替换");
                }
            }

        }

        /// <summary>
        /// 移除子节点n
        /// </summary>
        public void RemoveChild(Node n)
        {
            if (childs == null || n == null)
            {
                return;
            }
            if (childs.Count > 0 && childs.Contains(n))
            {
                childs.Remove(n);
                //移除之后先不能在这里释放掉内存
            }
        }

        public Node GetChildByName(string name, bool isMatch = false)
        {
            //1.先对特殊名进行字符进行转换
            string tmp = string.Empty;
            if(name == ".")  //文件name不能是. / \ 这种
            {
                //name转换成当前结点
                return disk.current;
            }
            else if(name == "..")
            {
                //name转换成上一节点
                Node f = this.parent;
                if (f != null)
                    return f;
                else
                    return this;
            }
            else
            {
                //如果是中间有空格的
                if (name.Contains("\"")) 
                {                
                    name = name.Trim(new char[] { '"' }).Replace('_', ' ');                   
                }
                //通配符
                if(isMatch && name.Contains("*") || name.Contains("?"))
                {
                    var rex = CmdStrTool.WildCardToRegex(name);
                    List<string> names = new List<string>();
                    foreach (Node item in childs)
                    {
                        names.Add(item.name);
                    }
                    List<string> list = names.Where(ex => Regex.IsMatch(ex, rex, RegexOptions.IgnoreCase)).ToList();
                    if (list.Count > 0)
                        name = list[0];
                }
            }

            //2.find node
            foreach (Node node in childs)
            {
                if(node.name == name)
                {
                    return node;
                }
            }
            return null;
        }

        public override void ShowInfoType()
        {
            Console.Write("\t类型: floder");
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(childs!= null)
                    {
                        childs.Clear();
                        childs = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Floder() {
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
