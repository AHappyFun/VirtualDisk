using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDisk
{
    class Program
    {
        static void Main(string[] args)
        {
            // 手动创建然后保存磁盘
           /* Disk disk = new Disk();
            disk.SetRoot(new Floder("c:", null, disk));
           
           File f1 = disk.CreateNode(0, "1.txt", disk.root) as File;
           File f2 = disk.CreateNode(0, "11.txt", disk.root) as File;
           File f3 = disk.CreateNode(0, "2.txt", disk.root) as File;
           
           Floder fd1 = disk.CreateNode(1, "b in", disk.root) as Floder;
           File fd1f1 = disk.CreateNode(0, "1.txt", fd1) as File;
           File fd1f2 = disk.CreateNode(0, "11.txt", fd1) as File;
           File fd1f3 = disk.CreateNode(0, "2.txt", fd1) as File;
           Floder fd1fd1 = disk.CreateNode(1, "st", fd1) as Floder;
           File fd1fd1f1 = disk.CreateNode(0, "fff.txt", fd1fd1) as File;

            Floder fd2 = disk.CreateNode(1, "baa", disk.root) as Floder;
           File fd2f1 = disk.CreateNode(0, "1.txt", fd2) as File;
           File fd2f2 = disk.CreateNode(0, "11.txt", fd2) as File;
           File fd2f3 = disk.CreateNode(0, "2.txt", fd2) as File;
           
           Symlink s1 = disk.CreateNode(2, "sym", disk.root) as Symlink;
           s1.SetLinkTarget(fd1);
           
           Symlink s2 = disk.CreateNode(2, "sym 1", disk.root) as Symlink;
           s2.SetLinkTarget(s1);
           
           Symlink s3 = disk.CreateNode(2, "s.txt", disk.root) as Symlink;
           s3.SetLinkTarget(f1);
           
           Symlink s4 = disk.CreateNode(2, "y.txt", disk.root) as Symlink;
           s4.SetLinkTarget(s3);
           
           disk.SaveToDisk("save.bin");
           Console.Read();*/

            //直接获取的本地保存的磁盘，如果第一次打开需要执行下上面的那段代码先构建一个磁盘


            Disk disk = RealDiskTool.Instance.DeserializeDisk("save.bin");

            Console.Write(disk.current.GetPath() + ">");
            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd == "exit")
                {
                    break;
                }
                Disk d = disk.ExecuteCmd(cmd);
                if (d != null)
                {
                    disk = d;
                    Console.WriteLine("磁盘已重新加载.");
                }
                Console.Write(disk.current.GetPath() + ">");
            }

        }
    }
}
