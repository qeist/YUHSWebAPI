using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUHS.WebAPI.Common.Utility
{
    public class Utils
    {
        public static void _WriteLog(string ret, string SPNm)
        {
            string LogPath = @"C:\MCare\log";
            System.IO.DirectoryInfo oDir = new System.IO.DirectoryInfo(LogPath);
            if (!oDir.Exists) oDir.Create();

            using (FileStream oFile = new FileStream(LogPath + @"\WebService.log", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter oWriter = new StreamWriter(oFile, System.Text.Encoding.UTF8))
                {
                    oWriter.BaseStream.Seek(0, SeekOrigin.End);
                    oWriter.WriteLine(string.Format("[{0}] Excute Module : {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), SPNm));
                    oWriter.WriteLine(string.Format("[{0}] return : {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ret));
                    oWriter.Flush();
                    oWriter.Close();
                }
            }
        }
    }
}
