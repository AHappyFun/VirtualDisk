using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VirtualDisk
{
    /// <summary>
    /// Cmd命令拆分等操作工具
    /// </summary>
    class CmdStrTool
    {
        /// <summary>
        /// 将cmd命令拆分成命令类型和后面的参数
        /// </summary>
        public static CmdType JudgeCmdType(string cmd, out string param)
        {
            string[] ar = cmd.ToLower().Trim().Split(new char[] { ' ' }, 2);
            string tmp = ar[0];
            if(ar.Length > 1)
            {
                param = ar[1];
            }
            else
            {
                param = "";
            }

            if (string.IsNullOrEmpty(tmp) || tmp == "")
            {
                ShowTips(1);
                return CmdType.None;
            }
            else
            {
                switch (tmp)
                {
                    case "dir":
                        return CmdType.Dir;
                    case "md":
                        return CmdType.Md;
                    case "cd":
                        return CmdType.Cd;
                    case "copy":
                        return CmdType.Copy;
                    case "del":
                        return CmdType.Del;
                    case "rd":
                        return CmdType.Rd;
                    case "ren":
                        return CmdType.Ren;
                    case "move":
                        return CmdType.Move;
                    case "mklink":
                        return CmdType.Mklink;
                    case "save":
                        return CmdType.Save;
                    case "load":
                        return CmdType.Load;
                    case "cls":
                        return CmdType.Cls;
                }
                ShowTips(1);
                return CmdType.None;
            }

        }

        /// <summary>
        /// 将命令参数拆分成路径参数和附加参数
        /// </summary>
        public  static void SplitParam(string param, string[] supportAddParam, out string path, out string addParam)
        {
            string[] p1 = SplitStringHasSpace(param);
            if (p1.Length > 0)
            {
                foreach (string item in supportAddParam)
                {
                    if (item == p1[0])
                    {
                        addParam = item;
                        if (p1.Length > 1)
                        {
                           //path = p1[1];
                           //path.Replace("/", "\\");
                            StringBuilder ssb = new StringBuilder();
                            for (int i = 1; i < p1.Length; i++)
                            {
                                if (i != 1)
                                    ssb.Append(" ");
                                ssb.Append(p1[i]);
                            }
                            ssb.Replace("/", "\\");
                            path = ssb.ToString();
                        }
                        else
                        {
                            path = "";
                        }
                        return;
                    }
                }
                //path = p1.;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < p1.Length; i++)
                {
                    if(i != 0)
                        sb.Append(" ");
                    sb.Append(p1[i]);
                }
                sb.Replace("/", "\\");
                path = sb.ToString();
                addParam = "";
            }
            else
            {
                path = "";
                addParam = "";
            }
        }

        /// <summary>
        /// 将路径参数拆分成多个路径
        /// </summary>
        public static string[] SplitPathParamToPathList(string pathParam)
        {
            string[] pathlist = pathParam.Split(' ');
            return pathlist;
        }

        /// <summary>
        /// 将路径拆分成namelist
        /// </summary>
        public static string[] SplitPathToNameList(string path)
        {
            string[] namelist = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            return namelist;
        }

        ///使用空格切分，但路径可能有空格时候的拆分
        static string[] SplitStringHasSpace(string path)
        {
            bool backing = false;
            char tmp = '_';
            char[] chars = path.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if(chars[i] == '\"')
                {
                    if (backing)  //双引号末尾了
                    {
                        backing = false;                 
                    }
                    else   //双引号开始
                    {
                        backing = true;
                    }
                }
                else
                {
                    if (backing)  //检查
                    {
                        if(chars[i] == ' ')
                        {
                            chars[i] = tmp;
                        }
                    }
                    else //下一个
                    {
                        continue;
                    }
                }
            }
            string str = new string(chars);
            string[] pathlist = str.Split(' ');
            //foreach (string item in pathlist)
            //{
            //    item.Replace("_", "&nbsp;");
            //}
            return pathlist;
        }

        /// <summary>
        /// 通配符转正则 处理 ? *
        /// </summary>
        public static String WildCardToRegex(string rex)
        {
            return "^" + Regex.Escape(rex).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        public static bool NameIsGood(string name)
        {
            return name.Contains("\\") || name.Contains("*") || name.Contains("/") || name.Contains("_") || name.Contains("?") || name.Contains(":") || name.Contains("\"");
        }

        //------------其他方法-------------
        public static void ShowTips(int tip)
        {
            Console.WriteLine(Tips[tip]);
        }

        readonly static Dictionary<int, string> Tips = new Dictionary<int, string>
        {
            {1, "命令格式不正确"},
            {2, "命令格式不正确或路径不正确"},
            {3, "不可以修改根结点"},
            {4, "已经存在此结点" },
            {5, "文件名不合规范还有非法字符"},
            {6, "找不到系统磁盘路径或加载文件格式错误"},
            {7, "保存文件失败"}
        };

    }
}
