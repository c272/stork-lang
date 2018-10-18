using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    class StorkIO
    {
        public static bool loadFile(string path, ref string strOut)
        {
            //Checking if file exists.
            if (!File.Exists(path))
            {
                return false;
            }

            //Opening a StreamReader.
            try
            {
                StreamReader sr = new StreamReader(path);
                strOut = sr.ReadToEnd();
            } catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
