using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using System.Text.RegularExpressions;
using static SoulsFormats.PARAM;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Media.TextFormatting;
using System.Windows.Controls;
using System.Diagnostics.Metrics;

namespace DS2_Scrambler
{
    public class EnemyScrambler
    {
        public Random rand;
        public Regulation regulation;

        List<string> BossIDs = new List<string>();
        List<string> CharacterIDs = new List<string>();
        List<string> EnemyIDs = new List<string>();

        public string EnemyScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Enemy-Scramble\\";

        public EnemyScrambler(Random random, Regulation reg)
        {
            rand = random;
            regulation = reg;

            BossIDs = Util.BuildIDList(EnemyScramblePath + "Boss-IDs");
            CharacterIDs = Util.BuildIDList(EnemyScramblePath + "Character-IDs");
            EnemyIDs = Util.BuildIDList(EnemyScramblePath + "Enemy-IDs");
        }


        #region Enemy Type
        public Regulation Scramble_Enemy_Type(bool sharedEnemyPool, bool ignoreKeyCharacters, bool ignoreBosses)
        {
            string paramName = "generatorregistparam";
            List<List<int>> valueList = new List<List<int>>();

            // Done per map
            if (!sharedEnemyPool)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name.Contains(paramName))
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = param.Rows;

                        foreach (PARAM.Row row in param_rows)
                        {
                            bool addRow = true;

                            int EnemyParamID = 0;
                            int LogicParamID = 0;
                            int DefaultLogicParamID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "EnemyParamID")
                                    EnemyParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "LogicParamID")
                                    LogicParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "DefaultLogicParamID")
                                    DefaultLogicParamID = (int)cell.Value;
                            }

                            if (EnemyParamID > 0 && EnemyParamID != 837400)
                            {
                                if (ignoreKeyCharacters)
                                    addRow = MayAdjustRow(EnemyParamID, CharacterIDs);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, BossIDs);

                                if (addRow)
                                {
                                    List<int> value_list = new List<int>() { EnemyParamID, LogicParamID, DefaultLogicParamID };
                                    valueList.Add(value_list);
                                }
                            }
                        }

                        // Change entries
                        foreach (PARAM.Row row in param_rows)
                        {
                            bool editRow = true;

                            List<int> chosenValues = valueList[rand.Next(valueList.Count)];

                            int newEnemyParamID = chosenValues[0];
                            int newLogicParamID = chosenValues[1];
                            int newDefaultLogicParamID = chosenValues[2];

                            int oldEnemyParamID = 0;
                            int oldLogicParamID = 0;
                            int oldDefaultLogicParamID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "EnemyParamID")
                                    oldEnemyParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "LogicParamID")
                                    oldLogicParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "DefaultLogicParamID")
                                    oldDefaultLogicParamID = (int)cell.Value;
                            }

                            if (oldEnemyParamID > 0 && oldEnemyParamID != 837400)
                            {
                                if (ignoreKeyCharacters)
                                    editRow = MayAdjustRow(oldEnemyParamID, CharacterIDs);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, BossIDs);

                                if (editRow)
                                {
                                    foreach (PARAM.Cell cell in row.Cells)
                                    {
                                        if (cell.Def.InternalName == "EnemyParamID")
                                            cell.Value = newEnemyParamID;

                                        if (cell.Def.InternalName == "LogicParamID")
                                            cell.Value = newLogicParamID;

                                        if (cell.Def.InternalName == "DefaultLogicParamID")
                                            cell.Value = newDefaultLogicParamID;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Done with a shared pool across all maps
            else
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name.Contains(paramName))
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = param.Rows;

                        foreach (PARAM.Row row in param_rows)
                        {
                            bool addRow = true;

                            int EnemyParamID = 0;
                            int LogicParamID = 0;
                            int DefaultLogicParamID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "EnemyParamID")
                                    EnemyParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "LogicParamID")
                                    LogicParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "DefaultLogicParamID")
                                    DefaultLogicParamID = (int)cell.Value;
                            }

                            if (EnemyParamID > 0 && EnemyParamID != 837400)
                            {
                                if (ignoreKeyCharacters)
                                    addRow = MayAdjustRow(EnemyParamID, CharacterIDs);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, BossIDs);

                                if (addRow)
                                {
                                    List<int> value_list = new List<int>() { EnemyParamID, LogicParamID, DefaultLogicParamID };
                                    valueList.Add(value_list);
                                }
                            }
                        }
                    }
                }

                // Change entries
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name.Contains(paramName))
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = param.Rows;

                        foreach (PARAM.Row row in param_rows)
                        {
                            bool editRow = true;

                            List<int> chosenValues = valueList[rand.Next(valueList.Count)];

                            int newEnemyParamID = chosenValues[0];
                            int newLogicParamID = chosenValues[1];
                            int newDefaultLogicParamID = chosenValues[2];

                            int oldEnemyParamID = 0;
                            int oldLogicParamID = 0;
                            int oldDefaultLogicParamID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "EnemyParamID")
                                    oldEnemyParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "LogicParamID")
                                    oldLogicParamID = (int)cell.Value;

                                if (cell.Def.InternalName == "DefaultLogicParamID")
                                    oldDefaultLogicParamID = (int)cell.Value;
                            }

                            if (oldEnemyParamID > 0 && oldEnemyParamID != 837400)
                            {
                                if (ignoreKeyCharacters)
                                    editRow = MayAdjustRow(oldEnemyParamID, CharacterIDs);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, BossIDs);

                                if (editRow)
                                {
                                    foreach (PARAM.Cell cell in row.Cells)
                                    {
                                        if (cell.Def.InternalName == "EnemyParamID")
                                            cell.Value = newEnemyParamID;

                                        if (cell.Def.InternalName == "LogicParamID")
                                            cell.Value = newLogicParamID;

                                        if (cell.Def.InternalName == "DefaultLogicParamID")
                                            cell.Value = newDefaultLogicParamID;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }

        public bool MayAdjustRow(int value, List<string> list)
        {
            bool change = true;

            foreach (string entry in list)
            {
                int target_id = int.Parse(entry.Remove(entry.Length - 2, 2));
                string r = value.ToString();
                r = r.Remove(r.Length - 2, 2);
                int short_row_id = int.Parse(r);

                if (short_row_id == target_id)
                {
                    change = false;
                }
            }

            return change;
        }
        #endregion

        #region Enemy Location
        public Regulation Scramble_Enemies(bool scrambleLocation, bool scrambleType, bool location_OrderedPlacement, bool location_IncludeCharacters, bool location_IncludeSpecial, bool type_IncludeBosses, bool type_IncludeCharacters)
        {
            Dictionary<string, List<int>> characterDict = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> bossDict = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> ngPlusDict = new Dictionary<string, List<int>>();

            // Build lists
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;
                List<int> characterRowIDs = new List<int>();
                List<int> bossRowIDs = new List<int>();
                List<int> ngPlusRowIDs = new List<int>();

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    // If enemy has ApperanceEventID between 2 and 8, add it to the NG+ enemy per map dictionary
                    if (ignoreNGPlus)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            bool isNGPlusRow = false;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "ApperanceEventID")
                                {
                                    uint flag = (uint)cell.Value;

                                    if (flag >= 2 && flag <= 8)
                                    {
                                        isNGPlusRow = true;
                                    }
                                }
                            }

                            if (isNGPlusRow)
                            {
                                ngPlusRowIDs.Add(row.ID);
                            }
                        }

                        ngPlusDict.Add(map_id, ngPlusRowIDs);
                    }

                    // If enemy is considered a 'key' character, add it to the Character per map dictionary
                    if (ignoreKeyCharacters)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            bool isCharacterRow = false;
                            uint EnemyID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "GeneratorRegistParam")
                                    EnemyID = (uint)cell.Value;
                            }

                            if (EnemyID > 0)
                            {
                                foreach (string entry in CharacterIDs)
                                {
                                    int target_id = int.Parse(entry.Remove(entry.Length - 2, 2));
                                    string r = EnemyID.ToString();
                                    r = r.Remove(r.Length - 4, 4);
                                    int short_row_id = int.Parse(r);

                                    if (short_row_id == target_id)
                                        isCharacterRow = true;
                                }
                            }

                            if (isCharacterRow)
                            {
                                characterRowIDs.Add(row.ID);
                            }
                        }

                        characterDict.Add(map_id, characterRowIDs);
                    }

                    // If enemy is considered a 'boss' enemy, add it to the Boss per map dictionary
                    if (ignoreBosses)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            bool isBossRow = false;
                            uint EnemyID = 0;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "GeneratorRegistParam")
                                    EnemyID = (uint)cell.Value;
                            }

                            if (EnemyID > 0)
                            {
                                foreach (string entry in BossIDs)
                                {
                                    int target_id = int.Parse(entry.Remove(entry.Length - 2, 2));
                                    string r = EnemyID.ToString();
                                    r = r.Remove(r.Length - 4, 4);
                                    int short_row_id = int.Parse(r);

                                    if (short_row_id == target_id)
                                        isBossRow = true;
                                }
                            }

                            if (isBossRow)
                            {
                                bossRowIDs.Add(row.ID);
                            }
                        }

                        bossDict.Add(map_id, bossRowIDs);
                    }
                }
            }

            // Apply changes
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorlocation"))
                {
                    string map_id = wrapper.Name.Replace("generatorlocation_", "");

                    if (ignoreNGPlus)
                    {
                        foreach (int row_id in ngPlusDict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }
                    if (ignoreKeyCharacters)
                    {
                        foreach (int row_id in characterDict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }
                    if (ignoreBosses)
                    {
                        foreach (int row_id in bossDict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }

                    if (useOrderedPlacement)
                    {
                        List<List<float>> positionList = new List<List<float>>();

                        foreach (PARAM.Row row in param_rows)
                        {
                            var values = new List<float>();

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "PositionX")
                                    values.Add((float)cell.Value);

                                if (cell.Def.InternalName == "PositionY")
                                    values.Add((float)cell.Value);

                                if (cell.Def.InternalName == "PositionZ")
                                    values.Add((float)cell.Value);
                            }

                            positionList.Add(values);
                        }

                        positionList.Shuffle(rand);
                        int index = 0;

                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName == "PositionX")
                                    cell.Value = positionList[index][0];

                                if (cell.Def.InternalName == "PositionY")
                                    cell.Value = positionList[index][1];

                                if (cell.Def.InternalName == "PositionZ")
                                    cell.Value = positionList[index][2];
                            }

                            index = index + 1;
                        }
                    }
                    else
                    {
                        RandomizeTriple<float, float, float>(param_rows, "PositionX", "PositionY", "PositionZ");
                    }

                    RandomizeTriple<float, float, float>(param_rows, "RotationX", "RotationY", "RotationZ");
                }
            }

            return regulation;
        }
        #endregion

        #region Util
        private void RandomizeTriple<T1, T2, T3>(IEnumerable<PARAM.Row> rows, string param1, string param2, string param3)
        {
            List<(T1, T2, T3)> options = rows.Select(row => ((T1)row[param1].Value, (T2)row[param2].Value, (T3)row[param3].Value)).ToList();
            foreach (PARAM.Row row in rows)
            {
                (T1 val1, T2 val2, T3 val3) = options.PopRandom(rand);
                row[param1].Value = val1;
                row[param2].Value = val2;
                row[param3].Value = val3;
            }
        }
        #endregion
    }
}
