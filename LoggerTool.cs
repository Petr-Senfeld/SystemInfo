using System;
using System.IO;
using System.Reflection;

namespace System_Information
{
    class LoggerTool
    {
        public static void Logger(string s)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\log.txt";
            if (File.Exists(path))
                File.Delete(path);
            else
                File.AppendAllText(path, (DateTime.Now.ToUniversalTime() + " " + s + "\n"));
        }
    }
}
