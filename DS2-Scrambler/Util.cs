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

        public static List<string> BuildIDList(string path)
        {
            List<string> idList = new List<string>();

            foreach (string line in File.ReadLines($"{path}.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                idList.Add(list[0]);
            }

            return idList;
        }

        public static List<PARAM.Row> GetRowsFromMatch(List<PARAM.Row> rows, List<string> list, bool invertMatch = false)
        {
            var new_rows = new List<PARAM.Row>();

            foreach (PARAM.Row row in rows)
            {
                bool addRow = false;

                foreach (string entry in list)
                {
                    int target_id = int.Parse(entry);
                    int short_row_id = int.Parse(row.ID.ToString());

                    if (invertMatch)
                    {
                        if (short_row_id != target_id)
                        {
                            addRow = true;
                        }
                        else
                        {
                            addRow = false;
                            break;
                        }
                    }
                    else
                    {
                        if (short_row_id == target_id)
                            addRow = true;
                    }
                }

                if (addRow)
                    new_rows.Add(row);
            }

            return new_rows;
        }
        
        public static List<PARAM.Row> GetRowsFromSubMatch(List<PARAM.Row> rows, List<string> list, int targetAdjust, int rowAdjust, string appendString, bool invertMatch = false)
        {
            var new_rows = new List<PARAM.Row>();

            foreach (PARAM.Row row in rows)
            {
                bool addRow = false;

                foreach (string entry in list)
                {
                    int target_id = int.Parse(appendString + entry.Remove(entry.Length - targetAdjust, targetAdjust));
                    string r = row.ID.ToString();
                    r = r.Remove(r.Length - rowAdjust, rowAdjust);
                    int short_row_id = int.Parse(r);

                    if (invertMatch)
                    {
                        if (short_row_id != target_id)
                        {
                            addRow = true;
                        }
                        else
                        {
                            addRow = false;
                            break;
                        }
                    }
                    else
                    {
                        if (short_row_id == target_id)
                            addRow = true;
                    }
                }

                if (addRow)
                    new_rows.Add(row);
            }

            return new_rows;
        }
    }
}
