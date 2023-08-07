using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoulsFormats.PARAM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DS2_Scrambler
{
    public class Util
    {
        public static void PrintLine(string line)
        {
            Console.WriteLine($"{line}");
        }

        // *** Random Items
        public static void SetRandomGood(PARAM.Row row, string field_1, ScramblerData Data, string field_2)
        {
            Random rand = new Random();

            int roll = rand.Next(100);

            // Souls
            if (roll >= 0 && roll < 10)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Soul_Consumables, field_2, 1, 3);
            }
            // Throwables
            if (roll >= 10 && roll < 20)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Throwable_Consumable, field_2, 5, 25);
            }
            // HP
            if (roll >= 20 && roll < 30)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_HP_Consumables, field_2, 1, 3);
            }
            // Cast
            if (roll >= 30 && roll < 40)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Cast_Consumables, field_2, 1, 3);
            }
            // Spell Upgrades
            if (roll >= 40 && roll < 50)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Spell_Upgrades, field_2, 1, 2);
            }
            // Flask Upgrades
            if (roll >= 50 && roll < 60)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Flask_Upgrades, field_2, 1, 1);
            }
            // Bird Trades
            if (roll >= 60 && roll < 70)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Bird_Consumables, field_2, 1, 3);
            }
            // Materials
            if (roll >= 70 && roll < 80)
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Materials, field_2, 1, 3);
            }
            // Misc
            else
            {
                Util.SetRandomItemWithAmount(row, field_1, Data.Row_List_Misc_Consumable, field_2, 1, 5);
            }
        }

        public static void SetRandomItem(PARAM.Row row, string field_1, List<PARAM.Row> list)
        {
            Random rand = new Random();

            row[field_1].Value = list[rand.Next(list.Count)].ID;
        }

        public static void SetRandomItemWithAmount(PARAM.Row row, string field_1, List<PARAM.Row> list, string field_2, int min, int max)
        {
            Random rand = new Random();

            row[field_1].Value = list[rand.Next(list.Count)].ID;

            if (max > min)
                row[field_2].Value = rand.Next(min, max);
            else
                row[field_2].Value = 1;
        }

        // *** Utility
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool CopyMod(string basePath, string scrambledPath)
        {
            if (basePath == "" || basePath == null || scrambledPath == "" || scrambledPath == null)
            {
                return false;
            }

            if (System.IO.Directory.Exists(scrambledPath))
            {
                System.IO.Directory.Delete(scrambledPath, true);
            }

            bool proceed = true;

            if(!Directory.Exists(basePath + "\\Param"))
            {
                DialogResult warning = MessageBox.Show($"This directory does not contain a Param folder.\n{basePath}\n\nDo you want to proceed?", "Warning", MessageBoxButtons.YesNo);

                switch (warning)
                {
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        proceed = false;
                        break;
                }
            }

            if (proceed)
            {
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
            else
            {
                return false;
            }

            return true;
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
        
        public static List<PARAM.Row> GetRowsFromSubMatch(List<PARAM.Row> rows, List<int> list, int targetAdjust, int rowAdjust, string appendString, bool invertMatch = false)
        {
            var new_rows = new List<PARAM.Row>();

            foreach (PARAM.Row row in rows)
            {
                bool addRow = false;

                foreach (int value in list)
                {
                    string entry = value.ToString();

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
