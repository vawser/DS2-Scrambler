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
        public EnemyData EnemyData;

        public bool Change_Enemy_Location = false;
        public bool Change_Enemy_Type_Basic = false;
        public bool Change_Enemy_Type_Boss = false;
        public bool Change_Enemy_Type_Character = false;
        public bool Location_Ordered_Placement = false;
        public bool Location_Include_Characters = false;
        public bool Location_Include_NGP = false;

        public Dictionary<string, List<int>> Per_Map_Excluded_Dict = new Dictionary<string, List<int>>
        {
            // Forest of Fallen Giants - Hollows Behind Door
            { "m10_10_00_00", new List<int>{ 2520, 2521, 2522 } }

            // Huntmans Copse
            // Add the skeletons used by the Skeleton Lords fight
        };


        // TODO: add Petrified enemy list
        // TODO: add Petrified enemy location scramble option.
        // FIX: player start enemy is being moved in Things Betwixt
        // FIX: add Vengarl Head npc to the character list

        public EnemyScrambler(Random random, Regulation reg, ScramblerData scramblerData, EnemyData enemyData)
        {
            Data = scramblerData;
            EnemyData = enemyData;
            rand = random;
            regulation = reg;
        }

        public Regulation Scramble_Enemies(bool scrambleLocation, bool scrambleTypeBasic, bool location_OrderedPlacement, bool location_IncludeCharacters, bool location_NGP, bool scrambleTypeBoss, bool scrambleTypeCharacter, bool enableFuriousEnemies)
        {
            Change_Enemy_Location = scrambleLocation;
            Change_Enemy_Type_Basic = scrambleTypeBasic;
            Change_Enemy_Type_Boss = scrambleTypeBoss;
            Change_Enemy_Type_Character = scrambleTypeCharacter;
            Location_Ordered_Placement = location_OrderedPlacement;
            Location_Include_Characters = location_IncludeCharacters;
            Location_Include_NGP = location_NGP; // New Game Plus

            if (scrambleLocation)
                ScrambleEnemyLocations();

            if (scrambleTypeBasic)
                ScrambleEnemyTypes();

            if (enableFuriousEnemies)
                ApplyEnemyAggressionMod();

            return regulation;
        }

        #region Enemy Type

        Dictionary<string, List<int>> enemyDictPerMap = new Dictionary<string, List<int>>();

        public void ScrambleEnemyTypes()
        {
           
            //Console.WriteLine("Dragonrider Test");
            //ChangeEnemyType("m10_31_00_00", 61100000, 842, "m10_02_00_00", 30000000, 1600);
        }

        public void ChangeEnemyType(string target_map_id, int target_generator_id, int target_regist_id, string source_map_id, int source_generator_id, int source_regist_id)
        {
            List<ParamWrapper> target_regist_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorregistparam_{target_map_id}")).ToList();
            List<ParamWrapper> target_generator_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorparam_{target_map_id}")).ToList();

            Console.WriteLine($"target_regist_wrapper: {target_regist_wrapper.Count}");
            Console.WriteLine($"target_generator_wrapper: {target_generator_wrapper.Count}");

            List<ParamWrapper> source_regist_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorregistparam_{source_map_id}")).ToList();
            List<ParamWrapper> source_generator_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorparam_{source_map_id}")).ToList();

            Console.WriteLine($"source_regist_wrapper: {source_regist_wrapper.Count}");
            Console.WriteLine($"source_generator_wrapper: {source_generator_wrapper.Count}");

            List<PARAM.Row> target_regist_rows_id_verification = target_regist_wrapper[0].Rows;

            List<PARAM.Row> target_regist_rows = target_regist_wrapper[0].Rows.Where(r => r.ID == target_generator_id).ToList();
            List<PARAM.Row> target_generator_rows = target_generator_wrapper[0].Rows.Where(row => row.ID == target_regist_id).ToList();

            Console.WriteLine($"target_regist_rows: {target_regist_rows.Count}");
            Console.WriteLine($"target_generator_rows: {target_generator_rows.Count}");

            List<PARAM.Row> source_regist_rows = source_regist_wrapper[0].Rows.Where(r => r.ID == source_generator_id).ToList();
            List<PARAM.Row> source_generator_rows = source_generator_wrapper[0].Rows.Where(row => row.ID == source_regist_id).ToList();

            Console.WriteLine($"source_regist_rows: {source_regist_rows.Count}");
            Console.WriteLine($"source_generator_rows: {source_generator_rows.Count}");

            PARAM.Row target_regist = target_regist_rows[0];
            PARAM.Row target_generator = target_generator_rows[0];

            Console.WriteLine($"target_regist: {target_regist.ID}");
            Console.WriteLine($"target_generator: {target_generator.ID}");

            PARAM.Row source_regist = source_regist_rows[0];
            PARAM.Row source_generator = source_generator_rows[0];

            Console.WriteLine($"source_regist: {source_regist.ID}");
            Console.WriteLine($"source_generator: {source_generator.ID}");

            // Change Dragonrider
            int new_id = target_regist.ID;

            bool isUnique = false;
            while (!isUnique)
            {
                new_id = new_id + 1;
                isUnique = IsUniqueRowID(target_regist_rows_id_verification, new_id);
                Console.WriteLine($"Unique ID Search result: {new_id} - {isUnique}");
            }

            target_regist.ID = new_id;
            target_regist["EnemyParamID"].Value = target_regist["EnemyParamID"].Value;
            target_regist["LogicParamID"].Value = target_regist["LogicParamID"].Value;
            target_regist["DefaultLogicParamID"].Value = target_regist["DefaultLogicParamID"].Value;
            target_regist["SpawnState"].Value = 0; // Zero it out to be safe

            target_generator["GeneratorRegistParam"].Value = target_regist.ID;
        }

        public bool IsUniqueRowID(List<PARAM.Row> verification_rows, int new_id)
        {
            bool isUnique = false;

            foreach (PARAM.Row row in verification_rows)
            {
                if (row.ID == new_id)
                {
                    isUnique = false;
                }
            }

            return isUnique;
        }
        #endregion

        #region Enemy Location

        public void ScrambleEnemyLocations()
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorlocation"))
                {
                    string map_id = wrapper.Name.Replace("generatorlocation_", "");

                    // Ignore excluded rows
                    if (Per_Map_Excluded_Dict.ContainsKey(map_id))
                    {
                        foreach (int row_id in Per_Map_Excluded_Dict[map_id])
                        {
                            param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                        }
                    }

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

        #endregion

        #region Enemy Tweaks
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
