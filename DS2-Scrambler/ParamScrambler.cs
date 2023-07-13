
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
    static class Extensions
    {
        public static T GetRandom<T>(this List<T> list, Random rand)
        {
            return list[rand.Next(list.Count)];
        }

        public static T PopRandom<T>(this List<T> list, Random rand)
        {
            int index = rand.Next(list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static void Shuffle<T>(this IList<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class ParamScrambler
    {
        public Random rand;
        public Regulation regulation;

        Dictionary<string, List<string>> ShuffleParamFields = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> GenerateParamFields = new Dictionary<string, List<string>>();
        Dictionary<string, Dictionary<string, List<string>>> GenerateParamValues = new Dictionary<string, Dictionary<string, List<string>>>();

        List<string> BossIDs = new List<string>();
        List<string> CharacterIDs = new List<string>();
        List<string> EnemyIDs = new List<string>();

        public ParamWrapper ItemParam;
        public ParamWrapper SpellParam;
        public ParamWrapper RingParam;
        public ParamWrapper WeaponParam;
        public ParamWrapper ArmorParam;

        public string ParamScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";
        public string EnemyScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Enemy-Scramble\\";

        public string OutputPath;

        public ParamScrambler(Random random, Regulation reg, string output_path)
        {
            rand = random;
            regulation = reg;
            OutputPath = output_path;

            BossIDs = Util.BuildIDList(EnemyScramblePath + "Boss-IDs");
            CharacterIDs = Util.BuildIDList(EnemyScramblePath + "Character-IDs");
            EnemyIDs = Util.BuildIDList(EnemyScramblePath + "Enemy-IDs");

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == "ItemParam")
                {
                    ItemParam = wrapper;
                }
                if (wrapper.Name == "SpellParam")
                {
                    SpellParam = wrapper;
                }
                if (wrapper.Name == "RingParam")
                {
                    RingParam = wrapper;
                }
                if (wrapper.Name == "WeaponParam")
                {
                    WeaponParam = wrapper;
                }
                if (wrapper.Name == "ArmorParam")
                {
                    ArmorParam = wrapper;
                }
            }

            // Build ShuffleParamFields dictionary
            foreach (string filepath in Directory.GetFiles(ParamScramblePath + "Param-Scramble\\Shuffle-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                }

                ShuffleParamFields.Add(name, list);
            }

            // Build GenerateParamFields dictionary
            foreach (string filepath in Directory.GetFiles(ParamScramblePath + "Param-Scramble\\Generate-Shuffle-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                }

                GenerateParamFields.Add(name, list);
            }

            // Build GenerateParamValues dictionary
            foreach (string filepath in Directory.GetFiles(ParamScramblePath + "Param-Scramble\\Generate-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var dict = new Dictionary<string, List<string>>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    var list = line.Split(";");

                    var value_list = new List<string> { list[1], list[2], list[3] };

                    dict.Add(list[0], value_list);
                }

                GenerateParamValues.Add(name, dict);
            }
        }

        #region Scramble - ArmorParam
        public Regulation Scramble_ArmorParam(string paramName, bool useGenerateType, bool ignoreRequirements)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 11010100 && row.ID <= 18000000).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    if (ignoreRequirements)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName.Contains("requirement"))
                                    cell.Value = 0;
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ArmorReinforceParam
        public Regulation Scramble_ArmorReinforceParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 11010100 && row.ID <= 18000000).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ItemParam
        public Regulation Scramble_ItemParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - RingParam
        public Regulation Scramble_RingParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows.Where(row => row.ID >= 40010000).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - SpellParam
        public Regulation Scramble_SpellParam(string paramName, bool useGenerateType, bool ignoreRequirements)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows.Where(row => row.ID >= 31010000 && row.ID <= 35310000).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    if (ignoreRequirements)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName.Contains("requirement"))
                                    cell.Value = 0;
                            }
                        }
                    }

                    // Update upper cast tiers based on shuffled base tier
                    foreach (PARAM.Row row in param_rows)
                    {
                        byte baseCasts = 1;

                        byte growth = (byte)rand.Next(1, 3);

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "casts_tier_1")
                                baseCasts = (byte)cell.Value;
                        }

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "casts_tier_2")
                                cell.Value = (byte)(baseCasts + (growth * 1));

                            if (cell.Def.InternalName == "casts_tier_3")
                                cell.Value = (byte)(baseCasts + (growth * 2));

                            if (cell.Def.InternalName == "casts_tier_4")
                                cell.Value = (byte)(baseCasts + (growth * 3));

                            if (cell.Def.InternalName == "casts_tier_5")
                                cell.Value = (byte)(baseCasts + (growth * 4));

                            if (cell.Def.InternalName == "casts_tier_6")
                                cell.Value = (byte)(baseCasts + (growth * 5));

                            if (cell.Def.InternalName == "casts_tier_7")
                                cell.Value = (byte)(baseCasts + (growth * 6));

                            if (cell.Def.InternalName == "casts_tier_8")
                                cell.Value = (byte)(baseCasts + (growth * 7));

                            if (cell.Def.InternalName == "casts_tier_9")
                                cell.Value = (byte)(baseCasts + (growth * 8));

                            if (cell.Def.InternalName == "casts_tier_10")
                                cell.Value = (byte)(baseCasts + (growth * 9));
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - BulletParam
        public Regulation Scramble_BulletParam(string paramName, bool useGenerateType, bool enforceVFX)
        {
            // Build a list of usable VFX IDs
            List<int> vfxIDs = new List<int>();

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                // Get VFX IDs from the player/enemy bullets
                if (wrapper.Name == "BulletParam")
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (enforceVFX)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = (int)cell.Value;

                                    if (value != 0)
                                        vfxIDs.Add(value);
                                }
                            }
                        }
                    }
                }
            }

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    // Force all bullet VFX fields to have a non-empty VFX ID
                    if (enforceVFX)
                    {
                        // Apply VFX IDs randomly
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = vfxIDs[rand.Next(vfxIDs.Count)];
                                    cell.Value = value;
                                }
                            }
                        }
                    }

                    // Update child bullet attributes if child bullet_id has changed
                    foreach (PARAM.Row row in param_rows)
                    {
                        bool hasChildBulletID_1 = false;
                        bool hasChildBulletID_2 = false;
                        bool hasChildBulletID_3 = false;

                        int childBulletID_1 = -1;
                        int childBulletID_2 = -1;
                        int childBulletID_3 = -1;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "child_bullet_1_bullet_id")
                            {
                                hasChildBulletID_1 = true;
                                childBulletID_1 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_1_damage_id" && hasChildBulletID_1)
                            {
                                cell.Value = childBulletID_1;
                            }

                            if (cell.Def.InternalName == "child_bullet_2_bullet_id")
                            {
                                hasChildBulletID_2 = true;
                                childBulletID_2 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_2_damage_id" && hasChildBulletID_2)
                            {
                                cell.Value = childBulletID_2;
                            }

                            if (cell.Def.InternalName == "child_bullet_3_bullet_id")
                            {
                                hasChildBulletID_3 = true;
                                childBulletID_3 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_3_damage_id" && hasChildBulletID_3)
                            {
                                cell.Value = childBulletID_3;
                            }

                            if (hasChildBulletID_1 || hasChildBulletID_2 || hasChildBulletID_3)
                            {
                                if (cell.Def.InternalName == "spawn_child_bullet_on_hit")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "spawn_child_bullet_on_expire")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "child_bullet_spawn_delay")
                                {
                                    if (rand.Next(100) < 25)
                                        cell.Value = rand.NextDouble();
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ArrowParam
        public Regulation Scramble_ArrowParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 1).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        bool enablePoison = false;
                        bool enableBleed = false;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "damage_poison")
                            {
                                if ((byte)cell.Value > 0)
                                {
                                    enablePoison = true;
                                }
                            }
                            if (cell.Def.InternalName == "damage_bleed")
                            {
                                if ((byte)cell.Value > 0)
                                {
                                    enableBleed = true;
                                }
                            }

                            if (cell.Def.InternalName == "apply_poison")
                            {
                                if (enablePoison)
                                    cell.Value = 1;
                                else
                                    cell.Value = 0;
                            }

                            if (cell.Def.InternalName == "apply_bleed")
                            {
                                if (enableBleed)
                                    cell.Value = 1;
                                else
                                    cell.Value = 0;
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponActionCategoryParam
        public Regulation Scramble_WeaponActionCategoryParam(string paramName)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponParam
        public Regulation Scramble_WeaponParam(string paramName, bool useGenerateType, bool ignoreFists, bool ignoreRequirements)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    var new_rows = new List<PARAM.Row>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        if (row.ID >= 1000000 && row.ID <= 6100000)
                            new_rows.Add(row);

                        if (row.ID >= 11000000 && row.ID <= 11840000)
                            new_rows.Add(row);

                        if(ignoreFists)
                        {
                            if (row.ID >= 3400000 && row.ID <= 3408000)
                                new_rows.Remove(row);
                        }
                    }

                    param_rows = new_rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    if (ignoreRequirements)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName.Contains("requirement"))
                                    cell.Value = 0;
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponReinforceParam
        public Regulation Scramble_WeaponReinforceParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    var new_rows = new List<PARAM.Row>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        if (row.ID >= 1000 && row.ID <= 5540)
                            new_rows.Add(row);

                        if (row.ID >= 11000 && row.ID <= 11840)
                            new_rows.Add(row);
                    }

                    param_rows = new_rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponTypeParam
        public Regulation Scramble_WeaponTypeParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    // Zero bow_distance for non-bow rows
                    foreach (PARAM.Row row in param_rows)
                    {
                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            List<int> rows = new List<int>{ 360, 361, 362, 363, 364, 365, 370, 371, 380, 381, 385, 386, 540 };

                            if (!rows.Contains(row.ID))
                            {
                                if (cell.Def.InternalName == "bow_distance")
                                {
                                    cell.Value = 0;
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - NpcPlayerStatusParam
        public Regulation Scramble_NpcPlayerStatusParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 6940).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);

                        List<PARAM.Row> weaponList = new List<PARAM.Row>();
                        List<PARAM.Row> shieldList = new List<PARAM.Row>();
                        List<PARAM.Row> npcSpellList = new List<PARAM.Row>();
                        List<PARAM.Row> headArmorList = new List<PARAM.Row>();
                        List<PARAM.Row> chestArmorList = new List<PARAM.Row>();
                        List<PARAM.Row> armArmorList = new List<PARAM.Row>();
                        List<PARAM.Row> legArmorList = new List<PARAM.Row>();
                        List<PARAM.Row> ringList = new List<PARAM.Row>();

                        // Armor
                        foreach (PARAM.Row aRow in ArmorParam.Rows)
                        {
                            var last_digit = (aRow.ID % 10);

                            if (aRow.ID >= 11010100 && aRow.ID <= 17950103)
                            {
                                if (last_digit == 0)
                                    headArmorList.Add(aRow);

                                if (last_digit == 1)
                                    chestArmorList.Add(aRow);

                                if (last_digit == 2)
                                    armArmorList.Add(aRow);

                                if (last_digit == 3)
                                    legArmorList.Add(aRow);
                             }
                        }

                        // Rings
                        foreach (PARAM.Row rRow in RingParam.Rows)
                        {
                            if (rRow.ID >= 40010000 && rRow.ID <= 41130000)
                                ringList.Add(rRow);
                        }

                        // NPC Spells
                        foreach (PARAM.Row sRow in SpellParam.Rows)
                        {
                            if (sRow.ID >= 71010000)
                                npcSpellList.Add(sRow);
                        }

                        // Weapons/Shields
                        foreach (PARAM.Row wRow in WeaponParam.Rows)
                        {
                            if (wRow.ID >= 1000000 && wRow.ID <= 5660000)
                                weaponList.Add(wRow);

                            if (wRow.ID >= 11000000 && wRow.ID <= 11840000)
                                shieldList.Add(wRow);
                        }

                        // Generate from lists
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName.Contains("spell_item"))
                                    cell.Value = GetRandomRowID(npcSpellList);

                                if (cell.Def.InternalName.Contains("right_weapon_item_1"))
                                    cell.Value = GetRandomRowID(weaponList);

                                if (cell.Def.InternalName.Contains("right_weapon_item_2"))
                                    cell.Value = GetRandomRowID(weaponList);

                                if (cell.Def.InternalName.Contains("right_weapon_item_3"))
                                    cell.Value = GetRandomRowID(weaponList);

                                if (rand.Next(100) < 50)
                                {
                                    if (cell.Def.InternalName.Contains("left_weapon_item_1"))
                                        cell.Value = GetRandomRowID(shieldList);
                                }
                                else
                                {
                                    if (cell.Def.InternalName.Contains("left_weapon_item_1"))
                                        cell.Value = GetRandomRowID(weaponList);
                                }

                                if (cell.Def.InternalName.Contains("right_weapon_item_2"))
                                    cell.Value = GetRandomRowID(weaponList);

                                if (cell.Def.InternalName.Contains("right_weapon_item_3"))
                                    cell.Value = GetRandomRowID(weaponList);

                                if (cell.Def.InternalName.Contains("head_item"))
                                    cell.Value = GetRandomRowID(headArmorList);

                                if (cell.Def.InternalName.Contains("chest_item"))
                                    cell.Value = GetRandomRowID(chestArmorList);

                                if (cell.Def.InternalName.Contains("hands_item"))
                                    cell.Value = GetRandomRowID(armArmorList);

                                if (cell.Def.InternalName.Contains("legs_item"))
                                    cell.Value = GetRandomRowID(legArmorList);

                                if (rand.Next(100) < 25)
                                {
                                    if (cell.Def.InternalName.Contains("ring_item_1"))
                                        cell.Value = GetRandomRowID(ringList);
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - BossBattleParam
        public Regulation Scramble_BossBattleParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EnemyParam
        public Regulation Scramble_EnemyParam(string paramName, bool useGenerateType, bool isBossOnly = false)
        {
            string fieldAppend = "";
            if (isBossOnly)
                fieldAppend = "_Boss";

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (isBossOnly)
                    {
                        param_rows = param_rows.Where(row => BossIDs.Contains(row.ID.ToString())).ToList();
                    }
                    else
                    {
                        param_rows = param_rows.Where(row => !BossIDs.Contains(row.ID.ToString())).ToList();
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields, fieldAppend);
                        GenerateValuesForParam(wrapper, param_rows, fieldAppend);
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        var baseHP = 100;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "stat_hp")
                            {
                                baseHP = (int)cell.Value;
                            }

                            // Add NG+ HP based on base HP value
                            if (cell.Def.InternalName == "stat_hp_ng1")
                            {
                                cell.Value = (int)(baseHP * 1.3);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng2")
                            {
                                cell.Value = (int)(baseHP * 1.55);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng3")
                            {
                                cell.Value = (int)(baseHP * 1.8);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng4")
                            {
                                cell.Value = (int)(baseHP * 1.95);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng5")
                            {
                                cell.Value = (int)(baseHP * 2.05);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng6")
                            {
                                cell.Value = (int)(baseHP * 2.2);
                            }
                            if (cell.Def.InternalName == "stat_hp_ng7")
                            {
                                cell.Value = (int)(baseHP * 2.45);
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EnemyMoveParam
        public Regulation Scramble_EnemyMoveParam(string paramName, bool useGenerateType, bool isBossOnly = false)
        {
            string fieldAppend = "";
            if (isBossOnly)
                fieldAppend = "_Boss";

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (isBossOnly)
                    {
                        param_rows = param_rows.Where(row => BossIDs.Contains(row.ID.ToString())).ToList();
                    }
                    else
                    {
                        param_rows = param_rows.Where(row => !BossIDs.Contains(row.ID.ToString())).ToList();
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields, fieldAppend);
                        GenerateValuesForParam(wrapper, param_rows, fieldAppend);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EnemyDamageParam
        public Regulation Scramble_EnemyDamageParam(string paramName, bool useGenerateType, bool isBossOnly = false)
        {
            string fieldAppend = "";
            if (isBossOnly)
                fieldAppend = "_Boss";

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows.Where(row => row.ID >= 100000000).ToList();

                    if (isBossOnly)
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, BossIDs, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, BossIDs, 2, 4, "1", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields, fieldAppend);
                        GenerateValuesForParam(wrapper, param_rows, fieldAppend);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EnemyBulletParam
        public Regulation Scramble_EnemyBulletParam(string paramName, bool useGenerateType, bool enforceVFX, bool isBossOnly = false)
        {
            string fieldAppend = "";
            if (isBossOnly)
                fieldAppend = "_Boss";

            // Build a list of usable VFX IDs
            List<int> vfxIDs = new List<int>();

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                // Get VFX IDs from the player/enemy bullets
                if (wrapper.Name == "BulletParam" || wrapper.Name == "EnemyBulletParam")
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (enforceVFX)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = (int)cell.Value;

                                    if (value != 0)
                                        vfxIDs.Add(value);
                                }
                            }
                        }
                    }
                }
            }

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (isBossOnly)
                    {
                        param_rows = Util.GetRowsFromSubMatch(param.Rows, BossIDs, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param.Rows, BossIDs, 2, 4, "1", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields, fieldAppend);
                        GenerateValuesForParam(wrapper, param_rows, fieldAppend);
                    }

                    // Force all bullet VFX fields to have a non-empty VFX ID
                    if (enforceVFX)
                    {
                        // Apply VFX IDs randomly
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = vfxIDs[rand.Next(vfxIDs.Count)];
                                    cell.Value = value;
                                }
                            }
                        }
                    }

                    // Adjustments
                    foreach (PARAM.Row row in param_rows)
                    {
                        bool hasChildBulletID_1 = false;
                        bool hasChildBulletID_2 = false;
                        bool hasChildBulletID_3 = false;

                        int childBulletID_1 = -1;
                        int childBulletID_2 = -1;
                        int childBulletID_3 = -1;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if(cell.Def.InternalName == "child_bullet_1_bullet_id")
                            {
                                hasChildBulletID_1 = true;
                                childBulletID_1 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_1_damage_id" && hasChildBulletID_1)
                            {
                                cell.Value = childBulletID_1;
                            }

                            if (cell.Def.InternalName == "child_bullet_2_bullet_id")
                            {
                                hasChildBulletID_2 = true;
                                childBulletID_2 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_2_damage_id" && hasChildBulletID_2)
                            {
                                cell.Value = childBulletID_2;
                            }

                            if (cell.Def.InternalName == "child_bullet_3_bullet_id")
                            {
                                hasChildBulletID_3 = true;
                                childBulletID_3 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_3_damage_id" && hasChildBulletID_3)
                            {
                                cell.Value = childBulletID_3;
                            }

                            if(hasChildBulletID_1 || hasChildBulletID_2 || hasChildBulletID_3)
                            {
                                if (cell.Def.InternalName == "spawn_child_bullet_on_hit")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "spawn_child_bullet_on_expire")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "child_bullet_spawn_delay")
                                {
                                    if(rand.Next(100) < 25)
                                        cell.Value = rand.NextDouble();
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EnemyBehaviorParam
        public Regulation Scramble_EnemyBehaviorParam(string paramName, bool useGenerateType, bool isBossOnly = false)
        {
            string fieldAppend = "";
            if (isBossOnly)
                fieldAppend = "_Boss";

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows.Where(row => row.ID >= 100000).ToList();

                    if (isBossOnly)
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, BossIDs, 2, 2, "");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, BossIDs, 2, 2, "", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields, fieldAppend);
                        GenerateValuesForParam(wrapper, param_rows, fieldAppend);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - LogicComParam
        public Regulation Scramble_LogicComParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - TreasureBoxParam
        public Regulation Scramble_TreasureBoxParam()
        {
            string paramName = "treasureboxparam";

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains(paramName))
                {
                    foreach (PARAM.Row row in param_rows)
                    {
                        bool isTrapped = true;

                        if (rand.Next(100) < 50)
                            isTrapped = false;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if(isTrapped)
                            {
                                if (cell.Def.InternalName == "chest_type")
                                    cell.Value = 1;

                                if (cell.Def.InternalName == "bullet_id_1")
                                    cell.Value = 200001030;

                                if (cell.Def.InternalName == "bullet_id_2")
                                    cell.Value = 200001130;

                                if (cell.Def.InternalName == "bullet_id_3")
                                    cell.Value = 200001330;

                                if (cell.Def.InternalName == "bullet_id_4")
                                    cell.Value = 200001230;
                            }
                            else
                            {
                                if (cell.Def.InternalName == "chest_type")
                                    cell.Value = 0;

                                if (cell.Def.InternalName == "bullet_id_1")
                                    cell.Value = 0;

                                if (cell.Def.InternalName == "bullet_id_2")
                                    cell.Value = 0;

                                if (cell.Def.InternalName == "bullet_id_3")
                                    cell.Value = 0;

                                if (cell.Def.InternalName == "bullet_id_4")
                                    cell.Value = 0;
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - SystemBulletParam
        public Regulation Scramble_SystemBulletParam(string paramName, bool useGenerateType, bool enforceVFX, bool onlyTraps)
        {
            // Build a list of usable VFX IDs
            List<int> vfxIDs = new List<int>();

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                // Get VFX IDs from the player/map bullets
                if (wrapper.Name == "BulletParam" || wrapper.Name == "SystemBulletParam")
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (enforceVFX)
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = (int)cell.Value;

                                    if (value != 0)
                                        vfxIDs.Add(value);
                                }
                            }
                        }
                    }
                }
            }

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (onlyTraps)
                        param_rows = param.Rows.Where(row => row.ID <= 220100700).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    // Force all bullet VFX fields to have a non-empty VFX ID
                    if (enforceVFX)
                    {
                        // Apply VFX IDs randomly
                        foreach (PARAM.Row row in param_rows)
                        {
                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                string cName = cell.Def.InternalName;

                                if (cName == "sfx_id" || cName == "hit_sfx_id" || cName == "expire_sfx_id")
                                {
                                    int value = vfxIDs[rand.Next(vfxIDs.Count)];
                                    cell.Value = value;
                                }
                            }
                        }
                    }

                    // Update child bullet attributes if child bullet_id has changed
                    foreach (PARAM.Row row in param_rows)
                    {
                        bool hasChildBulletID_1 = false;
                        bool hasChildBulletID_2 = false;
                        bool hasChildBulletID_3 = false;

                        int childBulletID_1 = -1;
                        int childBulletID_2 = -1;
                        int childBulletID_3 = -1;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "child_bullet_1_bullet_id")
                            {
                                hasChildBulletID_1 = true;
                                childBulletID_1 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_1_damage_id" && hasChildBulletID_1)
                            {
                                cell.Value = childBulletID_1;
                            }

                            if (cell.Def.InternalName == "child_bullet_2_bullet_id")
                            {
                                hasChildBulletID_2 = true;
                                childBulletID_2 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_2_damage_id" && hasChildBulletID_2)
                            {
                                cell.Value = childBulletID_2;
                            }

                            if (cell.Def.InternalName == "child_bullet_3_bullet_id")
                            {
                                hasChildBulletID_3 = true;
                                childBulletID_3 = (int)cell.Value;
                            }
                            if (cell.Def.InternalName == "child_bullet_3_damage_id" && hasChildBulletID_3)
                            {
                                cell.Value = childBulletID_3;
                            }

                            if (hasChildBulletID_1 || hasChildBulletID_2 || hasChildBulletID_3)
                            {
                                if (cell.Def.InternalName == "spawn_child_bullet_on_hit")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "spawn_child_bullet_on_expire")
                                {
                                    cell.Value = 1;
                                }
                                if (cell.Def.InternalName == "child_bullet_spawn_delay")
                                {
                                    if (rand.Next(100) < 25)
                                        cell.Value = rand.NextDouble();
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Util
        public int GetRandomRowID(List<PARAM.Row> list)
        {
            return list[rand.Next(list.Count)].ID;
        }

        public void ShuffleValuesForParam(ParamWrapper wrapper, List<PARAM.Row> param_rows, Dictionary<string, List<string>> dict, string wrapperAppend = "")
        {
            try
            {
                var fields = dict[wrapper.Name + wrapperAppend];

                RandomizeFromList(param_rows, fields);
            }
            catch (Exception ex)
            {
                Util.ShowError($"{ex}\n\nFailed to find fields for {wrapper.Name + wrapperAppend}.");
            }
        }

        public void GenerateValuesForParam(ParamWrapper wrapper, List<PARAM.Row> param_rows, string wrapperAppend = "")
        {
            var cellType = "";
            
            try
            {
                var dict = GenerateParamValues[wrapper.Name + wrapperAppend];

                foreach (PARAM.Row row in param_rows)
                {
                    foreach (PARAM.Cell cell in row.Cells)
                    {
                        cellType = cell.Def.InternalType;

                        // Field is within the dictionary
                        if (dict.ContainsKey(cell.Def.InternalName))
                        {
                            var list = dict[cell.Def.InternalName];

                            // Weighted means that the result should be weighted to zero.
                            bool isWeighted = list[2] == "WEIGHTED" ? true : false;

                            // Int32
                            if (cell.Def.InternalType == "s32" || cell.Def.InternalType == "u32")
                            {
                                var min = int.Parse(list[0]);
                                var max = int.Parse(list[1]);

                                if (min != -1 || max != -1)
                                {
                                    var value = rand.Next(min, max);

                                    if (isWeighted && rand.Next(100) < 50)
                                        value = 0;

                                    cell.Value = value;
                                }
                            }
                            // Int16
                            if (cell.Def.InternalType == "s16" || cell.Def.InternalType == "u16")
                            {
                                var min = short.Parse(list[0]);
                                var max = short.Parse(list[1]);

                                var value = rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // Int8
                            if (cell.Def.InternalType == "s8" || cell.Def.InternalType == "u8")
                            {
                                var min = byte.Parse(list[0]);
                                var max = byte.Parse(list[1]);

                                var value = rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // Float
                            if (cell.Def.InternalType == "f32")
                            {
                                var min = double.Parse(list[0]);
                                var max = double.Parse(list[1]);

                                var value = rand.NextDouble() * (max - min) + min;

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                        }
                    }
                }
            }
            catch (OverflowException ex)
            {
                Util.ShowError($"{ex}\n\nAttempted to assign use GenerateType value that exceeds {cellType} capacity - {wrapper.Name + wrapperAppend}.");
            }
            catch (Exception ex)
            {
                Util.ShowError($"{ex}\n\nFailed to find values for {wrapper.Name + wrapperAppend}.");
            }
        }

        // Credit to TKGP for these functions
        private void RandomizeOne<T>(IEnumerable<PARAM.Row> rows, string param, bool plusMode = false)
        {
            if (plusMode)
            {
                List<T> options = rows.Select(row => (T)row[param].Value).GroupBy(val => val).Select(group => group.First()).ToList();
                foreach (PARAM.Row row in rows)
                    row[param].Value = options.GetRandom(rand);
            }
            else
            {
                List<T> options = rows.Select(row => (T)row[param].Value).ToList();
                foreach (PARAM.Row row in rows)
                    row[param].Value = options.PopRandom(rand);
            }
        }

        private void RandomizePair<T1, T2>(IEnumerable<PARAM.Row> rows, string param1, string param2)
        {
            List<(T1, T2)> options = rows.Select(row => ((T1)row[param1].Value, (T2)row[param2].Value)).ToList();
            foreach (PARAM.Row row in rows)
            {
                (T1 val1, T2 val2) = options.PopRandom(rand);
                row[param1].Value = val1;
                row[param2].Value = val2;
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

        private void RandomizeAllExceptList(IEnumerable<PARAM.Row> rows, List<string> valid_fields)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                bool modify = true;

                foreach (string field in valid_fields)
                {
                    if (cName == field)
                    {
                        modify = false;
                    }
                }

                if (modify)
                {
                    if (cType == "u8" || cType == "x8")
                        RandomizeOne<byte>(rows, cName);
                    else if (cType == "s8")
                        RandomizeOne<sbyte>(rows, cName);
                    else if (cType == "u16" || cType == "x16")
                        RandomizeOne<ushort>(rows, cName);
                    else if (cType == "s16")
                        RandomizeOne<short>(rows, cName);
                    else if (cType == "u32" || cType == "x32")
                        RandomizeOne<uint>(rows, cName);
                    else if (cType == "s32")
                        RandomizeOne<int>(rows, cName);
                    else if (cType == "f32")
                        RandomizeOne<float>(rows, cName);
                    else if (cType == "b8" || cType == "b32")
                        RandomizeOne<bool>(rows, cName);
                    else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                        Console.WriteLine($"Skipped {cName}");
                    else
                        throw null;
                }
            }
        }

        private void RandomizeFromList(IEnumerable<PARAM.Row> rows, List<string> valid_fields)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                foreach (string field in valid_fields)
                {
                    if (cName == field)
                    {
                        if (cType == "u8" || cType == "x8")
                            RandomizeOne<byte>(rows, cName);
                        else if (cType == "s8")
                            RandomizeOne<sbyte>(rows, cName);
                        else if (cType == "u16" || cType == "x16")
                            RandomizeOne<ushort>(rows, cName);
                        else if (cType == "s16")
                            RandomizeOne<short>(rows, cName);
                        else if (cType == "u32" || cType == "x32")
                            RandomizeOne<uint>(rows, cName);
                        else if (cType == "s32")
                            RandomizeOne<int>(rows, cName);
                        else if (cType == "f32")
                            RandomizeOne<float>(rows, cName);
                        else if (cType == "b8" || cType == "b32")
                            RandomizeOne<bool>(rows, cName);
                        else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                            Console.WriteLine($"Skipped {cName}");
                        else
                            throw null;
                    }
                }
            }
        }

        private void RandomizeAll(IEnumerable<PARAM.Row> rows, bool plusMode = false)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                //Console.WriteLine($"{cName} - {cType}");

                if (cType == "u8" || cType == "x8")
                    RandomizeOne<byte>(rows, cName, plusMode);
                else if (cType == "s8")
                    RandomizeOne<sbyte>(rows, cName, plusMode);
                else if (cType == "u16" || cType == "x16")
                    RandomizeOne<ushort>(rows, cName, plusMode);
                else if (cType == "s16")
                    RandomizeOne<short>(rows, cName, plusMode);
                else if (cType == "u32" || cType == "x32")
                    RandomizeOne<uint>(rows, cName, plusMode);
                else if (cType == "s32")
                    RandomizeOne<int>(rows, cName, plusMode);
                else if (cType == "f32")
                    RandomizeOne<float>(rows, cName, plusMode);
                else if (cType == "b8" || cType == "b32")
                    RandomizeOne<bool>(rows, cName, plusMode);
                else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                    Console.WriteLine($"Skipped {cName}");
                else
                    throw null;
            }
        }
        #endregion
    }
}
