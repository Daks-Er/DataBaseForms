using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsLAB6
{
    internal class Protocol
    {
        public static void Create(string fileName)
        {
            FileStream F = new FileStream(fileName, FileMode.Create);
            F.Close();
        }

        public static void Open(string fileName)
        {
            StreamWriter file = new StreamWriter(fileName, true);
            WriteProtocol("Начало работы \t| ", file);
            file.Close();
        }

        public static void WriteProtocol(string str, StreamWriter file)
        {
            file.WriteLine(str + DateTime.Now);
        }

        public static void Close(string fileName)
        {
            StreamWriter file = new StreamWriter(fileName, true);
            WriteProtocol("Конец работы \t| ", file);
            file.Close();
        }
    }
}
