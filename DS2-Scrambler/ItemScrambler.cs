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
    public class ItemScrambler
    {
        public Random rand;
        public Regulation regulation;

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

        public List<PARAM.Row> Unassigned_Weapons;
        public List<PARAM.Row> Unassigned_Armor;
        public List<PARAM.Row> Unassigned_Spells;
        public List<PARAM.Row> Unassigned_Rings;
        public List<PARAM.Row> Unassigned_Key_Items;
        public List<PARAM.Row> Unassigned_Tool_Items;
        public List<PARAM.Row> Unassigned_Boss_Soul_List;

        public List<PARAM.Row> Weapon_List;
        public List<PARAM.Row> Armor_List;
        public List<PARAM.Row> Spell_List;
        public List<PARAM.Row> Ring_List;
        public List<PARAM.Row> Item_List;
        public List<PARAM.Row> Ammo_List;
        public List<PARAM.Row> Consumable_Item_List;
        public List<PARAM.Row> Multiplayer_Item_List;
        public List<PARAM.Row> Gesture_List;
        public List<PARAM.Row> Material_Item_List;

        public string ItemScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Item-Scramble\\";

        public ItemScrambler(Random random, Regulation reg)
        {
            rand = random;
            regulation = reg;

            Excluded_Weapons = Util.BuildIDList(ItemScramblePath + "Treasure-Excluded-Weapons");
            Excluded_Armor = Util.BuildIDList(ItemScramblePath + "Treasure-Excluded-Armor");
            Excluded_Spells = Util.BuildIDList(ItemScramblePath + "Treasure-Excluded-Spells");
            Excluded_Rings = Util.BuildIDList(ItemScramblePath + "Treasure-Excluded-Rings");
            Excluded_Items = Util.BuildIDList(ItemScramblePath + "Treasure-Excluded-Items");

            Treasure_Keys = Util.BuildIDList(ItemScramblePath + "Treasure-Keys");
            Treasure_Tools = Util.BuildIDList(ItemScramblePath + "Treasure-Tools");
            Treasure_Ammo = Util.BuildIDList(ItemScramblePath + "Treasure-Ammo");
            Treasure_Gestures = Util.BuildIDList(ItemScramblePath + "Treasure-Gestures");
            Treasure_Consumables = Util.BuildIDList(ItemScramblePath + "Treasure-Consumables");
            Treasure_Boss_Souls = Util.BuildIDList(ItemScramblePath + "Treasure-Boss-Souls");
            Treasure_Multiplayer = Util.BuildIDList(ItemScramblePath + "Treasure-Multiplayer");
            Treasure_Materials = Util.BuildIDList(ItemScramblePath + "Treasure-Materials");

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

            Unassigned_Weapons = ItemParam.Param.Rows.Where(row => row.ID >= 1000000 && row.ID <= 19999999 && !Excluded_Weapons.Contains(row.ID.ToString())).ToList();
            Weapon_List = new List<PARAM.Row>(Unassigned_Weapons);
            Unassigned_Armor = ItemParam.Param.Rows.Where(row => row.ID >= 21010100 && row.ID <= 29999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();
            Armor_List = new List<PARAM.Row>(Unassigned_Armor);
            Unassigned_Spells = ItemParam.Param.Rows.Where(row => row.ID >= 31010000 && row.ID <= 39999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();
            Spell_List = new List<PARAM.Row>(Unassigned_Spells);
            Unassigned_Rings = ItemParam.Param.Rows.Where(row => row.ID >= 40010000 && row.ID <= 49999999 && !Excluded_Armor.Contains(row.ID.ToString())).ToList();
            Ring_List = new List<PARAM.Row>(Unassigned_Rings);
            Item_List = ItemParam.Param.Rows.Where(row => row.ID >= 50600000 && row.ID <= 64610000 && !Excluded_Items.Contains(row.ID.ToString())).ToList();
            Ammo_List = Item_List.Where(row => Treasure_Ammo.Contains(row.ID.ToString())).ToList();
            Unassigned_Key_Items = Item_List.Where(row => Treasure_Keys.Contains(row.ID.ToString())).ToList();
            Unassigned_Tool_Items = Item_List.Where(row => Treasure_Tools.Contains(row.ID.ToString())).ToList();
            Consumable_Item_List = Item_List.Where(row => Treasure_Consumables.Contains(row.ID.ToString())).ToList();
            Gesture_List = Item_List.Where(row => Treasure_Gestures.Contains(row.ID.ToString())).ToList();
            Unassigned_Boss_Soul_List = Item_List.Where(row => Treasure_Boss_Souls.Contains(row.ID.ToString())).ToList();
            Multiplayer_Item_List = Item_List.Where(row => Treasure_Multiplayer.Contains(row.ID.ToString())).ToList();
            Material_Item_List = Item_List.Where(row => Treasure_Materials.Contains(row.ID.ToString())).ToList();
        }

        public bool T_Ignore_Keys = false;
        public bool T_Ignore_Tools = false;
        public bool T_Ignore_Boss_Souls = false;
        public bool T_Maintain_Item_Type = false;

        public List<PARAM.Row> LotRange_Things_Betwixt = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Majula = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Forest_Of_Fallen_Giants = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Brightstone_Cove_Tseldora = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Aldias_Keep = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Lost_Bastille = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Earthen_Peak = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_No_Mans_Wharf = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Iron_Keep = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Huntmans_Copse = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Gutter = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Dragon_Aerie = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Path_to_Shaded_Woods = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Unseen_Path_to_Heide = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Heide_Tower_of_Flame = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Shaded_Woods = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Doors_of_Pharros = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Grave_of_Saints = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Giants_Memory = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Shrine_of_Amana = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Drangleic_Castle = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Undead_Crypt = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Dragon_Memories = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Chasm_of_the_Abyss = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Shulva = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Brume_Tower = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_Eleum_Loyce = new List<PARAM.Row>();

        public List<PARAM.Row> LotRange_General = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_SoldierKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_KeyToKingsPassage = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_BastilleKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_IronKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_ForgottenKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_BrightstoneKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_AntiquatedKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_FangKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_HouseKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_LenigrastsKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_RotundaLockstone = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_GiantsKinship = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_AshenMistHeart = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_TseldoraDenKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_UndeadLockawayKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_AldiaKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_DragonTalon = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_HeavyIronKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_FrozenFlower = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_EternalSanctumKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_TowerKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_GarrisonWardKey = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_DragonStone = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_ScorchingIronScepter = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_EyeOfThePriestess = new List<PARAM.Row>();
        public List<PARAM.Row> LotRange_DullEmber = new List<PARAM.Row>();

        // These itemlots should not be used for the placement of keys/tools
        public List<int> T_Forbidden_Item_Lots = new List<int>
        {
            10026100, 10045010, 10045040, 10045050, 10045020, 10045030, 10046100, 10105120, 10105050, 10105150, 10105080, 10105090, 10105110, 10106010, 10106070, 10106080, 10106120, 10106290, 10106370, 10106420, 10106430, 10106510, 10106600, 10106610, 10106620, 10106630, 10106350, 10106360, 10106460, 10106470, 10106480, 10145070, 10145080, 10155010, 10155020, 10155000, 10155030, 10156000, 10156010, 10156030, 10156040, 10156050, 10156130, 10156140, 10156150, 10156200, 10156100, 10156110, 10156070, 10156060, 10156160, 10165010, 10165110, 10165150, 10165000, 10165070, 10165020, 10166050, 10166350, 10166330, 10166440, 10175130, 10185120, 10196140, 10195100, 10195110, 10256000, 10256360, 10305010, 10306030, 10325100, 10326220, 10326260, 10335000, 10335020, 10335040, 10336020, 10346100, 20105030, 20105040, 20115070, 20115110, 20115090, 20116200, 20215050, 20215130, 20215160, 20216000, 20216010, 20216020, 50355150, 50355180, 50355350, 50356150, 50356210, 50356380, 50356400, 50356430, 50356460, 50356470, 50356480, 50356490, 50356500, 50356520, 50356530, 50356540, 50356610, 50356620, 50365550, 50365560, 50365680, 50365690, 50365020, 50365080, 50366020, 50366210, 50366240, 50367130, 50366880, 50366890, 50366250, 50366710, 50366720, 50366740, 50366760, 50366830, 50366850, 50366860, 50366870, 50366910, 50366920, 50366930, 50366940, 50366950, 50366960, 50366970, 50366980, 50366990, 50367000, 50367010, 50367020, 50367030, 50367040, 50367050, 50367060, 50367090, 50367100, 50367110, 50367120, 50375740, 50376060, 50376210, 50376220, 50376230, 50376300, 50376450, 50376460, 50376470, 50376570, 50376580, 50376710, 50376730, 50376740
        };

        // These items should not be changed by the scrambler
        public List<int> T_Fixed_Items = new List<int>
        {
            60155000, 60155010, 60155020, 60155030, 62190000, // Estus Flask
            60360000, // Darksign
            53600000 // Eye of the Priestess
        };

        // These rows should be skipped as they are "unknown" 
        public List<int> T_Skipped_Item_Lots = new List<int>
        {
            514500, 1705100, 1705300, 1753010, 1757000, 1758000, 1759000, 1777000, 1786000
        };

        // Bird range
        public List<int> T_Bird_Item_Lots = new List<int>
        {
            50000000, 50000001, 50000002, 50000003, 50000100, 50000101, 50000102, 50000103, 50000200, 50000201, 50000202, 50000203, 50000300, 50000301, 50000302, 50000303, 50001000,
        };

        public int LOOP_LIMIT = 10000;

        #region Loot
        public Regulation Scramble_Loot(bool scrambleMapLoot, bool scrambleEnemyDrops, bool includeBossTreasure, bool includeCharacterTreasure, bool includeCovenantTreasure, bool includeBirdTreasure, bool includeEventTreasure, bool ignoreKeys, bool ignoreTools, bool ignoreBossSouls)
        {
            Console.WriteLine($"Map Loot Randomisation");

            string paramName = "ItemLotParam2_Other";

            T_Ignore_Keys = ignoreKeys;
            T_Ignore_Tools = ignoreTools;
            T_Ignore_Boss_Souls = ignoreBossSouls;

            BuildLotRanges();

            GetListCounts();

            #region Map Loot Randomisation
            if (scrambleMapLoot)
            {
                Console.WriteLine($"Scramble - Boss Rewards");
                ScrambleTreasure(paramName, includeBossTreasure, 106000, 862001);

                Console.WriteLine($"Scramble - Character Rewards");
                ScrambleTreasure(paramName, includeCharacterTreasure, 1307000, 1788030);

                Console.WriteLine($"Scramble - Covenant Rewards");
                ScrambleTreasure(paramName, includeCovenantTreasure, 2001000, 2009013);

                Console.WriteLine($"Scramble - Bird Rewards");
                ScrambleTreasure(paramName, includeBirdTreasure, 50000000, 50000303);

                Console.WriteLine($"Scramble - Event Rewards");
                ScrambleTreasure(paramName, includeEventTreasure, 60001000, 60050000);

                Console.WriteLine($"Scramble - Vanilla Loot");
                ScrambleTreasure(paramName, true, 10025010, 40036000);

                Console.WriteLine($"Scramble - DLC Loot");
                ScrambleTreasure(paramName, true, 50355010, 50376770);

                Console.WriteLine($"Scramble - Adding Keys");
                AddKeyItems(paramName);

                Console.WriteLine($"Scramble - Adding Tools");
                AddToolItems(paramName);

                Console.WriteLine($"Scramble - Adding Estus Flask Shard");
                ForceAddTreasure(paramName, LotRange_General, 60525000, 11);

                Console.WriteLine($"Scramble - Sublime Bone Dust");
                ForceAddTreasure(paramName, LotRange_General, 60526000, 5);

                Console.WriteLine($"Scramble - Adding Pharros Lockstone");
                ForceAddTreasure(paramName, LotRange_Things_Betwixt, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Majula, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Forest_Of_Fallen_Giants, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Brightstone_Cove_Tseldora, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Aldias_Keep, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Lost_Bastille, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Earthen_Peak, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_No_Mans_Wharf, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Iron_Keep, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Huntmans_Copse, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Gutter, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Dragon_Aerie, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Heide_Tower_of_Flame, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Shaded_Woods, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Doors_of_Pharros, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Grave_of_Saints, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Shrine_of_Amana, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Drangleic_Castle, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Undead_Crypt, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Shulva, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Brume_Tower, 60536000, 1);
                ForceAddTreasure(paramName, LotRange_Eleum_Loyce, 60536000, 1);

                Console.WriteLine($"Scramble - Adding Fragrant Branch of Yore");
                ForceAddTreasure(paramName, LotRange_Things_Betwixt, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Majula, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Forest_Of_Fallen_Giants, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Brightstone_Cove_Tseldora, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Aldias_Keep, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Lost_Bastille, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Earthen_Peak, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_No_Mans_Wharf, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Iron_Keep, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Huntmans_Copse, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Gutter, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Dragon_Aerie, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Heide_Tower_of_Flame, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Shaded_Woods, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Doors_of_Pharros, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Grave_of_Saints, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Shrine_of_Amana, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Drangleic_Castle, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Undead_Crypt, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Shulva, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Brume_Tower, 60537000, 1);
                ForceAddTreasure(paramName, LotRange_Eleum_Loyce, 60537000, 1);

                Console.WriteLine($"Scramble - Adding Smelter Wedge");
                ForceAddTreasure(paramName, LotRange_Brume_Tower, 53200000, 10);
            }

            GetListCounts();
            #endregion

            #region Enemy Drop Randomisation
            Console.WriteLine($"Enemy Drop Randomisation");

            paramName = "ItemLotParam2_Chr";

            if(scrambleEnemyDrops)
            {
                Console.WriteLine($"Scramble - Enemy Drops");
                ScrambleEnemyDrop(paramName, 10000000, 89800000);
            }

            GetListCounts();

            #endregion

            #region Final Adjustments
            Console.WriteLine($"Final Adjustments");

            paramName = "ItemLotParam2_Other";

            // These do not appear to remove the unassigned entries from the list, TODO fix it
            Console.WriteLine($"Scramble - Adding Boss Souls");
            AddBossSoulItems(paramName);

            Console.WriteLine($"Scramble - Adding Weapons");
            AddLeftoverItems(Unassigned_Weapons, paramName);

            Console.WriteLine($"Scramble - Adding Armor");
            AddLeftoverItems(Unassigned_Armor, paramName);

            Console.WriteLine($"Scramble - Adding Spells");
            AddLeftoverItems(Unassigned_Spells, paramName);

            Console.WriteLine($"Scramble - Adding Rings");
            AddLeftoverItems(Unassigned_Rings, paramName);

            #endregion

            GetListCounts();

            return regulation;
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
        public bool ScrambleEnemyDrop(string paramName, int start, int end)
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

        public void RandomiseItemLot(List<PARAM.Row> param_rows)
        {
            foreach (PARAM.Row row in param_rows)
            {
                bool editRow = true;

                // Skip row if the ID matches one of these lists
                if (HasMatchingItemLot(row, T_Fixed_Items))
                    editRow = false;

                if (T_Ignore_Keys && HasMatchingItemLot(row, Treasure_Keys))
                    editRow = false;

                if (T_Ignore_Tools && HasMatchingItemLot(row, Treasure_Tools))
                    editRow = false;

                if (T_Ignore_Boss_Souls && HasMatchingItemLot(row, Treasure_Boss_Souls))
                    editRow = false;

                if (editRow)
                {
                    PARAM.Row item = ClearItemLot(row);

                    int roll = rand.Next(100);

                    // Weapon
                    if (Unassigned_Weapons.Count > 0 && roll >= 0 && roll < 20)
                    {
                        int value = rand.Next(Unassigned_Weapons.Count);

                        item["item_lot_0"].Value = Unassigned_Weapons[value].ID;
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

                        Unassigned_Weapons.Remove(Unassigned_Weapons[value]);
                    }
                    else if (Unassigned_Weapons.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Armor
                    if (Unassigned_Armor.Count > 0 && roll >= 20 && roll < 40)
                    {
                        int value = rand.Next(Unassigned_Armor.Count);

                        item["item_lot_0"].Value = Unassigned_Armor[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Unassigned_Armor.Remove(Unassigned_Armor[value]);

                        int bonusRoll = rand.Next(100);

                        if (bonusRoll >= 50)
                        {
                            value = rand.Next(Unassigned_Armor.Count);

                            item["item_lot_1"].Value = Unassigned_Armor[value].ID;
                            item["amount_lot_1"].Value = 1;
                            item["chance_lot_1"].Value = 1;

                            Unassigned_Armor.Remove(Unassigned_Armor[value]);
                        }

                        if (bonusRoll >= 80)
                        {
                            value = rand.Next(Unassigned_Armor.Count);

                            item["item_lot_2"].Value = Unassigned_Armor[value].ID;
                            item["amount_lot_2"].Value = 1;
                            item["chance_lot_2"].Value = 1;

                            Unassigned_Armor.Remove(Unassigned_Armor[value]);
                        }

                        if (bonusRoll >= 95)
                        {
                            value = rand.Next(Unassigned_Armor.Count);

                            item["item_lot_3"].Value = Unassigned_Armor[value].ID;
                            item["amount_lot_3"].Value = 1;
                            item["chance_lot_3"].Value = 1;

                            Unassigned_Armor.Remove(Unassigned_Armor[value]);
                        }
                    }
                    else if (Unassigned_Armor.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Spell
                    if (Unassigned_Spells.Count > 0 && roll >= 40 && roll < 50)
                    {
                        int value = rand.Next(Unassigned_Spells.Count);

                        item["item_lot_0"].Value = Unassigned_Spells[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Unassigned_Spells.Remove(Unassigned_Spells[value]);

                    }
                    else if (Unassigned_Spells.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Ring
                    if (Unassigned_Rings.Count > 0 && roll >= 50 && roll < 60)
                    {
                        int value = rand.Next(Unassigned_Rings.Count);

                        item["item_lot_0"].Value = Unassigned_Rings[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Unassigned_Rings.Remove(Unassigned_Rings[value]);
                    }
                    else if (Unassigned_Rings.Count == 0)
                    {
                        roll = roll + 10;
                    }

                    // Tool
                    if (!T_Ignore_Tools && Unassigned_Tool_Items.Count > 0 && roll >= 60 && roll < 65)
                    {
                        int value = rand.Next(Unassigned_Tool_Items.Count);

                        item["item_lot_0"].Value = Unassigned_Tool_Items[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Unassigned_Tool_Items.Remove(Unassigned_Tool_Items[value]);
                    }
                    else if (!T_Ignore_Tools && Unassigned_Tool_Items.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (T_Ignore_Tools && roll >= 60 && roll < 65)
                    {
                        int value = rand.Next(Material_Item_List.Count);

                        item["item_lot_0"].Value = Material_Item_List[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Boss Soul
                    if (!T_Ignore_Boss_Souls && Unassigned_Boss_Soul_List.Count > 0 && roll >= 65 && roll < 70)
                    {
                        int value = rand.Next(Unassigned_Boss_Soul_List.Count);

                        item["item_lot_0"].Value = Unassigned_Boss_Soul_List[value].ID;
                        item["amount_lot_0"].Value = 1;
                        item["chance_lot_0"].Value = 1;

                        Unassigned_Boss_Soul_List.Remove(Unassigned_Boss_Soul_List[value]);
                    }
                    else if (!T_Ignore_Boss_Souls && Unassigned_Boss_Soul_List.Count == 0)
                    {
                        roll = roll + 5;
                    }
                    else if (T_Ignore_Boss_Souls && roll >= 65 && roll < 70)
                    {
                        int value = rand.Next(Material_Item_List.Count);

                        item["item_lot_0"].Value = Material_Item_List[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Material
                    if (roll >= 70 && roll < 80)
                    {
                        int value = rand.Next(Material_Item_List.Count);

                        item["item_lot_0"].Value = Material_Item_List[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Ammo
                    if (roll >= 80 && roll < 90)
                    {
                        int value = rand.Next(Ammo_List.Count);

                        item["item_lot_0"].Value = Ammo_List[value].ID;
                        item["amount_lot_0"].Value = rand.Next(5, 25);
                        item["chance_lot_0"].Value = 1;
                    }

                    // Consumables
                    if (roll >= 90)
                    {
                        int value = rand.Next(Consumable_Item_List.Count);

                        item["item_lot_0"].Value = Consumable_Item_List[value].ID;
                        item["amount_lot_0"].Value = rand.Next(1, 10);
                        item["chance_lot_0"].Value = 1;
                    }
                }
            }
        }

        public void RandomiseEnemyDrop(List<PARAM.Row> param_rows)
        {
            foreach (PARAM.Row row in param_rows)
            {
                bool editRow = true;

                if (T_Ignore_Keys && HasMatchingItemLot(row, Treasure_Keys))
                    editRow = false;

                if (T_Ignore_Tools && HasMatchingItemLot(row, Treasure_Tools))
                    editRow = false;

                if (T_Ignore_Boss_Souls && HasMatchingItemLot(row, Treasure_Boss_Souls))
                    editRow = false;

                if (editRow)
                {
                    PARAM.Row item = row;

                    bool useBackup = false;

                    for (int x = 0; x < 10; x++)
                    {
                        int item_id = (int)row[$"item_lot_{x}"].Value;
                        float chance = (float)row[$"chance_lot_{x}"].Value;

                        int roll = rand.Next(100);

                        // Not Rubbish or unused
                        if (item_id != 60510000 && chance > 0)
                        {
                            // Weapon
                            if (roll >= 50 && Weapon_List.Any(row => row.ID == item_id))
                            {
                                if(Unassigned_Weapons.Count > 0)
                                {
                                    int value = rand.Next(Unassigned_Weapons.Count);

                                    item[$"item_lot_{x}"].Value = Unassigned_Weapons[value].ID;
                                    item[$"amount_lot_{x}"].Value = 1;

                                    if (rand.Next(100) <= 15)
                                    {
                                        item[$"reinforcement_lot_{x}"].Value = rand.Next(9);
                                        item[$"infusion_lot_{x}"].Value = rand.Next(9);
                                    }
                                    else
                                    {
                                        item[$"reinforcement_lot_{x}"].Value = 0;
                                        item[$"infusion_lot_{x}"].Value = 0;
                                    }

                                    Unassigned_Weapons.Remove(Unassigned_Weapons[value]);
                                }
                                else
                                {
                                    useBackup = true;
                                }
                            }
                            // Armor
                            if (roll >= 50 && Armor_List.Any(row => row.ID == item_id))
                            {
                                if (Unassigned_Armor.Count > 0)
                                {
                                    int value = rand.Next(Unassigned_Armor.Count);

                                    item[$"item_lot_{x}"].Value = Unassigned_Armor[value].ID;
                                    item[$"amount_lot_{x}"].Value = 1;

                                    Unassigned_Armor.Remove(Unassigned_Armor[value]);
                                }
                                else
                                {
                                    useBackup = true;
                                }
                            }
                            // Spell
                            if (roll >= 50 && Spell_List.Any(row => row.ID == item_id))
                            {
                                if (Unassigned_Spells.Count > 0)
                                {
                                    int value = rand.Next(Unassigned_Spells.Count);

                                    item[$"item_lot_{x}"].Value = Unassigned_Spells[value].ID;
                                    item[$"amount_lot_{x}"].Value = 1;

                                    Unassigned_Spells.Remove(Unassigned_Spells[value]);
                                }
                                else
                                {
                                    useBackup = true;
                                }
                            }
                            // Ring
                            if (roll >= 50 && Ring_List.Any(row => row.ID == item_id))
                            {
                                if (Unassigned_Rings.Count > 0)
                                {
                                    int value = rand.Next(Unassigned_Rings.Count);

                                    item[$"item_lot_{x}"].Value = Unassigned_Rings[value].ID;
                                    item[$"amount_lot_{x}"].Value = 1;

                                    Unassigned_Rings.Remove(Unassigned_Rings[value]);
                                }
                                else
                                {
                                    useBackup = true;
                                }
                            }

                            // Good
                            if (Item_List.Any(row => row.ID == item_id) || useBackup || roll < 50)
                            {
                                roll = rand.Next(100);

                                if (roll >= 0 && roll < 33)
                                {
                                    int value = rand.Next(Consumable_Item_List.Count);

                                    item[$"item_lot_{x}"].Value = Consumable_Item_List[value].ID;
                                    item[$"amount_lot_{x}"].Value = rand.Next(1, 10);
                                }
                                else if (roll >= 33 && roll < 66)
                                {
                                    int value = rand.Next(Material_Item_List.Count);

                                    item[$"item_lot_{x}"].Value = Material_Item_List[value].ID;
                                    item[$"amount_lot_{x}"].Value = rand.Next(1, 5);
                                }
                                else
                                {
                                    int value = rand.Next(Ammo_List.Count);

                                    item[$"item_lot_{x}"].Value = Ammo_List[value].ID;
                                    item[$"amount_lot_{x}"].Value = rand.Next(5, 25);
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool AddLeftoverItems(List<PARAM.Row> list, string paramName)
        {
            if (list.Count > 0)
            {
                foreach (PARAM.Row row in list)
                {
                    AddTreasureToValidLot(paramName, LotRange_General, row, list);
                }
            }

            return true;
        }

        public bool AddBossSoulItems(string paramName)
        {
            if (!T_Ignore_Boss_Souls)
            {
                if (Unassigned_Boss_Soul_List.Count > 0)
                {
                    foreach (PARAM.Row row in Unassigned_Boss_Soul_List)
                    {
                        AddTreasureToValidLot(paramName, LotRange_General, row, Unassigned_Boss_Soul_List);
                    }
                }
            }

            return true;
        }

        public bool AddToolItems(string paramName)
        {
            if (!T_Ignore_Tools)
            {
                if (Unassigned_Tool_Items.Count > 0)
                {
                    foreach (PARAM.Row row in Unassigned_Tool_Items)
                    {
                        AddTreasureToValidLot(paramName, LotRange_General, row, Unassigned_Tool_Items);
                    }
                }
            }

            return true;
        }

        public bool AddKeyItems(string paramName)
        {
            // If any Key items were not assinged during the random scramble, forcefully add them in.
            if (!T_Ignore_Keys)
            {
                if (Unassigned_Key_Items.Count > 0)
                {
                    foreach (PARAM.Row row in Unassigned_Key_Items)
                    {
                        // Soldier Key
                        if (row.ID == 50600000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_SoldierKey, row, Unassigned_Key_Items);
                        }
                        // Key to King's Passage
                        if (row.ID == 50610000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_KeyToKingsPassage, row, Unassigned_Key_Items);
                        }
                        // Bastille Key
                        if (row.ID == 50800000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_BastilleKey, row, Unassigned_Key_Items);
                        }
                        // Iron Key
                        if (row.ID == 50810000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_IronKey, row, Unassigned_Key_Items);
                        }
                        // Forgotten Key
                        if (row.ID == 50820000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_ForgottenKey, row, Unassigned_Key_Items);
                        }
                        // Brightstone Key
                        if (row.ID == 50830000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_BrightstoneKey, row, Unassigned_Key_Items);
                        }
                        // Antiquated Key
                        if (row.ID == 50840000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_AntiquatedKey, row, Unassigned_Key_Items);
                        }
                        // Fang Key
                        if (row.ID == 50850000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_FangKey, row, Unassigned_Key_Items);
                        }
                        // House Key
                        if (row.ID == 50860000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_HouseKey, row, Unassigned_Key_Items);
                        }
                        // Lenigrast's Key
                        if (row.ID == 50870000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_LenigrastsKey, row, Unassigned_Key_Items);
                        }
                        // Rotunda Lockstone
                        if (row.ID == 50890000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_RotundaLockstone, row, Unassigned_Key_Items);
                        }
                        // Giant's Kinship
                        if (row.ID == 50900000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_GiantsKinship, row, Unassigned_Key_Items);
                        }
                        // Ashen Mist Heart
                        if (row.ID == 50910000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_AshenMistHeart, row, Unassigned_Key_Items);
                        }
                        // Tseldora Den Key
                        if (row.ID == 50930000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_TseldoraDenKey, row, Unassigned_Key_Items);
                        }
                        // Undead Lockaway Key
                        if (row.ID == 50970000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_UndeadLockawayKey, row, Unassigned_Key_Items);
                        }
                        // Aldia Key
                        if (row.ID == 51030000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_AldiaKey, row, Unassigned_Key_Items);
                        }
                        // Dragon Talon
                        if (row.ID == 52000000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_DragonTalon, row, Unassigned_Key_Items);
                        }
                        // Heavy Iron Key
                        if (row.ID == 52100000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_HeavyIronKey, row, Unassigned_Key_Items);
                        }
                        // Frozen Flower
                        if (row.ID == 52200000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_FrozenFlower, row, Unassigned_Key_Items);
                        }
                        // Eternal Sanctum Key
                        if (row.ID == 52300000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_EternalSanctumKey, row, Unassigned_Key_Items);
                        }
                        // Tower Key
                        if (row.ID == 52400000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_TowerKey, row, Unassigned_Key_Items);
                        }
                        // Garrison Ward Key
                        if (row.ID == 52500000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_GarrisonWardKey, row, Unassigned_Key_Items);
                        }
                        // Dragon Stone
                        if (row.ID == 52650000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_DragonStone, row, Unassigned_Key_Items);
                        }
                        // Scorching Iron Scepter
                        if (row.ID == 53100000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_ScorchingIronScepter, row, Unassigned_Key_Items);
                        }
                        // Eye of the Priestess
                        // The pickup location for this needs to remain vanilla as the uncloaking effect only occurs when picked up at the vanilla location.
                        /*
                        if (row.ID == 53600000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_EyeOfThePriestess, row, Unassigned_Key_Items);
                        }
                        */
                        // Dull Ember
                        if (row.ID == 50990000)
                        {
                            AddTreasureToValidLot(paramName, LotRange_DullEmber, row, Unassigned_Key_Items);
                        }
                    }
                }
            }

            return true;
        }

        public void ForceAddTreasure(string paramName, List<PARAM.Row> list, int target_id, int target_amount)
        {
            int placementCount = 0;
            int loopCounter = 0;
            int loopLimit = LOOP_LIMIT;

            while (placementCount < target_amount && loopCounter < loopLimit)
            {
                int count = 0;
                foreach (PARAM.Row row in list)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        int item_id = (int)row[$"item_lot_{x}"].Value;

                        if (item_id == target_id)
                            count = count + 1;
                    }
                }

                if (placementCount < target_amount)
                {
                    AddTreasureToValidLot(paramName, list, ItemParam.Rows.Find(row => row.ID == target_id), null);
                }

                placementCount = count;
                loopCounter = loopCounter + 1;
            }

            if (loopCounter >= loopLimit)
                Console.WriteLine("Failed to place item in ForceAddTreasure, exited function due to loop limit.");
        }

        public bool AddTreasureToValidLot(string paramName, List<PARAM.Row> list, PARAM.Row new_item, List<PARAM.Row> sourceList)
        {
            bool assigned = false;

            int loopCounter = 0;
            int loopLimit = LOOP_LIMIT;

            // Iterate until it can be assigned
            while (!assigned && loopCounter < loopLimit)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    if (wrapper.Name == paramName)
                    {
                        PARAM param = wrapper.Param;
                        var param_rows = list;

                        PARAM.Row row = param_rows[rand.Next(param_rows.Count)];

                        for (int x = 0; x < 10; x++)
                        {
                            int item_id = (int)row[$"item_lot_{x}"].Value;

                            bool replacableRow = false;

                            foreach (PARAM.Row cRow in Consumable_Item_List)
                            {
                                if (cRow.ID == item_id)
                                    replacableRow = true;
                            }
                            foreach (PARAM.Row mRow in Material_Item_List)
                            {
                                if (mRow.ID == item_id)
                                    replacableRow = true;
                            }

                            // This row can be replaced
                            if (replacableRow)
                            {
                                PARAM.Row item = row;

                                item[$"item_lot_{x}"].Value = new_item.ID;
                                item[$"amount_lot_{x}"].Value = 1;
                                item[$"chance_lot_{x}"].Value = 1;
                                item[$"unk_lot_{x}"].Value = 0;

                                assigned = true;

                                if (sourceList != null)
                                    sourceList.Remove(row);
                            }
                        }
                    }

                    loopCounter = loopCounter + 1;
                }
            }

            if (loopCounter >= loopLimit)
                Console.WriteLine("Failed to place item in AddTreasureToValidLot, exited function due to loop limit.");

            return true;
        }

        public PARAM.Row ClearItemLot(PARAM.Row itemlot)
        {
            itemlot["Unk00"].Value = 3;
            itemlot["Unk01"].Value = 1;
            itemlot["Unk02"].Value = 0;
            itemlot["Unk03"].Value = 0;

            for (int x = 0; x < 10; x++)
            {
                itemlot[$"amount_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 10; x++)
            {
                itemlot[$"reinforcement_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 10; x++)
            {
                itemlot[$"infusion_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 10; x++)
            {
                itemlot[$"unk_lot_{x}"].Value = 0;
            }
            for (int x = 0; x < 10; x++)
            {
                itemlot[$"item_lot_{x}"].Value = 10;
            }
            for (int x = 0; x < 10; x++)
            {
                itemlot[$"chance_lot_{x}"].Value = 0;
            }

            return itemlot;
        }

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

        public void BuildLotRanges()
        {
            // Build the row lot ranges for each Key item
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == "ItemLotParam2_Other")
                {
                    foreach (PARAM.Row row in wrapper.Rows)
                    {
                        // Exclude the forbidden rows
                        if (!T_Forbidden_Item_Lots.Contains(row.ID) && !T_Skipped_Item_Lots.Contains(row.ID) && !T_Bird_Item_Lots.Contains(row.ID))
                        {
                            if (row.ID > 10025010 && row.ID <= 50376770)
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
        }

        public void GetListCounts()
        {
            Console.WriteLine($"**************************************");
            Console.WriteLine($"Unassigned Weapons: {Unassigned_Weapons.Count}");
            Console.WriteLine($"Unassigned Armor: {Unassigned_Armor.Count}");
            Console.WriteLine($"Unassigned Spells: {Unassigned_Spells.Count}");
            Console.WriteLine($"Unassigned Rings: {Unassigned_Rings.Count}");
            Console.WriteLine($"Unassigned Tool Items: {Unassigned_Tool_Items.Count}");
            Console.WriteLine($"Unassigned Boss Souls: {Unassigned_Boss_Soul_List.Count}");
            Console.WriteLine($"-------------------");
            Console.WriteLine($"Total Weapons: {Weapon_List.Count}");
            Console.WriteLine($"Total Armor: {Armor_List.Count}");
            Console.WriteLine($"Total Spells: {Spell_List.Count}");
            Console.WriteLine($"Total Rings: {Ring_List.Count}");
            Console.WriteLine($"Total Items: {Item_List.Count}");
            Console.WriteLine($"**************************************");
        }

        #endregion
    }
}
