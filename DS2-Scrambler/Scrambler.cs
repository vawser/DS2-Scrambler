
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

        #region Scramble - Treasure - Map Loot

        public bool T_Ignore_Keys = false;
        public bool T_Ignore_Tools = false;
        public bool T_Ignore_Boss_Souls = false;

        // These itemlots should not be used in the placement of keys/tools
        public List<int> T_Forbidden_Item_Lots = new List<int>
        {
            10026100, 10045010, 10045040, 10045050, 10045020, 10045030, 10046100, 10105120, 10105050, 10105150, 10105080, 10105090, 10105110, 10106010, 10106070, 10106080, 10106120, 10106290, 10106370, 10106420, 10106430, 10106510, 10106600, 10106610, 10106620, 10106630, 10106350, 10106360, 10106460, 10106470, 10106480, 10145070, 10145080, 10155010, 10155020, 10155000, 10155030, 10156000, 10156010, 10156030, 10156040, 10156050, 10156130, 10156140, 10156150, 10156200, 10156100, 10156110, 10156070, 10156060, 10156160, 10165010, 10165110, 10165150, 10165000, 10165070, 10165020, 10166050, 10166350, 10166330, 10166440, 10175130, 10185120, 10196140, 10195100, 10195110, 10256000, 10256360, 10305010, 10306030, 10325100, 10326220, 10326260, 10335000, 10335020, 10335040, 10336020, 10346100, 20105030, 20105040, 20115070, 20115110, 20115090, 20116200, 20215050, 20215130, 20215160, 20216000, 20216010, 20216020, 50355150, 50355180, 50355350, 50356150, 50356210, 50356380, 50356400, 50356430, 50356460, 50356470, 50356480, 50356490, 50356500, 50356520, 50356530, 50356540, 50356610, 50356620, 50365550, 50365560, 50365680, 50365690, 50365020, 50365080, 50366020, 50366210, 50366240, 50367130, 50366880, 50366890, 50366250, 50366710, 50366720, 50366740, 50366760, 50366830, 50366850, 50366860, 50366870, 50366910, 50366920, 50366930, 50366940, 50366950, 50366960, 50366970, 50366980, 50366990, 50367000, 50367010, 50367020, 50367030, 50367040, 50367050, 50367060, 50367090, 50367100, 50367110, 50367120, 50375740, 50376060, 50376210, 50376220, 50376230, 50376300, 50376450, 50376460, 50376470, 50376570, 50376580, 50376710, 50376730, 50376740
        };

        // These items (and their associated itemlots) should not be changed.
        public List<int> T_Fixed_Items = new List<int>
        {
            60155000, 60155010, 60155020, 60155030, 62190000, 60360000
        };

        // These rows should be skipped when 
        public List<int> T_Skipped_Item_Lots = new List<int>
        {
            514500, 1705100, 1705300, 1753010, 1757000, 1758000, 1759000, 1777000, 1786000
        };

        // Bird range
        public List<int> T_Bird_Item_Lots = new List<int>
        {
            50000000, 50000001, 50000002, 50000003, 50000100, 50000101, 50000102, 50000103, 50000200, 50000201, 50000202, 50000203, 50000300, 50000301, 50000302, 50000303, 50001000,
        };

        public Regulation Scramble_Treasure(string paramName, bool ignoreKeys, bool ignoreTools, bool ignoreBossSouls, bool includeBossTreasure, bool includeCharacterTreasure, bool includeCovenantTreasure, bool includeBirdTreasure, bool includeEventTreasure)
        {
            T_Ignore_Keys = ignoreKeys;
            T_Ignore_Tools = ignoreTools;
            T_Ignore_Boss_Souls = ignoreBossSouls;

            // Treasure: Boss Rewards
            ScrambleTreasure(paramName, includeBossTreasure, 106000, 862001);

            // Treasure: Character Treasure
            ScrambleTreasure(paramName, includeCharacterTreasure, 1307000, 1788030);

            // Treasure: Covenant Rewards
            ScrambleTreasure(paramName, includeCovenantTreasure, 2001000, 2009013);

            // Treasure: Bird Rewards
            ScrambleTreasure(paramName, includeBirdTreasure, 50000000, 50000303);

            // Treasure: Event Rewards
            ScrambleTreasure(paramName, includeEventTreasure, 60001000, 60050000);

            // Treasure: Vanilla
            ScrambleTreasure(paramName, true, 10025010, 40036000);

            // Treasure: DLC
            ScrambleTreasure(paramName, true, 50355010, 50376770);

            // Lot Ranges
            List<PARAM.Row> LotRange_Things_Betwixt = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Majula = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Forest_Of_Fallen_Giants = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Brightstone_Cove_Tseldora = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Aldias_Keep = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Lost_Bastille = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Earthen_Peak = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_No_Mans_Wharf = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Iron_Keep = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Huntmans_Copse = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Gutter = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Dragon_Aerie = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Path_to_Shaded_Woods = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Unseen_Path_to_Heide = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Heide_Tower_of_Flame = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Shaded_Woods = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Doors_of_Pharros = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Grave_of_Saints = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Giants_Memory = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Shrine_of_Amana = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Drangleic_Castle = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Undead_Crypt = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Dragon_Memories = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Chasm_of_the_Abyss = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Shulva = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Brume_Tower = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_Eleum_Loyce = new List<PARAM.Row>();

            List<PARAM.Row> LotRange_General = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_SoldierKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_KeyToKingsPassage = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_BastilleKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_IronKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_ForgottenKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_BrightstoneKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_AntiquatedKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_FangKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_HouseKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_LenigrastsKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_RotundaLockstone = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_GiantsKinship = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_AshenMistHeart = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_TseldoraDenKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_UndeadLockawayKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_AldiaKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_DragonTalon = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_HeavyIronKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_FrozenFlower = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_EternalSanctumKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_TowerKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_GarrisonWardKey = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_DragonStone = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_ScorchingIronScepter = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_EyeOfThePriestess = new List<PARAM.Row>();
            List<PARAM.Row> LotRange_DullEmber = new List<PARAM.Row>();

            // Build the row lot ranges for each Key item
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if(wrapper.Name == paramName)
                {
                    foreach(PARAM.Row row in wrapper.Rows)
                    {
                        // Exclude the forbidden rows
                        if (!T_Forbidden_Item_Lots.Contains(row.ID) && !T_Skipped_Item_Lots.Contains(row.ID) && !T_Bird_Item_Lots.Contains(row.ID))
                        {
                            if(row.ID > 10025010 && row.ID <= 50376770)
                                LotRange_General.Add(row);

                            // Build each lot range, assigning the maps that the item should be allowed to be scrambled into.

                            // Things Betwixt
                            if (row.ID >= 10025010 && row.ID <= 10027000)
                            {
                                LotRange_Things_Betwixt.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                            }
                            // Majula
                            if (row.ID >= 10045000 && row.ID <= 10046150)
                            {
                                LotRange_Majula.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                            }
                            // Forest of Fallen Giants
                            if (row.ID >= 10105010 && row.ID <= 10106630)
                            {
                                LotRange_Forest_Of_Fallen_Giants.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_AntiquatedKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                                LotRange_BastilleKey.Add(row);
                                LotRange_SoldierKey.Add(row);
                            }
                            // Brightstone Cove Tseldora
                            if (row.ID >= 10145050 && row.ID <= 10146520)
                            {
                                LotRange_Brightstone_Cove_Tseldora.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_TseldoraDenKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_FangKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                                LotRange_IronKey.Add(row);
                            }
                            // Aldia's Keep
                            if (row.ID >= 10155000 && row.ID <= 10156200)
                            {
                                LotRange_Aldias_Keep.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Lost Bastille
                            if (row.ID >= 10165000 && row.ID <= 10166490)
                            {
                                LotRange_Lost_Bastille.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_AntiquatedKey.Add(row);
                                LotRange_BastilleKey.Add(row);
                                LotRange_SoldierKey.Add(row);
                            }
                            // Earthen Peak
                            if (row.ID >= 10175020 && row.ID <= 10176630)
                            {
                                LotRange_Earthen_Peak.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_IronKey.Add(row);
                            }
                            // No Man's Wharf
                            if (row.ID >= 10185000 && row.ID <= 10186170)
                            {
                                LotRange_No_Mans_Wharf.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_AntiquatedKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                                LotRange_BastilleKey.Add(row);
                                LotRange_SoldierKey.Add(row);
                            }
                            // Iron Keep
                            if (row.ID >= 10195000 && row.ID <= 10196220)
                            {
                                LotRange_Iron_Keep.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_ForgottenKey.Add(row);
                                LotRange_IronKey.Add(row);
                                LotRange_KeyToKingsPassage.Add(row);
                            }
                            // Huntman's Copse
                            if (row.ID >= 10235010 && row.ID <= 10236270)
                            {
                                LotRange_Huntmans_Copse.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_UndeadLockawayKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                            }
                            // Gutter
                            if (row.ID >= 10255010 && row.ID <= 10256500)
                            {
                                LotRange_Gutter.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_ForgottenKey.Add(row);
                                LotRange_IronKey.Add(row);
                            }
                            // Dragon Aerie
                            if (row.ID >= 10275000 && row.ID <= 10276190)
                            {
                                LotRange_Dragon_Aerie.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_KeyToKingsPassage.Add(row);
                            }
                            // Path to the Shaded Woods
                            if (row.ID >= 10295000 && row.ID <= 10296020)
                            {
                                LotRange_Path_to_Shaded_Woods.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_UndeadLockawayKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_FangKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Unseen Path to Heide
                            if (row.ID >= 10305010 && row.ID <= 10306030)
                            {
                                LotRange_Unseen_Path_to_Heide.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Heide Tower of Flame
                            if (row.ID >= 10315000 && row.ID <= 10316110)
                            {
                                LotRange_Heide_Tower_of_Flame.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_LenigrastsKey.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_AntiquatedKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Shaded Woods
                            if (row.ID >= 10325000 && row.ID <= 10326280)
                            {
                                LotRange_Shaded_Woods.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_UndeadLockawayKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_FangKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Doors of Pharros
                            if (row.ID >= 10335000 && row.ID <= 10336080)
                            {
                                LotRange_Doors_of_Pharros.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_TseldoraDenKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_FangKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                            }
                            // Grave of Saints
                            if (row.ID >= 10345000 && row.ID <= 10346110)
                            {
                                LotRange_Grave_of_Saints.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_DullEmber.Add(row);
                                LotRange_AldiaKey.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_RotundaLockstone.Add(row);
                                LotRange_HouseKey.Add(row);
                                LotRange_BrightstoneKey.Add(row);
                                LotRange_ForgottenKey.Add(row);
                            }
                            // Giant's Memory
                            if (row.ID >= 20105000 && row.ID <= 20106150)
                            {
                                LotRange_Giants_Memory.Add(row);
                            }
                            // Shrine of Amana
                            if (row.ID >= 20115000 && row.ID <= 20116220)
                            {
                                LotRange_Shrine_of_Amana.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                            }
                            // Drangleic Castle
                            if (row.ID >= 20215000 && row.ID <= 20216140)
                            {
                                LotRange_Drangleic_Castle.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                                LotRange_KeyToKingsPassage.Add(row);
                            }
                            // Undead Crypt
                            if (row.ID >= 20245000 && row.ID <= 20246500)
                            {
                                LotRange_Undead_Crypt.Add(row);

                                LotRange_DragonTalon.Add(row);
                                LotRange_HeavyIronKey.Add(row);
                                LotRange_FrozenFlower.Add(row);
                                LotRange_AshenMistHeart.Add(row);
                                LotRange_GiantsKinship.Add(row);
                            }
                            // Dragon Memories
                            if (row.ID >= 20265000 && row.ID <= 20266000)
                            {
                                LotRange_Dragon_Memories.Add(row);
                            }
                            // Chasm of the Abyss
                            if (row.ID >= 40035000 && row.ID <= 40036000)
                            {
                                LotRange_Chasm_of_the_Abyss.Add(row);
                            }
                            // Shulva
                            if (row.ID >= 50355010 && row.ID <= 50356670)
                            {
                                LotRange_Shulva.Add(row);

                                LotRange_EternalSanctumKey.Add(row);
                                LotRange_DragonStone.Add(row);
                            }
                            // Brume Tower
                            if (row.ID >= 50365000 && row.ID <= 50368090)
                            {
                                LotRange_Brume_Tower.Add(row);

                                LotRange_ScorchingIronScepter.Add(row);
                                LotRange_TowerKey.Add(row);
                            }
                            // Eleum Loyce
                            if (row.ID >= 50375500 && row.ID <= 50376770)
                            {
                                LotRange_Eleum_Loyce.Add(row);

                                LotRange_EyeOfThePriestess.Add(row);
                                LotRange_GarrisonWardKey.Add(row);
                            }
                        }
                    }
                }
            }

            // If any Key items were not assinged during the random scramble, forcefully add them in.
            if (!T_Ignore_Keys)
            {
                if (Valid_Key_Items.Count > 0)
                {
                    Console.WriteLine("Forcefully adding Key Items");
                    foreach (PARAM.Row row in Valid_Key_Items)
                    {
                        // Soldier Key
                        if (row.ID == 50600000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_SoldierKey, row);
                        }
                        // Key to King's Passage
                        if (row.ID == 50610000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_KeyToKingsPassage, row);
                        }
                        // Bastille Key
                        if (row.ID == 50800000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_BastilleKey, row);
                        }
                        // Iron Key
                        if (row.ID == 50810000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_IronKey, row);
                        }
                        // Forgotten Key
                        if (row.ID == 50820000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_ForgottenKey, row);
                        }
                        // Brightstone Key
                        if (row.ID == 50830000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_BrightstoneKey, row);
                        }
                        // Antiquated Key
                        if (row.ID == 50840000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_AntiquatedKey, row);
                        }
                        // Fang Key
                        if (row.ID == 50850000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_FangKey, row);
                        }
                        // House Key
                        if (row.ID == 50860000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_HouseKey, row);
                        }
                        // Lenigrast's Key
                        if (row.ID == 50870000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_LenigrastsKey, row);
                        }
                        // Rotunda Lockstone
                        if (row.ID == 50890000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_RotundaLockstone, row);
                        }
                        // Giant's Kinship
                        if (row.ID == 50900000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_GiantsKinship, row);
                        }
                        // Ashen Mist Heart
                        if (row.ID == 50910000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_AshenMistHeart, row);
                        }
                        // Tseldora Den Key
                        if (row.ID == 50930000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_TseldoraDenKey, row);
                        }
                        // Undead Lockaway Key
                        if (row.ID == 50970000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_UndeadLockawayKey, row);
                        }
                        // Aldia Key
                        if (row.ID == 51030000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_AldiaKey, row);
                        }
                        // Dragon Talon
                        if (row.ID == 52000000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_DragonTalon, row);
                        }
                        // Heavy Iron Key
                        if (row.ID == 52100000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_HeavyIronKey, row);
                        }
                        // Frozen Flower
                        if (row.ID == 52200000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_FrozenFlower, row);
                        }
                        // Eternal Sanctum Key
                        if (row.ID == 52300000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_EternalSanctumKey, row);
                        }
                        // Tower Key
                        if (row.ID == 52400000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_TowerKey, row);
                        }
                        // Garrison Ward Key
                        if (row.ID == 52500000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_GarrisonWardKey, row);
                        }
                        // Dragon Stone
                        if (row.ID == 52650000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_DragonStone, row);
                        }
                        // Scorching Iron Scepter
                        if (row.ID == 53100000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_ScorchingIronScepter, row);
                        }
                        // Eye of the Priestess
                        if (row.ID == 53600000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_EyeOfThePriestess, row);
                        }
                        // Dull Ember
                        if (row.ID == 50990000)
                        {
                            ScrambleTreasure_ForItem(paramName, LotRange_DullEmber, row);
                        }
                    }
                }
            }

            // If any Tool items were not assinged during the random scramble, forcefully add them in.
            if (!T_Ignore_Tools)
            {
                if (Valid_Tool_Items.Count > 0)
                {
                    Console.WriteLine("Forcefully adding Tool Items");
                    foreach (PARAM.Row row in Valid_Tool_Items)
                    {
                        ScrambleTreasure_ForItem(paramName, LotRange_General, row);
                    }
                }
            }

            // If any Boss Soul items were not assinged during the random scramble, forcefully add them in.
            if (!T_Ignore_Boss_Souls)
            {
                if (Valid_Boss_Soul_Items.Count > 0)
                {
                    Console.WriteLine("Forcefully adding Boss Soul Items");
                    foreach (PARAM.Row row in Valid_Boss_Soul_Items)
                    {
                        ScrambleTreasure_ForItem(paramName, LotRange_General, row);
                    }
                }
            }

            // Estus Flask Shard
            EnsureItemAvailability(paramName, LotRange_General, 60525000, 11);

            // Sublime Bone Dust
            EnsureItemAvailability(paramName, LotRange_General, 60526000, 5);

            // Pharros' Lockstone
            EnsureItemAvailability(paramName, LotRange_Things_Betwixt, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Majula, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Forest_Of_Fallen_Giants, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Brightstone_Cove_Tseldora, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Aldias_Keep, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Lost_Bastille, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Earthen_Peak, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_No_Mans_Wharf, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Iron_Keep, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Huntmans_Copse, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Gutter, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Dragon_Aerie, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Heide_Tower_of_Flame, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Shaded_Woods, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Doors_of_Pharros, 60536000, 3);
            EnsureItemAvailability(paramName, LotRange_Grave_of_Saints, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Shrine_of_Amana, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Drangleic_Castle, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Undead_Crypt, 60536000, 1);
            EnsureItemAvailability(paramName, LotRange_Shulva, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Brume_Tower, 60536000, 2);
            EnsureItemAvailability(paramName, LotRange_Eleum_Loyce, 60536000, 2);

            // Fragrant Branch of Yore
            EnsureItemAvailability(paramName, LotRange_Things_Betwixt, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Majula, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Forest_Of_Fallen_Giants, 60537000, 2);
            EnsureItemAvailability(paramName, LotRange_Brightstone_Cove_Tseldora, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Aldias_Keep, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Lost_Bastille, 60537000, 2);
            EnsureItemAvailability(paramName, LotRange_Earthen_Peak, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_No_Mans_Wharf, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Iron_Keep, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Huntmans_Copse, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Gutter, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Dragon_Aerie, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Heide_Tower_of_Flame, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Shaded_Woods, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Doors_of_Pharros, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Grave_of_Saints, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Shrine_of_Amana, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Drangleic_Castle, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Undead_Crypt, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Shulva, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Brume_Tower, 60537000, 1);
            EnsureItemAvailability(paramName, LotRange_Eleum_Loyce, 60537000, 1);

            // Smelter Wedge
            EnsureItemAvailability(paramName, LotRange_Brume_Tower, 53200000, 10);

            return regulation;
        }

        public void EnsureItemAvailability(string paramName, List<PARAM.Row> list, int target_id, int target_amount)
        {
            int counter = 0;

            Console.WriteLine($"Ensuring {target_id} is available");

            while (counter < target_amount)
            {
                Console.WriteLine($"{target_id}");

                int count = 0;
                foreach (PARAM.Row row in list)
                {
                    int item_id = (int)row["item_lot_0"].Value;

                    if (item_id == target_id)
                        count = count + 1;
                }

                if (counter < target_amount)
                {
                    Console.WriteLine($"{counter}");
                    ScrambleTreasure_ForItem(paramName, list, ItemParam.Rows.Find(row => row.ID == target_id));
                }

                counter = count;
            }

            Console.WriteLine($"Finished ensuring {target_id} is available");
        }

        public bool ScrambleTreasure_ForItem(string paramName, List<PARAM.Row> list, PARAM.Row new_item)
        {
            bool assigned = false;

            int loop = 0;

            // Iterate until it can be assigned
            while (!assigned || loop <= 1000)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name == paramName)
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = list;

                        PARAM.Row row = param_rows[rand.Next(param_rows.Count)];

                        int item_id = (int)row["item_lot_0"].Value;

                        bool replacableRow = false;

                        foreach (PARAM.Row cRow in Valid_Consumable_Items)
                        {
                            if (cRow.ID == item_id)
                                replacableRow = true;
                        }
                        foreach (PARAM.Row mRow in Valid_Material_Items)
                        {
                            if (mRow.ID == item_id)
                                replacableRow = true;
                        }

                        // This row can be replaced
                        if (replacableRow)
                        {
                            PARAM.Row item = MakeItemLot(row);

                            item["item_lot_0"].Value = new_item.ID;
                            item["amount_lot_0"].Value = 1;
                            item["chance_lot_0"].Value = 1;

                            assigned = true;
                        }
                    }

                    loop = loop + 1;
                }
            }

            return true;
        }

        public bool ScrambleTreasure(string paramName, bool condition, int start, int end)
        {
            if (condition)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name == paramName)
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = param.Rows.Where(row => row.ID >= start && row.ID <= end && !T_Skipped_Item_Lots.Contains(row.ID)).ToList();

                        RandomiseItemLot(param_rows);
                    }
                }
            }

            return true;
        }

        public void RandomiseItemLot(List<PARAM.Row> param_rows)
        {
            foreach (PARAM.Row row in param_rows)
            {
                bool editRow = true;

                // Skip row if the ID matches one of these lists
                if(HasMatchingItemLot(row, T_Fixed_Items))
                    editRow = false;

                if (T_Ignore_Keys && HasMatchingItemLot(row, Treasure_Keys))
                    editRow = false;

                if (T_Ignore_Tools && HasMatchingItemLot(row, Treasure_Tools))
                    editRow = false;

                if (T_Ignore_Boss_Souls && HasMatchingItemLot(row, Treasure_Boss_Souls))
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

                        if (rand.Next(100) <= 15)
                        {
                            item["reinforcement_lot_0"].Value = rand.Next(9);
                            item["infusion_lot_0"].Value = rand.Next(9);
                        }
                        else
                        {
                            item["reinforcement_lot_0"].Value = 0;
                            item["infusion_lot_0"].Value = 0;
                        }

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

                        Valid_Armor.Remove(Valid_Armor[value]);

                        int bonusRoll = rand.Next(100);

                        if(bonusRoll >= 50)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_1"].Value = Valid_Armor[value].ID;
                            item["amount_lot_1"].Value = 1;
                            item["chance_lot_1"].Value = 1;

                            Valid_Armor.Remove(Valid_Armor[value]);
                        }

                        if (bonusRoll >= 80)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_2"].Value = Valid_Armor[value].ID;
                            item["amount_lot_2"].Value = 1;
                            item["chance_lot_2"].Value = 1;

                            Valid_Armor.Remove(Valid_Armor[value]);
                        }

                        if (bonusRoll >= 95)
                        {
                            value = rand.Next(Valid_Armor.Count);

                            item["item_lot_3"].Value = Valid_Armor[value].ID;
                            item["amount_lot_3"].Value = 1;
                            item["chance_lot_3"].Value = 1;

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

                        Valid_Rings.Remove(Valid_Rings[value]);
                    }
                    else if (Valid_Rings.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Key
                    // Do these forcefully always
                    /*
                    if (!T_Ignore_Keys && Valid_Key_Items.Count > 0 && roll >= 40 && roll < 45 && !HasMatchingItemLot(row, T_Forbidden_Item_Lots))
                    {
                        int value = rand.Next(Valid_Key_Items.Count);

                        item["item_lot_0"].Value = Valid_Key_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Key_Items.Remove(Valid_Key_Items[value]);
                    }
                    else if (!T_Ignore_Keys && (Valid_Key_Items.Count == 0 || HasMatchingItemLot(row, T_Forbidden_Item_Lots)))
                    {
                        roll = roll + 5;
                    }
                    else if(T_Ignore_Keys && roll >= 40 && roll < 45 && !HasMatchingItemLot(row, T_Forbidden_Item_Lots))
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }
                    */

                    // Tool
                    if (!T_Ignore_Tools && Valid_Tool_Items.Count > 0 && roll >= 40 && roll < 42)
                    {
                        int value = rand.Next(Valid_Tool_Items.Count);

                        item["item_lot_0"].Value = Valid_Tool_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Tool_Items.Remove(Valid_Tool_Items[value]);
                    }
                    else if (!T_Ignore_Tools && Valid_Tool_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (T_Ignore_Tools && roll >= 40 && roll < 42)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Boss Soul
                    if (!T_Ignore_Boss_Souls && Valid_Boss_Soul_Items.Count > 0 && roll >= 42 && roll < 46)
                    {
                        int value = rand.Next(Valid_Boss_Soul_Items.Count);

                        item["item_lot_0"].Value = Valid_Boss_Soul_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Valid_Boss_Soul_Items.Remove(Valid_Boss_Soul_Items[value]);
                    }
                    else if (!T_Ignore_Boss_Souls && Valid_Boss_Soul_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (T_Ignore_Boss_Souls && roll >= 42 && roll < 46)
                    {
                        int value = rand.Next(Valid_Material_Items.Count);

                        item["item_lot_0"].Value = Valid_Material_Items[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Material
                    if (roll >= 46 && roll < 60)
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

        public bool HasMatchingItemLot(PARAM.Row itemlot, List<int> list)
        {
            bool match = false;

            for (int x = 0; x < 9; x++)
            {
                if (list.Contains((int)itemlot[$"item_lot_{x}"].Value))
                    match = true;
            }

            return match;
        }
        #endregion

        #region Scramble - Treasure - Enemy Drops

        public bool T_Maintain_Item_Type = false;

        public Regulation Scramble_Treasure_Enemies(string paramName, bool maintainItemType)
        {
            T_Maintain_Item_Type = maintainItemType;

            ScrambleEnemyDrop(paramName, 10000000, 89800000);

            return regulation;
        }
        public bool ScrambleEnemyDrop(string paramName,int start, int end)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows.Where(row => row.ID >= start && row.ID <= end).ToList();

                    RandomiseEnemyDrop(param_rows);
                }
            }

            return true;
        }

        public void RandomiseEnemyDrop(List<PARAM.Row> param_rows)
        {
            foreach (PARAM.Row row in param_rows)
            {
                bool editRow = true;

                if (editRow)
                {
                    PARAM.Row item = MakeItemLot(row);


                    if (T_Maintain_Item_Type)
                    {

                    }
                    else
                    {
                        int roll = rand.Next(100);

                        // Weapon
                        if (Valid_Weapons.Count > 0 && roll >= 0 && roll < 10)
                        {
                            int value = rand.Next(Valid_Weapons.Count);

                            item["item_lot_0"].Value = Valid_Weapons[value].ID;
                            item["amount_lot_0"].Value = 1;
                            item["chance_lot_0"].Value = 1;

                            if (rand.Next(100) <= 15)
                            {
                                item["reinforcement_lot_0"].Value = rand.Next(9);
                                item["infusion_lot_0"].Value = rand.Next(9);
                            }
                            else
                            {
                                item["reinforcement_lot_0"].Value = 0;
                                item["infusion_lot_0"].Value = 0;
                            }

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

                            Valid_Armor.Remove(Valid_Armor[value]);

                            int bonusRoll = rand.Next(100);

                            if (bonusRoll >= 50)
                            {
                                value = rand.Next(Valid_Armor.Count);

                                item["item_lot_1"].Value = Valid_Armor[value].ID;
                                item["amount_lot_1"].Value = 1;
                                item["chance_lot_1"].Value = 1;

                                Valid_Armor.Remove(Valid_Armor[value]);
                            }

                            if (bonusRoll >= 80)
                            {
                                value = rand.Next(Valid_Armor.Count);

                                item["item_lot_2"].Value = Valid_Armor[value].ID;
                                item["amount_lot_2"].Value = 1;
                                item["chance_lot_2"].Value = 1;

                                Valid_Armor.Remove(Valid_Armor[value]);
                            }

                            if (bonusRoll >= 95)
                            {
                                value = rand.Next(Valid_Armor.Count);

                                item["item_lot_3"].Value = Valid_Armor[value].ID;
                                item["amount_lot_3"].Value = 1;
                                item["chance_lot_3"].Value = 1;

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

                            Valid_Rings.Remove(Valid_Rings[value]);
                        }
                        else if (Valid_Rings.Count == 0)
                        {
                            roll = roll + 10;
                        }

                        // Material
                        if (roll >= 46 && roll < 60)
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
