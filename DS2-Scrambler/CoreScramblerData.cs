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
using System.Security.Cryptography.X509Certificates;
using static SoulsFormats.PARAM;

namespace DS2_Scrambler
{
    public class CoreScramblerData
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
        public List<PARAM.Row> Row_List_Armor_Head = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Armor_Chest = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Armor_Arms = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Armor_Legs = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Spells;
        public List<PARAM.Row> Row_List_Rings;
        public List<PARAM.Row> Row_List_Items;
        public List<PARAM.Row> Row_List_Ammunition;
        public List<PARAM.Row> Row_List_Consumables;
        public List<PARAM.Row> Row_List_Materials;

        public List<PARAM.Row> Row_List_ActualArmor;
        public List<PARAM.Row> Row_List_ActualArmor_Head = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_ActualArmor_Chest = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_ActualArmor_Arms = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_ActualArmor_Legs = new List<PARAM.Row>();

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
        public List<PARAM.Row> Shoplot_List_Gilligan_InitialStage; // 70400000 - 70400605
        public List<PARAM.Row> Shoplot_List_Gilligan_SecondStage; // 70400000 - 70400605
        public List<PARAM.Row> Shoplot_List_Wellager; // 72110000 - 72110607
        public List<PARAM.Row> Shoplot_List_Grandahl; // 72500300 - 72500601
        public List<PARAM.Row> Shoplot_List_Gavlan; // 72600400 - 72600607
        public List<PARAM.Row> Shoplot_List_Melentia; // 75400000 - 75400615
        public List<PARAM.Row> Shoplot_List_Rat_King; // 75600600 - 75600604
        public List<PARAM.Row> Shoplot_List_Maughlin; // 76100000 - 76100266
        public List<PARAM.Row> Shoplot_List_Chloanne; // 76200300 - 76200618
        public List<PARAM.Row> Shoplot_List_Rosabeth; // 76300300 - 76300603
        public List<PARAM.Row> Shoplot_List_Lenigrast; // 76400000 - 76400605
        public List<PARAM.Row> Shoplot_List_McDuff; // 76430000 - 
        public List<PARAM.Row> Shoplot_List_Carhillion; // 76600000 - 76600602
        public List<PARAM.Row> Shoplot_List_Carhillion_InitialStage; // 76600000 - 76600602
        public List<PARAM.Row> Shoplot_List_Carhillion_SecondStage; // 76600000 - 76600602
        public List<PARAM.Row> Shoplot_List_Straid; // 76800000 - 76800600
        public List<PARAM.Row> Shoplot_List_Licia; // 76900000 - 76900400
        public List<PARAM.Row> Shoplot_List_Licia_InitialStage; // 76900000 - 76900400
        public List<PARAM.Row> Shoplot_List_Licia_SecondStage; // 76900000 - 76900400
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
        public List<PARAM.Row> Row_List_King_Ring = new List<PARAM.Row>();

        public List<PARAM.Row> Row_List_Aged_Feather = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Champion_Tablet = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Dragon_Head_Stone = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Dragon_Torso_Stone = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Hello_Carving = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Thank_You_Carving = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Im_Sorry_Carving = new List<PARAM.Row>();
        public List<PARAM.Row> Row_List_Very_Good_Carving = new List<PARAM.Row>();

        public Dictionary<string, List<PARAM.Row>> map_itemlots = new Dictionary<string, List<PARAM.Row>>();

        // *** Strings ***
        public string ParamScramblePath;
        public string OutputPath;

        public CoreScramblerData(Regulation reg, string output_path)
        {
            RegulationData = reg;
            OutputPath = output_path;

            ParamScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";

            // *** Enemies
            ConstructPerMapDicts();

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

            // *** PARAM.Rows
            // Items
            Row_List_Weapons = WeaponParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.WeaponParam_Category_Start && row.ID <= ItemScramblerData.Static.WeaponParam_Category_End && !ItemScramblerData.Static.Blacklist_Weapons.Contains(row.ID)).ToList();

            Row_List_Armor = ItemParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.ArmorParam_Category_Start && row.ID <= ItemScramblerData.Static.ArmorParam_Category_End && !ItemScramblerData.Static.Blacklist_Armor.Contains(row.ID)).ToList();

            foreach(PARAM.Row row in Row_List_Armor)
            {
                string armor_id_string = "1" + row.ID.ToString().Substring(1);
                int armor_id = int.Parse(armor_id_string);
                List<PARAM.Row> armor_rows = ArmorParam.Rows.Where(r => r.ID == armor_id).ToList();

                if ((byte)armor_rows[0]["slot_category"].Value == 1)
                    Row_List_Armor_Head.Add(row);

                if ((byte)armor_rows[0]["slot_category"].Value == 2)
                    Row_List_Armor_Chest.Add(row);

                if ((byte)armor_rows[0]["slot_category"].Value == 3)
                    Row_List_Armor_Arms.Add(row);

                if ((byte)armor_rows[0]["slot_category"].Value == 4)
                    Row_List_Armor_Legs.Add(row);
            }

            Row_List_ActualArmor = ArmorParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.ActualArmorParam_Category_Start && row.ID <= ItemScramblerData.Static.ActualArmorParam_Category_End
            ).ToList();

            foreach (PARAM.Row row in Row_List_ActualArmor)
            {
                if ((byte)row["slot_category"].Value == 1)
                    Row_List_ActualArmor_Head.Add(row);

                if ((byte)row["slot_category"].Value == 2)
                    Row_List_ActualArmor_Chest.Add(row);

                if ((byte)row["slot_category"].Value == 3)
                    Row_List_ActualArmor_Arms.Add(row);

                if ((byte)row["slot_category"].Value == 4)
                    Row_List_ActualArmor_Legs.Add(row);
            }

            Row_List_Spells = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.SpellParam_Category_Start && row.ID <= ItemScramblerData.Static.SpellParam_Category_End && !ItemScramblerData.Static.Blacklist_Spells.Contains(row.ID)).ToList();
            Row_List_Rings = RingParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.RingParam_Category_Start && row.ID <= ItemScramblerData.Static.RingParam_Category_End && !ItemScramblerData.Static.Blacklist_Rings.Contains(row.ID)).ToList();
            Row_List_Items = ItemParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.ItemParam_Category_Start && row.ID <= ItemScramblerData.Static.ItemParam_Category_End && !ItemScramblerData.Static.Blacklist_Items.Contains(row.ID)).ToList();
            Row_List_Spells_NPC = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.NPC_SpellParam_Category_Start &&row.ID <= ItemScramblerData.Static.NPC_SpellParam_Category_End).ToList();
            Row_List_Ammunition = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Category_Ammo.Contains(row.ID)).ToList();
            Row_List_Consumables = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Category_Consumable.Contains(row.ID)).ToList();
            Row_List_Materials = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Category_Material.Contains(row.ID)).ToList();
            Row_List_Soul_Consumables = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Consumable_Soul.Contains(row.ID)).ToList();
            Row_List_Throwable_Consumable = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Throwable_Item.Contains(row.ID)).ToList();
            Row_List_Misc_Consumable = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_General_Consumable.Contains(row.ID)).ToList();
            Row_List_HP_Consumables = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_HP_Consumable.Contains(row.ID)).ToList();
            Row_List_Cast_Consumables = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Cast_Consumable.Contains(row.ID)).ToList();
            Row_List_Spell_Upgrades = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Spell_Tier_Consumable.Contains(row.ID)).ToList();
            Row_List_Flask_Upgrades = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Flask_Tier_Consumable.Contains(row.ID)).ToList();
            Row_List_Bird_Consumables = ItemParam.Param.Rows.Where(row => ItemScramblerData.Static.Assignment_Category_Trade_Consumable.Contains(row.ID)).ToList();
            Row_List_Keys = ItemParam.Rows.Where(row => ItemScramblerData.Static.Category_Key_Item.Contains(row.ID)).ToList();
            Row_List_Tools = ItemParam.Rows.Where(row => ItemScramblerData.Static.Category_Tool_Item.Contains(row.ID)).ToList();
            Row_List_Boss_Souls = ItemParam.Rows.Where(row => ItemScramblerData.Static.Category_Boss_Soul.Contains(row.ID)).ToList();
            Row_List_Ammunition_Arrow = ItemParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Arrow_ArrowParam_Category_Start && row.ID <= ItemScramblerData.Static.Arrow_ArrowParam_Category_End).ToList();
            Row_List_Ammunition_Greatarrow = ItemParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Greatarrow_ArrowParam_Category_Start && row.ID <= ItemScramblerData.Static.Greatarrow_ArrowParam_Category_End).ToList();
            Row_List_Ammunition_Bolt = ItemParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Bolt_ArrowParam_Category_Start && row.ID <= ItemScramblerData.Static.Bolt_ArrowParam_Category_End).ToList();
            Row_List_Weapons_Bow = WeaponParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Bow_WeaponParam_Category_Start && row.ID <= ItemScramblerData.Static.Bow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Greatbow = WeaponParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Greatbow_WeaponParam_Category_Start && row.ID <= ItemScramblerData.Static.Greatbow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Crossbow = WeaponParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Crossbow_WeaponParam_Category_Start && row.ID <= ItemScramblerData.Static.Crossbow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Shield = WeaponParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Shield_WeaponParam_Category_Start && row.ID <= ItemScramblerData.Static.Shield_WeaponParam_Category_End).ToList();
            Row_List_Spell_Sorceries = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Sorceries_SpellParam_Category_Start && row.ID <= ItemScramblerData.Static.Sorceries_SpellParam_Category_End).ToList();
            Row_List_Spell_Miracles = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Miracles_SpellParam_Category_Start && row.ID <= ItemScramblerData.Static.Miracles_SpellParam_Category_End).ToList();
            Row_List_Spell_Pyromancies = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Pyromancies_SpellParam_Category_Start && row.ID <= ItemScramblerData.Static.Pyromancies_SpellParam_Category_End).ToList();
            Row_List_Spell_Hexes = SpellParam.Param.Rows.Where(row => row.ID >= ItemScramblerData.Static.Hexes_SpellParam_Category_Start && row.ID <= ItemScramblerData.Static.Hexes_SpellParam_Category_End).ToList();
            Row_List_Weapons_Melee = Row_List_Weapons.Where(row => row.ID >= ItemScramblerData.Static.Melee_WeaponParam_Category_Start &&row.ID <= ItemScramblerData.Static.Melee_WeaponParam_Category_End && !ItemScramblerData.Static.Category_Fists.Contains(row.ID) && !ItemScramblerData.Static.Category_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Sorcery = Row_List_Weapons.Where(row => ItemScramblerData.Static.Category_Sorcery_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Miracles = Row_List_Weapons.Where(row => ItemScramblerData.Static.Category_Miracle_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Pyromancy = Row_List_Weapons.Where(row => ItemScramblerData.Static.Category_Pyromancy_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Hex = Row_List_Weapons.Where(row => ItemScramblerData.Static.Category_Hex_Catalysts.Contains(row.ID)).ToList();

            // Map Itemlots
            Itemlot_List_Vanilla = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.Vanilla_Itemlot_Start && row.ID <= ItemScramblerData.Static.Vanilla_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_DLC = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.DLC_Itemlot_Start &&row.ID <= ItemScramblerData.Static.DLC_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();

            Itemlot_List_Things_Betwixt = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_02_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_02_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_02_00_00", Itemlot_List_Things_Betwixt);

            Itemlot_List_Majula = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_04_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_04_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_04_00_00", Itemlot_List_Majula);

            Itemlot_List_Forest_of_Fallen_Giants = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_10_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_10_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_10_00_00", Itemlot_List_Forest_of_Fallen_Giants);

            Itemlot_List_Brightstone_Cove_Tseldora = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_14_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_14_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_14_00_00", Itemlot_List_Brightstone_Cove_Tseldora);

            Itemlot_List_Aldias_Keep = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_15_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_15_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_15_00_00", Itemlot_List_Aldias_Keep);

            Itemlot_List_Lost_Bastille = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_16_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_16_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_16_00_00", Itemlot_List_Lost_Bastille);

            Itemlot_List_Earthen_Peak = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_17_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_17_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_17_00_00", Itemlot_List_Earthen_Peak);

            Itemlot_List_No_Mans_Wharf = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_18_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_18_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_18_00_00", Itemlot_List_No_Mans_Wharf);

            Itemlot_List_Iron_Keep = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_19_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_19_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_19_00_00", Itemlot_List_Iron_Keep);

            Itemlot_List_Huntmans_Copse = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_23_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_23_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_23_00_00", Itemlot_List_Iron_Keep);

            Itemlot_List_The_Gutter = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_25_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_25_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_25_00_00", Itemlot_List_The_Gutter);

            Itemlot_List_Dragon_Aerie = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_27_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_27_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_27_00_00", Itemlot_List_Dragon_Aerie);

            Itemlot_List_Path_to_the_Shaded_Woods = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_29_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_29_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_29_00_00", Itemlot_List_Path_to_the_Shaded_Woods);

            Itemlot_List_Unseen_Path_to_Heide = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_30_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_30_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_30_00_00", Itemlot_List_Unseen_Path_to_Heide);

            Itemlot_List_Heides_Tower_of_Flame = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_31_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_31_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_31_00_00", Itemlot_List_Heides_Tower_of_Flame);

            Itemlot_List_Shaded_Woods = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_32_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_32_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_32_00_00", Itemlot_List_Shaded_Woods);

            Itemlot_List_Doors_of_Pharros = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_33_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_33_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_33_00_00", Itemlot_List_Doors_of_Pharros);

            Itemlot_List_Grave_of_Saints = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m10_34_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m10_34_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_34_00_00", Itemlot_List_Grave_of_Saints);

            Itemlot_List_Giants_Memory = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m20_10_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m20_10_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_10_00_00", Itemlot_List_Giants_Memory);

            Itemlot_List_Shrine_of_Amana = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m20_11_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m20_11_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_11_00_00", Itemlot_List_Shrine_of_Amana);

            Itemlot_List_Drangleic_Castle = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m20_21_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m20_21_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_21_00_00", Itemlot_List_Drangleic_Castle);

            Itemlot_List_Undead_Crypt = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m20_24_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m20_24_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_24_00_00", Itemlot_List_Undead_Crypt);

            Itemlot_List_Dragon_Memories = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m20_26_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m20_26_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_26_00_00", Itemlot_List_Dragon_Memories);

            Itemlot_List_Chasm_of_the_Abyss = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m40_03_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m40_03_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m40_03_00_00", Itemlot_List_Chasm_of_the_Abyss);

            Itemlot_List_Shulva = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m50_35_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m50_35_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_35_00_00", Itemlot_List_Shulva);

            Itemlot_List_Brume_Tower = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m50_36_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m50_36_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_36_00_00", Itemlot_List_Brume_Tower);

            Itemlot_List_Eleum_Loyce = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.m50_37_00_00_Itemlot_Start &&row.ID <= ItemScramblerData.Static.m50_37_00_00_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_37_00_00", Itemlot_List_Eleum_Loyce);

            // Itemlot Groups
            Itemlot_List_Boss_Drops = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.Boss_Drops_Itemlot_Start &&row.ID <= ItemScramblerData.Static.Boss_Drops_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_NPC_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.NPC_Rewards_Itemlot_Start &&row.ID <= ItemScramblerData.Static.NPC_Rewards_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Covenant_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.Covenant_Rewards_Itemlot_Start &&row.ID <= ItemScramblerData.Static.Covenant_Rewards_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Bird_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.Trade_Rewards_Itemlot_Start &&row.ID <= ItemScramblerData.Static.Trade_Rewards_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Event_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ItemScramblerData.Static.Event_Rewards_Itemlot_Start &&row.ID <= ItemScramblerData.Static.Event_Rewards_Itemlot_End && !ItemScramblerData.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();

            // Shoplots
            Shoplot_List_Vengarl = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Vengarl_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Vengarl_Shoplot_End).ToList();
            Shoplot_List_Agdayne = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Agdayne_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Agdayne_Shoplot_End).ToList();
            Shoplot_List_Gilligan_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Gilligan_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Gilligan_Shoplot_End && ItemScramblerData.Static.Gilligan_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Gilligan_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Gilligan_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Gilligan_Shoplot_End && !ItemScramblerData.Static.Gilligan_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Wellager = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Wellager_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Wellager_Shoplot_End).ToList();
            Shoplot_List_Grandahl = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Grandahl_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Grandahl_Shoplot_End).ToList();
            Shoplot_List_Gavlan = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Gavlan_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Gavlan_Shoplot_End).ToList();
            Shoplot_List_Melentia = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Melentia_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Melentia_Shoplot_End).ToList();
            Shoplot_List_Rat_King = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Rat_King_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Rat_King_Shoplot_End).ToList();
            Shoplot_List_Maughlin = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Maughlin_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Maughlin_Shoplot_End).ToList();
            Shoplot_List_Chloanne = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Chloanne_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Chloanne_Shoplot_End).ToList();
            Shoplot_List_Rosabeth = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Rosabeth_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Rosabeth_Shoplot_End).ToList();
            Shoplot_List_Lenigrast = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Lenigrast_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Lenigrast_Shoplot_End).ToList();
            Shoplot_List_McDuff = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.McDuff_Shoplot_Start &&row.ID <= ItemScramblerData.Static.McDuff_Shoplot_End).ToList();
            Shoplot_List_Carhillion = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Carhillion_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Carhillion_Shoplot_End && !ItemScramblerData.Static.Carhillion_Shop_Initial_Lots.Contains(row.ID) && !ItemScramblerData.Static.Carhillion_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Carhillion_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Carhillion_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Carhillion_Shoplot_End && ItemScramblerData.Static.Carhillion_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Carhillion_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Carhillion_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Carhillion_Shoplot_End && ItemScramblerData.Static.Carhillion_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Straid = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Straid_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Straid_Shoplot_End).ToList();
            Shoplot_List_Licia = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Licia_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Licia_Shoplot_End && !ItemScramblerData.Static.Licia_Shop_Initial_Lots.Contains(row.ID) && !ItemScramblerData.Static.Licia_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Licia_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Licia_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Licia_Shoplot_End && ItemScramblerData.Static.Licia_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Licia_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Licia_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Licia_Shoplot_End && ItemScramblerData.Static.Licia_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Felkin = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Felkin_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Felkin_Shoplot_End).ToList();
            Shoplot_List_Navlaan = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Navlaan_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Navlaan_Shoplot_End).ToList();
            Shoplot_List_Magerold = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Magerold_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Magerold_Shoplot_End).ToList();
            Shoplot_List_Ornifex = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Ornifex_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Ornifex_Shoplot_End).ToList();
            Shoplot_List_Shalquoir = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Shalquior_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Shalquior_Shoplot_End).ToList();
            Shoplot_List_Gren = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Titchy_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Titchy_Shoplot_End).ToList();
            Shoplot_List_Cromwell = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Cromwell_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Cromwell_Shoplot_End).ToList();
            Shoplot_List_Targray = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Targray_Shoplot_Start &&row.ID <= ItemScramblerData.Static.Targray_Shoplot_End).ToList();

            // Shoplots - Boss Trades
            Shoplot_List_Straid_Boss = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Straid_Bosslot_Start &&row.ID <= ItemScramblerData.Static.Straid_Bosslot_End).ToList();
            Shoplot_List_Ornifex_Boss = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Ornifex_Bosslot_Start &&row.ID <= ItemScramblerData.Static.Ornifex_Bosslot_End).ToList();
            Shoplot_List_Ornifex_Boss_Free = ShopLineupParam.Rows.Where(row => row.ID >= ItemScramblerData.Static.Ornifex_Free_Bosslot_Start &&row.ID <= ItemScramblerData.Static.Ornifex_Free_Bosslot_End).ToList();

            // Keys 
            Row_List_Soldier_Key = BuildKeyItemRowList(ItemScramblerData.Static.Soldier_Key);
            Row_List_Dull_Ember = BuildKeyItemRowList(ItemScramblerData.Static.Dull_Ember); 
            Row_List_Aldia_Key = BuildKeyItemRowList(ItemScramblerData.Static.Aldia_Key);
            Row_List_Ashen_Mist_Heart = BuildKeyItemRowList(ItemScramblerData.Static.Ashen_Mist_Heart);
            Row_List_Giants_Kinship = BuildKeyItemRowList(ItemScramblerData.Static.Giants_Kinship);
            Row_List_Rotunda_Lockstone = BuildKeyItemRowList(ItemScramblerData.Static.Rotunda_Lockstone);
            Row_List_Lenigrasts_Key = BuildKeyItemRowList(ItemScramblerData.Static.Lenigrasts_Key);
            Row_List_House_Key = BuildKeyItemRowList(ItemScramblerData.Static.House_Key);
            Row_List_Antiquated_Key = BuildKeyItemRowList(ItemScramblerData.Static.Antiquated_Key);
            Row_List_Brightstone_Key = BuildKeyItemRowList(ItemScramblerData.Static.Brightstone_Key);
            Row_List_Bastille_Key = BuildKeyItemRowList(ItemScramblerData.Static.Bastille_Key);
            Row_List_Tseldora_Den_Key = BuildKeyItemRowList(ItemScramblerData.Static.Tseldora_Den_Key);
            Row_List_Fang_Key = BuildKeyItemRowList(ItemScramblerData.Static.Fang_Key);
            Row_List_Iron_Key = BuildKeyItemRowList(ItemScramblerData.Static.Iron_Key);
            Row_List_Forgotten_Key = BuildKeyItemRowList(ItemScramblerData.Static.Forgotten_Key);
            Row_List_Key_to_the_Kings_Passage = BuildKeyItemRowList(ItemScramblerData.Static.Key_to_the_Kings_Passage);
            Row_List_Undead_Lockaway_Key = BuildKeyItemRowList(ItemScramblerData.Static.Undead_Lockaway_Key);
            Row_List_Eternal_Sanctum_Key = BuildKeyItemRowList(ItemScramblerData.Static.Eternal_Sanctum_Key);
            Row_List_Dragon_Stone = BuildKeyItemRowList(ItemScramblerData.Static.Dragon_Stone);
            Row_List_Scorching_Iron_Scepter = BuildKeyItemRowList(ItemScramblerData.Static.Scorching_Iron_Scepter);
            Row_List_Tower_Key = BuildKeyItemRowList(ItemScramblerData.Static.Tower_Key);
            Row_List_Garrison_Ward_Key = BuildKeyItemRowList(ItemScramblerData.Static.Garrison_Ward_Key);
            Row_List_Dragon_Talon = BuildKeyItemRowList(ItemScramblerData.Static.Dragon_Talon);
            Row_List_Heavy_Iron_Key = BuildKeyItemRowList(ItemScramblerData.Static.Heavy_Iron_Key);
            Row_List_Frozen_Flower = BuildKeyItemRowList(ItemScramblerData.Static.Frozen_Flower);
            Row_List_Key_to_the_Embedded = BuildKeyItemRowList(ItemScramblerData.Static.Key_to_the_Embedded);
            Row_List_King_Ring = BuildKeyItemRowList(ItemScramblerData.Static.King_Ring);

            // Tools
            Row_List_Aged_Feather = BuildToolItemRowList(ItemScramblerData.Static.Aged_Feather);
            Row_List_Champion_Tablet = BuildToolItemRowList(ItemScramblerData.Static.Champion_Tablet);
            Row_List_Dragon_Head_Stone = BuildToolItemRowList(ItemScramblerData.Static.Dragon_Head_Stone);
            Row_List_Dragon_Torso_Stone = BuildToolItemRowList(ItemScramblerData.Static.Dragon_Torso_Stone);
            Row_List_Hello_Carving = BuildToolItemRowList(ItemScramblerData.Static.Hello_Carving);
            Row_List_Thank_You_Carving = BuildToolItemRowList(ItemScramblerData.Static.Thank_You_Carving);
            Row_List_Im_Sorry_Carving = BuildToolItemRowList(ItemScramblerData.Static.Im_Sorry_Carving);
            Row_List_Very_Good_Carving = BuildToolItemRowList(ItemScramblerData.Static.Very_Good_Carving);

        }

        public List<PARAM.Row> BuildKeyItemRowList(KeyItem data)
        {
            List<PARAM.Row> new_rows = new List<PARAM.Row>();

            foreach(string map in data.Maps)
            {
                if (map_itemlots.ContainsKey(map))
                {
                    List<PARAM.Row> rows = map_itemlots[map];
                    new_rows = new_rows.Concat(rows.Where(row => !ItemScramblerData.Static.Blacklist_Itemlots_for_Keys.Contains(row.ID)).ToList()).ToList();
                }
            }

            return new_rows;
        }

        public List<PARAM.Row> BuildToolItemRowList(ToolItem data)
        {
            List<PARAM.Row> new_rows = new List<PARAM.Row>();

            foreach (string map in data.Maps)
            {
                if (map_itemlots.ContainsKey(map))
                {
                    List<PARAM.Row> rows = map_itemlots[map];
                    new_rows = new_rows.Concat(rows).ToList();
                }
            }

            return new_rows;
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
                            foreach (int entry in ParamScramblerData.Static.Character_EnemyParamID_List)
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
                            foreach (int entry in ParamScramblerData.Static.Boss_EnemyParamID_List)
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
                            foreach (int entry in ParamScramblerData.Static.Skipped_EnemyParamID_List)
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
    }
}
