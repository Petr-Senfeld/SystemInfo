using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace System_Information
{
    class LoggerTool
    {
        public static void Logger(string s)
        {
            File.AppendAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log.txt", s + "\n");
        }
    }
}
