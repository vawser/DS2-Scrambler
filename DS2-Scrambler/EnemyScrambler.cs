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

        public List<string> Boss_ID_List = new List<string>();
        public List<string> Character_ID_List = new List<string>();
        public List<string> Enemy_ID_List = new List<string>();
        public List<string> Skip_ID_List = new List<string> { "837400" };

        public Dictionary<string, List<int>> Character_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Boss_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> NGP_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Skip_Dict = new Dictionary<string, List<int>>();

        public List<int> Character_Row_ID_List = new List<int>();
        public List<int> Boss_Row_ID_List = new List<int>();
        public List<int> NGP_Row_ID_List = new List<int>();
        public List<int> Skip_Row_ID_List = new List<int>();

        public bool Change_Enemy_Location = false;
        public bool Change_Enemy_Type_Basic = false;
        public bool Change_Enemy_Type_Boss = false;
        public bool Change_Enemy_Type_Character = false;
        public bool Location_Ordered_Placement = false;
        public bool Location_Include_Characters = false;
        public bool Location_Include_NGP = false;

        public string EnemyScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Enemy-Scramble\\";

        public string OutputPath;

        public EnemyScrambler(Random random, Regulation reg, string output_path)
        {
            rand = random;
            regulation = reg;
            OutputPath = output_path;

            Boss_ID_List = Util.BuildIDList(EnemyScramblePath + "Boss-IDs");
            Character_ID_List = Util.BuildIDList(EnemyScramblePath + "Character-IDs");
            Enemy_ID_List = Util.BuildIDList(EnemyScramblePath + "Enemy-IDs");
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
                                    addRow = MayAdjustRow(EnemyParamID, Character_ID_List);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, Boss_ID_List);

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
                                    editRow = MayAdjustRow(oldEnemyParamID, Character_ID_List);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, Boss_ID_List);

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
                                    addRow = MayAdjustRow(EnemyParamID, Character_ID_List);

                                // Only check if editRow is still true
                                if (addRow && ignoreBosses)
                                    addRow = MayAdjustRow(EnemyParamID, Boss_ID_List);

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
                                    editRow = MayAdjustRow(oldEnemyParamID, Character_ID_List);

                                // Only check if editRow is still true
                                if (editRow && ignoreBosses)
                                    editRow = MayAdjustRow(oldEnemyParamID, Boss_ID_List);

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
        public Regulation Scramble_Enemies(bool scrambleLocation, bool scrambleTypeBasic, bool location_OrderedPlacement, bool location_IncludeCharacters, bool location_NGP, bool scrambleTypeBoss, bool scrambleTypeCharacter, bool enableFuriousEnemies)
        {
            Change_Enemy_Location = scrambleLocation;
            Change_Enemy_Type_Basic = scrambleTypeBasic;
            Change_Enemy_Type_Boss = scrambleTypeBoss;
            Change_Enemy_Type_Character = scrambleTypeCharacter;
            Location_Ordered_Placement = location_OrderedPlacement;
            Location_Include_Characters = location_IncludeCharacters;
            Location_Include_NGP = location_NGP; // New Game Plus

            BuildReferenceLists();

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
                    foreach (int row_id in Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore NG+ rows unless included
                    if (!Location_Include_NGP)
                    {
                        foreach (int row_id in NGP_Dict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }

                    // Ignore Character rows unless included
                    if (!Location_Include_Characters)
                    {
                        foreach (int row_id in Character_Dict[map_id])
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
                    foreach (int row_id in Skip_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore boss rows
                    foreach (int row_id in Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore character
                    foreach (int row_id in Character_Dict[map_id])
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
                    foreach (int row_id in Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore character
                    foreach (int row_id in Character_Dict[map_id])
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
        public void GetAIInfo()
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    foreach (PARAM.Row row in param_rows)
                    {
                        string chr_id = row["GeneratorRegistParam"].Value.ToString();
                        chr_id = chr_id.Remove(chr_id.Length - 4, 4);

                        string think_id = row["AIThinkID"].Value.ToString();

                        Console.WriteLine($"{chr_id};{think_id}");
                    }
                }
            }
        }

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

        public void BuildReferenceLists()
        {
            // Build lists
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

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
                            NGP_Row_ID_List.Add(row.ID);
                        }
                    }

                    NGP_Dict.Add(map_id, NGP_Row_ID_List);

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
                            foreach (string entry in Character_ID_List)
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
                            Character_Row_ID_List.Add(row.ID);
                        }
                    }

                    Character_Dict.Add(map_id, Character_Row_ID_List);

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
                            foreach (string entry in Boss_ID_List)
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
                            Boss_Row_ID_List.Add(row.ID);
                        }
                    }

                    Boss_Dict.Add(map_id, Boss_Row_ID_List);

                    // Skip List
                    foreach (PARAM.Row row in param_rows)
                    {
                        bool isSkipRow = false;
                        uint EnemyID = 0;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "GeneratorRegistParam")
                                EnemyID = (uint)cell.Value;
                        }

                        if (EnemyID > 0)
                        {
                            foreach (string entry in Skip_ID_List)
                            {
                                int target_id = int.Parse(entry.Remove(entry.Length - 2, 2));
                                string r = EnemyID.ToString();
                                r = r.Remove(r.Length - 4, 4);
                                int short_row_id = int.Parse(r);

                                if (short_row_id == target_id)
                                    isSkipRow = true;
                            }
                        }

                        if (isSkipRow)
                        {
                            Skip_Row_ID_List.Add(row.ID);
                        }
                    }

                    Skip_Dict.Add(map_id, Skip_Row_ID_List);
                }
            }
        }
        #endregion

        public Dictionary<string, int> Enemy_Info = new Dictionary<string, int>
        {
            { "1000", 1 }, // Forest Grotesque
            { "1010", 1 }, // Kobold
            { "1020", 1 }, // Hollow Soldier
            { "1021", 1 }, // Royal Soldier
            { "1030", 1 }, // Hollow Infantry
            { "1031", 1 }, // Drangleic Infantry
            { "1050", 1 }, // Amana Shrine Maiden
            { "1060", 1 }, // Hollow Priest
            { "1062", 1 }, // Hollow Priestess
            { "1070", 1 }, // Parasitized Undead
            { "1080", 1 }, // Hollow Rogue
            { "1130", 1 }, // Varangian Sailor
            { "1150", 1 }, // Undead Traveler
            { "1170", 1 }, // Stone Soldier
            { "1180", 1 }, // Black Hollow Mage
            { "1182", 1 }, // White Hollow Mage
            { "1190", 1 }, // Lion Clan Mage
            { "1210", 1 }, // Giant
            { "1230", 1 }, // Suspicious Shadow
            { "1240", 1 }, // Manikin
            { "1250", 1 }, // Rupturing Hollow
            { "1270", 1 }, // Captive Undead
            { "1271", 1 }, // Captive Undead
            { "1290", 1 }, // Forest Spirit
            { "1292", 1 }, // Forest Spirit
            { "1310", 1 }, // Lindelt Cleric
            { "1320", 1 }, // Skeleton
            { "1340", 1 }, // Gyrm
            { "1350", 1 }, // Gyrm Warrior
            { "1370", 1 }, // Prowling Magus
            { "1380", 1 }, // Torturer
            { "1390", 1 }, // Artificial Undead
            { "1410", 1 }, // Undead Aberration
            { "1460", 1 }, // Lord Tseldora
            { "1470", 1 }, // Undead Supplicant
            { "1480", 1 }, // Undead Peasant
            { "1490", 1 }, // Undead Steelworker
            { "1500", 1 }, // Stone Knight
            { "1510", 1 }, // Ironclad Soldier
            { "1512", 1 }, // Ironclad Soldier
            { "1520", 1 }, // Royal Soldier
            { "1530", 1 }, // Syan Knight
            { "1540", 1 }, // Skeleton Lord
            { "1550", 1 }, // Amana Aberration
            { "1560", 1 }, // UNKNOWN
            { "1570", 1 }, // Dual-wielding Skeleton
            { "2011", 1 }, // Small Boar
            { "2021", 1 }, // Undead Boar
            { "2030", 1 }, // Parasite Spider
            { "2040", 1 }, // Poison Moth
            { "2050", 1 }, // Poison Horn Beetle
            { "2051", 1 }, // Acid Horn Beetle
            { "2060", 1 }, // Razorback Nightcrawler
            { "2090", 1 }, // Hunting Dog
            { "2100", 1 }, // Basilisk
            { "2120", 1 }, // Guardian Dragon
            { "2130", 1 }, // Crystal Lizard
            { "2131", 1 }, // Red Crystal Lizard
            { "2140", 1 }, // Giant Undead Boar
            { "2160", 1 }, // Wall Ghost
            { "2170", 1 }, // Dark Stalker
            { "2200", 1 }, // Giant Acid Horn Beetle
            { "2220", 1 }, // Giant Basilisk
            { "2230", 1 }, // Mongrel Rat
            { "2240", 1 }, // Darksucker
            { "2260", 1 }, // Corpse Rat
            { "2261", 1 }, // Corpse Rat
            { "2262", 1 }, // Corpse Rat
            { "2270", 1 }, // Stray Dog
            { "2271", 1 }, // Stray Dog
            { "3000", 1 }, // Ogre
            { "3010", 1 }, // Heide Knight
            { "3020", 1 }, // Undead Jailer
            { "3033", 1 }, // Flexile Sentry
            { "3040", 1 }, // Milfanito
            { "3050", 1 }, // Smelter Demon
            { "3052", 1 }, // Blue Smelter Demon
            { "3060", 1 }, // Alonne Captain
            { "3070", 1 }, // Headless Vengarl
            { "3071", 1 }, // Vengarl
            { "3080", 1 }, // Lion Clan Warrior
            { "3090", 1 }, // Forgotten Giant
            { "3096", 1 }, // Last Giant
            { "3097", 1 }, // Giant Lord
            { "3110", 1 }, // Mounted Overseer
            { "3120", 1 }, // Grave Warden
            { "3130", 1 }, // Hollow Falconer
            { "3140", 1 }, // Hollow Primal Knight
            { "3150", 1 }, // Primal Knight
            { "3160", 1 }, // Desert Sorceress
            { "3170", 1 }, // Dragon Acolyte
            { "3180", 1 }, // The Pursuer
            { "3190", 1 }, // Alonne Knight
            { "3210", 1 }, // Mimic
            { "3240", 1 }, // Belfry Gargoyle
            { "3250", 1 }, // Ruin Sentinel
            { "3260", 1 }, // The Rotten
            { "3270", 1 }, // Dragon Skeleton
            { "3300", 1 }, // Old Knight
            { "3310", 1 }, // Drakekeeper
            { "3320", 1 }, // Throne Defender
            { "3330", 1 }, // Velstadt
            { "3340", 1 }, // Throne Watcher
            { "3370", 1 }, // Captive Undead
            { "5000", 1 }, // Covetous Demon
            { "5010", 1 }, // Mytha
            { "5020", 1 }, // Manscorpion Tark
            { "5030", 1 }, // Scorpioness Najka
            { "5040", 1 }, // Looking Glass Knight
            { "5061", 1 }, // Darklurker
            { "5062", 1 }, // UNKNOWN
            { "5065", 1 }, // Grave Warden Agdayne
            { "5090", 1 }, // Leydia Witch
            { "5110", 1 }, // Imperious Knight
            { "5120", 1 }, // Leydia Pyromancer
            { "5146", 1 }, // Vendrick (hollow)
            { "6000", 1 }, // Ancient Dragon
            { "6010", 1 }, // Flame Lizard
            { "6020", 1 }, // Demon of Song
            { "6030", 1 }, // The Duke's Dear Freja
            { "6031", 1 }, // The Duke's Dear Freja - Encounter
            { "6070", 1 }, // Old Iron King
            { "6080", 1 }, // Corrosive Ant Queen
            { "6110", 1 }, // Dragonrider (Heide)
            { "6115", 1 }, // Dragonrider (Drangleic)
            { "6191", 1 }, // Executioner's Chariot
            { "6250", 1 }, // Old Dragonslayer
            { "6260", 1 }, // The Lost Sinner
            { "6270", 1 }, // Nashandra
            { "6280", 1 }, // Royal Rat Authority
            { "6500", 1 }, // Iron Warrior
            { "6510", 1 }, // Fume Sorcerer
            { "6530", 1 }, // Ashen Warrior
            { "6540", 1 }, // Ashen Crawler
            { "6560", 1 }, // Possessed Armor
            { "6570", 1 }, // Barrel Carrier
            { "6580", 1 }, // Retainer
            { "6590", 1 }, // Rampart Golem
            { "6600", 1 }, // Crystal Golem
            { "6610", 1 }, // Frozen Reindeer
            { "6620", 1 }, // Rampart Hedgehog
            { "6630", 1 }, // Rampart Spearman
            { "6650", 1 }, // Sanctum Knight
            { "6660", 1 }, // Sanctum Soldier
            { "6700", 1 }, // Sanctum Priestess
            { "6710", 1 }, // Poison Statue Cluster
            { "6711", 1 }, // Poison Statue Cluster (petrify)
            { "6720", 1 }, // Corrosive Ant
            { "6740", 1 }, // Pagan Tree
            { "6750", 1 }, // Fume Knight
            { "6770", 1 }, // Retainer Sorcerer
            { "6780", 1 }, // Ice Golem
            { "6790", 1 }, // Aava
            { "6791", 1 }, // Lud - Zallen
            { "6800", 1 }, // Sir Alonne
            { "6810", 1 }, // Sinh
            { "6820", 1 }, // Elana
            { "6830", 1 }, // Imperfect
            { "6840", 1 }, // Vendrick
            { "6850", 1 }, // Skeleton (Elana)
            { "6860", 1 }, // Small Undead Boar (Elana)
            { "6870", 1 }, // Velstadt (Elana)
            { "6880", 1 }, // Loyce Knight
            { "6890", 1 }, // Charred Loyce Knight
            { "6900", 1 }, // Burnt Ivory King
            { "6920", 1 }, // Aldia
            { "6940", 1 }, // Forlorn (Greatsword)
            { "6950", 1 }, // Forlorn (Scythe)
            { "6960", 1 }, // Invisible Hollow
            { "7005", 1 }, // Emerald Herald
            { "7015", 1 }, // Emerald Herald
            { "7036", 1 }, // Nashandra (human)
            { "7045", 1 }, // Laddersmith Gilligan
            { "7050", 1 }, // Strowen
            { "7051", 1 }, // Morrel
            { "7053", 1 }, // Griant
            { "7055", 1 }, // Strowen
            { "7056", 1 }, // Morrel
            { "7058", 1 }, // Griant
            { "7211", 1 }, // Chancellor Wellager
            { "7230", 1 }, // Milibeth
            { "7240", 1 }, // Captain Drummond
            { "7250", 1 }, // Darkdiver Grandahl
            { "7260", 1 }, // Lonesome Gavlan
            { "7300", 1 }, // Looking Glass Phantom (Faraam)
            { "7310", 1 }, // Looking Glass Phantom (Knight)
            { "7320", 1 }, // Lost Sinner Pyromancer
            { "7330", 1 }, // Lost Sinner Pyromancer
            { "7410", 1 }, // Crestfallen Saulden
            { "7420", 1 }, // Creighton the Wanderer
            { "7430", 1 }, // Benhart of Jugo
            { "7440", 1 }, // Mild Mannered Pate
            { "7510", 1 }, // Cartographer Cale
            { "7520", 1 }, // Lucatiel of Mirrah
            { "7530", 1 }, // Bell Keeper
            { "7540", 1 }, // Merchant Hag Melentia
            { "7600", 1 }, // Milfanito 1
            { "7601", 1 }, // Milfanito 2
            { "7602", 1 }, // Imprisoned Milfanito
            { "7610", 1 }, // Maughlin the Armourer
            { "7620", 1 }, // Stone Trader Chloanne
            { "7630", 1 }, // Rosabeth of Melfia
            { "7640", 1 }, // Blacksmith Lenigrast
            { "7643", 1 }, // Steady Hand McDuff
            { "7660", 1 }, // Carhillion of the Fold
            { "7680", 1 }, // Straid of Olaphis
            { "7690", 1 }, // Licia of Lindeldt
            { "7700", 1 }, // Felkin the Outcast
            { "7710", 1 }, // Royal Sorcerer Navlaan
            { "7720", 1 }, // Magerold of Lanafir
            { "7760", 1 }, // Weaponsmith Ornifex
            { "7770", 1 }, // Sweet Shalquoir
            { "7830", 1 }, // Titchy Gren
            { "7840", 1 }, // Cromwell the Pardoner
            { "7850", 1 }, // Blue Sentinel Targray
            { "7860", 1 }, // Dyna & Tillo
        };
    }
}
