using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoulsFormats.PARAM;

namespace DS2_Scrambler
{
    public class Util
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void CopyMod(string basePath, string scrambledPath)
        {
            if (basePath == "" || basePath == null || scrambledPath == "" || scrambledPath == null)
            {
                return;
            }

            if (System.IO.Directory.Exists(scrambledPath))
            {
                System.IO.Directory.Delete(scrambledPath, true);
            }

            System.IO.Directory.CreateDirectory(scrambledPath);

            string source = basePath;
            string destination = scrambledPath;

            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            foreach (string newPath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }
    }
}
