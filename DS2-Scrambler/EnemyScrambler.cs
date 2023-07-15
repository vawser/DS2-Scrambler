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
using System.Reflection;
using Org.BouncyCastle.Crypto;

namespace DS2_Scrambler
{
    public class EnemyScrambler
    {
        public Random rand;
        public Regulation regulation;
        public ScramblerData Data;

        public bool Change_Enemy_Location = false;
        public bool Change_Enemy_Type_Basic = false;
        public bool Change_Enemy_Type_Boss = false;
        public bool Change_Enemy_Type_Character = false;
        public bool Location_Ordered_Placement = false;
        public bool Location_Include_Characters = false;
        public bool Location_Include_NGP = false;

        public EnemyScrambler(Random random, Regulation reg, ScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;
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
                                    addRow = MayAdjustRow(EnemyParamID, Data.ID_List_Characters);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, Data.ID_List_Bosses);

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
                                    editRow = MayAdjustRow(oldEnemyParamID, Data.ID_List_Characters);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, Data.ID_List_Bosses);

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
                                    addRow = MayAdjustRow(EnemyParamID, Data.ID_List_Characters);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, Data.ID_List_Bosses);

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
                                    editRow = MayAdjustRow(oldEnemyParamID, Data.ID_List_Characters);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, Data.ID_List_Bosses);

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

        public bool MayAdjustRow(int value, List<int> list)
        {
            bool change = true;

            foreach (int entry in list)
            {
                string entryStr = entry.ToString();
                int target_id = int.Parse(entryStr.Remove(entryStr.Length - 2, 2));
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
        public Regulation Scramble_Enemies(bool scrambleLocation, bool scrambleTypeBasic, bool location_OrderedPlacement, bool location_IncludeCharacters, bool location_NGP, bool scrambleTypeBoss, bool scrambleTypeCharacter, bool enableFuriousEnemies)
        {
            Change_Enemy_Location = scrambleLocation;
            Change_Enemy_Type_Basic = scrambleTypeBasic;
            Change_Enemy_Type_Boss = scrambleTypeBoss;
            Change_Enemy_Type_Character = scrambleTypeCharacter;
            Location_Ordered_Placement = location_OrderedPlacement;
            Location_Include_Characters = location_IncludeCharacters;
            Location_Include_NGP = location_NGP; // New Game Plus

            if(scrambleLocation)
                ScrambleEnemyLocations();

            if(scrambleTypeBasic)
                ScrambleEnemyTypes();

            if (enableFuriousEnemies)
                ApplyEnemyAggressionMod();

            return regulation;
        }

        public void ScrambleEnemyLocations()
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorlocation"))
                {
                    string map_id = wrapper.Name.Replace("generatorlocation_", "");

                    // Ignore boss rows
                    foreach (int row_id in Data.Per_Map_Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore NG+ rows unless included
                    if (!Location_Include_NGP)
                    {
                        foreach (int row_id in Data.Per_Map_NGP_Dict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }

                    // Ignore Character rows unless included
                    if (!Location_Include_Characters)
                    {
                        foreach (int row_id in Data.Per_Map_Character_Dict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }

                    if (Location_Ordered_Placement)
                    {
                        List<List<float>> positionList = new List<List<float>>();

                        foreach (PARAM.Row row in param_rows)
                        {
                            var values = new List<float>();

                            values.Add((float)row["PositionX"].Value);
                            values.Add((float)row["PositionY"].Value);
                            values.Add((float)row["PositionZ"].Value);

                            positionList.Add(values);
                        }

                        positionList.Shuffle(rand);
                        int index = 0;

                        foreach (PARAM.Row row in param_rows)
                        {
                            row["PositionX"].Value = positionList[index][0];
                            row["PositionY"].Value = positionList[index][1];
                            row["PositionZ"].Value = positionList[index][2];

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
        }

        public void ScrambleEnemyTypes()
        {
            List<PARAM.Row> GeneratorRegisters = new List<PARAM.Row>();

            // Build generator register
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorregistparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorregistparam", "");

                    // Ignore skip rows
                    foreach (int row_id in Data.Per_Map_Skip_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore boss rows
                    foreach (int row_id in Data.Per_Map_Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore character
                    foreach (int row_id in Data.Per_Map_Character_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        
                    }
                }
            }

            // Apply the scrambled registers to the generators
        }

        public void ApplyEnemyAggressionMod()
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    // Ignore boss rows
                    foreach (int row_id in Data.Per_Map_Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore character
                    foreach (int row_id in Data.Per_Map_Character_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        row["AggroGroup"].Value = 2;
                        row["Leash Distance"].Value = 10000;
                    }
                }
            }
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
