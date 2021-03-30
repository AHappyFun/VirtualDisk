using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// 对真磁盘操作的工具类
    /// </summary>
    public class RealDiskTool
    {
        private static RealDiskTool _instance;
        public static RealDiskTool Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    _instance = new RealDiskTool();
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 分块读二进制文件，也可以都保存在一个byte[]中，但是需要消耗一整块连续的内存。分块的话可以利用一些内存的小空余。
        /// 1024bytes == 1kb = 0.001mb
        /// </summary>
        List<byte[]> ReadBinFile(string path)
        {
            List<byte[]> list = new List<byte[]>();
            byte[] tmp = null;

            using (FileStream fsread = new FileStream(path, FileMode.Open))
            {
                try
                {
                    BinaryReader br = new BinaryReader(fsread);
                    int offset = 0;
                    tmp = new byte[1024];
                    tmp = br.ReadBytes(1024);
                    offset += 1024;
                    list.Add(tmp);
                    while (offset < fsread.Length)
                    {
                        tmp = br.ReadBytes(1024);
                        offset += 1024;
                        list.Add(tmp);
                    }
                    tmp = null;
                    br.Close();
                    fsread.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("读取真磁盘数据失败.");
                    return null;
                }
            }
                return list;
        }

        /// <summary>
        /// 读文本数据
        /// </summary>
        /// <param name="path"></param>
        string ReadStrFile(string path)
        {
            string tmp = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                try
                {
                    tmp = sr.ReadToEnd();
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("读取真磁盘数据失败.");
                    return null;
                }
            }
            return tmp;
        }

        /// <summary>
        /// 将文件内容拷贝到node里，返回node 
        /// </summary>
        public Node CopyRealFileToNode(string srcPath, Node destPath, Disk disk)
        {
            if (destPath == null) return null;

            //判断源文件是二进制还是文本
            bool isBin = false;
            if (System.IO.File.Exists(srcPath))
            {
                FileInfo fileinfo = new FileInfo(srcPath);
                isBin = !(fileinfo.Extension == ".txt");
            }
            else
            {
                Console.WriteLine("源文件不存在");
                disk.RemoveNode(destPath);
                return null;
            }
            
            if (isBin)
            {
                if(destPath is File f)
                {
                    List<byte[]> bs = ReadBinFile(srcPath);
                    if (bs != null)
                        f.SetBinData(bs);
                    else
                    {
                        disk.RemoveNode(destPath);
                        return null;
                    }
                }
            }
            else
            {
                if(destPath is File f)
                {
                    string str = ReadStrFile(srcPath);
                    if (str != null)
                        f.SetStrData(str);
                    else
                    {
                        disk.RemoveNode(destPath);
                        return null;
                    }
                }
            }

            return destPath;
        }

        public bool SerializeDisk(List<Node> nodes, string realPath)
        {
            try
            {
                FileStream fs = new FileStream(realPath, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, nodes);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                CmdStrTool.ShowTips(6);
                return false;
            }    
        }

        public Disk DeserializeDisk(string realPath)
        {
            try
            {
                FileStream fs = new FileStream(realPath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                List<Node> nodes = bf.Deserialize(fs) as List<Node>;
                fs.Close();
                if (nodes != null && nodes.Count > 0)
                {
                    Disk d = new Disk(nodes[0]);
                    d.nodeCount = nodes.Count;
                    d.allNodes = nodes;
                    //List<Node> ns = new List<Node>();
                    foreach (Node item in nodes)
                    {
                        item.SetDisk(d);
                    }
                    return d;
                }
                else
                {
                    Console.WriteLine("加载二进制序列化文件失败");
                    return null;
                }  
            }
            catch (Exception)
            {
                CmdStrTool.ShowTips(6);
                return null;
            }
        }
    }
}
