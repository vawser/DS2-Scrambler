using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;
using System.Windows.Documents;
using System.Windows.Shapes;
using SoulsFormats;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Windows.Forms;

namespace DS2_Scrambler
{
    public class ScramblerData
    {
        // *** Core ***
        Regulation RegulationData;

        // *** Dictionaries ***
        // Used to hold the enemy IDs for the specified type in each map.
        public Dictionary<string, List<int>> Per_Map_Character_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_Boss_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_NGP_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_Skip_Dict = new Dictionary<string, List<int>>();

        // Used to hold the fields to be shuffled.
        public Dictionary<string, List<string>> Scramble_Type_Shuffle_Field_Dict = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Scramble_Type_Generate_Field_Dict = new Dictionary<string, List<string>>();

        // Used to hold the fields and the min and max values to generate from.
        public Dictionary<string, Dictionary<string, List<string>>> Scramble_Type_Generate_Field_And_Values_Dict = new Dictionary<string, Dictionary<string, List<string>>>();

        // *** Lists ***
        // ID Lists correspond to EnemyParam
        public List<int> ID_List_Bosses = new List<int>();
        public List<int> ID_List_Characters = new List<int>();
        public List<int> ID_List_Enemies = new List<int>();
        public List<int> ID_List_Skipped_Enemies = new List<int>();

        // ID Lists corresponds to ItemParam
        public List<int> ID_List_Excluded_Weapons = new List<int>();
        public List<int> ID_List_Excluded_Armor = new List<int>();
        public List<int> ID_List_Excluded_Spells = new List<int>();
        public List<int> ID_List_Excluded_Rings = new List<int>();
        public List<int> ID_List_Excluded_Items = new List<int>();
        public List<int> ID_List_Keys = new List<int>();
        public List<int> ID_List_Tools = new List<int>();
        public List<int> ID_List_Ammo = new List<int>();
        public List<int> ID_List_Consumables = new List<int>();
        public List<int> ID_List_Materials = new List<int>();
        public List<int> ID_List_Boss_Souls = new List<int>();

        // *** ParamWrappers ***
        public ParamWrapper ItemParam;
        public ParamWrapper SpellParam;
        public ParamWrapper RingParam;
        public ParamWrapper WeaponParam;
        public ParamWrapper ArmorParam;
        public ParamWrapper ItemlotParam_Map;
        public ParamWrapper ItemlotParam_Chr;
        public ParamWrapper ShopLineupParam;

        // ** PARAM.Rows ***
        // Items
        public List<PARAM.Row> Row_List_Weapons;
        public List<PARAM.Row> Row_List_Armor;
        public List<PARAM.Row> Row_List_Spells;
        public List<PARAM.Row> Row_List_Rings;
        public List<PARAM.Row> Row_List_Items;
        public List<PARAM.Row> Row_List_Ammunition;
        public List<PARAM.Row> Row_List_Consumables;
        public List<PARAM.Row> Row_List_Materials;

        public List<PARAM.Row> Row_List_Armor_Head;
        public List<PARAM.Row> Row_List_Armor_Chest;
        public List<PARAM.Row> Row_List_Armor_Arms;
        public List<PARAM.Row> Row_List_Armor_Legs;

        public List<PARAM.Row> Row_List_Weapons_Melee;
        public List<PARAM.Row> Row_List_Weapons_Shield;

        public List<PARAM.Row> Row_List_Weapons_Catalyst_Sorcery;
        public List<PARAM.Row> Row_List_Weapons_Catalyst_Miracles;
        public List<PARAM.Row> Row_List_Weapons_Catalyst_Pyromancy;
        public List<PARAM.Row> Row_List_Weapons_Catalyst_Hex;

        public List<PARAM.Row> Row_List_Weapons_Bow;
        public List<PARAM.Row> Row_List_Weapons_Greatbow;
        public List<PARAM.Row> Row_List_Weapons_Crossbow;

        public List<PARAM.Row> Row_List_Spell_Sorceries;
        public List<PARAM.Row> Row_List_Spell_Miracles;
        public List<PARAM.Row> Row_List_Spell_Pyromancies;
        public List<PARAM.Row> Row_List_Spell_Hexes;

        public List<PARAM.Row> Row_List_Ammunition_Arrow;
        public List<PARAM.Row> Row_List_Ammunition_Greatarrow;
        public List<PARAM.Row> Row_List_Ammunition_Bolt;

        public List<PARAM.Row> Row_List_Soul_Consumables;
        public List<PARAM.Row> Row_List_Throwable_Consumable;
        public List<PARAM.Row> Row_List_HP_Consumables;
        public List<PARAM.Row> Row_List_Cast_Consumables;
        public List<PARAM.Row> Row_List_Spell_Upgrades;
        public List<PARAM.Row> Row_List_Flask_Upgrades;
        public List<PARAM.Row> Row_List_Bird_Consumables;
        public List<PARAM.Row> Row_List_Misc_Consumable;

        public List<PARAM.Row> Row_List_Keys;
        public List<PARAM.Row> Row_List_Tools;
        public List<PARAM.Row> Row_List_Boss_Souls;

        public List<PARAM.Row> Row_List_Spells_NPC;

        // Itemlots
        public List<PARAM.Row> Itemlot_List_Vanilla;
        public List<PARAM.Row> Itemlot_List_DLC;

        public List<PARAM.Row> Itemlot_List_Things_Betwixt; // 10025010 - 10027000
        public List<PARAM.Row> Itemlot_List_Majula; // 10045000 - 10046150
        public List<PARAM.Row> Itemlot_List_Forest_of_Fallen_Giants; // 10105010 - 10106630
        public List<PARAM.Row> Itemlot_List_Brightstone_Cove_Tseldora; // 10145050 - 10146520
        public List<PARAM.Row> Itemlot_List_Aldias_Keep; // 10155000 - 10156200
        public List<PARAM.Row> Itemlot_List_Lost_Bastille; // 10165000 - 10166490
        public List<PARAM.Row> Itemlot_List_Earthen_Peak; // 10175020 - 10176630
        public List<PARAM.Row> Itemlot_List_No_Mans_Wharf; // 10185000 - 10186170
        public List<PARAM.Row> Itemlot_List_Iron_Keep; // 10195000 - 10196220
        public List<PARAM.Row> Itemlot_List_Huntmans_Copse; // 10235010 - 10236270
        public List<PARAM.Row> Itemlot_List_The_Gutter; // 10255010 - 10256500
        public List<PARAM.Row> Itemlot_List_Dragon_Aerie; // 10275000 - 10276190
        public List<PARAM.Row> Itemlot_List_Path_to_the_Shaded_Woods; // 10295000 - 10296020
        public List<PARAM.Row> Itemlot_List_Unseen_Path_to_Heide; // 10305010 - 10306030
        public List<PARAM.Row> Itemlot_List_Heides_Tower_of_Flame; // 10315000 - 10316110
        public List<PARAM.Row> Itemlot_List_Shaded_Woods; // 10325000 - 10326280
        public List<PARAM.Row> Itemlot_List_Doors_of_Pharros; // 10335000 - 10336080
        public List<PARAM.Row> Itemlot_List_Grave_of_Saints; // 10345000 - 10346110
        public List<PARAM.Row> Itemlot_List_Giants_Memory; // 20105000 - 20106150
        public List<PARAM.Row> Itemlot_List_Shrine_of_Amana; // 20115000 - 20116220
        public List<PARAM.Row> Itemlot_List_Drangleic_Castle; // 20215000 - 20216140
        public List<PARAM.Row> Itemlot_List_Undead_Crypt; // 20245000 - 20246500
        public List<PARAM.Row> Itemlot_List_Dragon_Memories; // 20265000 - 20266000
        public List<PARAM.Row> Itemlot_List_Chasm_of_the_Abyss; // 40035000 - 40036000
        public List<PARAM.Row> Itemlot_List_Shulva; // 50355010 - 50356670
        public List<PARAM.Row> Itemlot_List_Brume_Tower; // 50365000 - 50368090
        public List<PARAM.Row> Itemlot_List_Eleum_Loyce; // 50375510 - 50376770

        public List<PARAM.Row> Itemlot_List_Boss_Drops; // 106000 - 862001
        public List<PARAM.Row> Itemlot_List_NPC_Rewards; // 1307000 - 1788030
        public List<PARAM.Row> Itemlot_List_Covenant_Rewards; // 2001000 - 2009013
        public List<PARAM.Row> Itemlot_List_Bird_Rewards; // 50000000 - 50000303
        public List<PARAM.Row> Itemlot_List_Event_Rewards; // 60001000 - 60050000

        public List<PARAM.Row> Shoplot_List_Vengarl; // 30700000 - 30700602
        public List<PARAM.Row> Shoplot_List_Agdayne; // 50600300 - 50600603
        public List<PARAM.Row> Shoplot_List_Gilligan; // 70400000 - 70400605
        public List<PARAM.Row> Shoplot_List_Wellager; // 72110000 - 72110607
        public List<PARAM.Row> Shoplot_List_Grandahl; // 72500300 - 72500601
        public List<PARAM.Row> Shoplot_List_Gavlan; // 72600400 - 72600607
        public List<PARAM.Row> Shoplot_List_Melentia; // 75400000 - 75400615
        public List<PARAM.Row> Shoplot_List_Rat_King; // 75600600 - 75600604
        public List<PARAM.Row> Shoplot_List_Maughlin; // 76100000 - 76100266
        public List<PARAM.Row> Shoplot_List_Chloanne; // 76200300 - 76200618
        public List<PARAM.Row> Shoplot_List_Rosabeth; // 76300300 - 76300603
        public List<PARAM.Row> Shoplot_List_Lenigrast; // 76400000 - 76400605
        public List<PARAM.Row> Shoplot_List_McDuff; // 76430000 - 76430606
        public List<PARAM.Row> Shoplot_List_Carhillion; // 76600000 - 76600602
        public List<PARAM.Row> Shoplot_List_Straid; // 76800000 - 76800600
        public List<PARAM.Row> Shoplot_List_Licia; // 76900000 - 76900400
        public List<PARAM.Row> Shoplot_List_Felkin; // 77000000 - 77000600
        public List<PARAM.Row> Shoplot_List_Navlaan; // 77100200 - 77100604
        public List<PARAM.Row> Shoplot_List_Magerold; // 77200200 - 77200606
        public List<PARAM.Row> Shoplot_List_Ornifex; // 77600000 - 77600606
        public List<PARAM.Row> Shoplot_List_Shalquoir; // 77700200 - 77700607
        public List<PARAM.Row> Shoplot_List_Gren; // 78300000 - 78300604
        public List<PARAM.Row> Shoplot_List_Cromwell; // 78400200 - 78400600
        public List<PARAM.Row> Shoplot_List_Targray; // 78500000 - 78500603

        public List<PARAM.Row> Shoplot_List_Straid_Boss; // 76801000 - 76801306
        public List<PARAM.Row> Shoplot_List_Ornifex_Boss; // 77601000 - 77602121
        public List<PARAM.Row> Shoplot_List_Ornifex_Boss_Free; // 77601000 - 77602121

        public List<PARAM.Row> Row_List_Soldier_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Key_to_the_Kings_Passage = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Bastille_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Iron_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Forgotten_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Brightstone_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Antiquated_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Fang_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_House_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Lenigrasts_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Rotunda_Lockstone = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Giants_Kinship = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Ashen_Mist_Heart = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Tseldora_Den_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Undead_Lockaway_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Aldia_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Dragon_Talon = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Heavy_Iron_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Frozen_Flower = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Eternal_Sanctum_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Tower_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Garrison_Ward_Key = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Dragon_Stone = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Scorching_Iron_Scepter = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Eye_of_the_Priestess = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Dull_Ember = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Key_to_the_Embedded = new List<PARAM.Row>();

        // *** Strings ***
        public string ParamScramblePath;
        public string EnemyScramblePath;
        public string ItemScramblePath;
        public string OutputPath;

        public ScramblerData(Regulation reg, string output_path)
        {
            RegulationData = reg;
            OutputPath = output_path;

            ParamScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";
            EnemyScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Enemy-Scramble\\";
            ItemScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Item-Scramble\\";

            // *** Enemies
            ID_List_Bosses = ConstructIntegerDataList($"{EnemyScramblePath}\\Boss-IDs");
            ID_List_Characters = ConstructIntegerDataList($"{EnemyScramblePath}\\Character-IDs");
            ID_List_Enemies = ConstructIntegerDataList($"{EnemyScramblePath}\\Enemy-IDs");
            ID_List_Skipped_Enemies.Add(837400);

            Console.WriteLine($"ID_List_Bosses - {ID_List_Bosses.Count}");
            Console.WriteLine($"ID_List_Characters - {ID_List_Characters.Count}");
            Console.WriteLine($"ID_List_Enemies - {ID_List_Enemies.Count}");
            Console.WriteLine($"ID_List_Skipped_Enemies - {ID_List_Skipped_Enemies.Count}");

            ConstructPerMapDicts();

            // *** Items
            ID_List_Excluded_Weapons = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Excluded-Weapons");
            ID_List_Excluded_Armor = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Excluded-Armor");
            ID_List_Excluded_Spells = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Excluded-Spells");
            ID_List_Excluded_Rings = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Excluded-Rings");
            ID_List_Excluded_Items = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Excluded-Items");

            Console.WriteLine($"ID_List_Excluded_Weapons - {ID_List_Excluded_Weapons.Count}");
            Console.WriteLine($"ID_List_Excluded_Armor - {ID_List_Excluded_Armor.Count}");
            Console.WriteLine($"ID_List_Excluded_Spells - {ID_List_Excluded_Spells.Count}");
            Console.WriteLine($"ID_List_Excluded_Rings - {ID_List_Excluded_Rings.Count}");
            Console.WriteLine($"ID_List_Excluded_Items - {ID_List_Excluded_Items.Count}");

            ID_List_Keys = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Keys");
            ID_List_Tools = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Tools");
            ID_List_Boss_Souls = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Boss-Souls");
            ID_List_Ammo = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Ammo");
            ID_List_Consumables = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Consumables");
            ID_List_Materials = ConstructIntegerDataList($"{ItemScramblePath}\\Treasure-Materials");

            Console.WriteLine($"ID_List_Keys - {ID_List_Keys.Count}");
            Console.WriteLine($"ID_List_Tools - {ID_List_Tools.Count}");
            Console.WriteLine($"ID_List_Ammo - {ID_List_Ammo.Count}");
            Console.WriteLine($"ID_List_Consumables - {ID_List_Consumables.Count}");
            Console.WriteLine($"ID_List_Materials - {ID_List_Materials.Count}");
            Console.WriteLine($"ID_List_Boss_Souls - {ID_List_Boss_Souls.Count}");

            // *** Params
            ConstructScramblerFieldDicts();

            // *** ParamWrappers
            foreach (ParamWrapper wrapper in RegulationData.regulationParamWrappers)
            {
                if (wrapper.Name == "WeaponParam")
                    WeaponParam = wrapper;

                if (wrapper.Name == "ArmorParam")
                    ArmorParam = wrapper;

                if (wrapper.Name == "SpellParam")
                    SpellParam = wrapper;

                if (wrapper.Name == "RingParam")
                    RingParam = wrapper;

                if (wrapper.Name == "ItemParam")
                    ItemParam = wrapper;

                if (wrapper.Name == "ItemLotParam2_Other")
                    ItemlotParam_Map = wrapper;

                if (wrapper.Name == "ItemLotParam2_Chr")
                    ItemlotParam_Chr = wrapper;

                if (wrapper.Name == "ShopLineupParam")
                    ShopLineupParam = wrapper;
            }

            Console.WriteLine($"WeaponParam - {WeaponParam.Rows.Count}");
            Console.WriteLine($"ArmorParam - {ArmorParam.Rows.Count}");
            Console.WriteLine($"SpellParam - {SpellParam.Rows.Count}");
            Console.WriteLine($"RingParam - {RingParam.Rows.Count}");
            Console.WriteLine($"ItemParam - {ItemParam.Rows.Count}");
            Console.WriteLine($"ItemlotParam_Map - {ItemlotParam_Map.Rows.Count}");
            Console.WriteLine($"ItemlotParam_Chr - {ItemlotParam_Chr.Rows.Count}");
            Console.WriteLine($"ShopLineupParam - {ShopLineupParam.Rows.Count}");

            // *** PARAM.Rows
            // Items
            Row_List_Weapons = ItemParam.Param.Rows.Where(row => 
                row.ID >= 1000000 && 
                row.ID <= 19999999 && 
                !ID_List_Excluded_Weapons.Contains(row.ID)
                ).ToList();

            Row_List_Armor = ItemParam.Param.Rows.Where(row => 
                row.ID >= 21010100 && 
                row.ID <= 29999999 && 
                !ID_List_Excluded_Armor.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Row_List_Weapons - {Row_List_Weapons.Count}");
            Console.WriteLine($"Row_List_Armor - {Row_List_Armor.Count}");

            Row_List_Armor_Head = ArmorParam.Rows.Where(row => (byte)row["slot_category"].Value == 1).ToList();
            Row_List_Armor_Chest = ArmorParam.Rows.Where(row => (byte)row["slot_category"].Value == 2).ToList();
            Row_List_Armor_Arms = ArmorParam.Rows.Where(row => (byte)row["slot_category"].Value == 3).ToList();
            Row_List_Armor_Legs = ArmorParam.Rows.Where(row => (byte)row["slot_category"].Value == 4).ToList();

            Console.WriteLine($"Row_List_Armor_Head - {Row_List_Armor_Head.Count}");
            Console.WriteLine($"Row_List_Armor_Chest - {Row_List_Armor_Chest.Count}");
            Console.WriteLine($"Row_List_Armor_Arms - {Row_List_Armor_Arms.Count}");
            Console.WriteLine($"Row_List_Armor_Legs - {Row_List_Armor_Legs.Count}");

            Row_List_Spells = ItemParam.Param.Rows.Where(row =>
                row.ID >= 31010000 && 
                row.ID <= 39999999 && 
                !ID_List_Excluded_Spells.Contains(row.ID)
                ).ToList();

            Row_List_Rings = ItemParam.Param.Rows.Where(row => 
                row.ID >= 40010000 && 
                row.ID <= 49999999 && 
                !ID_List_Excluded_Rings.Contains(row.ID)
                ).ToList();

            Row_List_Items = ItemParam.Param.Rows.Where(row => 
                row.ID >= 50600000 && 
                row.ID <= 64610000 && 
                !ID_List_Excluded_Items.Contains(row.ID)
                ).ToList();

            Row_List_Spells_NPC = SpellParam.Param.Rows.Where(row =>
                row.ID >= 71010000 &&
                row.ID <= 75310000
                ).ToList();

            Console.WriteLine($"Row_List_Spells - {Row_List_Spells.Count}");
            Console.WriteLine($"Row_List_Rings - {Row_List_Rings.Count}");
            Console.WriteLine($"Row_List_Items - {Row_List_Items.Count}");
            Console.WriteLine($"Row_List_Spells_NPC - {Row_List_Spells_NPC.Count}");

            Row_List_Ammunition = ItemParam.Param.Rows.Where(row => ID_List_Ammo.Contains(row.ID)).ToList();
            Row_List_Consumables = ItemParam.Param.Rows.Where(row => ID_List_Consumables.Contains(row.ID)).ToList();
            Row_List_Materials = ItemParam.Param.Rows.Where(row => ID_List_Materials.Contains(row.ID)).ToList();

            Row_List_Soul_Consumables = ItemParam.Param.Rows.Where(row => Row_Info_Soul_Consumable.Contains(row.ID)).ToList();
            Row_List_Throwable_Consumable = ItemParam.Param.Rows.Where(row => Row_Info_Throwable_Consumable.Contains(row.ID)).ToList();
            Row_List_Misc_Consumable = ItemParam.Param.Rows.Where(row => Row_Info_Misc_Consumable.Contains(row.ID)).ToList();
            Row_List_HP_Consumables = ItemParam.Param.Rows.Where(row => Row_Info_HP_Consumable.Contains(row.ID)).ToList();
            Row_List_Cast_Consumables = ItemParam.Param.Rows.Where(row => Row_Info_Cast_Consumable.Contains(row.ID)).ToList();
            Row_List_Spell_Upgrades = ItemParam.Param.Rows.Where(row => Row_Info_Spell_Upgrades.Contains(row.ID)).ToList();
            Row_List_Flask_Upgrades = ItemParam.Param.Rows.Where(row => Row_Info_Flask_Upgrades.Contains(row.ID)).ToList();
            Row_List_Bird_Consumables = ItemParam.Param.Rows.Where(row => Row_Info_Trade_Items.Contains(row.ID)).ToList();

            Console.WriteLine($"Row_List_Ammunition - {Row_List_Ammunition.Count}");
            Console.WriteLine($"Row_List_Consumables - {Row_List_Consumables.Count}");
            Console.WriteLine($"Row_List_Materials - {Row_List_Materials.Count}");
            Console.WriteLine($"Row_List_Soul_Consumables - {Row_List_Soul_Consumables.Count}");
            Console.WriteLine($"Row_List_Throwable_Consumable - {Row_List_Throwable_Consumable.Count}");
            Console.WriteLine($"Row_List_Misc_Consumable - {Row_List_Misc_Consumable.Count}");
            Console.WriteLine($"Row_List_HP_Consumables - {Row_List_HP_Consumables.Count}");
            Console.WriteLine($"Row_List_Cast_Consumables - {Row_List_Cast_Consumables.Count}");
            Console.WriteLine($"Row_List_Spell_Upgrades - {Row_List_Spell_Upgrades.Count}");
            Console.WriteLine($"Row_List_Flask_Upgrades - {Row_List_Flask_Upgrades.Count}");
            Console.WriteLine($"Row_List_Bird_Consumables - {Row_List_Bird_Consumables.Count}");

            Row_List_Keys = ItemParam.Rows.Where(row => ID_List_Keys.Contains(row.ID)).ToList();
            Row_List_Tools = ItemParam.Rows.Where(row => ID_List_Tools.Contains(row.ID)).ToList();
            Row_List_Boss_Souls = ItemParam.Rows.Where(row => ID_List_Boss_Souls.Contains(row.ID)).ToList();

            Row_List_Ammunition_Arrow = ItemParam.Param.Rows.Where(row => row.ID >= 60760000 && row.ID <= 60830000).ToList();
            Row_List_Ammunition_Greatarrow = ItemParam.Param.Rows.Where(row => row.ID >= 60850000 && row.ID <= 60900000).ToList();
            Row_List_Ammunition_Bolt = ItemParam.Param.Rows.Where(row => row.ID >= 60910000 && row.ID <= 60960000).ToList();

            Console.WriteLine($"Row_List_Ammunition_Arrow - {Row_List_Ammunition_Arrow.Count}");
            Console.WriteLine($"Row_List_Ammunition_Greatarrow - {Row_List_Ammunition_Greatarrow.Count}");
            Console.WriteLine($"Row_List_Ammunition_Bolt - {Row_List_Ammunition_Bolt.Count}");

            Row_List_Weapons_Bow = ItemParam.Param.Rows.Where(row => row.ID >= 4200000 && row.ID <= 4290000).ToList();
            Row_List_Weapons_Greatbow = ItemParam.Param.Rows.Where(row => row.ID >= 4400000 && row.ID <= 4440000).ToList();
            Row_List_Weapons_Crossbow = ItemParam.Param.Rows.Where(row => row.ID >= 4600000 && row.ID <= 4680000).ToList();
            Row_List_Weapons_Shield = ItemParam.Param.Rows.Where(row => row.ID >= 11000000 && row.ID <= 11840000).ToList();

            Console.WriteLine($"Row_List_Weapons_Bow - {Row_List_Weapons_Bow.Count}");
            Console.WriteLine($"Row_List_Weapons_Greatbow - {Row_List_Weapons_Greatbow.Count}");
            Console.WriteLine($"Row_List_Weapons_Crossbow - {Row_List_Weapons_Crossbow.Count}");
            Console.WriteLine($"Row_List_Weapons_Shield - {Row_List_Weapons_Shield.Count}");

            Row_List_Spell_Sorceries = ItemParam.Param.Rows.Where(row => row.ID >= 31010000 && row.ID <= 31310000).ToList();
            Row_List_Spell_Miracles = ItemParam.Param.Rows.Where(row => row.ID >= 32010000 && row.ID <= 32310000).ToList();
            Row_List_Spell_Pyromancies = ItemParam.Param.Rows.Where(row => row.ID >= 33010000 && row.ID <= 33320000).ToList();
            Row_List_Spell_Hexes = ItemParam.Param.Rows.Where(row => row.ID >= 34010000 && row.ID <= 35310000).ToList();

            Console.WriteLine($"Row_List_Spell_Sorceries - {Row_List_Spell_Sorceries.Count}");
            Console.WriteLine($"Row_List_Spell_Miracles - {Row_List_Spell_Miracles.Count}");
            Console.WriteLine($"Row_List_Spell_Pyromancies - {Row_List_Spell_Pyromancies.Count}");
            Console.WriteLine($"Row_List_Spell_Hexes - {Row_List_Spell_Hexes.Count}");

            Row_List_Weapons_Melee = Row_List_Weapons.Where(row =>
                ((row.ID >= 1000000 && row.ID <= 5660000) || row.ID >= 5000000 && row.ID <= 5660000) &&
                !Row_Info_Fists.Contains(row.ID) &&
                !Row_Info_Catalysts.Contains(row.ID)
            ).ToList();

            Console.WriteLine($"Row_List_Weapons_Melee - {Row_List_Weapons_Melee.Count}");

            Row_List_Weapons_Catalyst_Sorcery = Row_List_Weapons.Where(row =>
                Row_Info_Catalyst_Sorcery.Contains(row.ID)
            ).ToList();

            Row_List_Weapons_Catalyst_Miracles = Row_List_Weapons.Where(row =>
                Row_Info_Catalyst_Miracles.Contains(row.ID)
            ).ToList();

            Row_List_Weapons_Catalyst_Pyromancy = Row_List_Weapons.Where(row =>
                Row_Info_Catalyst_Pyromancy.Contains(row.ID)
            ).ToList();

            Row_List_Weapons_Catalyst_Hex = Row_List_Weapons.Where(row =>
                Row_Info_Catalyst_Hex.Contains(row.ID)
            ).ToList();

            Console.WriteLine($"Row_List_Weapons_Catalyst_Sorcery - {Row_List_Weapons_Catalyst_Sorcery.Count}");
            Console.WriteLine($"Row_List_Weapons_Catalyst_Miracles - {Row_List_Weapons_Catalyst_Miracles.Count}");
            Console.WriteLine($"Row_List_Weapons_Catalyst_Pyromancy - {Row_List_Weapons_Catalyst_Pyromancy.Count}");
            Console.WriteLine($"Row_List_Weapons_Catalyst_Hex - {Row_List_Weapons_Catalyst_Hex.Count}");

            // Map Itemlots
            Itemlot_List_Vanilla = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10025090 && row.ID <= 50359000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Vanilla - {Itemlot_List_Vanilla.Count}");

            Itemlot_List_DLC = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 50355010 && row.ID <= 50376770 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_DLC - {Itemlot_List_DLC.Count}");

            Itemlot_List_Things_Betwixt = ItemlotParam_Map.Rows.Where(row => 
                row.ID >= 10025010 && row.ID <= 50379000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Things_Betwixt - {Itemlot_List_Things_Betwixt.Count}");

            Itemlot_List_Majula = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10045000 && row.ID <= 10049000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Majula - {Itemlot_List_Majula.Count}");

            Itemlot_List_Forest_of_Fallen_Giants = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10105000 && row.ID <= 10109000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Forest_of_Fallen_Giants - {Itemlot_List_Forest_of_Fallen_Giants.Count}");

            Itemlot_List_Brightstone_Cove_Tseldora = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10145000 && row.ID <= 10149000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Brightstone_Cove_Tseldora - {Itemlot_List_Brightstone_Cove_Tseldora.Count}");

            Itemlot_List_Aldias_Keep = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10155000 && row.ID <= 10159000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Aldias_Keep - {Itemlot_List_Aldias_Keep.Count}");

            Itemlot_List_Lost_Bastille = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10165000 && row.ID <= 10169000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Lost_Bastille - {Itemlot_List_Lost_Bastille.Count}");

            Itemlot_List_Earthen_Peak = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10175000 && row.ID <= 10179000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Earthen_Peak - {Itemlot_List_Earthen_Peak.Count}");

            Itemlot_List_No_Mans_Wharf = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10185000 && row.ID <= 10189000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_No_Mans_Wharf - {Itemlot_List_No_Mans_Wharf.Count}");

            Itemlot_List_Iron_Keep = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10195000 && row.ID <= 10199000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Iron_Keep - {Itemlot_List_Iron_Keep.Count}");

            Itemlot_List_Huntmans_Copse = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10235000 && row.ID <= 10239000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Huntmans_Copse - {Itemlot_List_Huntmans_Copse.Count}");

            Itemlot_List_The_Gutter = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10255000 && row.ID <= 10259000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_The_Gutter - {Itemlot_List_The_Gutter.Count}");

            Itemlot_List_Dragon_Aerie = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10275000 && row.ID <= 10279000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_The_Gutter - {Itemlot_List_The_Gutter.Count}");

            Itemlot_List_Path_to_the_Shaded_Woods = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10295000 && row.ID <= 10299000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Path_to_the_Shaded_Woods - {Itemlot_List_Path_to_the_Shaded_Woods.Count}");

            Itemlot_List_Unseen_Path_to_Heide = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10305000 && row.ID <= 10309000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Unseen_Path_to_Heide - {Itemlot_List_Unseen_Path_to_Heide.Count}");

            Itemlot_List_Heides_Tower_of_Flame = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10315000 && row.ID <= 10319000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Heides_Tower_of_Flame - {Itemlot_List_Heides_Tower_of_Flame.Count}");

            Itemlot_List_Shaded_Woods = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10325000 && row.ID <= 10329000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Shaded_Woods - {Itemlot_List_Shaded_Woods.Count}");

            Itemlot_List_Doors_of_Pharros = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10335000 && row.ID <= 10339000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Doors_of_Pharros - {Itemlot_List_Doors_of_Pharros.Count}");

            Itemlot_List_Grave_of_Saints = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 10345000 && row.ID <= 10349000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Grave_of_Saints - {Itemlot_List_Grave_of_Saints.Count}");

            Itemlot_List_Giants_Memory = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 20105000 && row.ID <= 20109000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Giants_Memory - {Itemlot_List_Giants_Memory.Count}");

            Itemlot_List_Shrine_of_Amana = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 20115000 && row.ID <= 20119000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Shrine_of_Amana - {Itemlot_List_Shrine_of_Amana.Count}");

            Itemlot_List_Drangleic_Castle = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 20215000 && row.ID <= 20219000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Drangleic_Castle - {Itemlot_List_Drangleic_Castle.Count}");

            Itemlot_List_Undead_Crypt = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 20245000 && row.ID <= 20249000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Undead_Crypt - {Itemlot_List_Undead_Crypt.Count}");

            Itemlot_List_Dragon_Memories = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 20265000 && row.ID <= 20269000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Dragon_Memories - {Itemlot_List_Dragon_Memories.Count}");

            Itemlot_List_Chasm_of_the_Abyss = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 40035000 && row.ID <= 40039000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Chasm_of_the_Abyss - {Itemlot_List_Chasm_of_the_Abyss.Count}");

            Itemlot_List_Shulva = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 50355000 && row.ID <= 50359000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Shulva - {Itemlot_List_Shulva.Count}");

            Itemlot_List_Brume_Tower = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 50365000 && row.ID <= 50369000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Brume_Tower - {Itemlot_List_Brume_Tower.Count}");

            Itemlot_List_Eleum_Loyce = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 50375000 && row.ID <= 50379000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Eleum_Loyce - {Itemlot_List_Eleum_Loyce.Count}");

            // Itemlot Groups
            Itemlot_List_Boss_Drops = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 100000 && row.ID <= 900000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Boss_Drops - {Itemlot_List_Boss_Drops.Count}");

            Itemlot_List_NPC_Rewards = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 1300000 && row.ID <= 1800000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_NPC_Rewards - {Itemlot_List_NPC_Rewards.Count}");

            Itemlot_List_Covenant_Rewards = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 2001000 && row.ID <= 2009013 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Covenant_Rewards - {Itemlot_List_Covenant_Rewards.Count}");

            Itemlot_List_Bird_Rewards = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 50000000 && row.ID <= 50000303 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Bird_Rewards - {Itemlot_List_Bird_Rewards.Count}");

            Itemlot_List_Event_Rewards = ItemlotParam_Map.Rows.Where(row =>
                row.ID >= 60001000 && row.ID <= 60100000 &&
                !Itemlot_Info_Skipped_Lots.Contains(row.ID)
                ).ToList();

            Console.WriteLine($"Itemlot_List_Event_Rewards - {Itemlot_List_Event_Rewards.Count}");

            // Shoplots
            Shoplot_List_Vengarl = ShopLineupParam.Rows.Where(row =>
                row.ID >= 30700000 && row.ID <= 30700900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Vengarl - {Shoplot_List_Vengarl.Count}");

            Shoplot_List_Agdayne = ShopLineupParam.Rows.Where(row =>
                row.ID >= 50600000 && row.ID <= 50600900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Agdayne - {Shoplot_List_Agdayne.Count}");

            Shoplot_List_Gilligan = ShopLineupParam.Rows.Where(row =>
                row.ID >= 70400000 && row.ID <= 70400900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Gilligan - {Shoplot_List_Gilligan.Count}");

            Shoplot_List_Wellager = ShopLineupParam.Rows.Where(row =>
                row.ID >= 72110000 && row.ID <= 72110900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Wellager - {Shoplot_List_Wellager.Count}");

            Shoplot_List_Grandahl = ShopLineupParam.Rows.Where(row =>
                row.ID >= 72500000 && row.ID <= 72500900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Grandahl - {Shoplot_List_Grandahl.Count}");

            Shoplot_List_Gavlan = ShopLineupParam.Rows.Where(row =>
                row.ID >= 72600000 && row.ID <= 72600900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Gavlan - {Shoplot_List_Gavlan.Count}");

            Shoplot_List_Melentia = ShopLineupParam.Rows.Where(row =>
                row.ID >= 75400000 && row.ID <= 75400900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Melentia - {Shoplot_List_Melentia.Count}");

            Shoplot_List_Rat_King = ShopLineupParam.Rows.Where(row =>
                row.ID >= 75600000 && row.ID <= 75600900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Rat_King - {Shoplot_List_Rat_King.Count}");

            Shoplot_List_Maughlin = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76100000 && row.ID <= 76100900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Maughlin - {Shoplot_List_Maughlin.Count}");

            Shoplot_List_Chloanne = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76200000 && row.ID <= 76200900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Chloanne - {Shoplot_List_Chloanne.Count}");

            Shoplot_List_Rosabeth = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76300000 && row.ID <= 76300900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Rosabeth - {Shoplot_List_Rosabeth.Count}");

            Shoplot_List_Lenigrast = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76400000 && row.ID <= 76400900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Lenigrast - {Shoplot_List_Lenigrast.Count}");

            Shoplot_List_McDuff = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76430000 && row.ID <= 76430900
                ).ToList();

            Console.WriteLine($"Shoplot_List_McDuff - {Shoplot_List_McDuff.Count}");

            Shoplot_List_Carhillion = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76600000 && row.ID <= 76600900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Carhillion - {Shoplot_List_Carhillion.Count}");

            Shoplot_List_Straid = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76800000 && row.ID <= 76800900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Straid - {Shoplot_List_Straid.Count}");

            Shoplot_List_Licia = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76900000 && row.ID <= 76900900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Licia - {Shoplot_List_Licia.Count}");

            Shoplot_List_Felkin = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77000000 && row.ID <= 77000900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Felkin - {Shoplot_List_Felkin.Count}");

            Shoplot_List_Navlaan = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77100000 && row.ID <= 77100900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Navlaan - {Shoplot_List_Navlaan.Count}");

            Shoplot_List_Magerold = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77200000 && row.ID <= 77200900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Magerold - {Shoplot_List_Magerold.Count}");

            Shoplot_List_Ornifex = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77600000 && row.ID <= 77600900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Ornifex - {Shoplot_List_Ornifex.Count}");

            Shoplot_List_Shalquoir = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77700000 && row.ID <= 77700900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Shalquoir - {Shoplot_List_Shalquoir.Count}");

            Shoplot_List_Gren = ShopLineupParam.Rows.Where(row =>
                row.ID >= 78300000 && row.ID <= 78300900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Gren - {Shoplot_List_Gren.Count}");

            Shoplot_List_Cromwell = ShopLineupParam.Rows.Where(row =>
                row.ID >= 78400000 && row.ID <= 78400900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Cromwell - {Shoplot_List_Cromwell.Count}");

            Shoplot_List_Targray = ShopLineupParam.Rows.Where(row =>
                row.ID >= 78500000 && row.ID <= 78500900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Targray - {Shoplot_List_Targray.Count}");

            // Shoplots - Boss Trades
            Shoplot_List_Straid_Boss = ShopLineupParam.Rows.Where(row =>
                row.ID >= 76801000 && row.ID <= 76801900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Straid_Boss - {Shoplot_List_Straid_Boss.Count}");

            Shoplot_List_Ornifex_Boss = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77601000 && row.ID <= 77601900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Ornifex_Boss - {Shoplot_List_Ornifex_Boss.Count}");

            Shoplot_List_Ornifex_Boss_Free = ShopLineupParam.Rows.Where(row =>
                row.ID >= 77602000 && row.ID <= 77602900
                ).ToList();

            Console.WriteLine($"Shoplot_List_Ornifex_Boss_Free - {Shoplot_List_Ornifex_Boss_Free.Count}");

            // *** Keys ***
            // Soldier Key
            Row_List_Soldier_Key = Row_List_Soldier_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Soldier_Key = Row_List_Soldier_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Soldier_Key = Row_List_Soldier_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Soldier_Key - {Row_List_Soldier_Key.Count}");

            // Dull Ember
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dull_Ember = Row_List_Dull_Ember.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Dull_Ember - {Row_List_Dull_Ember.Count}");

            // Aldia Key
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Aldia_Key = Row_List_Aldia_Key.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Aldia_Key - {Row_List_Aldia_Key.Count}");

            // Ashen Mist Heart
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Ashen_Mist_Heart = Row_List_Ashen_Mist_Heart.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Ashen_Mist_Heart - {Row_List_Ashen_Mist_Heart.Count}");

            // Giant's Kinship
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Shrine_of_Amana.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Giants_Kinship = Row_List_Giants_Kinship.Concat(Itemlot_List_Undead_Crypt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Giants_Kinship - {Row_List_Giants_Kinship.Count}");

            // Rotunda Lockstone
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Rotunda_Lockstone = Row_List_Rotunda_Lockstone.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Rotunda_Lockstone - {Row_List_Rotunda_Lockstone.Count}");

            // Lenigrasts Key
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Lenigrasts_Key = Row_List_Lenigrasts_Key.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Lenigrasts_Key - {Row_List_Lenigrasts_Key.Count}");

            // House Key
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_House_Key = Row_List_House_Key.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_House_Key - {Row_List_House_Key.Count}");

            // Antiquated Key
            Row_List_Antiquated_Key = Row_List_Antiquated_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Antiquated_Key = Row_List_Antiquated_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Antiquated_Key = Row_List_Antiquated_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Antiquated_Key = Row_List_Antiquated_Key.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Antiquated_Key - {Row_List_Antiquated_Key.Count}");

            // Brightstone Key
            Row_List_Brightstone_Key = Row_List_Brightstone_Key.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Brightstone_Key = Row_List_Brightstone_Key.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Brightstone_Key = Row_List_Brightstone_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Brightstone_Key - {Row_List_Brightstone_Key.Count}");

            // Bastille Key
            Row_List_Bastille_Key = Row_List_Bastille_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Bastille_Key = Row_List_Bastille_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Bastille_Key = Row_List_Bastille_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Bastille_Key - {Row_List_Bastille_Key.Count}");

            // Tseldora Den Key
            Row_List_Tseldora_Den_Key = Row_List_Tseldora_Den_Key.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Tseldora_Den_Key = Row_List_Tseldora_Den_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Tseldora_Den_Key - {Row_List_Tseldora_Den_Key.Count}");

            // Fang Key
            Row_List_Fang_Key = Row_List_Fang_Key.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Fang_Key = Row_List_Fang_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Fang_Key - {Row_List_Fang_Key.Count}");

            // Iron Key
            Row_List_Iron_Key = Row_List_Iron_Key.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Iron_Key = Row_List_Iron_Key.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Iron_Key = Row_List_Iron_Key.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Iron_Key - {Row_List_Iron_Key.Count}");

            // Forgotten Key
            Row_List_Forgotten_Key = Row_List_Forgotten_Key.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Forgotten_Key = Row_List_Forgotten_Key.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Forgotten_Key - {Row_List_Forgotten_Key.Count}");

            // Key to the Kings Passage
            Row_List_Key_to_the_Kings_Passage = Row_List_Key_to_the_Kings_Passage.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Key_to_the_Kings_Passage - {Row_List_Key_to_the_Kings_Passage.Count}");

            // Undead Lockaway Key
            Row_List_Undead_Lockaway_Key = Row_List_Undead_Lockaway_Key.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Undead_Lockaway_Key - {Row_List_Undead_Lockaway_Key.Count}");

            // Eternal Sanctum Key
            Row_List_Eternal_Sanctum_Key = Row_List_Eternal_Sanctum_Key = Row_List_Eternal_Sanctum_Key.Concat(Itemlot_List_Shulva.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Eternal_Sanctum_Key - {Row_List_Eternal_Sanctum_Key.Count}");

            // Dragon Stone
            Row_List_Dragon_Stone = Row_List_Dragon_Stone.Concat(Itemlot_List_Shulva.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Dragon_Stone - {Row_List_Dragon_Stone.Count}");

            // Scorching Iron Scepter
            Row_List_Scorching_Iron_Scepter = Row_List_Scorching_Iron_Scepter.Concat(Itemlot_List_Brume_Tower.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Scorching_Iron_Scepter - {Row_List_Scorching_Iron_Scepter.Count}");

            // Tower Key
            Row_List_Tower_Key = Row_List_Tower_Key.Concat(Itemlot_List_Brume_Tower.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Tower_Key - {Row_List_Tower_Key.Count}");

            // Eye of the Priestess
            Row_List_Eye_of_the_Priestess = Row_List_Eye_of_the_Priestess.Concat(Itemlot_List_Eleum_Loyce.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Eye_of_the_Priestess - {Row_List_Eye_of_the_Priestess.Count}");

            // Garrison Ward Key
            Row_List_Garrison_Ward_Key = Row_List_Garrison_Ward_Key.Concat(Itemlot_List_Eleum_Loyce.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Garrison_Ward_Key - {Row_List_Garrison_Ward_Key.Count}");

            // Dragon Talon
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Shrine_of_Amana.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Dragon_Talon = Row_List_Dragon_Talon.Concat(Itemlot_List_Undead_Crypt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Dragon_Talon - {Row_List_Dragon_Talon.Count}");

            // Heavy Iron Key
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Shrine_of_Amana.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Heavy_Iron_Key = Row_List_Heavy_Iron_Key.Concat(Itemlot_List_Undead_Crypt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Heavy_Iron_Key - {Row_List_Heavy_Iron_Key.Count}");

            // Frozen Flower
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Shrine_of_Amana.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Frozen_Flower = Row_List_Frozen_Flower.Concat(Itemlot_List_Undead_Crypt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Frozen_Flower - {Row_List_Frozen_Flower.Count}");

            // Key to the Embedded
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Things_Betwixt.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Majula.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Forest_of_Fallen_Giants.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Aldias_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Lost_Bastille.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Earthen_Peak.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_No_Mans_Wharf.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Iron_Keep.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Huntmans_Copse.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_The_Gutter.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Heides_Tower_of_Flame.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Shaded_Woods.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Doors_of_Pharros.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Grave_of_Saints.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Shrine_of_Amana.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();
            Row_List_Key_to_the_Embedded = Row_List_Key_to_the_Embedded.Concat(Itemlot_List_Drangleic_Castle.Where(row => !Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList()).ToList();

            Console.WriteLine($"Row_List_Key_to_the_Embedded - {Row_List_Key_to_the_Embedded.Count}");
        }

        public List<int> ConstructIntegerDataList(string path)
        {
            List<int> data_list = new List<int>();

            foreach (string line in File.ReadLines($"{path}.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                data_list.Add(int.Parse(list[0]));
            }

            return data_list;
        }

        public List<string> ConstructStringDataList(string path)
        {
            List<string> data_list = new List<string>();

            foreach (string line in File.ReadLines($"{path}.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                data_list.Add(list[0]);
            }

            return data_list;
        }

        public List<float> ConstructFloatDataList(string path)
        {
            List<float> data_list = new List<float>();

            foreach (string line in File.ReadLines($"{path}.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                data_list.Add(float.Parse(list[0]));
            }

            return data_list;
        }

        public void ConstructPerMapDicts()
        {
            foreach (ParamWrapper wrapper in RegulationData.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    // NG+ Dict
                    List<int> temp_list = new List<int>();

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
                            temp_list.Add(row.ID);
                        }
                    }

                    Per_Map_NGP_Dict.Add(map_id, temp_list);

                    // Character Dict
                    temp_list = new List<int>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        bool isCharacterRow = false;
                        uint EnemyID = (uint)row["GeneratorRegistParam"].Value;

                        if (EnemyID > 0)
                        {
                            foreach (int entry in ID_List_Characters)
                            {
                                if (IsMatchingStringPair(entry.ToString(), EnemyID.ToString()))
                                    isCharacterRow = true;
                            }
                        }

                        if (isCharacterRow)
                        {
                            temp_list.Add(row.ID);
                        }
                    }

                    Per_Map_Character_Dict.Add(map_id, temp_list);

                    // Boss Dict
                    temp_list = new List<int>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        bool isBossRow = false;
                        uint EnemyID = (uint)row["GeneratorRegistParam"].Value;

                        if (EnemyID > 0)
                        {
                            foreach (int entry in ID_List_Bosses)
                            {
                                if (IsMatchingStringPair(entry.ToString(), EnemyID.ToString()))
                                    isBossRow = true;
                            }
                        }

                        if (isBossRow)
                        {
                            temp_list.Add(row.ID);
                        }
                    }

                    Per_Map_Boss_Dict.Add(map_id, temp_list);

                    // Skip Dict
                    temp_list = new List<int>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        bool isSkipRow = false;
                        uint EnemyID = (uint)row["GeneratorRegistParam"].Value;

                        if (EnemyID > 0)
                        {
                            foreach (int entry in ID_List_Skipped_Enemies)
                            {
                                if (IsMatchingStringPair(entry.ToString(), EnemyID.ToString()))
                                    isSkipRow = true;
                            }
                        }

                        if (isSkipRow)
                        {
                            temp_list.Add(row.ID);
                        }
                    }

                    Per_Map_Skip_Dict.Add(map_id, temp_list);
                }
            }
        }

        public bool IsMatchingStringPair(string string_a, string string_b)
        {
            int target_id = int.Parse(string_a.Remove(string_a.Length - 2, 2));
            string r = string_b;
            r = r.Remove(r.Length - 4, 4);
            int short_row_id = int.Parse(r);

            if (short_row_id == target_id)
                return true;
            else
                return false;
        }

        public void ConstructScramblerFieldDicts()
        {
            foreach (string filepath in Directory.GetFiles($"{ParamScramblePath}\\Param-Scramble\\Shuffle-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                }

                Scramble_Type_Shuffle_Field_Dict.Add(name, list);
            }

            foreach (string filepath in Directory.GetFiles($"{ParamScramblePath}\\Param-Scramble\\Generate-Shuffle-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                }

                Scramble_Type_Generate_Field_Dict.Add(name, list);
            }

            foreach (string filepath in Directory.GetFiles($"{ParamScramblePath}\\Param-Scramble\\Generate-Type"))
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(filepath);
                var dict = new Dictionary<string, List<string>>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    var list = line.Split(";");

                    var value_list = new List<string> { list[1], list[2], list[3] };

                    dict.Add(list[0], value_list);
                }

                Scramble_Type_Generate_Field_And_Values_Dict.Add(name, dict);
            }
        }

        // ** Items
        List<int> Row_Info_Soul_Consumable = new List<int>
        {
            60625000, 60630000, 60640000, 60650000, 60660000, 60670000, 60680000, 60690000, 60700000, 60710000, 60720000
        };

        List<int> Row_Info_Throwable_Consumable = new List<int>
        {
            60530000, 60531000, 60540000, 60540000, 60550000, 60560000, 60570000, 60575000, 60580000, 60590000, 60595000, 60600000, 60610000, 60620000
        };

        List<int> Row_Info_Misc_Consumable = new List<int>
        {
            60160000, 60170000, 60180000, 60190000, 60200000, 60210000, 60230000, 60235000, 60236000, 60237000, 60238000, 60239000, 60240000, 60250000, 60260000, 60270000, 60280000, 60290000, 60310000, 60320000, 60410000, 60430000, 60450000, 60370000
        };

        List<int> Row_Info_HP_Consumable = new List<int>
        {
            60010000, 60020000, 60030000, 60035000, 60036000, 60105000, 60100000, 60100000
        };

        List<int> Row_Info_Cast_Consumable = new List<int>
        {
            60040000, 60050000, 60060000, 60110000, 60120000
        };

        List<int> Row_Info_Spell_Upgrades = new List<int>
        {
            51010000, 51020000
        };

        List<int> Row_Info_Flask_Upgrades = new List<int>
        {
            60525000, 60526000
        };

        List<int> Row_Info_Trade_Items = new List<int>
        {
            60511000, 50880000, 50885000
        };

        List<int> Row_Info_Fists = new List<int>
        {
            3400000, 3401000, 3402000, 3403000, 3404000, 3405000, 3406000, 3407000, 3408000
        };
        List<int> Row_Info_Catalysts = new List<int>
        {
            4020000, 3830000, 5540000, 3800000, 3810000, 3850000, 3860000, 3870000, 3880000, 3890000, 3900000, 4150000, 3930000, 3940000, 4010000, 4020000, 4030000, 4040000, 4050000, 4060000, 4080000, 4090000, 4100000,4110000, 4120000, 4150000, 5400000, 5410000
        };
        List<int> Row_Info_Catalyst_Sorcery = new List<int>{
            4020000, 3830000, 5540000, 3800000, 3810000, 3850000, 3860000, 3870000, 3880000, 3890000, 3900000, 4150000, 3930000, 3940000
        };
        List<int> Row_Info_Catalyst_Miracles = new List<int>{
            4010000, 4020000, 4030000, 4040000, 4050000, 4060000, 4080000, 4090000, 4100000,4110000, 4120000, 4150000
        };
        List<int> Row_Info_Catalyst_Pyromancy = new List<int>{
            5400000, 5410000
        };
        List<int> Row_Info_Catalyst_Hex = new List<int>{
            4020000, 3830000, 5540000, 3800000, 3810000, 3850000, 3860000, 3870000, 3880000, 3890000, 3900000, 4150000, 3930000, 3940000
        };

        // *** Itemlots
        public List<int> Itemlot_Info_Forbidden_For_Keys = new List<int>
        {
            10026100, 10045010, 10045040, 10045050, 10045020, 10045030, 10046100, 10105120, 10105050, 10105150, 10105080, 10105090, 10105110, 10106010, 10106070, 10106080, 10106120, 10106290, 10106370, 10106420, 10106430, 10106510, 10106600, 10106610, 10106620, 10106630, 10106350, 10106360, 10106460, 10106470, 10106480, 10145070, 10145080, 10155010, 10155020, 10155000, 10155030, 10156000, 10156010, 10156030, 10156040, 10156050, 10156130, 10156140, 10156150, 10156200, 10156100, 10156110, 10156070, 10156060, 10156160, 10165010, 10165110, 10165150, 10165000, 10165070, 10165020, 10166050, 10166350, 10166330, 10166440, 10175130, 10185120, 10196140, 10195100, 10195110, 10256000, 10256360, 10305010, 10306030, 10325100, 10326220, 10326260, 10335000, 10335020, 10335040, 10336020, 10346100, 20105030, 20105040, 20115070, 20115110, 20115090, 20116200, 20215050, 20215130, 20215160, 20216000, 20216010, 20216020, 50355150, 50355180, 50355350, 50356150, 50356210, 50356380, 50356400, 50356430, 50356460, 50356470, 50356480, 50356490, 50356500, 50356520, 50356530, 50356540, 50356610, 50356620, 50365550, 50365560, 50365680, 50365690, 50365020, 50365080, 50366020, 50366210, 50366240, 50367130, 50366880, 50366890, 50366250, 50366710, 50366720, 50366740, 50366760, 50366830, 50366850, 50366860, 50366870, 50366910, 50366920, 50366930, 50366940, 50366950, 50366960, 50366970, 50366980, 50366990, 50367000, 50367010, 50367020, 50367030, 50367040, 50367050, 50367060, 50367090, 50367100, 50367110, 50367120, 50375740, 50376060, 50376210, 50376220, 50376230, 50376300, 50376450, 50376460, 50376470, 50376570, 50376580, 50376710, 50376730, 50376740
        };

        public List<int> Itemlot_Info_Never_Change = new List<int>
        {
            60155000, 60155010, 60155020, 60155030, 62190000, // Estus Flask
            60360000, // Darksign
            53600000, // Eye of the Priestess
            64600000 // Loyce Soul
        };

        public List<int> Itemlot_Info_Skipped_Lots = new List<int>
        {
            514500, 1705100, 1705300, 1753010, 1757000, 1758000, 1759000, 1777000, 1786000
        };

        public List<int> Itemlot_Info_Bird_Trades = new List<int>
        {
            50000000, 50000001, 50000002, 50000003, 50000100, 50000101, 50000102, 50000103, 50000200, 50000201, 50000202, 50000203, 50000300, 50000301, 50000302, 50000303, 50001000,
        };

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
