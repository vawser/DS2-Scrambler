
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
using Org.BouncyCastle.Crypto;
using System.IO;
using System.DirectoryServices;
using System.Globalization;
using static SoulsFormats.PARAMDEF;

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
        public CoreScramblerData Data;

        public ParamScrambler(Random random, Regulation reg, CoreScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;
        }

        public int GetRandomInt(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (int)result;
        }

        public uint GetRandomUInt(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (uint)result;
        }

        public short GetRandomShort(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (short)result;
        }

        public ushort GetRandomUShort(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (ushort)result;
        }

        public byte GetRandomByte(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (byte)result;
        }

        public sbyte GetRandomSByte(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (sbyte)result;
        }

        public float GetRandomFloat(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (float)result;
        }

        #region Weapons
        public Regulation Scramble_WeaponAttributes(
            bool c_ItemParam_Weapon_Price,
            bool c_ItemParam_Weapon_Effect,
            bool c_WeaponParam_Weapon_Weight,
            bool c_WeaponParam_Weapon_Durability,
            bool c_ItemParam_Weapon_Animation_Speed,
            bool c_WeaponParam_StatRequirements,
            bool c_WeaponParam_Damage,
            bool c_WeaponReinforceParam_Reinforcement,
            bool c_WeaponParam_StaminaConsumption,
            bool c_WeaponTypeParam_CastSpeed,
            bool c_WeaponTypeParam_BowDistance,
            bool c_ArrowParam_AmmoDamage,
            bool c_WeaponActionCategoryParam_Moveset,
            bool c_Tweak_WeaponParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponParam")).ToList()[0].Rows;
            List<PARAM.Row> RingParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"RingParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponReinforceParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponReinforceParam")).ToList()[0].Rows;
            List<PARAM.Row> ArrowParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArrowParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponTypeParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponTypeParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponActionCategoryParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponActionCategoryParam")).ToList()[0].Rows;

            if (c_ItemParam_Weapon_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.WeaponParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(10, 10000);
                    row["sell_price"].Value = GetRandomInt(10, 10000);
                }
            }
            if (c_ItemParam_Weapon_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var list = ParamScramblerData.Static.SpEffect_ID_List;
                    var result = list[rand.Next(list.Count)];

                    row["speffect_id"].Value = result;
                }
            }
            if (c_WeaponParam_Weapon_Weight)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["weight"].Value = GetRandomFloat(0.1, 30);
                }
            }
            if (c_WeaponParam_Weapon_Durability)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["max_durability"].Value = (float)GetRandomInt(10, 300);
                }
            }
            if (c_ItemParam_Weapon_Animation_Speed)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.WeaponParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["animation_speed"].Value = GetRandomFloat(0.2, 1.5);
                }
            }
            if (c_WeaponParam_StatRequirements)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["str_requirement"].Value = GetRandomShort(1, 50);
                    row["dex_requirement"].Value = GetRandomShort(1, 50);
                    row["int_requirement"].Value = GetRandomShort(1, 50);
                    row["fth_requirement"].Value = GetRandomShort(1, 50);
                }
            }
            if (c_WeaponParam_Damage)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["stamina_damage"].Value = GetRandomFloat(100, 1000);
                    row["damage_mult"].Value = GetRandomFloat(0.5, 2.0);
                    row["stamina_damage_mult"].Value = GetRandomFloat(0.5, 2.0);
                    row["durability_damage_mult"].Value = GetRandomFloat(0.5, 2.0);

                    // Only apply if the weapon already uses it
                    if ((float)row["status_effect_amount"].Value > 0)
                        row["status_effect_amount"].Value = GetRandomFloat(0.5, 2.0);

                    row["posture_damage_mult"].Value = GetRandomFloat(0.5, 2.0);
                }

                // WeaponTypeParam
                rows = WeaponTypeParam;

                foreach (PARAM.Row row in rows)
                {
                    row["counter_damage"].Value = GetRandomFloat(0.5, 2.0);
                }
            }
            if (c_WeaponReinforceParam_Reinforcement)
            {
                List<PARAM.Row> rows = WeaponReinforceParam;

                foreach (PARAM.Row row in rows)
                {
                    // Damage
                    var base_physical = GetRandomFloat(20, 200);
                    var base_magic = GetRandomFloat(20, 200);
                    var base_lightning = GetRandomFloat(20, 200);
                    var base_fire = GetRandomFloat(20, 200);
                    var base_dark = GetRandomFloat(20, 200);
                    var base_status = GetRandomFloat(200, 1000);

                    var growth = GetRandomFloat(1.1, 2.5);

                    row["damage_physical"].Value = base_physical;
                    row["damage_magic"].Value = base_magic;
                    row["damage_lightning"].Value = base_lightning;
                    row["damage_fire"].Value = base_fire;
                    row["damage_dark"].Value = base_dark;
                    row["damage_poison"].Value = base_status;
                    row["damage_bleed"].Value = base_status;

                    row["max_damage_physical"].Value = (int)(base_physical * growth);
                    row["max_damage_magic"].Value = (int)(base_magic * growth);
                    row["max_damage_lightning"].Value = (int)(base_lightning * growth);
                    row["max_damage_fire"].Value = (int)(base_fire * growth);
                    row["max_damage_dark"].Value = (int)(base_dark * growth);
                    row["max_damage_poison"].Value = (int)(base_status * growth);
                    row["max_damage_bleed"].Value = (int)(base_status * growth);


                    var stability = GetRandomFloat(10, 100);
                    var stability_factor = GetRandomFloat(1.01, 1.2);

                    // Shields
                    if(row.ID >= 11000)
                    {
                        row["stability_0"].Value = stability;
                        row["stability_1"].Value = (stability * (stability_factor * 1));
                        row["stability_2"].Value = (stability * (stability_factor * 2));
                        row["stability_3"].Value = (stability * (stability_factor * 3));
                        row["stability_4"].Value = (stability * (stability_factor * 4));
                        row["stability_5"].Value = (stability * (stability_factor * 5));
                        row["stability_6"].Value = (stability * (stability_factor * 6));
                        row["stability_7"].Value = (stability * (stability_factor * 7));
                        row["stability_8"].Value = (stability * (stability_factor * 8));
                        row["stability_9"].Value = (stability * (stability_factor * 9));
                        row["stability_10"].Value = (stability * (stability_factor * 10));

                        if ((float)row["stability_0"].Value > 100)
                            row["stability_0"].Value = 100;

                        if ((float)row["stability_1"].Value > 100)
                            row["stability_1"].Value = 100;

                        if ((float)row["stability_2"].Value > 100)
                            row["stability_2"].Value = 100;

                        if ((float)row["stability_3"].Value > 100)
                            row["stability_3"].Value = 100;

                        if ((float)row["stability_4"].Value > 100)
                            row["stability_4"].Value = 100;

                        if ((float)row["stability_5"].Value > 100)
                            row["stability_5"].Value = 100;

                        if ((float)row["stability_6"].Value > 100)
                            row["stability_6"].Value = 100;

                        if ((float)row["stability_7"].Value > 100)
                            row["stability_7"].Value = 100;

                        if ((float)row["stability_8"].Value > 100)
                            row["stability_8"].Value = 100;

                        if ((float)row["stability_9"].Value > 100)
                            row["stability_9"].Value = 100;

                        if ((float)row["stability_10"].Value > 100)
                            row["stability_10"].Value = 100;
                    }
                    else
                    {
                        row["stability_0"].Value = stability;
                        row["stability_1"].Value = stability;
                        row["stability_2"].Value = stability;
                        row["stability_3"].Value = stability;
                        row["stability_4"].Value = stability;
                        row["stability_5"].Value = stability;
                        row["stability_6"].Value = stability;
                        row["stability_7"].Value = stability;
                        row["stability_8"].Value = stability;
                        row["stability_9"].Value = stability;
                        row["stability_10"].Value = stability;
                    }

                    // Absorption
                    var base_abs_physical = GetRandomFloat(10, 100);
                    var base_abs_magic = GetRandomFloat(10, 100);
                    var base_abs_lightning = GetRandomFloat(10, 100);
                    var base_abs_fire = GetRandomFloat(10, 100);
                    var base_abs_dark = GetRandomFloat(10, 100);
                    var base_abs_poison = GetRandomFloat(10, 100);
                    var base_abs_bleed = GetRandomFloat(10, 100);
                    var base_abs_curse = GetRandomFloat(10, 100);
                    var base_abs_petrify = GetRandomFloat(10, 100);

                    var factor = 1.0;

                    if (row.ID >= 11000)
                        factor = 1.2;

                    row["absorption_physical"].Value = (base_abs_physical * factor);
                    row["absorption_magic"].Value = (base_abs_magic * factor);
                    row["absorption_fire"].Value = (base_abs_fire * factor);
                    row["absorption_lightning"].Value = (base_abs_lightning * factor);
                    row["absorption_dark"].Value = (base_abs_dark * factor);
                    row["absorption_poison"].Value = (base_abs_poison * factor);
                    row["absorption_bleed"].Value = (base_abs_bleed * factor);
                    row["absorption_petrify"].Value = (base_abs_curse * factor);
                    row["absorption_curse"].Value = (base_abs_petrify * factor);
                }
            }
            if (c_WeaponParam_StaminaConsumption)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["stamina_consumption"].Value = GetRandomFloat(100, 1000);
                }
            }
            if (c_WeaponTypeParam_CastSpeed)
            {
                List<PARAM.Row> rows = WeaponTypeParam.Where(row => row.ID >= 300 && row.ID <= 358).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["cast_speed"].Value = GetRandomFloat(0.5, 2.0);
                }
            }
            if (c_WeaponTypeParam_BowDistance)
            {
                List<PARAM.Row> rows = WeaponTypeParam.Where(row => row.ID >= 360 && row.ID <= 386).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["bow_distance"].Value = GetRandomUShort(10, 80);
                }
            }
            if (c_ArrowParam_AmmoDamage)
            {
                List<PARAM.Row> rows = ArrowParam;

                foreach (PARAM.Row row in rows)
                {
                    if((ushort)row["damage_physical"].Value > 0)
                        row["damage_physical"].Value = GetRandomUShort(10, 200);

                    if ((ushort)row["damage_magic"].Value > 0)
                        row["damage_magic"].Value = GetRandomUShort(10, 200);

                    if ((ushort)row["damage_lightning"].Value > 0)
                        row["damage_lightning"].Value = GetRandomUShort(10, 200);

                    if ((ushort)row["damage_fire"].Value > 0)
                        row["damage_fire"].Value = GetRandomUShort(10, 200);

                    if ((ushort)row["damage_dark"].Value > 0)
                        row["damage_dark"].Value = GetRandomUShort(10, 200);

                    if ((byte)row["damage_poison"].Value > 0)
                        row["damage_poison"].Value = GetRandomByte(10, 200);

                    if ((byte)row["damage_bleed"].Value > 0)
                        row["damage_bleed"].Value = GetRandomByte(10, 200);
                }
            }
            if (c_WeaponActionCategoryParam_Moveset)
            {
                List<PARAM.Row> rows = WeaponActionCategoryParam;

                // This assigns new values based on the range of values within the existing fields.
                // This is due to the fact that the movesets require similar moves in each slot.
                // Failure to do this causes weird issues to occur.
                foreach (string value in ParamScramblerData.Static.WeaponActionCategoryFields)
                {
                    AssignRandomMoveset(value, rows);
                }
            }

            // Tweaks
            if(c_Tweak_WeaponParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.WeaponParam_Category_End &&
                    !ItemScramblerData.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["str_requirement"].Value = 0;
                    row["dex_requirement"].Value = 0;
                    row["int_requirement"].Value = 0;
                    row["fth_requirement"].Value = 0;
                }
            }

            return regulation;
        }

        public void AssignRandomMoveset(string field, List<PARAM.Row> rows)
        {
            List<uint> new_list = new List<uint>();

            // Build list of possible values
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        new_list.Add((uint)cell.Value);
                    }
                }
            }

            // Assign new
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        cell.Value = new_list[rand.Next(new_list.Count)];
                    }
                }
            }
        }

        #endregion

        public Regulation Scramble_ArmorAttributes(
            bool c_ItemParam_Armor_Price,
            bool c_ItemParam_Armor_Effect,
            bool c_ArmorParam_Armor_Weight,
            bool c_ArmorParam_Armor_Durability,
            bool c_ArmorParam_Defence,
            bool c_ArmorParam_StatRequirements,
            bool c_ArmorParam_Poise,
            bool c_ArmorReinforceParam_Absorption,
            bool c_Tweak_ArmorParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorReinforceParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorReinforceParam")).ToList()[0].Rows;

            if(c_ItemParam_Armor_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
               row.ID >= ItemScramblerData.Static.ArmorParam_Category_Start &&
               row.ID <= ItemScramblerData.Static.ArmorParam_Category_End
               ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(10, 10000);
                    row["sell_price"].Value = GetRandomInt(10, 10000);
                }
            }
            if (c_ItemParam_Armor_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                    row.ID >= ItemScramblerData.Static.ArmorParam_Category_Start &&
                    row.ID <= ItemScramblerData.Static.ArmorParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var list = ParamScramblerData.Static.SpEffect_ID_List;
                    var result = list[rand.Next(list.Count)];

                    row["speffect_id"].Value = result;
                }
            }
            if (c_ArmorParam_Armor_Weight)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row => 
                row.ID >= 11010100 && 
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["weight"].Value = GetRandomFloat(0.1, 15);
                }
            }
            if (c_ArmorParam_Armor_Durability)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                 row.ID >= 11010100 &&
                 row.ID <= 17950103
                 ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["durability"].Value = (float)GetRandomInt(10, 100);
                }
            }
            if (c_ArmorParam_Defence)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["physical_defence_scaling"].Value = GetRandomFloat(0.1, 1);
                    row["Unk11"].Value = GetRandomFloat(0.8, 1.2);
                    row["Unk12"].Value = GetRandomFloat(0.5, 1.0);
                    row["Unk13"].Value = GetRandomFloat(0.75, 1.25);
                }
            }
            if (c_ArmorParam_StatRequirements)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["strength_requirement"].Value = GetRandomUShort(1, 50);
                    row["dexterity_requirement"].Value = GetRandomUShort(1, 50);
                    row["intelligence_requirement"].Value = GetRandomUShort(1, 50);
                    row["faith_requirement"].Value = GetRandomUShort(1, 50);
                }
            }
            if (c_ArmorParam_Poise)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["poise"].Value = GetRandomFloat(1, 20);
                }
            }
            if (c_ArmorReinforceParam_Absorption)
            {
                List<PARAM.Row> rows = ArmorReinforceParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    // Defence
                    var base_slash = GetRandomFloat(10, 100);
                    var base_thrust = GetRandomFloat(10, 100);
                    var base_strike = GetRandomFloat(10, 100);
                    var base_standard = GetRandomFloat(10, 100);

                    var base_magic = GetRandomFloat(1, 5);
                    var base_lightning = GetRandomFloat(1, 5);
                    var base_fire = GetRandomFloat(1, 5);
                    var base_dark = GetRandomFloat(1, 5);

                    var base_poison = GetRandomFloat(1, 5);
                    var base_bleed = GetRandomFloat(1, 5);
                    var base_petrify = GetRandomFloat(1, 5);
                    var base_curse = GetRandomFloat(1, 5);

                    var growth = GetRandomFloat(1.1, 2.5);

                    row["defence_slash"].Value = base_slash;
                    row["defence_thrust"].Value = base_thrust;
                    row["defence_strike"].Value = base_strike;
                    row["defence_standard"].Value = base_standard;

                    row["absorption_magic"].Value = base_magic;
                    row["absorption_lightning"].Value = base_lightning;
                    row["absorption_fire"].Value = base_fire;
                    row["absorption_dark"].Value = base_dark;

                    row["absorption_poison"].Value = base_poison;
                    row["absorption_bleed"].Value = base_bleed;
                    row["absorption_petrify"].Value = base_petrify;
                    row["absorption_curse"].Value = base_curse;

                    row["max_defence_slash"].Value = (int)(base_slash * growth);
                    row["max_defence_thrust"].Value = (int)(base_thrust * growth);
                    row["max_defence_strike"].Value = (int)(base_strike * growth);
                    row["max_defence_standard"].Value = (int)(base_standard * growth);

                    row["max_absorption_magic"].Value = (int)(base_magic * growth);
                    row["max_absorption_lightning"].Value = (int)(base_lightning * growth);
                    row["max_absorption_fire"].Value = (int)(base_fire * growth);
                    row["max_absorption_dark"].Value = (int)(base_dark * growth);

                    row["max_absorption_poison"].Value = (int)(base_poison * growth);
                    row["max_absorption_bleed"].Value = (int)(base_bleed * growth);
                    row["max_absorption_petrify"].Value = (int)(base_petrify * growth);
                    row["max_absorption_curse"].Value = (int)(base_curse * growth);
                }
            }

            // Tweaks
            if (c_Tweak_ArmorParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["strength_requirement"].Value = 0;
                    row["dexterity_requirement"].Value = 0;
                    row["intelligence_requirement"].Value = 0;
                    row["faith_requirement"].Value = 0;
                }
            }

            return regulation;
        }

        public Regulation Scramble_RingAttributes(
            bool c_ItemParam_Ring_Price,
            bool c_ItemParam_Ring_Effect,
            bool c_RingParam_Ring_Weight,
            bool c_RingParam_Ring_Durability
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> RingParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"RingParam")).ToList()[0].Rows;

            if (c_ItemParam_Ring_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.RingParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(10, 10000);
                    row["sell_price"].Value = GetRandomInt(10, 10000);
                }
            }
            if (c_ItemParam_Ring_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.RingParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var list = ParamScramblerData.Static.SpEffect_ID_List;
                    var result = list[rand.Next(list.Count)];

                    row["speffect_id"].Value = result;
                }
            }
            if (c_RingParam_Ring_Weight)
            {
                List<PARAM.Row> rows = RingParam.Where(row =>
                row.ID >= ItemScramblerData.Static.RingParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["weight"].Value = GetRandomFloat(0.1, 10);
                }
            }
            if (c_RingParam_Ring_Durability)
            {
                List<PARAM.Row> rows = RingParam.Where(row =>
                row.ID >= ItemScramblerData.Static.RingParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["durability"].Value = (float)GetRandomInt(10, 300);
                }
            }

            return regulation;
        }
        public Regulation Scramble_ItemAttributes(
            bool c_ItemParam_Item_Price,
            bool c_ItemParam_Item_Animation_Speed,
            bool c_ItemParam_Item_Max_Hold_Count,
            bool c_ItemParam_Item_Effect
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;

            if (c_ItemParam_Item_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.ItemParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(10, 10000);
                    row["sell_price"].Value = GetRandomInt(10, 10000);
                }
            }
            if (c_ItemParam_Item_Animation_Speed)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.ItemParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["animation_speed"].Value = GetRandomFloat(0.5, 2.0);
                }
            }
            if (c_ItemParam_Item_Max_Hold_Count)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.ItemParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    if((ushort)row["max_held_count"].Value > 1)
                        row["max_held_count"].Value = GetRandomUShort(2, 99);
                }
            }
            if (c_ItemParam_Item_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.ItemParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var list = ParamScramblerData.Static.SpEffect_ID_List;
                    var result = list[rand.Next(list.Count)];

                    row["speffect_id"].Value = result;
                }
            }

            return regulation;
        }

        public Regulation Scramble_SpellAttributes(
            bool c_ItemParam_Spell_Price,
            bool c_SpellParam_StatRequirements,
            bool c_SpellParam_StartupSpeed,
            bool c_SpellParam_CastAnimations,
            bool c_SpellParam_StaminaConsumption,
            bool c_SpellParam_CastSpeed,
            bool c_SpellParam_SlotsUsed,
            bool c_SpellParam_Casts,
            bool c_Tweak_SpellParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> SpellParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"SpellParam")).ToList()[0].Rows;

            if (c_ItemParam_Spell_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(10, 10000);
                    row["sell_price"].Value = GetRandomInt(10, 10000);
                }
            }
            if (c_SpellParam_StatRequirements)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["int_requirement"].Value = GetRandomUShort(1, 50);
                    row["fth_requirement"].Value = GetRandomUShort(1, 50);
                }
            }
            if (c_SpellParam_StartupSpeed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["startup_duration"].Value = GetRandomFloat(0.2, 1.5);
                }
            }
            if (c_SpellParam_CastAnimations)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                // This assigns new values based on the range of values within the existing fields.
                foreach (string value in ParamScramblerData.Static.SpellCastAnimationFields)
                {
                    AssignRandomCast(value, rows);
                }
            }
            if (c_SpellParam_StaminaConsumption)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["stamina_cost_1h_left"].Value = GetRandomFloat(0.2, 3.0);
                    row["stamina_cost_1h_right"].Value = GetRandomFloat(0.2, 3.0);
                    row["stamina_cost_2h_left"].Value = GetRandomFloat(0.2, 3.0);
                    row["stamina_cost_2h_right"].Value = GetRandomFloat(0.2, 3.0);
                    row["Unk13"].Value = GetRandomFloat(0.2, 3.0);
                    row["Unk18"].Value = GetRandomFloat(0.2, 3.0);
                    row["Unk23"].Value = GetRandomFloat(0.2, 3.0);
                    row["Unk28"].Value = GetRandomFloat(0.2, 3.0);
                }
            }
            if (c_SpellParam_CastSpeed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["cast_speed"].Value = GetRandomFloat(0.5, 2.0);
                }
            }
            if (c_SpellParam_SlotsUsed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["slots_used"].Value = GetRandomInt(1, 3);
                }
            }
            if (c_SpellParam_Casts)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var base_casts = GetRandomInt(2, 15);
                    var growth = GetRandomInt(1, 3);

                    row["casts_tier_1"].Value = base_casts;
                    row["casts_tier_2"].Value = (base_casts + growth);
                    row["casts_tier_3"].Value = (base_casts + (growth * 2));
                    row["casts_tier_4"].Value = (base_casts + (growth * 3));
                    row["casts_tier_5"].Value = (base_casts + (growth * 4));
                    row["casts_tier_6"].Value = (base_casts + (growth * 5));
                    row["casts_tier_7"].Value = (base_casts + (growth * 6));
                    row["casts_tier_8"].Value = (base_casts + (growth * 7));
                    row["casts_tier_9"].Value = (base_casts + (growth * 8));
                    row["casts_tier_10"].Value = (base_casts + (growth * 9));
                }
            }

            // Tweaks
            if (c_Tweak_SpellParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ItemScramblerData.Static.SpellParam_Category_Start &&
                row.ID <= ItemScramblerData.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["int_requirement"].Value = 0;
                    row["fth_requirement"].Value = 0;
                }
            }

            return regulation;
        }

        public void AssignRandomCast(string field, List<PARAM.Row> rows)
        {
            List<int> new_list = new List<int>();

            // Build list of possible values
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        new_list.Add((int)cell.Value);
                    }
                }
            }

            // Assign new
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        cell.Value = new_list[rand.Next(new_list.Count)];
                    }
                }
            }
        }

        public Regulation Scramble_BulletParams(
            bool c_Bullet_IncludePlayer,
            bool c_Bullet_IncludeEnemy,
            bool c_Bullet_IncludeBoss,
            bool c_Bullet_IncludeTraps,
            bool c_Bullet_VFX,
            bool c_Bullet_Movement,
            bool c_Bullet_Angle,
            bool c_Bullet_SpawnDistance,
            bool c_Bullet_Duration,
            bool c_Bullet_Tracking,
            bool c_Bullet_Effect
        )
        {
            List<PARAM.Row> BulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"BulletParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyBulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyBulletParam")).ToList()[0].Rows;
            List<PARAM.Row> SystemBulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"SystemBulletParam")).ToList()[0].Rows;

            return regulation;
        }

        public Regulation Scramble_PlayerParams(
            bool c_PlayerStatusParam_StartingAttributes,
            bool c_PlayerStatusParam_StartingEquipment,
            bool c_PlayerStatusParam_StartingGifts,
            bool c_PlayerLevelUpSoulsParam_LevelupCost,
            bool c_EventCommonParam_ShrineOfWinter_Cost,
            bool c_BossBattleParam_BossSoulDrops,
            bool c_LockOnParam_CameraDistance,
            bool c_LockOnParam_CameraFOV,
            bool c_ChrMoveParam_Walk,
            bool c_ChrMoveParam_Run,
            bool c_ChrMoveParam_Jump,
            bool c_ChrMoveParam_Ladder,
            bool c_ChrMoveParam_Turn,
            bool c_Tweak_AnyEquipmentForStartingEquipment,
            bool c_Tweak_BigJumpMode
        )
        {
            List<PARAM.Row> PlayerStatusParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"PlayerStatusParam")).ToList()[0].Rows;
            List<PARAM.Row> PlayerLevelUpSoulsParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"PlayerLevelUpSoulsParam")).ToList()[0].Rows;
            List<PARAM.Row> EventCommonParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EventCommonParam")).ToList()[0].Rows;
            List<PARAM.Row> BossBattleParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"BossBattleParam")).ToList()[0].Rows;
            List<PARAM.Row> ChrMoveParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ChrMoveParam")).ToList()[0].Rows;

            return regulation;
        }

        public Regulation Scramble_MapParams(
            bool c_TreasureBoxParam_TrappedChests
        )
        {

            return regulation;
        }

        public Regulation Scramble_CharacterParams(
            bool c_NpcPlayerStatusParam_Equipment
        )
        {
            List<PARAM.Row> NpcPlayerStatusParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"NpcPlayerStatusParam")).ToList()[0].Rows;

            return regulation;
        }

        public Regulation Scramble_EnemyParams(
            bool c_Enemy_IncludeEnemies,
            bool c_Enemy_IncludeBosses,
            bool c_LogicComParam_Detection,
            bool c_EnemyParam_HP,
            bool c_EnemyParam_Souls,
            bool c_EnemyParam_Stamina,
            bool c_EnemyParam_Defence,
            bool c_EnemyParam_ShieldDefence,
            bool c_EnemyParam_Poise,
            bool c_EnemyDamageParam_Damage,
            bool c_EnemyDamageParam_Knockback,
            bool c_EnemyDamageParam_AttackSpeed,
            bool c_EnemyMoveParam_Walk,
            bool c_EnemyMoveParam_Run,
            bool c_EnemyMoveParam_Jump,
            bool c_EnemyMoveParam_Climb,
            bool c_EnemyMoveParam_Turn
        )
        {
            List<PARAM.Row> LogicComParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"LogicComParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyDamageParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyDamageParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyMoveParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyMoveParam")).ToList()[0].Rows;

            return regulation;
        }

        /*
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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

                    ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
                        GenerateValuesForParam(wrapper, param_rows);

                        // Generate from lists
                        foreach (PARAM.Row row in param_rows)
                        {
                            Util.SetRandomItem(row, "spell_item_1", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_2", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_3", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_4", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_5", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_6", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_7", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_8", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_9", Data.Row_List_Spells_NPC);
                            Util.SetRandomItem(row, "spell_item_10", Data.Row_List_Spells_NPC);

                            Util.SetRandomItem(row, "right_weapon_item_1", Data.Row_List_Weapons);
                            Util.SetRandomItem(row, "right_weapon_item_2", Data.Row_List_Weapons);

                            if (rand.Next(100) <= 50)
                                Util.SetRandomItem(row, "right_weapon_item_3", Data.Row_List_Weapons);
                            else
                                row["right_weapon_item_3"].Value = -1;

                            int roll = rand.Next(100);

                            if (roll <= 20)
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Sorcery);
                            else if (roll > 20 && roll <= 30)
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Miracles);
                            else if (roll > 30 && roll <= 40)
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Pyromancy);
                            else if (roll > 40 && roll <= 50)
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Hex);
                            else if (roll > 50 && roll <= 75)
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Shield);
                            else
                                Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons);

                            roll = rand.Next(100);

                            if (roll <= 20)
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Sorcery);
                            else if (roll > 20 && roll <= 30)
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Miracles);
                            else if (roll > 30 && roll <= 40)
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Pyromancy);
                            else if (roll > 40 && roll <= 50)
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Hex);
                            else if (roll > 50 && roll <= 75)
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Shield);
                            else
                                Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons);

                            if(rand.Next(100) < 50)
                            {
                                if (roll <= 20)
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Sorcery);
                                else if (roll > 20 && roll <= 30)
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Miracles);
                                else if (roll > 30 && roll <= 40)
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Pyromancy);
                                else if (roll > 40 && roll <= 50)
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Hex);
                                else if (roll > 50 && roll <= 75)
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Shield);
                                else
                                    Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons);
                            }

                            // Armor
                            Util.SetRandomItem(row, "head_item", Data.Row_List_Armor_Head);
                            Util.SetRandomItem(row, "chest_item", Data.Row_List_Armor_Chest);
                            Util.SetRandomItem(row, "hands_item", Data.Row_List_Armor_Arms);
                            Util.SetRandomItem(row, "legs_item", Data.Row_List_Armor_Legs);

                            // Rings
                            if (rand.Next(100) < 25)
                            {
                                Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        param_rows = param_rows.Where(row => ParamScramblerData.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                    }
                    else
                    {
                        param_rows = param_rows.Where(row => !ParamScramblerData.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, fieldAppend);
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
                        param_rows = param_rows.Where(row => ParamScramblerData.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                    }
                    else
                    {
                        param_rows = param_rows.Where(row => !ParamScramblerData.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, fieldAppend);
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
                        param_rows = Util.GetRowsFromSubMatch(param_rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 4, "1", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, fieldAppend);
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
                        param_rows = Util.GetRowsFromSubMatch(param.Rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param.Rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 4, "1", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, fieldAppend);
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
                        param_rows = Util.GetRowsFromSubMatch(param_rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 2, "");
                    }
                    else
                    {
                        param_rows = Util.GetRowsFromSubMatch(param_rows, ParamScramblerData.Static.Boss_EnemyParamID_List, 2, 2, "", true);
                    }

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict, fieldAppend);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, fieldAppend);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Shuffle_Field_Dict);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict);
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

        #region Scramble - ChrMoveParam
        public Regulation Scramble_ChrMoveParam(string paramName)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    ShuffleValuesForParam(wrapper, param_rows, Data.Scramble_Type_Generate_Field_Dict, "");
                    GenerateValuesForParam(wrapper, param_rows, "");
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - EventCommonParam
        public Regulation Scramble_EventCommonParam(string paramName)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name == paramName)
                {
                    param_rows = param_rows.Where(row => row.ID == 14).ToList();

                    GenerateValuesForParam(wrapper, param_rows, "");
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - LockOnParam
        public Regulation Scramble_LockOnParam(string paramName, bool adjustDistance, bool adjustFOV)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name == paramName)
                {
                    foreach (PARAM.Row row in param_rows)
                    {
                        if(adjustDistance)
                            row["max_dist_behind_player"].Value = (1 + rand.NextDouble() * rand.Next(5, 20));

                        if (adjustFOV)
                        {
                            row["fov_0"].Value = rand.Next(30, 150);
                            row["fov_1"].Value = rand.Next(10, 100);
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - PlayerLevelUpSoulsParam
        public Regulation Scramble_PlayerLevelUpSoulsParam(string paramName)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    var dict = Data.Scramble_Type_Generate_Field_And_Values_Dict[wrapper.Name];
                    int min = 0;
                    int max = 1;

                    // Get the min and max
                    foreach (PARAM.Row row in param_rows)
                    {
                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            // Field is within the dictionary
                            if (dict.ContainsKey(cell.Def.InternalName))
                            {
                                var list = dict[cell.Def.InternalName];
                                min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                max = int.Parse(list[1], CultureInfo.InvariantCulture);
                            }
                        }
                    }

                    // Apply in a controlled way so the level-up cost isn't stupidly high in the early levels
                    foreach (PARAM.Row row in param_rows)
                    {
                        var current_min = min;
                        var current_max = max;

                        if(row.ID <= 50)
                        {
                            if(current_max > 10000)
                                current_max = 10000;
                        }

                        if (row.ID > 50 && row.ID <= 100)
                        {
                            if (current_max > 20000)
                                current_max = 20000;
                        }

                        if (row.ID > 100 && row.ID <= 200)
                        {
                            if (current_max > 60000)
                                current_max = 60000;
                        }

                        if (row.ID > 200 && row.ID <= 300)
                        {
                            if (current_max > 300000)
                                current_max = 30000;
                        }

                        if (row.ID > 300 && row.ID <= 400)
                        {
                            if (current_max > 600000)
                                current_max = 60000;
                        }

                        row["soul_level_cost"].Value = rand.Next(current_min, current_max);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - PlayerStatusParam
        public Regulation Scramble_PlayerStatusParam(string paramName, bool scrambleClasses, bool scrambleGifts, int statSkew, bool limitEquipment)
        {
            if (scrambleClasses)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    PARAM param = wrapper.Param;
                    List<PARAM.Row> param_rows = param.Rows;

                    if (wrapper.Name == paramName)
                    {
                        param_rows = param_rows.Where(row => row.ID >= 20 && row.ID <= 100).ToList();

                        RandomiseClassStats(param_rows, statSkew);
                        RandomiseClassEquipment(param_rows, limitEquipment);

                        // Support for mod added ones
                        param_rows = param_rows.Where(row => row.ID > 110 && row.ID < 500).ToList();

                        RandomiseClassStats(param_rows, statSkew);
                        RandomiseClassEquipment(param_rows, limitEquipment);
                    }
                }
            }

            if (scrambleGifts)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    PARAM param = wrapper.Param;
                    List<PARAM.Row> param_rows = param.Rows;

                    if (wrapper.Name == paramName)
                    {
                        param_rows = param_rows.Where(row => row.ID >= 510 && row.ID <= 570).ToList();

                        // TODO: add fmg support so the gift text can be changed to the right items
                        RandomiseGifts(param_rows);
                    }
                }
            }

            return regulation;
        }
        public void RandomiseGifts(List<PARAM.Row> rows)
        {
            foreach (PARAM.Row row in rows)
            {
                int roll = rand.Next(100);

                // Clean up current values
                row["ring_item_1"].Value = -1;

                for (int x = 1; x < 10; x++)
                {
                    row[$"item_{x}"].Value = -1;
                    row[$"item_amount_{x}"].Value = 0;
                }

                // Ring
                if (roll <= 25)
                {
                    Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);
                }
                // Good
                else
                {
                    int endChance = 20;

                    if (row.ID == 530)
                        endChance = 0;

                    for (int x = 1; x < 10; x++)
                    {
                        Util.SetRandomGood(row, $"item_{x}", Data, $"item_amount_{x}");

                        endChance = endChance + rand.Next(1, x * 10);
                        if (endChance > 25)
                            break;
                    }
                }
            }
        }

        public void RandomiseClassStats(List<PARAM.Row> rows, int statSkew)
        {
            foreach (PARAM.Row row in rows)
            {
                ushort vigor = GetRandomStat(row, "vigor", statSkew);
                ushort endurance = GetRandomStat(row, "endurance", statSkew);
                ushort attunement = GetRandomStat(row, "attunement", statSkew);
                ushort vitality = GetRandomStat(row, "vitality", statSkew);
                ushort strength = GetRandomStat(row, "strength", statSkew);
                ushort dexterity = GetRandomStat(row, "dexterity", statSkew);
                ushort intelligence = GetRandomStat(row, "intelligence", statSkew);
                ushort faith = GetRandomStat(row, "faith", statSkew);
                ushort adaptability = GetRandomStat(row, "adaptability", statSkew);

                ushort total = (ushort)((vigor + endurance + attunement + vitality + strength + dexterity + intelligence + faith + adaptability) - 54);

                if (total < 1)
                    total = 1;

                Util.PrintLine($"Total Stats: {total}");

                row["soul_level"].Value = total;
                row["vigor"].Value = vigor;
                row["endurance"].Value = endurance;
                row["attunement"].Value = attunement;
                row["vitality"].Value = vitality;
                row["strength"].Value = strength;
                row["dexterity"].Value = dexterity;
                row["intelligence"].Value = intelligence;
                row["faith"].Value = faith;
                row["adaptability"].Value = adaptability;
            }
        }

        public ushort GetRandomStat(PARAM.Row row, string stat, int statSkew)
        {
            ushort adjust = (ushort)statSkew;

            ushort stat_value = (ushort)row[stat].Value;

            ushort lower = (ushort)(stat_value - adjust);
            ushort upper = (ushort)(stat_value + adjust);


            if (lower > upper)
                lower = (ushort)(upper - 1);

            if (lower < 1)
                lower = 1;

            if (upper < lower)
                upper = (ushort)(lower + 1);

            return (ushort)rand.Next(lower, upper);
        }

        List<PARAM.Row> valid_melee_weapons = new List<PARAM.Row>();
        List<PARAM.Row> valid_sorcery_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_miracle_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_pyromancy_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_hex_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_bows = new List<PARAM.Row>();
        List<PARAM.Row> valid_greatbows = new List<PARAM.Row>();
        List<PARAM.Row> valid_crossbows = new List<PARAM.Row>();
        List<PARAM.Row> valid_shields = new List<PARAM.Row>();
        List<PARAM.Row> valid_head_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_chest_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_arm_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_leg_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_sorceries = new List<PARAM.Row>();
        List<PARAM.Row> valid_miracles = new List<PARAM.Row>();
        List<PARAM.Row> valid_pyromancies = new List<PARAM.Row>();
        List<PARAM.Row> valid_hexes = new List<PARAM.Row>();

        public bool usesSorceries = false;
        public bool usesMiracles = false;
        public bool usesPyromancies = false;
        public bool usesHexes = false;
        public bool usesArrows = false;
        public bool usesGreatarrows = false;
        public bool usesBolts = false;
        public bool setFirstSpell = false;

        public void RandomiseClassEquipment(List<PARAM.Row> rows, bool limitEquipment)
        {

            foreach (PARAM.Row row in rows)
            {
                ushort attunement = (ushort)row["attunement"].Value;
                ushort vitality = (ushort)row["vitality"].Value;
                ushort strength = (ushort)row["strength"].Value;
                ushort dexterity = (ushort)row["dexterity"].Value;
                ushort intelligence = (ushort)row["intelligence"].Value;
                ushort faith = (ushort)row["faith"].Value;

                usesSorceries = false;
                usesMiracles = false;
                usesPyromancies = false;
                usesHexes = false;
                usesArrows = false;
                usesGreatarrows = false;
                usesBolts = false;
                setFirstSpell = false;

                UpdateSelectionListsForClass((int)attunement, (int)vitality, (int)strength, (int)dexterity, (int)intelligence, (int)faith, limitEquipment);

                // Weapons
                AssignWeaponSlot(row, "right_weapon_item_1", false);

                if(rand.Next(100) >= 50)
                    AssignWeaponSlot(row, "right_weapon_item_2", false);

                if (rand.Next(100) >= 75)
                    AssignWeaponSlot(row, "right_weapon_item_3", false);

                AssignWeaponSlot(row, "left_weapon_item_1", true);

                if (rand.Next(100) >= 50)
                    AssignWeaponSlot(row, "left_weapon_item_2", true);

                if (rand.Next(100) >= 75)
                    AssignWeaponSlot(row, "left_weapon_item_3", true);

                // Spells
                AssignSpell(row, "spell_item_1");

                if (rand.Next(100) >= 50)
                    AssignSpell(row, "spell_item_2");

                if (rand.Next(100) >= 75)
                    AssignSpell(row, "spell_item_3");

                // Armor
                AssignArmor(row, "head_item", valid_head_armor);
                AssignArmor(row, "chest_item", valid_chest_armor);
                AssignArmor(row, "hands_item", valid_arm_armor);
                AssignArmor(row, "legs_item", valid_leg_armor);

                // Rings
                if (rand.Next(100) >= 50)
                    Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);

                // Ammo
                if(usesArrows)
                    Util.SetRandomItemWithAmount(row, "arrow_item_1", Data.Row_List_Ammunition_Arrow, "arrow_amount_1", 25, 50);

                if (usesGreatarrows)
                    Util.SetRandomItemWithAmount(row, "arrow_item_1", Data.Row_List_Ammunition_Greatarrow, "arrow_amount_1", 25, 50);

                if (usesBolts)
                    Util.SetRandomItemWithAmount(row, "bolt_item_1", Data.Row_List_Ammunition_Bolt, "bolt_amount_1", 25, 50);

                // Starting Item
                for(int x = 1; x <= 7; x++)
                {
                    if(rand.Next(100) > 50 || row.ID == 90)
                        Util.SetRandomGood(row, $"item_{x}", Data, $"item_amount_{x}");
                }
            }
        }

        public void AssignArmor(PARAM.Row row, string slot, List<PARAM.Row> armorList)
        {
            Random rand = new Random();

            if (armorList.Count > 0)
            {
                row[slot].Value = armorList[rand.Next(armorList.Count)].ID;
            }
        }

        public void AssignSpell(PARAM.Row row, string slot)
        {
            Random rand = new Random();

            int roll = rand.Next(100);

            if (!setFirstSpell)
            {
                roll = 25;
                setFirstSpell = true;
            }

            // Sorceries
            if (usesSorceries && valid_sorceries.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_sorceries[rand.Next(valid_sorceries.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Miracles
            if (usesMiracles && valid_miracles.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_miracles[rand.Next(valid_miracles.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Pyromancies
            if (usesPyromancies && valid_pyromancies.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_pyromancies[rand.Next(valid_pyromancies.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Hexes
            if (usesHexes && valid_hexes.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_hexes[rand.Next(valid_hexes.Count)].ID;
                    usesSorceries = true;
                }
            }
        }

        public void AssignWeaponSlot(PARAM.Row row, string slot, bool allowShields)
        {
            Random rand = new Random();

            // Right Weapon 1
            int roll = rand.Next(100);

            // Caster
            if (roll >= 66 && !usesSorceries && !usesMiracles && !usesPyromancies && !usesHexes)
            {
                roll = rand.Next(100);

                // Sorceries
                if (roll >= 75 && valid_sorceries.Count > 0 && valid_sorcery_catalyst.Count > 0)
                {
                    row[slot].Value = valid_sorcery_catalyst[rand.Next(valid_sorcery_catalyst.Count)].ID;
                    usesSorceries = true;
                }
                // Miracles
                else if (roll >= 50 && roll < 75 && valid_miracles.Count > 0 && valid_miracle_catalyst.Count > 0)
                {
                    row[slot].Value = valid_miracle_catalyst[rand.Next(valid_miracle_catalyst.Count)].ID;
                    usesMiracles = true;
                }
                // Pyromancies
                else if (roll >= 25 && roll < 50 && valid_pyromancies.Count > 0 && valid_pyromancy_catalyst.Count > 0)
                {
                    row[slot].Value = valid_pyromancy_catalyst[rand.Next(valid_pyromancy_catalyst.Count)].ID;
                    usesPyromancies = true;
                }
                // Hexes
                else if(roll < 25 && valid_hexes.Count > 0 && valid_hex_catalyst.Count > 0)
                {
                    row[slot].Value = valid_hex_catalyst[rand.Next(valid_hex_catalyst.Count)].ID;
                    usesHexes = true;
                }
                // Fallback
                else
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
            }
            // Ranged
            else if (roll >= 33 && roll < 66 && !usesArrows && !usesGreatarrows && !usesArrows)
            {
                roll = rand.Next(100);

                // Crossbow
                if (roll >= 66 && valid_crossbows.Count > 0)
                {
                    row[slot].Value = valid_crossbows[rand.Next(valid_crossbows.Count)].ID;
                    usesArrows = true;
                }
                // Greatbow
                else if (roll >= 33 && roll < 66 && valid_greatbows.Count > 0)
                {
                    row[slot].Value = valid_greatbows[rand.Next(valid_greatbows.Count)].ID;
                    usesGreatarrows = true;
                }
                // Bow
                else if(roll < 33 && valid_bows.Count > 0)
                {
                    row[slot].Value = valid_bows[rand.Next(valid_bows.Count)].ID;
                    usesArrows = true;
                }
                // Fallback
                else
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
            }
            // Melee
            else
            {
                if (allowShields)
                {
                    roll = rand.Next(100);

                    if(roll >= 50 && valid_shields.Count > 0)
                    {
                        row[slot].Value = valid_shields[rand.Next(valid_shields.Count)].ID;
                    }
                    else
                    {
                        row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                    }
                }
                else
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
            }
        }

        public void UpdateSelectionListsForClass(int attunement, int vitality, int strength, int dexterity, int intelligence, int faith, bool limitEquipment)
        {
            strength = (int)(strength * 1.5);

            if (limitEquipment)
            {
                valid_melee_weapons = Data.Row_List_Weapons_Melee.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_melee_weapons: {valid_melee_weapons.Count}");

                valid_sorcery_catalyst = Data.Row_List_Weapons_Catalyst_Sorcery.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_sorcery_catalyst: {valid_sorcery_catalyst.Count}");

                valid_miracle_catalyst = Data.Row_List_Weapons_Catalyst_Miracles.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_miracle_catalyst: {valid_miracle_catalyst.Count}");

                valid_pyromancy_catalyst = Data.Row_List_Weapons_Catalyst_Pyromancy.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_pyromancy_catalyst: {valid_pyromancy_catalyst.Count}");

                valid_hex_catalyst = Data.Row_List_Weapons_Catalyst_Hex.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_hex_catalyst: {valid_hex_catalyst.Count}");

                valid_bows = Data.Row_List_Weapons_Bow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_bows: {valid_bows.Count}");

                valid_greatbows = Data.Row_List_Weapons_Greatbow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_greatbows: {valid_greatbows.Count}");

                valid_crossbows = Data.Row_List_Weapons_Crossbow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_crossbows: {valid_crossbows.Count}");

                valid_shields = Data.Row_List_Weapons_Shield.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_shields: {valid_shields.Count}");

                valid_head_armor = Data.Row_List_Armor_Head.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_head_armor: {valid_head_armor.Count}");

                valid_chest_armor = Data.Row_List_Armor_Chest.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_chest_armor: {valid_chest_armor.Count}");

                valid_arm_armor = Data.Row_List_Armor_Arms.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_arm_armor: {valid_arm_armor.Count}");

                valid_leg_armor = Data.Row_List_Armor_Legs.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_leg_armor: {valid_leg_armor.Count}");

                valid_sorceries = Data.Row_List_Spell_Sorceries.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_sorceries: {valid_sorceries.Count}");

                valid_miracles = Data.Row_List_Spell_Miracles.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_miracles: {valid_miracles.Count}");

                valid_pyromancies = Data.Row_List_Spell_Pyromancies.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_pyromancies: {valid_pyromancies.Count}");

                valid_hexes = Data.Row_List_Spell_Hexes.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                Util.PrintLine($"valid_hexes: {valid_hexes.Count}");
            }
            else
            {
                valid_melee_weapons = Data.Row_List_Weapons_Melee;

                Util.PrintLine($"valid_melee_weapons: {valid_melee_weapons.Count}");

                valid_sorcery_catalyst = Data.Row_List_Weapons_Catalyst_Sorcery;

                Util.PrintLine($"valid_sorcery_catalyst: {valid_sorcery_catalyst.Count}");

                valid_miracle_catalyst = Data.Row_List_Weapons_Catalyst_Miracles;

                Util.PrintLine($"valid_miracle_catalyst: {valid_miracle_catalyst.Count}");

                valid_pyromancy_catalyst = Data.Row_List_Weapons_Catalyst_Pyromancy;

                Util.PrintLine($"valid_pyromancy_catalyst: {valid_pyromancy_catalyst.Count}");

                valid_hex_catalyst = Data.Row_List_Weapons_Catalyst_Hex;

                Util.PrintLine($"valid_hex_catalyst: {valid_hex_catalyst.Count}");

                valid_bows = Data.Row_List_Weapons_Bow;

                Util.PrintLine($"valid_bows: {valid_bows.Count}");

                valid_greatbows = Data.Row_List_Weapons_Greatbow;

                Util.PrintLine($"valid_greatbows: {valid_greatbows.Count}");

                valid_crossbows = Data.Row_List_Weapons_Crossbow;

                Util.PrintLine($"valid_crossbows: {valid_crossbows.Count}");

                valid_shields = Data.Row_List_Weapons_Shield;

                Util.PrintLine($"valid_shields: {valid_shields.Count}");

                valid_head_armor = Data.Row_List_Armor_Head;

                Util.PrintLine($"valid_head_armor: {valid_head_armor.Count}");

                valid_chest_armor = Data.Row_List_Armor_Chest;

                Util.PrintLine($"valid_chest_armor: {valid_chest_armor.Count}");

                valid_arm_armor = Data.Row_List_Armor_Arms;

                Util.PrintLine($"valid_arm_armor: {valid_arm_armor.Count}");

                valid_leg_armor = Data.Row_List_Armor_Legs;

                Util.PrintLine($"valid_leg_armor: {valid_leg_armor.Count}");

                valid_sorceries = Data.Row_List_Spell_Sorceries;

                Util.PrintLine($"valid_sorceries: {valid_sorceries.Count}");

                valid_miracles = Data.Row_List_Spell_Miracles;

                Util.PrintLine($"valid_miracles: {valid_miracles.Count}");

                valid_pyromancies = Data.Row_List_Spell_Pyromancies;

                Util.PrintLine($"valid_pyromancies: {valid_pyromancies.Count}");

                valid_hexes = Data.Row_List_Spell_Hexes;

                Util.PrintLine($"valid_hexes: {valid_hexes.Count}");
            }
        }
        #endregion

        */

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
                var dict = Data.Scramble_Type_Generate_Field_And_Values_Dict[wrapper.Name + wrapperAppend];

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
                            if (cell.Def.InternalType == "s32")
                            {
                                int min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = int.Parse(list[1], CultureInfo.InvariantCulture);

                                if (min != -1 || max != -1)
                                {
                                    int value = rand.Next(min, max);

                                    if (isWeighted && rand.Next(100) < 50)
                                        value = 0;

                                    cell.Value = value;
                                }
                            }
                            // UInt32
                            if (cell.Def.InternalType == "u32")
                            {
                                int min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = int.Parse(list[1], CultureInfo.InvariantCulture);

                                if (min != -1 || max != -1)
                                {
                                    uint value = (uint)rand.Next(min, max);

                                    if (isWeighted && rand.Next(100) < 50)
                                        value = 0;

                                    cell.Value = value;
                                }
                            }

                            // Int16
                            if (cell.Def.InternalType == "s16")
                            {
                                int min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = int.Parse(list[1], CultureInfo.InvariantCulture);

                                short value = (short)rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // UInt16
                            if (cell.Def.InternalType == "u16")
                            {
                                int min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = int.Parse(list[1], CultureInfo.InvariantCulture);

                                ushort value = (ushort)rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }

                            // Int8
                            if (cell.Def.InternalType == "s8")
                            {
                                int min = int.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = int.Parse(list[1], CultureInfo.InvariantCulture);

                                sbyte value = (sbyte)rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // UInt8
                            if (cell.Def.InternalType == "u8")
                            {
                                int min = byte.Parse(list[0], CultureInfo.InvariantCulture);
                                int max = byte.Parse(list[1], CultureInfo.InvariantCulture);

                                byte value = (byte)rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }

                            // Float
                            if (cell.Def.InternalType == "f32")
                            {
                                double min = double.Parse(list[0], CultureInfo.InvariantCulture);
                                double max = double.Parse(list[1], CultureInfo.InvariantCulture);

                                float value = (float)(rand.NextDouble() * (max - min) + min);

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
                        Util.PrintLine($"Skipped {cName}");
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
                            Util.PrintLine($"Skipped {cName}");
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

                //Util.PrintLine($"{cName} - {cType}");

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
                    Util.PrintLine($"Skipped {cName}");
                else
                    throw null;
            }
        }
        #endregion
    }
}
