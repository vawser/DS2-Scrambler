
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

    public class Scrambler
    {
        private readonly Random rand;
        public Regulation regulation;

        public string configPath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";

        Dictionary<string, List<string>> ShuffleParamFields = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> GenerateParamFields = new Dictionary<string, List<string>>();
        Dictionary<string, Dictionary<string, List<string>>> GenerateParamValues = new Dictionary<string, Dictionary<string, List<string>>>();

        List<string> BossIDs = new List<string>();
        List<string> CharacterIDs = new List<string>();
        List<string> EnemyIDs = new List<string>();

        List<string> Excluded_Weapons = new List<string>();
        List<string> Excluded_Armor = new List<string>();
        List<string> Excluded_Spells = new List<string>();
        List<string> Excluded_Rings = new List<string>();
        List<string> Excluded_Items = new List<string>();

        List<string> Treasure_Keys = new List<string>();
        List<string> Treasure_Tools = new List<string>();
        List<string> Treasure_Ammo = new List<string>();
        List<string> Treasure_Consumables = new List<string>();
        List<string> Treasure_Gestures = new List<string>();
        List<string> Treasure_Boss_Souls = new List<string>();
        List<string> Treasure_Multiplayer = new List<string>();
        List<string> Treasure_Materials = new List<string>();

        public ParamWrapper ItemParam;
        public ParamWrapper SpellParam;
        public ParamWrapper RingParam;
        public ParamWrapper WeaponParam;
        public ParamWrapper ArmorParam;

        public List<PARAM.Row> Valid_Weapons;
        public List<PARAM.Row> Valid_Armor;
        public List<PARAM.Row> Valid_Armor_HEAD;
        public List<PARAM.Row> Valid_Armor_CHEST;
        public List<PARAM.Row> Valid_Armor_ARMS;
        public List<PARAM.Row> Valid_Armor_LEGS;
        public List<PARAM.Row> Valid_Spells;
        public List<PARAM.Row> Valid_Rings;
        public List<PARAM.Row> Valid_Items;
        public List<PARAM.Row> Valid_Ammunition;
        public List<PARAM.Row> Valid_Key_Items;
        public List<PARAM.Row> Valid_Tool_Items;
        public List<PARAM.Row> Valid_Consumable_Items;
        public List<PARAM.Row> Valid_Multiplayer_Items;
        public List<PARAM.Row> Valid_Boss_Soul_Items;
        public List<PARAM.Row> Valid_Gesture_Items;
        public List<PARAM.Row> Valid_Material_Items;

        #region Scramble - Core
        public Scrambler(string seed, Regulation reg)
        {
            regulation = reg;

            if (seed == string.Empty)
                rand = new Random();
            else
                rand = new Random(seed.GetHashCode());

            BossIDs = BuildIDList("Enemy-Scramble", "Boss-IDs");
            CharacterIDs = BuildIDList("Enemy-Scramble", "Character-IDs");
            EnemyIDs = BuildIDList("Enemy-Scramble", "Enemy-IDs");

            Excluded_Weapons = BuildIDList("Item-Scramble", "Treasure-Excluded-Weapons");
            Excluded_Armor = BuildIDList("Item-Scramble", "Treasure-Excluded-Armor");
            Excluded_Spells = BuildIDList("Item-Scramble", "Treasure-Excluded-Spells");
            Excluded_Rings = BuildIDList("Item-Scramble", "Treasure-Excluded-Rings");
            Excluded_Items = BuildIDList("Item-Scramble", "Treasure-Excluded-Items");

            Treasure_Keys = BuildIDList("Item-Scramble", "Treasure-Keys");
            Treasure_Tools = BuildIDList("Item-Scramble", "Treasure-Tools");
            Treasure_Ammo = BuildIDList("Item-Scramble", "Treasure-Ammo");
            Treasure_Gestures = BuildIDList("Item-Scramble", "Treasure-Gestures");
            Treasure_Consumables = BuildIDList("Item-Scramble", "Treasure-Consumables");
            Treasure_Boss_Souls = BuildIDList("Item-Scramble", "Treasure-Boss-Souls");
            Treasure_Multiplayer = BuildIDList("Item-Scramble", "Treasure-Multiplayer");
            Treasure_Materials = BuildIDList("Item-Scramble", "Treasure-Materials");

            // Build param references
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

            Valid_Weapons = ItemParam.Param.Rows.Where(row => row.ID >= 1000000 && row.ID <= 19999999 && !Excluded_Weapons.Contains(row.ID.ToString())).ToList();

            Valid_Armor = ItemParam.Param.Rows.Where(row => row.ID >= 21010100 && row.ID <= 29999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();

            Valid_Armor_HEAD = new List<PARAM.Row>();
            Valid_Armor_CHEST = new List<PARAM.Row>();
            Valid_Armor_ARMS = new List<PARAM.Row>();
            Valid_Armor_LEGS = new List<PARAM.Row>();

            foreach (PARAM.Row entry in Valid_Armor)
            {
                var last_digit = (entry.ID % 10);

                if (last_digit == 0)
                    Valid_Armor_HEAD.Add(entry);

                if (last_digit == 1)
                    Valid_Armor_CHEST.Add(entry);

                if (last_digit == 2)
                    Valid_Armor_ARMS.Add(entry);

                if (last_digit == 3)
                    Valid_Armor_LEGS.Add(entry);
            }

            Valid_Spells = ItemParam.Param.Rows.Where(row => row.ID >= 31010000 && row.ID <= 39999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();

            Valid_Rings = ItemParam.Param.Rows.Where(row => row.ID >= 40010000 && row.ID <= 49999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();

            Valid_Items = ItemParam.Param.Rows.Where(row => row.ID >= 50600000 && row.ID <= 64610000 && !Excluded_Items.Contains(row.ID.ToString())).ToList();

            Valid_Ammunition = Valid_Items.Where(row => Treasure_Ammo.Contains(row.ID.ToString())).ToList();

            Valid_Key_Items = Valid_Items.Where(row => Treasure_Keys.Contains(row.ID.ToString())).ToList();

            Valid_Tool_Items = Valid_Items.Where(row => Treasure_Tools.Contains(row.ID.ToString())).ToList();

            Valid_Consumable_Items = Valid_Items.Where(row => Treasure_Consumables.Contains(row.ID.ToString())).ToList();

            Valid_Gesture_Items = Valid_Items.Where(row => Treasure_Gestures.Contains(row.ID.ToString())).ToList();

            Valid_Boss_Soul_Items = Valid_Items.Where(row => Treasure_Boss_Souls.Contains(row.ID.ToString())).ToList();

            Valid_Multiplayer_Items = Valid_Items.Where(row => Treasure_Multiplayer.Contains(row.ID.ToString())).ToList();

            Valid_Material_Items = Valid_Items.Where(row => Treasure_Materials.Contains(row.ID.ToString())).ToList();

            // Build ShuffleParamFields dictionary
            foreach (string filepath in Directory.GetFiles(configPath + "\\Param-Scramble\\Shuffle-Type"))
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
            foreach (string filepath in Directory.GetFiles(configPath + "\\Param-Scramble\\Generate-Shuffle-Type"))
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
            foreach (string filepath in Directory.GetFiles(configPath + "\\Param-Scramble\\Generate-Type"))
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

        public List<string> BuildIDList(string folder, string name)
        {
            List<string> idList = new List<string>();

            foreach (string line in File.ReadLines(configPath + $"{folder}//{name}.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                idList.Add(list[0]);
            }

            return idList;
        }
        #endregion

        #region Scramble - ItemLotParam_Map

        public Regulation Scramble_ItemLotParam_Map(string paramName, bool randomiseWeapon, bool ignoreKeys, bool ignoreTools, bool ignoreBossSouls, bool includeBossTreasure, bool includeCharacterTreasure, bool includeCovenantTreasure, bool includeBirdTreasure, bool includeEventTreasure, bool allowDuplicates)
        {
            // Treasure: Boss Rewards
            ScrambleTreasure(paramName, includeBossTreasure, 106000, 862001, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates, new List<int> { 514500 });

            // Treasure: Character Treasure
            ScrambleTreasure(paramName, includeCharacterTreasure, 1307000, 1788030, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates, new List<int> { 1705100, 1705300, 1753010, 1757000, 1758000, 1759000, 1777000, 1786000 });

            // Treasure: Covenant Rewards
            ScrambleTreasure(paramName, includeCovenantTreasure, 2001000, 2009013, randomiseWeapon, true, true, ignoreBossSouls, allowDuplicates);

            // Treasure: Bird Rewards
            ScrambleTreasure(paramName, includeBirdTreasure, 50000000, 50000303, randomiseWeapon, true, true, ignoreBossSouls, allowDuplicates);

            // Treasure: Event Rewards
            ScrambleTreasure(paramName, includeEventTreasure, 60001000, 60050000, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Things Betwixt
            ScrambleTreasure(paramName, true, 10025010, 10027000, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Majula
            ScrambleTreasure(paramName, true, 10045000, 10046150, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Forest of Fallen Giants
            ScrambleTreasure(paramName, true, 10105010, 10106630, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Brightstone Cove Tseldora
            ScrambleTreasure(paramName, true, 10145050, 10146520, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Aldia's Keep
            ScrambleTreasure(paramName, true, 10155000, 10156200, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Lost Bastille
            ScrambleTreasure(paramName, true, 10165000, 10166490, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Earthen Peak
            ScrambleTreasure(paramName, true, 10175020, 10176630, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: No-man's Wharf
            ScrambleTreasure(paramName, true, 10185000, 10186170, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Iron Keep
            ScrambleTreasure(paramName, true, 10195000, 10196220, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Huntman's Copse
            ScrambleTreasure(paramName, true, 10235010, 10236270, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: The Gutter
            ScrambleTreasure(paramName, true, 10255010, 10256500, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Dragon Aerie
            ScrambleTreasure(paramName, true, 10275000, 10276190, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Path to Shaded Woods
            ScrambleTreasure(paramName, true, 10295000, 10296020, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Unseen Path to Heide
            ScrambleTreasure(paramName, true, 10305010, 10306030, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Heide's Tower of Flame
            ScrambleTreasure(paramName, true, 10315000, 10316110, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Shaded Woods
            ScrambleTreasure(paramName, true, 10325000, 10326280, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Doors of Pharros
            ScrambleTreasure(paramName, true, 10335000, 10336080, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Grave of Saints
            ScrambleTreasure(paramName, true, 10345000, 10346110, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Giants' Memory
            ScrambleTreasure(paramName, true, 20105000, 20106150, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Shrine of Amana
            ScrambleTreasure(paramName, true, 20115000, 20116220, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Drangleic Castle
            ScrambleTreasure(paramName, true, 20215000, 20216140, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Undead Crypt
            ScrambleTreasure(paramName, true, 20245000, 20246500, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Dragon Memories
            ScrambleTreasure(paramName, true, 20265000, 20266000, randomiseWeapon, true, true, ignoreBossSouls, allowDuplicates);

            // Treasure: Chasm of the Abyss
            ScrambleTreasure(paramName, true, 40035000, 40036000, randomiseWeapon, true, true, ignoreBossSouls, allowDuplicates);

            // Treasure: Shulva
            ScrambleTreasure(paramName, true, 50355010, 50356670, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Brume Tower
            ScrambleTreasure(paramName, true, 50365000, 50368090, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            // Treasure: Eleum Loyce
            ScrambleTreasure(paramName, true, 50375500, 50376770, randomiseWeapon, ignoreKeys, ignoreTools, ignoreBossSouls, allowDuplicates);

            return regulation;
        }

        public bool ScrambleTreasure(string paramName, bool condition, int start, int end, bool randomiseWeapon, bool ignoreKeys, bool ignoreTools, bool ignoreBossSouls, bool allowDuplicates, List<int> ignore_list = null)
        {
            if (condition)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name == paramName)
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = param.Rows.Where(row => row.ID >= start && row.ID <= end).ToList();

                        if (ignore_list != null)
                        {
                            param_rows = param_rows.Where(row => !ignore_list.Contains(row.ID)).ToList();
                        }

                        RandomiseItemLot(param_rows, ignoreKeys, ignoreTools, ignoreBossSouls, randomiseWeapon, allowDuplicates);
                    }
                }
            }

            return true;
        }

        public void RandomiseItemLot(List<PARAM.Row> param_rows, bool ignoreKeys, bool ignoreTools, bool ignoreBossSouls, bool randomiseWeapon, bool allowDuplicates)
        {
            foreach (PARAM.Row row in param_rows)
            {
                bool editRow = true;

                // Skip row if the ID matches one of these lists
                if (ignoreKeys && HasMatchingItemLot(row, Treasure_Keys))
                    editRow = false;

                if (ignoreTools && HasMatchingItemLot(row, Treasure_Tools))
                    editRow = false;

                if (ignoreBossSouls && HasMatchingItemLot(row, Treasure_Boss_Souls))
                    editRow = false;

                if (editRow)
                {
                    PARAM.Row item = MakeItemLot(row);

                    int roll = rand.Next(100);

                    // Weapon
                    if (Valid_Weapons.Count > 0 && roll >= 0 && roll < 10)
                    {
                        int value = rand.Next(Valid_Weapons.Count);

                        item["item_lot_0"].Value = Valid_Weapons[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        if(randomiseWeapon)
                        {
                            item["reinforcement_lot_0"].Value = rand.Next(9);
                            item["infusion_lot_0"].Value = rand.Next(9);
                        }

                        if (!allowDuplicates)
                            Valid_Weapons.Remove(Valid_Weapons[value]);
                    }
                    else if (Valid_Weapons.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Armor
                    if (Valid_Armor.Count > 0 && roll >= 10 && roll < 20)
                    {
                        int value = rand.Next(Valid_Armor.Count);

                        item["item_lot_0"].Value = Valid_Armor[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        if (!allowDuplicates)
                            Valid_Armor.Remove(Valid_Armor[value]);

                        int bonusRoll = rand.Next(100);

                        if(bonusRoll >= 30)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_1"].Value = Valid_Armor[value].ID;
                            item["amount_lot_1"].Value = 1;
                            item["chance_lot_1"].Value = 1;

                            if (!allowDuplicates)
                                Valid_Armor.Remove(Valid_Armor[value]);
                        }

                        if (bonusRoll >= 60)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_2"].Value = Valid_Armor[value].ID;
                            item["amount_lot_2"].Value = 1;
                            item["chance_lot_2"].Value = 1;

                            if (!allowDuplicates)
                                Valid_Armor.Remove(Valid_Armor[value]);
                        }

                        if (bonusRoll >= 90)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_3"].Value = Valid_Armor[value].ID;
                            item["amount_lot_3"].Value = 1;
                            item["chance_lot_3"].Value = 1;

                            if (!allowDuplicates)
                                Valid_Armor.Remove(Valid_Armor[value]);
                        }
                    }
                    else if (Valid_Armor.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Spell
                    if (Valid_Spells.Count > 0 && roll >= 20 && roll < 30)
                    {
                        int value = rand.Next(Valid_Spells.Count);

                        item["item_lot_0"].Value = Valid_Spells[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        if (!allowDuplicates)
                            Valid_Spells.Remove(Valid_Spells[value]);

                    }
                    else if (Valid_Spells.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Ring
                    if (Valid_Rings.Count > 0 && roll >= 30 && roll < 40)
                    {
                        int value = rand.Next(Valid_Rings.Count);

                        item["item_lot_0"].Value = Valid_Rings[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        if (!allowDuplicates)
                            Valid_Rings.Remove(Valid_Rings[value]);
                    }
                    else if (Valid_Rings.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Key
                    if (!ignoreKeys && Valid_Key_Items.Count > 0 && roll >= 40 && roll < 45)
                    {
                        int value = rand.Next(Valid_Key_Items.Count);

                        item["item_lot_0"].Value = Valid_Key_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Key_Items.Remove(Valid_Key_Items[value]);
                    }
                    else if (!ignoreKeys && Valid_Key_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if(ignoreKeys && roll >= 40 && roll < 45)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Tool
                    if (!ignoreTools && Valid_Tool_Items.Count > 0 && roll >= 45 && roll < 50)
                    {
                        int value = rand.Next(Valid_Tool_Items.Count);

                        item["item_lot_0"].Value = Valid_Tool_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Tool_Items.Remove(Valid_Tool_Items[value]);
                    }
                    else if (!ignoreTools && Valid_Tool_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (ignoreTools && roll >= 45 && roll < 50)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Boss Soul
                    if (!ignoreBossSouls && Valid_Boss_Soul_Items.Count > 0 && roll >= 50 && roll < 55)
                    {
                        int value = rand.Next(Valid_Boss_Soul_Items.Count);

                        item["item_lot_0"].Value = Valid_Boss_Soul_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Boss_Soul_Items.Remove(Valid_Boss_Soul_Items[value]);
                    }
                    else if (!ignoreBossSouls && Valid_Boss_Soul_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (ignoreBossSouls && roll >= 50 && roll < 55)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Material
                    if (roll >= 55 && roll < 60)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Ammo
                    if (roll >= 60 && roll < 70)
                    {
                        int value = rand.Next(Valid_Ammunition.Count);

                        item["item_lot_0"].Value = Valid_Ammunition[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5, 25);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Consumables
                    if (roll >= 70)
                    {
                        int value = rand.Next(Valid_Consumable_Items.Count);

                        item["item_lot_0"].Value = Valid_Consumable_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(1, 10);
                        item["chance_lot_0"].Value = 1;
                    }
                }
            }
        }

        // Cleans the itemlot row 
        public PARAM.Row MakeItemLot(PARAM.Row itemlot)
        {
            itemlot["Unk00"].Value = 3;
            itemlot["Unk01"].Value = 1;
            itemlot["Unk02"].Value = 0;
            itemlot["Unk03"].Value = 0;

            for (int x = 0; x < 9; x++)
            {
                itemlot[$"amount_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 9; x++)
            {
                itemlot[$"reinforcement_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 9; x++)
            {
                itemlot[$"infusion_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 9; x++)
            {
                itemlot[$"unk_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 9; x++)
            {
                itemlot[$"item_lot_{x}"].Value = 10;
            }
            for (int x = 0; x < 9; x++)
            {
                itemlot[$"chance_lot_{x}"].Value = 0;
            }

            return itemlot;
        }

        // Check if the row matches a member of the list
        public bool HasMatchingItemLot(PARAM.Row itemlot, List<string> list)
        {
            bool match = false;

            for (int x = 0; x < 9; x++)
            {
                if (list.Contains(itemlot[$"item_lot_{x}"].Value.ToString()))
                    match = true;
            }

            return match;
        }
        #endregion

        #region Scramble - Param
        public Regulation ScrambleParam(string paramName, bool useGenerateType)
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
                    else if(useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

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
                        param_rows = GetRowsFromSubMatch(param_rows, BossIDs, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = GetRowsFromSubMatch(param_rows, BossIDs, 2, 4, "1", true);
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
                        param_rows = GetRowsFromSubMatch(param.Rows, BossIDs, 2, 4, "1");
                    }
                    else
                    {
                        param_rows = GetRowsFromSubMatch(param.Rows, BossIDs, 2, 4, "1", true);
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
                        param_rows = GetRowsFromSubMatch(param_rows, BossIDs, 2, 2, "");
                    }
                    else
                    {
                        param_rows = GetRowsFromSubMatch(param_rows, BossIDs, 2, 2, "", true);
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

        #region Scramble - EnemyGeneratorRegist
        // Not used
        public Regulation Scramble_EnemyGeneratorRegist(bool sharedEnemyPool, bool ignoreKeyCharacters, bool ignoreBosses)
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

        #region Scramble - EnemyGeneratorLocation
        public Regulation Scramble_EnemyGeneratorLocation(bool useOrderedPlacement, bool ignoreKeyCharacters, bool ignoreBosses, bool ignoreNGPlus)
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

                    if(ignoreNGPlus)
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

        public List<PARAM.Row> GetRowsFromMatch(List<PARAM.Row> rows, List<string> list, bool invertMatch = false)
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

        // Used to get matches rows based on a substring match
        // For example, matches 3160 is a boss, 1316000 is a row that should be matched
        public List<PARAM.Row> GetRowsFromSubMatch(List<PARAM.Row> rows, List<string> list, int targetAdjust, int rowAdjust, string appendString, bool invertMatch = false)
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

                if(addRow)
                    new_rows.Add(row);
            }

            return new_rows;
        }

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
