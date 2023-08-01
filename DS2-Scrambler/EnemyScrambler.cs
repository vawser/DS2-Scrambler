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
using static SoulsFormats.MSBD.Event;

namespace DS2_Scrambler
{
    public class EnemyScrambler
    {
        public Random rand;
        public Regulation regulation;
        public CoreScramblerData Data;

        public bool T_IncludeCharacters = false;

        public EnemyScrambler(Random random, Regulation reg, CoreScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;
        }

        public Regulation Scramble_Enemies(bool scrambleEnemyLocation, bool enableEnemySharedAggro, bool includeCharacters, bool enable_Things_Betwixt, bool enable_Majula, bool enable_Forest_of_Fallen_Giants, bool enable_Brightstone_Cove_Tseldora, bool enable_Aldias_Keep, bool enable_Lost_Bastille, bool enable_Earthen_Peak, bool enable_No_mans_Wharf, bool enable_Iron_Keep, bool enable_Huntmans_Copse, bool enable_Gutter, bool enable_Dragon_Aerie, bool enable_Path_to_Shaded_Woods, bool enable_Unseen_Path_to_Heides, bool enable_Heides_Tower_of_Flame, bool enable_Shaded_Woods, bool enable_Doors_of_Pharros, bool enable_Grave_of_Saints, bool enable_Giant_Memories, bool enable_Shrine_of_Amana, bool enable_Drangleic_Castle, bool enable_Undead_Crypt, bool enable_Dragon_Memories, bool enable_Dark_Chasm_of_Old, bool enable_Shulva, bool enable_Brume_Tower, bool enable_Eleum_Loyce, bool enable_Memory_of_the_King)
        {
            T_IncludeCharacters = includeCharacters;

            if (enableEnemySharedAggro)
                ApplyEnemyAggressionMod();

            if (scrambleEnemyLocation)
            {
                if (enable_Things_Betwixt)
                {
                    ScrambleEnemyLocation("m10_02_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_02_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_02_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_02_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_02_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_02_00_00
                        );
                    }
                }

                if (enable_Majula)
                {
                    ScrambleEnemyLocation("m10_04_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_04_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_04_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_04_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_04_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_04_00_00
                        );
                    }
                }

                if (enable_Forest_of_Fallen_Giants)
                {
                    ScrambleEnemyLocation("m10_10_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_10_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_10_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_10_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_10_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_10_00_00
                        );
                    }
                }

                if (enable_Brightstone_Cove_Tseldora)
                {
                    ScrambleEnemyLocation("m10_14_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_14_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_14_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_14_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_14_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_14_00_00
                        );
                    }

                }

                if (enable_Aldias_Keep)
                {
                    ScrambleEnemyLocation("m10_15_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_15_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_15_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_15_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_15_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_15_00_00
                        );
                    }
                }

                if (enable_Lost_Bastille)
                {
                    ScrambleEnemyLocation("m10_16_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_16_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_16_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_16_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_16_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_16_00_00
                        );
                    }
                }

                if (enable_Earthen_Peak)
                {
                    ScrambleEnemyLocation("m10_17_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_17_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_17_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_17_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_17_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_17_00_00
                        );
                    }
                }

                if (enable_No_mans_Wharf)
                {
                    ScrambleEnemyLocation("m10_18_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_18_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_18_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_18_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_18_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_18_00_00
                        );
                    }
                }

                if (enable_Iron_Keep)
                {
                    ScrambleEnemyLocation("m10_19_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_19_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_19_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_19_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_19_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_19_00_00
                        );
                    }
                }

                if (enable_Huntmans_Copse)
                {
                    ScrambleEnemyLocation("m10_23_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_23_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_23_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_23_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_23_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_23_00_00
                        );
                    }
                }

                if (enable_Gutter)
                {
                    ScrambleEnemyLocation("m10_25_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_25_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_25_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_25_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_25_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_25_00_00
                        );
                    }
                }

                if (enable_Dragon_Aerie)
                {
                    ScrambleEnemyLocation("m10_27_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_27_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_27_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_27_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_27_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_27_00_00
                        );
                    }
                }

                if (enable_Path_to_Shaded_Woods)
                {
                    ScrambleEnemyLocation("m10_29_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_29_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_29_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_29_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_29_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_29_00_00
                        );
                    }
                }

                if (enable_Unseen_Path_to_Heides)
                {
                    ScrambleEnemyLocation("m10_30_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_30_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_30_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_30_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_30_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_30_00_00
                        );
                    }
                }

                if (enable_Heides_Tower_of_Flame)
                {
                    ScrambleEnemyLocation("m10_31_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_31_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_31_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_31_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_31_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_31_00_00
                        );
                    }
                }

                if (enable_Shaded_Woods)
                {
                    ScrambleEnemyLocation("m10_32_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_32_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_32_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_32_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_32_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_32_00_00
                        );
                    }
                }

                if (enable_Doors_of_Pharros)
                {
                    ScrambleEnemyLocation("m10_33_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_33_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_33_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_33_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_33_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_33_00_00
                        );
                    }
                }

                if (enable_Grave_of_Saints)
                {
                    ScrambleEnemyLocation("m10_34_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m10_34_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m10_34_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_34_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m10_34_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m10_34_00_00
                        );
                    }
                }

                if (enable_Giant_Memories)
                {
                    ScrambleEnemyLocation("m20_10_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m20_10_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m20_10_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_10_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m20_10_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m20_10_00_00
                        );
                    }
                }

                if (enable_Shrine_of_Amana)
                {
                    ScrambleEnemyLocation("m20_11_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m20_11_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m20_11_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_11_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m20_11_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m20_11_00_00
                        );
                    }
                }

                if (enable_Drangleic_Castle)
                {
                    ScrambleEnemyLocation("m20_21_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m20_21_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m20_21_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_21_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m20_21_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m20_21_00_00
                        );
                    }
                }

                if (enable_Undead_Crypt)
                {

                    ScrambleEnemyLocation("m20_24_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m20_24_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m20_24_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_24_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m20_24_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m20_24_00_00
                        );
                    }
                }

                if (enable_Dragon_Memories)
                {

                    ScrambleEnemyLocation("m20_26_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m20_26_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m20_26_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_26_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m20_26_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m20_26_00_00
                        );
                    }
                }

                if (enable_Dark_Chasm_of_Old)
                {
                    ScrambleEnemyLocation("m40_03_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m40_03_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m40_03_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m40_03_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m40_03_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m40_03_00_00
                        );
                    }
                }

                if (enable_Shulva)
                {
                    ScrambleEnemyLocation("m50_35_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m50_35_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m50_35_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_35_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m50_35_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m50_35_00_00
                        );
                    }
                }

                if (enable_Brume_Tower)
                {
                    ScrambleEnemyLocation("m50_36_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m50_36_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m50_36_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_36_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m50_36_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m50_36_00_00
                        );
                    }
                }

                if (enable_Eleum_Loyce)
                {
                    ScrambleEnemyLocation("m50_37_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m50_37_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m50_37_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_37_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m50_37_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m50_37_00_00
                        );
                    }
                }

                if (enable_Memory_of_the_King)
                {
                    ScrambleEnemyLocation("m50_38_00_00",
                    EnemyScramblerData.Static.Generator_Skip_List_m50_38_00_00,
                    EnemyScramblerData.Static.Additional_Locations_Enemy_List_m50_38_00_00
                    );

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_38_00_00",
                        EnemyScramblerData.Static.Character_Generator_List_m50_38_00_00,
                        EnemyScramblerData.Static.Additional_Locations_Character_List_m50_38_00_00
                        );
                    }
                }
            }

            return regulation;
        }

        public void ScrambleEnemyLocation(string map_id, List<int> Ignored_Generator_ID_List, List<float>[] additionalLocations)
        {
            Console.WriteLine($"ScrambleEnemyLocation - {map_id}");

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name == $"generatorlocation_{map_id}")
                {
                    param_rows = param_rows.Where(r => 
                    !Ignored_Generator_ID_List.Contains(r.ID)
                    ).ToList();

                    AssignNewLocations(param_rows, additionalLocations);
                }
            }
        }

        public void ScrambleCharacterLocation(string map_id, List<int> Character_Generator_ID_List, List<float>[] additionalLocations)
        {
            Console.WriteLine($"ScrambleCharacterLocation - {map_id}");

            if (Character_Generator_ID_List.Count > 0)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    PARAM param = wrapper.Param;
                    List<PARAM.Row> param_rows = param.Rows;

                    if (wrapper.Name == $"generatorlocation_{map_id}")
                    {
                        param_rows = param_rows.Where(r =>
                        Character_Generator_ID_List.Contains(r.ID)
                        ).ToList();

                        AssignNewLocations(param_rows, additionalLocations);
                    }
                }
            }
        }

        public void AssignNewLocations(List<PARAM.Row> param_rows, List<float>[] additionalLocations)
        {
            List<Position> positionList = new List<Position>();

            if (additionalLocations.Length > 0)
            {
                foreach (var entry in additionalLocations)
                {
                    Position pos = new Position(entry[0], entry[1], entry[2], entry[3], entry[4], entry[5]);
                    positionList.Add(pos);
                }
            }

            // Build position list
            foreach (PARAM.Row row in param_rows)
            {
                positionList.Add(
                    new Position(
                        (float)row["PositionX"].Value,
                        (float)row["PositionY"].Value,
                        (float)row["PositionZ"].Value,
                        (float)row["RotationX"].Value,
                        (float)row["RotationY"].Value,
                        (float)row["RotationZ"].Value
                    )
                );
            }

            // Shuffle positions
            positionList.Shuffle(rand);
            int index = 0;

            // Assign new positions
            foreach (PARAM.Row row in param_rows)
            {
                row["PositionX"].Value = positionList[index].PosX;
                row["PositionY"].Value = positionList[index].PosY;
                row["PositionZ"].Value = positionList[index].PosZ;

                row["RotationX"].Value = positionList[index].RotX;
                row["RotationY"].Value = positionList[index].RotY;
                row["RotationZ"].Value = positionList[index].RotZ;

                index = index + 1;
            }
        }
        #region Enemy Type

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

        // *** Enemies
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
