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
using System.Text.Json;

namespace DS2_Scrambler
{
    public class ScramblerData
    {
        // *** Core ***
        Regulation RegulationData;

        public Dictionary<string, List<int>> Per_Map_Character_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_Boss_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_NGP_Dict = new Dictionary<string, List<int>>();
        public Dictionary<string, List<int>> Per_Map_Skip_Dict = new Dictionary<string, List<int>>();

        public ParamWrapper ItemParam;
        public ParamWrapper SpellParam;
        public ParamWrapper RingParam;
        public ParamWrapper WeaponParam;
        public ParamWrapper ArmorParam;
        public ParamWrapper ItemlotParam_Map;
        public ParamWrapper ItemlotParam_Chr;
        public ParamWrapper ShopLineupParam;

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
        public string OutputPath;

        public ScramblerData(Regulation reg, string output_path)
        {
            RegulationData = reg;
            OutputPath = output_path;

            // *** Enemies
            ConstructPerMapDicts();

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
            Row_List_Weapons = WeaponParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End && !ScramblerData_Items.Static.Blacklist_Weapons.Contains(row.ID)).ToList();

            Row_List_Armor = ItemParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.ArmorParam_Category_Start && row.ID <= ScramblerData_Items.Static.ArmorParam_Category_End && !ScramblerData_Items.Static.Blacklist_Armor.Contains(row.ID)).ToList();

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

            Row_List_ActualArmor = ArmorParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.ActualArmorParam_Category_Start && row.ID <= ScramblerData_Items.Static.ActualArmorParam_Category_End
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

            Row_List_Spells = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start && row.ID <= ScramblerData_Items.Static.SpellParam_Category_End && !ScramblerData_Items.Static.Blacklist_Spells.Contains(row.ID)).ToList();
            Row_List_Rings = RingParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.RingParam_Category_Start && row.ID <= ScramblerData_Items.Static.RingParam_Category_End && !ScramblerData_Items.Static.Blacklist_Rings.Contains(row.ID)).ToList();
            Row_List_Items = ItemParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.ItemParam_Category_Start && row.ID <= ScramblerData_Items.Static.ItemParam_Category_End && !ScramblerData_Items.Static.Blacklist_Items.Contains(row.ID)).ToList();
            Row_List_Spells_NPC = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.NPC_SpellParam_Category_Start &&row.ID <= ScramblerData_Items.Static.NPC_SpellParam_Category_End).ToList();
            Row_List_Ammunition = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Category_Ammo.Contains(row.ID)).ToList();
            Row_List_Consumables = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Category_Consumable.Contains(row.ID)).ToList();
            Row_List_Materials = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Category_Material.Contains(row.ID)).ToList();
            Row_List_Soul_Consumables = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Consumable_Soul.Contains(row.ID)).ToList();
            Row_List_Throwable_Consumable = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Throwable_Item.Contains(row.ID)).ToList();
            Row_List_Misc_Consumable = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_General_Consumable.Contains(row.ID)).ToList();
            Row_List_HP_Consumables = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_HP_Consumable.Contains(row.ID)).ToList();
            Row_List_Cast_Consumables = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Cast_Consumable.Contains(row.ID)).ToList();
            Row_List_Spell_Upgrades = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Spell_Tier_Consumable.Contains(row.ID)).ToList();
            Row_List_Flask_Upgrades = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Flask_Tier_Consumable.Contains(row.ID)).ToList();
            Row_List_Bird_Consumables = ItemParam.Param.Rows.Where(row => ScramblerData_Items.Static.Assignment_Category_Trade_Consumable.Contains(row.ID)).ToList();
            Row_List_Keys = ItemParam.Rows.Where(row => ScramblerData_Items.Static.Category_Key_Item.Contains(row.ID)).ToList();
            Row_List_Tools = ItemParam.Rows.Where(row => ScramblerData_Items.Static.Category_Tool_Item.Contains(row.ID)).ToList();
            Row_List_Boss_Souls = ItemParam.Rows.Where(row => ScramblerData_Items.Static.Category_Boss_Soul.Contains(row.ID)).ToList();
            Row_List_Ammunition_Arrow = ItemParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Arrow_ArrowParam_Category_Start && row.ID <= ScramblerData_Items.Static.Arrow_ArrowParam_Category_End).ToList();
            Row_List_Ammunition_Greatarrow = ItemParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Greatarrow_ArrowParam_Category_Start && row.ID <= ScramblerData_Items.Static.Greatarrow_ArrowParam_Category_End).ToList();
            Row_List_Ammunition_Bolt = ItemParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Bolt_ArrowParam_Category_Start && row.ID <= ScramblerData_Items.Static.Bolt_ArrowParam_Category_End).ToList();
            Row_List_Weapons_Bow = WeaponParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Bow_WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.Bow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Greatbow = WeaponParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Greatbow_WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.Greatbow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Crossbow = WeaponParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Crossbow_WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.Crossbow_WeaponParam_Category_End).ToList();
            Row_List_Weapons_Shield = WeaponParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Shield_WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.Shield_WeaponParam_Category_End).ToList();
            Row_List_Spell_Sorceries = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Sorceries_SpellParam_Category_Start && row.ID <= ScramblerData_Items.Static.Sorceries_SpellParam_Category_End).ToList();
            Row_List_Spell_Miracles = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Miracles_SpellParam_Category_Start && row.ID <= ScramblerData_Items.Static.Miracles_SpellParam_Category_End).ToList();
            Row_List_Spell_Pyromancies = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Pyromancies_SpellParam_Category_Start && row.ID <= ScramblerData_Items.Static.Pyromancies_SpellParam_Category_End).ToList();
            Row_List_Spell_Hexes = SpellParam.Param.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Hexes_SpellParam_Category_Start && row.ID <= ScramblerData_Items.Static.Hexes_SpellParam_Category_End).ToList();
            Row_List_Weapons_Melee = Row_List_Weapons.Where(row => row.ID >= ScramblerData_Items.Static.Melee_WeaponParam_Category_Start && row.ID <= ScramblerData_Items.Static.Melee_WeaponParam_Category_End && !ScramblerData_Items.Static.Category_Fists.Contains(row.ID) && !ScramblerData_Items.Static.Category_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Sorcery = Row_List_Weapons.Where(row => ScramblerData_Items.Static.Category_Sorcery_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Miracles = Row_List_Weapons.Where(row => ScramblerData_Items.Static.Category_Miracle_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Pyromancy = Row_List_Weapons.Where(row => ScramblerData_Items.Static.Category_Pyromancy_Catalysts.Contains(row.ID)).ToList();
            Row_List_Weapons_Catalyst_Hex = Row_List_Weapons.Where(row => ScramblerData_Items.Static.Category_Hex_Catalysts.Contains(row.ID)).ToList();

            // Map Itemlots
            Itemlot_List_Vanilla = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Vanilla_Itemlot_Start && row.ID <= ScramblerData_Items.Static.Vanilla_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_DLC = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.DLC_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.DLC_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();

            Itemlot_List_Things_Betwixt = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_02_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_02_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_02_00_00", Itemlot_List_Things_Betwixt);

            Itemlot_List_Majula = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_04_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_04_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_04_00_00", Itemlot_List_Majula);

            Itemlot_List_Forest_of_Fallen_Giants = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_10_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_10_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_10_00_00", Itemlot_List_Forest_of_Fallen_Giants);

            Itemlot_List_Brightstone_Cove_Tseldora = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_14_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_14_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_14_00_00", Itemlot_List_Brightstone_Cove_Tseldora);

            Itemlot_List_Aldias_Keep = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_15_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_15_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_15_00_00", Itemlot_List_Aldias_Keep);

            Itemlot_List_Lost_Bastille = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_16_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_16_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_16_00_00", Itemlot_List_Lost_Bastille);

            Itemlot_List_Earthen_Peak = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_17_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_17_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_17_00_00", Itemlot_List_Earthen_Peak);

            Itemlot_List_No_Mans_Wharf = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_18_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_18_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_18_00_00", Itemlot_List_No_Mans_Wharf);

            Itemlot_List_Iron_Keep = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_19_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_19_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_19_00_00", Itemlot_List_Iron_Keep);

            Itemlot_List_Huntmans_Copse = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_23_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_23_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_23_00_00", Itemlot_List_Iron_Keep);

            Itemlot_List_The_Gutter = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_25_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_25_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_25_00_00", Itemlot_List_The_Gutter);

            Itemlot_List_Dragon_Aerie = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_27_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_27_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_27_00_00", Itemlot_List_Dragon_Aerie);

            Itemlot_List_Path_to_the_Shaded_Woods = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_29_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_29_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_29_00_00", Itemlot_List_Path_to_the_Shaded_Woods);

            Itemlot_List_Unseen_Path_to_Heide = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_30_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_30_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_30_00_00", Itemlot_List_Unseen_Path_to_Heide);

            Itemlot_List_Heides_Tower_of_Flame = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_31_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_31_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_31_00_00", Itemlot_List_Heides_Tower_of_Flame);

            Itemlot_List_Shaded_Woods = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_32_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_32_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_32_00_00", Itemlot_List_Shaded_Woods);

            Itemlot_List_Doors_of_Pharros = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_33_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_33_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_33_00_00", Itemlot_List_Doors_of_Pharros);

            Itemlot_List_Grave_of_Saints = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m10_34_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m10_34_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m10_34_00_00", Itemlot_List_Grave_of_Saints);

            Itemlot_List_Giants_Memory = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m20_10_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m20_10_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_10_00_00", Itemlot_List_Giants_Memory);

            Itemlot_List_Shrine_of_Amana = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m20_11_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m20_11_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_11_00_00", Itemlot_List_Shrine_of_Amana);

            Itemlot_List_Drangleic_Castle = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m20_21_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m20_21_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_21_00_00", Itemlot_List_Drangleic_Castle);

            Itemlot_List_Undead_Crypt = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m20_24_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m20_24_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_24_00_00", Itemlot_List_Undead_Crypt);

            Itemlot_List_Dragon_Memories = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m20_26_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m20_26_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m20_26_00_00", Itemlot_List_Dragon_Memories);

            Itemlot_List_Chasm_of_the_Abyss = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m40_03_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m40_03_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m40_03_00_00", Itemlot_List_Chasm_of_the_Abyss);

            Itemlot_List_Shulva = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m50_35_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m50_35_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_35_00_00", Itemlot_List_Shulva);

            Itemlot_List_Brume_Tower = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m50_36_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m50_36_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_36_00_00", Itemlot_List_Brume_Tower);

            Itemlot_List_Eleum_Loyce = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.m50_37_00_00_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.m50_37_00_00_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            map_itemlots.Add("m50_37_00_00", Itemlot_List_Eleum_Loyce);

            // Itemlot Groups
            Itemlot_List_Boss_Drops = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Boss_Drops_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.Boss_Drops_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_NPC_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.NPC_Rewards_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.NPC_Rewards_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Covenant_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Covenant_Rewards_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.Covenant_Rewards_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Bird_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Trade_Rewards_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.Trade_Rewards_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();
            Itemlot_List_Event_Rewards = ItemlotParam_Map.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Event_Rewards_Itemlot_Start &&row.ID <= ScramblerData_Items.Static.Event_Rewards_Itemlot_End && !ScramblerData_Items.Static.Blacklist_Itemlots.Contains(row.ID)).ToList();

            // Shoplots
            Shoplot_List_Vengarl = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Vengarl_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Vengarl_Shoplot_End).ToList();
            Shoplot_List_Agdayne = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Agdayne_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Agdayne_Shoplot_End).ToList();
            Shoplot_List_Gilligan_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Gilligan_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Gilligan_Shoplot_End && ScramblerData_Items.Static.Gilligan_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Gilligan_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Gilligan_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Gilligan_Shoplot_End && !ScramblerData_Items.Static.Gilligan_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Wellager = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Wellager_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Wellager_Shoplot_End).ToList();
            Shoplot_List_Grandahl = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Grandahl_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Grandahl_Shoplot_End).ToList();
            Shoplot_List_Gavlan = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Gavlan_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Gavlan_Shoplot_End).ToList();
            Shoplot_List_Melentia = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Melentia_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Melentia_Shoplot_End).ToList();
            Shoplot_List_Rat_King = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Rat_King_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Rat_King_Shoplot_End).ToList();
            Shoplot_List_Maughlin = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Maughlin_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Maughlin_Shoplot_End).ToList();
            Shoplot_List_Chloanne = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Chloanne_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Chloanne_Shoplot_End).ToList();
            Shoplot_List_Rosabeth = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Rosabeth_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Rosabeth_Shoplot_End).ToList();
            Shoplot_List_Lenigrast = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Lenigrast_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Lenigrast_Shoplot_End).ToList();
            Shoplot_List_McDuff = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.McDuff_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.McDuff_Shoplot_End).ToList();
            Shoplot_List_Carhillion = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Carhillion_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Carhillion_Shoplot_End && !ScramblerData_Items.Static.Carhillion_Shop_Initial_Lots.Contains(row.ID) && !ScramblerData_Items.Static.Carhillion_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Carhillion_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Carhillion_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Carhillion_Shoplot_End && ScramblerData_Items.Static.Carhillion_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Carhillion_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Carhillion_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Carhillion_Shoplot_End && ScramblerData_Items.Static.Carhillion_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Straid = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Straid_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Straid_Shoplot_End).ToList();
            Shoplot_List_Licia = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Licia_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Licia_Shoplot_End && !ScramblerData_Items.Static.Licia_Shop_Initial_Lots.Contains(row.ID) && !ScramblerData_Items.Static.Licia_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Licia_InitialStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Licia_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Licia_Shoplot_End && ScramblerData_Items.Static.Licia_Shop_Initial_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Licia_SecondStage = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Licia_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Licia_Shoplot_End && ScramblerData_Items.Static.Licia_Shop_Secondary_Lots.Contains(row.ID)).ToList();
            Shoplot_List_Felkin = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Felkin_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Felkin_Shoplot_End).ToList();
            Shoplot_List_Navlaan = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Navlaan_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Navlaan_Shoplot_End).ToList();
            Shoplot_List_Magerold = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Magerold_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Magerold_Shoplot_End).ToList();
            Shoplot_List_Ornifex = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Ornifex_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Ornifex_Shoplot_End).ToList();
            Shoplot_List_Shalquoir = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Shalquior_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Shalquior_Shoplot_End).ToList();
            Shoplot_List_Gren = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Titchy_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Titchy_Shoplot_End).ToList();
            Shoplot_List_Cromwell = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Cromwell_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Cromwell_Shoplot_End).ToList();
            Shoplot_List_Targray = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Targray_Shoplot_Start &&row.ID <= ScramblerData_Items.Static.Targray_Shoplot_End).ToList();

            // Shoplots - Boss Trades
            Shoplot_List_Straid_Boss = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Straid_Bosslot_Start &&row.ID <= ScramblerData_Items.Static.Straid_Bosslot_End).ToList();
            Shoplot_List_Ornifex_Boss = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Ornifex_Bosslot_Start &&row.ID <= ScramblerData_Items.Static.Ornifex_Bosslot_End).ToList();
            Shoplot_List_Ornifex_Boss_Free = ShopLineupParam.Rows.Where(row => row.ID >= ScramblerData_Items.Static.Ornifex_Free_Bosslot_Start &&row.ID <= ScramblerData_Items.Static.Ornifex_Free_Bosslot_End).ToList();

            // Keys 
            Row_List_Soldier_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Soldier_Key);
            Row_List_Dull_Ember = BuildKeyItemRowList(ScramblerData_Items.Static.Dull_Ember); 
            Row_List_Aldia_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Aldia_Key);
            Row_List_Ashen_Mist_Heart = BuildKeyItemRowList(ScramblerData_Items.Static.Ashen_Mist_Heart);
            Row_List_Giants_Kinship = BuildKeyItemRowList(ScramblerData_Items.Static.Giants_Kinship);
            Row_List_Rotunda_Lockstone = BuildKeyItemRowList(ScramblerData_Items.Static.Rotunda_Lockstone);
            Row_List_Lenigrasts_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Lenigrasts_Key);
            Row_List_House_Key = BuildKeyItemRowList(ScramblerData_Items.Static.House_Key);
            Row_List_Antiquated_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Antiquated_Key);
            Row_List_Brightstone_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Brightstone_Key);
            Row_List_Bastille_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Bastille_Key);
            Row_List_Tseldora_Den_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Tseldora_Den_Key);
            Row_List_Fang_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Fang_Key);
            Row_List_Iron_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Iron_Key);
            Row_List_Forgotten_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Forgotten_Key);
            Row_List_Key_to_the_Kings_Passage = BuildKeyItemRowList(ScramblerData_Items.Static.Key_to_the_Kings_Passage);
            Row_List_Undead_Lockaway_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Undead_Lockaway_Key);
            Row_List_Eternal_Sanctum_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Eternal_Sanctum_Key);
            Row_List_Dragon_Stone = BuildKeyItemRowList(ScramblerData_Items.Static.Dragon_Stone);
            Row_List_Scorching_Iron_Scepter = BuildKeyItemRowList(ScramblerData_Items.Static.Scorching_Iron_Scepter);
            Row_List_Tower_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Tower_Key);
            Row_List_Garrison_Ward_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Garrison_Ward_Key);
            Row_List_Dragon_Talon = BuildKeyItemRowList(ScramblerData_Items.Static.Dragon_Talon);
            Row_List_Heavy_Iron_Key = BuildKeyItemRowList(ScramblerData_Items.Static.Heavy_Iron_Key);
            Row_List_Frozen_Flower = BuildKeyItemRowList(ScramblerData_Items.Static.Frozen_Flower);
            Row_List_Key_to_the_Embedded = BuildKeyItemRowList(ScramblerData_Items.Static.Key_to_the_Embedded);
            Row_List_King_Ring = BuildKeyItemRowList(ScramblerData_Items.Static.King_Ring);

            // Tools
            Row_List_Aged_Feather = BuildToolItemRowList(ScramblerData_Items.Static.Aged_Feather);
            Row_List_Champion_Tablet = BuildToolItemRowList(ScramblerData_Items.Static.Champion_Tablet);
            Row_List_Dragon_Head_Stone = BuildToolItemRowList(ScramblerData_Items.Static.Dragon_Head_Stone);
            Row_List_Dragon_Torso_Stone = BuildToolItemRowList(ScramblerData_Items.Static.Dragon_Torso_Stone);
            Row_List_Hello_Carving = BuildToolItemRowList(ScramblerData_Items.Static.Hello_Carving);
            Row_List_Thank_You_Carving = BuildToolItemRowList(ScramblerData_Items.Static.Thank_You_Carving);
            Row_List_Im_Sorry_Carving = BuildToolItemRowList(ScramblerData_Items.Static.Im_Sorry_Carving);
            Row_List_Very_Good_Carving = BuildToolItemRowList(ScramblerData_Items.Static.Very_Good_Carving);

        }

        public List<PARAM.Row> BuildKeyItemRowList(KeyItem data)
        {
            List<PARAM.Row> new_rows = new List<PARAM.Row>();

            foreach(string map in data.Maps)
            {
                if (map_itemlots.ContainsKey(map))
                {
                    List<PARAM.Row> rows = map_itemlots[map];
                    new_rows = new_rows.Concat(rows.Where(row => !ScramblerData_Items.Static.Blacklist_Itemlots_for_Keys.Contains(row.ID)).ToList()).ToList();
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
                            foreach (int entry in ScramblerData_Core.Static.Character_EnemyParamID_List)
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
                            foreach (int entry in ScramblerData_Core.Static.Boss_EnemyParamID_List)
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
                            foreach (int entry in ScramblerData_Core.Static.Skipped_EnemyParamID_List)
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
    }

    public class ScramblerData_Core
    {
        public List<int> Boss_EnemyParamID_List { get; set; }
        public List<int> Character_EnemyParamID_List { get; set; }
        public List<int> Summon_Character_EnemyParamID_List { get; set; }
        public List<int> Hostile_Character_EnemyParamID_List { get; set; }
        public List<int> Enemy_EnemyParamID_List { get; set; }
        public List<int> Skipped_EnemyParamID_List { get; set; }
        public List<int> SpEffect_ID_List { get; set; }

        public List<string> WeaponActionCategoryFields { get; set; }
        public List<string> SpellCastAnimationFields { get; set; }
        public List<int> FFX_List { get; set; }

        public static ScramblerData_Core Static { get; }

        static ScramblerData_Core()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\core.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ScramblerData_Core>(File.OpenRead(json_filepath), options);
        }
    }

    public class ScramblerData_Params
    {
        public ParamData_Weapon WeaponParamData { get; set; }
        public ParamData_Armor ArmorParamData { get; set; }
        public ParamData_Ring RingParamData { get; set; }
        public ParamData_Item ItemParamData { get; set; }
        public ParamData_Spell SpellParamData { get; set; }
        public ParamData_Projectiles ProjectileParamData { get; set; }
        public ParamData_Player PlayerParamData { get; set; }
        public ParamData_Map MapParamData { get; set; }
        public ParamData_Character CharacterParamData { get; set; }
        public ParamData_Enemy EnemyParamData { get; set; }

        public static ScramblerData_Params Static { get; }

        static ScramblerData_Params()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\params.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ScramblerData_Params>(File.OpenRead(json_filepath), options);
        }
    }

    public class ParamData_Weapon
    {
        public int Base_Price_Min { get; set; }
        public int Base_Price_Max { get; set; }
        public int Sell_Price_Min { get; set; }
        public int Sell_Price_Max { get; set; }
        public float Weight_Min { get; set; }
        public float Weight_Max { get; set; }
        public int Durability_Min { get; set; }
        public int Durability_Max { get; set; }
        public float Animation_Speed_Min { get; set; }
        public float Animation_Speed_Max { get; set; }
        public short STR_Requirement_Min { get; set; }
        public short STR_Requirement_Max { get; set; }
        public short DEX_Requirement_Min { get; set; }
        public short DEX_Requirement_Max { get; set; }
        public short INT_Requirement_Min { get; set; }
        public short INT_Requirement_Max { get; set; }
        public short FTH_Requirement_Min { get; set; }
        public short FTH_Requirement_Max { get; set; }

        public float Stamina_Damage_Min { get; set; }
        public float Stamina_Damage_Max { get; set; }
        public float Damage_Multiplier_Min { get; set; }
        public float Damage_Multiplier_Max { get; set; }
        public float Stamina_Damage_Multiplier_Min { get; set; }
        public float Stamina_Damage_Multiplier_Max { get; set; }
        public float Durability_Damage_Multiplier_Min { get; set; }
        public float Durability_Damage_Multiplier_Max { get; set; }
        public float Status_Inflict_Multiplier_Min { get; set; }
        public float Status_Inflict_Multiplier_Max { get; set; }
        public float Posture_Damage_Multiplier_Min { get; set; }
        public float Posture_Damage_Multiplier_Max { get; set; }
        public float Counter_Damage_Multiplier_Min { get; set; }
        public float Counter_Damage_Multiplier_Max { get; set; }

        public float Physical_Damage_Min { get; set; }
        public float Physical_Damage_Max { get; set; }
        public float Magic_Damage_Min { get; set; }
        public float Magic_Damage_Max { get; set; }
        public float Lightning_Damage_Min { get; set; }
        public float Lightning_Damage_Max { get; set; }
        public float Fire_Damage_Min { get; set; }
        public float Fire_Damage_Max { get; set; }
        public float Dark_Damage_Min { get; set; }
        public float Dark_Damage_Max { get; set; }
        public float Poison_Damage_Min { get; set; }
        public float Poison_Damage_Max { get; set; }
        public float Bleed_Damage_Min { get; set; }
        public float Bleed_Damage_Max { get; set; }
        public float Damage_Growth_Min { get; set; }
        public float Damage_Growth_Max { get; set; }
        public float Shield_Stability_Min { get; set; }
        public float Shield_Stability_Max { get; set; }
        public float Shield_Stability_Growth_Min { get; set; }
        public float Shield_Stability_Growth_Max { get; set; }
        public float Physical_Absorption_Min { get; set; }
        public float Physical_Absorption_Max { get; set; }
        public float Magic_Absorption_Min { get; set; }
        public float Magic_Absorption_Max { get; set; }
        public float Lightning_Absorption_Min { get; set; }
        public float Lightning_Absorption_Max { get; set; }
        public float Fire_Absorption_Min { get; set; }
        public float Fire_Absorption_Max { get; set; }
        public float Dark_Absorption_Min { get; set; }
        public float Dark_Absorption_Max { get; set; }
        public float Poison_Absorption_Min { get; set; }
        public float Poison_Absorption_Max { get; set; }
        public float Bleed_Absorption_Min { get; set; }
        public float Bleed_Absorption_Max { get; set; }
        public float Curse_Absorption_Min { get; set; }
        public float Curse_Absorption_Max { get; set; }
        public float Petrify_Absorption_Min { get; set; }
        public float Petrify_Absorption_Max { get; set; }
        public float Shield_Absorption_Factor { get; set; }
        public float Stamina_Consumption_Min { get; set; }
        public float Stamina_Consumption_Max { get; set; }
        public float Cast_Speed_Min { get; set; }
        public float Cast_Speed_Max { get; set; }
        public ushort Bow_Distance_Min { get; set; }
        public ushort Bow_Distance_Max { get; set; }
        public ushort Arrow_Physical_Damage_Min { get; set; }
        public ushort Arrow_Physical_Damage_Max { get; set; }
        public ushort Arrow_Magic_Damage_Min { get; set; }
        public ushort Arrow_Magic_Damage_Max { get; set; }
        public ushort Arrow_Lightning_Damage_Min { get; set; }
        public ushort Arrow_Lightning_Damage_Max { get; set; }
        public ushort Arrow_Fire_Damage_Min { get; set; }
        public ushort Arrow_Fire_Damage_Max { get; set; }
        public ushort Arrow_Dark_Damage_Min { get; set; }
        public ushort Arrow_Dark_Damage_Max { get; set; }
        public byte Arrow_Poison_Damage_Min { get; set; }
        public byte Arrow_Poison_Damage_Max { get; set; }
        public byte Arrow_Bleed_Damage_Min { get; set; }
        public byte Arrow_Bleed_Damage_Max { get; set; }
    }
    public class ParamData_Armor
    {
        public int Base_Price_Min { get; set; }
        public int Base_Price_Max { get; set; }
        public int Sell_Price_Min { get; set; }
        public int Sell_Price_Max { get; set; }
        public float Weight_Min { get; set; }
        public float Weight_Max { get; set; }
        public float Durability_Min { get; set; }
        public float Durability_Max { get; set; }
        public ushort STR_Requirement_Min { get; set; }
        public ushort STR_Requirement_Max { get; set; }
        public ushort DEX_Requirement_Min { get; set; }
        public ushort DEX_Requirement_Max { get; set; }
        public ushort INT_Requirement_Min { get; set; }
        public ushort INT_Requirement_Max { get; set; }
        public ushort FTH_Requirement_Min { get; set; }
        public ushort FTH_Requirement_Max { get; set; }
    }
    public class ParamData_Ring
    {

    }
    public class ParamData_Item
    {

    }
    public class ParamData_Spell
    {

    }
    public class ParamData_Projectiles
    {

    }
    public class ParamData_Player
    {

    }
    public class ParamData_Map
    {

    }
    public class ParamData_Character
    {

    }
    public class ParamData_Enemy
    {

    }

    public class ScramblerData_Map
    {
        public List<int> Generator_Skip_List_m10_02_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_04_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_10_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_14_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_15_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_16_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_17_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_18_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_19_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_23_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_25_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_27_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_29_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_30_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_31_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_32_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_33_00_00 { get; set; }
        public List<int> Generator_Skip_List_m10_34_00_00 { get; set; }
        public List<int> Generator_Skip_List_m20_10_00_00 { get; set; }
        public List<int> Generator_Skip_List_m20_11_00_00 { get; set; }
        public List<int> Generator_Skip_List_m20_21_00_00 { get; set; }
        public List<int> Generator_Skip_List_m20_24_00_00 { get; set; }
        public List<int> Generator_Skip_List_m20_26_00_00 { get; set; }
        public List<int> Generator_Skip_List_m40_03_00_00 { get; set; }
        public List<int> Generator_Skip_List_m50_35_00_00 { get; set; }
        public List<int> Generator_Skip_List_m50_36_00_00 { get; set; }
        public List<int> Generator_Skip_List_m50_37_00_00 { get; set; }
        public List<int> Generator_Skip_List_m50_38_00_00 { get; set; }

        public List<int> Character_Generator_List_m10_02_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_04_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_10_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_14_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_15_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_16_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_17_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_18_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_19_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_23_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_25_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_27_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_29_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_30_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_31_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_32_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_33_00_00 { get; set; }
        public List<int> Character_Generator_List_m10_34_00_00 { get; set; }
        public List<int> Character_Generator_List_m20_10_00_00 { get; set; }
        public List<int> Character_Generator_List_m20_11_00_00 { get; set; }
        public List<int> Character_Generator_List_m20_21_00_00 { get; set; }
        public List<int> Character_Generator_List_m20_24_00_00 { get; set; }
        public List<int> Character_Generator_List_m20_26_00_00 { get; set; }
        public List<int> Character_Generator_List_m40_03_00_00 { get; set; }
        public List<int> Character_Generator_List_m50_35_00_00 { get; set; }
        public List<int> Character_Generator_List_m50_36_00_00 { get; set; }
        public List<int> Character_Generator_List_m50_37_00_00 { get; set; }
        public List<int> Character_Generator_List_m50_38_00_00 { get; set; }

        public List<float>[] Additional_Locations_Enemy_List_m10_02_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_04_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_14_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_15_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_16_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_17_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_18_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_19_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_23_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_25_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_27_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_29_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_30_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_31_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_32_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_33_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m10_34_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m20_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m20_11_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m20_21_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m20_24_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m20_26_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m40_03_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m50_35_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m50_36_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m50_37_00_00 { get; set; }
        public List<float>[] Additional_Locations_Enemy_List_m50_38_00_00 { get; set; }

        public List<float>[] Additional_Locations_Character_List_m10_02_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_04_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_14_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_15_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_16_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_17_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_18_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_19_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_23_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_25_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_27_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_29_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_30_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_31_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_32_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_33_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m10_34_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m20_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m20_11_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m20_21_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m20_24_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m20_26_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m40_03_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m50_35_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m50_36_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m50_37_00_00 { get; set; }
        public List<float>[] Additional_Locations_Character_List_m50_38_00_00 { get; set; }

        public List<float>[] Additional_Locations_Petrified_List_m10_02_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_04_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_14_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_15_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_16_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_17_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_18_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_19_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_23_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_25_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_27_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_29_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_30_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_31_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_32_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_33_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m10_34_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m20_10_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m20_11_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m20_21_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m20_24_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m20_26_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m40_03_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m50_35_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m50_36_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m50_37_00_00 { get; set; }
        public List<float>[] Additional_Locations_Petrified_List_m50_38_00_00 { get; set; }

        public List<int> Substitution_List_m10_02_00_00 { get; set; }
        public List<int> Substitution_List_m10_04_00_00 { get; set; }
        public List<int> Substitution_List_m10_10_00_00 { get; set; }
        public List<int> Substitution_List_m10_14_00_00 { get; set; }
        public List<int> Substitution_List_m10_15_00_00 { get; set; }
        public List<int> Substitution_List_m10_16_00_00 { get; set; }
        public List<int> Substitution_List_m10_17_00_00 { get; set; }
        public List<int> Substitution_List_m10_18_00_00 { get; set; }
        public List<int> Substitution_List_m10_19_00_00 { get; set; }
        public List<int> Substitution_List_m10_23_00_00 { get; set; }
        public List<int> Substitution_List_m10_25_00_00 { get; set; }
        public List<int> Substitution_List_m10_27_00_00 { get; set; }
        public List<int> Substitution_List_m10_29_00_00 { get; set; }
        public List<int> Substitution_List_m10_30_00_00 { get; set; }
        public List<int> Substitution_List_m10_31_00_00 { get; set; }
        public List<int> Substitution_List_m10_32_00_00 { get; set; }
        public List<int> Substitution_List_m10_33_00_00 { get; set; }
        public List<int> Substitution_List_m10_34_00_00 { get; set; }
        public List<int> Substitution_List_m20_10_00_00 { get; set; }
        public List<int> Substitution_List_m20_11_00_00 { get; set; }
        public List<int> Substitution_List_m20_21_00_00 { get; set; }
        public List<int> Substitution_List_m20_24_00_00 { get; set; }
        public List<int> Substitution_List_m20_26_00_00 { get; set; }
        public List<int> Substitution_List_m40_03_00_00 { get; set; }
        public List<int> Substitution_List_m50_35_00_00 { get; set; }
        public List<int> Substitution_List_m50_36_00_00 { get; set; }
        public List<int> Substitution_List_m50_37_00_00 { get; set; }
        public List<int> Substitution_List_m50_38_00_00 { get; set; }

        public static ScramblerData_Map Static { get; }

        static ScramblerData_Map()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\map.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ScramblerData_Map>(File.OpenRead(json_filepath), options);
        }
    }

    public class ScramblerData_Items
    {
        public int WeaponParam_Category_Start { get; set; }
        public int WeaponParam_Category_End { get; set; }
        public int ActualArmorParam_Category_Start { get; set; }
        public int ActualArmorParam_Category_End { get; set; }
        public int ArmorParam_Category_Start { get; set; }
        public int ArmorParam_Category_End { get; set; }
        public int SpellParam_Category_Start { get; set; }
        public int SpellParam_Category_End { get; set; }
        public int NPC_SpellParam_Category_Start { get; set; }
        public int NPC_SpellParam_Category_End { get; set; }
        public int RingParam_Category_Start { get; set; }
        public int RingParam_Category_End { get; set; }
        public int ItemParam_Category_Start { get; set; }
        public int ItemParam_Category_End { get; set; }
        public int Arrow_ArrowParam_Category_Start { get; set; }
        public int Arrow_ArrowParam_Category_End { get; set; }
        public int Greatarrow_ArrowParam_Category_Start { get; set; }
        public int Greatarrow_ArrowParam_Category_End { get; set; }
        public int Bolt_ArrowParam_Category_Start { get; set; }
        public int Bolt_ArrowParam_Category_End { get; set; }
        public int Bow_WeaponParam_Category_Start { get; set; }
        public int Bow_WeaponParam_Category_End { get; set; }
        public int Greatbow_WeaponParam_Category_Start { get; set; }
        public int Greatbow_WeaponParam_Category_End { get; set; }
        public int Crossbow_WeaponParam_Category_Start { get; set; }
        public int Crossbow_WeaponParam_Category_End { get; set; }
        public int Shield_WeaponParam_Category_Start { get; set; }
        public int Shield_WeaponParam_Category_End { get; set; }
        public int Sorceries_SpellParam_Category_Start { get; set; }
        public int Sorceries_SpellParam_Category_End { get; set; }
        public int Miracles_SpellParam_Category_Start { get; set; }
        public int Miracles_SpellParam_Category_End { get; set; }
        public int Pyromancies_SpellParam_Category_Start { get; set; }
        public int Pyromancies_SpellParam_Category_End { get; set; }
        public int Hexes_SpellParam_Category_Start { get; set; }
        public int Hexes_SpellParam_Category_End { get; set; }
        public int Melee_WeaponParam_Category_Start { get; set; }
        public int Melee_WeaponParam_Category_End { get; set; }

        public int Vanilla_Itemlot_Start { get; set; }
        public int Vanilla_Itemlot_End { get; set; }
        public int DLC_Itemlot_Start { get; set; }
        public int DLC_Itemlot_End { get; set; }
        public int Boss_Drops_Itemlot_Start { get; set; }
        public int Boss_Drops_Itemlot_End { get; set; }
        public int NPC_Rewards_Itemlot_Start { get; set; }
        public int NPC_Rewards_Itemlot_End { get; set; }
        public int Covenant_Rewards_Itemlot_Start { get; set; }
        public int Covenant_Rewards_Itemlot_End { get; set; }
        public int Trade_Rewards_Itemlot_Start { get; set; }
        public int Trade_Rewards_Itemlot_End { get; set; }
        public int Event_Rewards_Itemlot_Start { get; set; }
        public int Event_Rewards_Itemlot_End { get; set; }
        public int m10_02_00_00_Itemlot_Start { get; set; }
        public int m10_02_00_00_Itemlot_End { get; set; }
        public int m10_04_00_00_Itemlot_Start { get; set; }
        public int m10_04_00_00_Itemlot_End { get; set; }
        public int m10_10_00_00_Itemlot_Start { get; set; }
        public int m10_10_00_00_Itemlot_End { get; set; }
        public int m10_14_00_00_Itemlot_Start { get; set; }
        public int m10_14_00_00_Itemlot_End { get; set; }
        public int m10_15_00_00_Itemlot_Start { get; set; }
        public int m10_15_00_00_Itemlot_End { get; set; }
        public int m10_16_00_00_Itemlot_Start { get; set; }
        public int m10_16_00_00_Itemlot_End { get; set; }
        public int m10_17_00_00_Itemlot_Start { get; set; }
        public int m10_17_00_00_Itemlot_End { get; set; }
        public int m10_18_00_00_Itemlot_Start { get; set; }
        public int m10_18_00_00_Itemlot_End { get; set; }
        public int m10_19_00_00_Itemlot_Start { get; set; }
        public int m10_19_00_00_Itemlot_End { get; set; }
        public int m10_23_00_00_Itemlot_Start { get; set; }
        public int m10_23_00_00_Itemlot_End { get; set; }
        public int m10_25_00_00_Itemlot_Start { get; set; }
        public int m10_25_00_00_Itemlot_End { get; set; }
        public int m10_27_00_00_Itemlot_Start { get; set; }
        public int m10_27_00_00_Itemlot_End { get; set; }
        public int m10_29_00_00_Itemlot_Start { get; set; }
        public int m10_29_00_00_Itemlot_End { get; set; }
        public int m10_30_00_00_Itemlot_Start { get; set; }
        public int m10_30_00_00_Itemlot_End { get; set; }
        public int m10_31_00_00_Itemlot_Start { get; set; }
        public int m10_31_00_00_Itemlot_End { get; set; }
        public int m10_32_00_00_Itemlot_Start { get; set; }
        public int m10_32_00_00_Itemlot_End { get; set; }
        public int m10_33_00_00_Itemlot_Start { get; set; }
        public int m10_33_00_00_Itemlot_End { get; set; }
        public int m10_34_00_00_Itemlot_Start { get; set; }
        public int m10_34_00_00_Itemlot_End { get; set; }
        public int m20_10_00_00_Itemlot_Start { get; set; }
        public int m20_10_00_00_Itemlot_End { get; set; }
        public int m20_11_00_00_Itemlot_Start { get; set; }
        public int m20_11_00_00_Itemlot_End { get; set; }
        public int m20_21_00_00_Itemlot_Start { get; set; }
        public int m20_21_00_00_Itemlot_End { get; set; }
        public int m20_24_00_00_Itemlot_Start { get; set; }
        public int m20_24_00_00_Itemlot_End { get; set; }
        public int m20_26_00_00_Itemlot_Start { get; set; }
        public int m20_26_00_00_Itemlot_End { get; set; }
        public int m40_03_00_00_Itemlot_Start { get; set; }
        public int m40_03_00_00_Itemlot_End { get; set; }
        public int m50_35_00_00_Itemlot_Start { get; set; }
        public int m50_35_00_00_Itemlot_End { get; set; }
        public int m50_36_00_00_Itemlot_Start { get; set; }
        public int m50_36_00_00_Itemlot_End { get; set; }
        public int m50_37_00_00_Itemlot_Start { get; set; }
        public int m50_37_00_00_Itemlot_End { get; set; }


        public int Vengarl_Shoplot_Start { get; set; }
        public int Vengarl_Shoplot_End { get; set; }
        public int Agdayne_Shoplot_Start { get; set; }
        public int Agdayne_Shoplot_End { get; set; }
        public int Gilligan_Shoplot_Start { get; set; }
        public int Gilligan_Shoplot_End { get; set; }
        public List<int> Gilligan_Shop_Initial_Lots { get; set; }
        public int Wellager_Shoplot_Start { get; set; }
        public int Wellager_Shoplot_End { get; set; }
        public int Grandahl_Shoplot_Start { get; set; }
        public int Grandahl_Shoplot_End { get; set; }
        public int Gavlan_Shoplot_Start { get; set; }
        public int Gavlan_Shoplot_End { get; set; }
        public int Melentia_Shoplot_Start { get; set; }
        public int Melentia_Shoplot_End { get; set; }
        public int Rat_King_Shoplot_Start { get; set; }
        public int Rat_King_Shoplot_End { get; set; }
        public int Maughlin_Shoplot_Start { get; set; }
        public int Maughlin_Shoplot_End { get; set; }
        public int Chloanne_Shoplot_Start { get; set; }
        public int Chloanne_Shoplot_End { get; set; }
        public int Rosabeth_Shoplot_Start { get; set; }
        public int Rosabeth_Shoplot_End { get; set; }
        public int Lenigrast_Shoplot_Start { get; set; }
        public int Lenigrast_Shoplot_End { get; set; }
        public int McDuff_Shoplot_Start { get; set; }
        public int McDuff_Shoplot_End { get; set; }
        public int Carhillion_Shoplot_Start { get; set; }
        public int Carhillion_Shoplot_End { get; set; }
        public List<int> Carhillion_Shop_Initial_Lots { get; set; }
        public List<int> Carhillion_Shop_Secondary_Lots { get; set; }
        public int Straid_Shoplot_Start { get; set; }
        public int Straid_Shoplot_End { get; set; }
        public int Licia_Shoplot_Start { get; set; }
        public int Licia_Shoplot_End { get; set; }
        public List<int> Licia_Shop_Initial_Lots { get; set; }
        public List<int> Licia_Shop_Secondary_Lots { get; set; }
        public int Felkin_Shoplot_Start { get; set; }
        public int Felkin_Shoplot_End { get; set; }
        public int Navlaan_Shoplot_Start { get; set; }
        public int Navlaan_Shoplot_End { get; set; }
        public int Magerold_Shoplot_Start { get; set; }
        public int Magerold_Shoplot_End { get; set; }
        public int Ornifex_Shoplot_Start { get; set; }
        public int Ornifex_Shoplot_End { get; set; }
        public int Shalquior_Shoplot_Start { get; set; }
        public int Shalquior_Shoplot_End { get; set; }
        public int Titchy_Shoplot_Start { get; set; }
        public int Titchy_Shoplot_End { get; set; }
        public int Cromwell_Shoplot_Start { get; set; }
        public int Cromwell_Shoplot_End { get; set; }
        public int Targray_Shoplot_Start { get; set; }
        public int Targray_Shoplot_End { get; set; }

        public int Straid_Bosslot_Start { get; set; }
        public int Straid_Bosslot_End { get; set; }
        public int Ornifex_Bosslot_Start { get; set; }
        public int Ornifex_Bosslot_End { get; set; }
        public int Ornifex_Free_Bosslot_Start { get; set; }
        public int Ornifex_Free_Bosslot_End { get; set; }

        public int Enemy_Drop_Itemlot_Start { get; set; }
        public int Enemy_Drop_Itemlot_End { get; set; }

        public List<int> Blacklist_Weapons { get; set; }
        public List<int> Blacklist_Armor { get; set; }
        public List<int> Blacklist_Spells { get; set; }
        public List<int> Blacklist_Rings { get; set; }
        public List<int> Blacklist_Items { get; set; }
        public List<int> Blacklist_Itemlots { get; set; }
        public List<int> Blacklist_Itemlot_Contents { get; set; }
        public List<int> Blacklist_Itemlots_for_Keys { get; set; }

        public List<int> Category_Ammo { get; set; }
        public List<int> Category_Boss_Soul { get; set; }
        public List<int> Category_Consumable { get; set; }
        public List<int> Category_Gesture { get; set; }
        public List<int> Category_Key_Item { get; set; }
        public List<int> Category_Material { get; set; }
        public List<int> Category_Multiplayer_Item { get; set; }
        public List<int> Category_Tool_Item { get; set; }
        public List<int> Category_Fists { get; set; }
        public List<int> Category_Catalysts { get; set; }
        public List<int> Category_Sorcery_Catalysts { get; set; }
        public List<int> Category_Miracle_Catalysts { get; set; }
        public List<int> Category_Pyromancy_Catalysts { get; set; }
        public List<int> Category_Hex_Catalysts { get; set; }

        public List<int> Assignment_Category_General_Consumable { get; set; }
        public List<int> Assignment_Category_Consumable_Soul { get; set; }
        public List<int> Assignment_Category_Throwable_Item { get; set; }
        public List<int> Assignment_Category_HP_Consumable { get; set; }
        public List<int> Assignment_Category_Cast_Consumable { get; set; }
        public List<int> Assignment_Category_Spell_Tier_Consumable { get; set; }
        public List<int> Assignment_Category_Flask_Tier_Consumable { get; set; }
        public List<int> Assignment_Category_Trade_Consumable { get; set; }

        public KeyItem Soldier_Key { get; set; }
        public KeyItem Dull_Ember { get; set; }
        public KeyItem Aldia_Key { get; set; }
        public KeyItem Ashen_Mist_Heart { get; set; }
        public KeyItem Giants_Kinship { get; set; }
        public KeyItem Rotunda_Lockstone { get; set; }
        public KeyItem Lenigrasts_Key { get; set; }
        public KeyItem House_Key { get; set; }
        public KeyItem Antiquated_Key { get; set; }
        public KeyItem Brightstone_Key { get; set; }
        public KeyItem Bastille_Key { get; set; }
        public KeyItem Tseldora_Den_Key { get; set; }
        public KeyItem Fang_Key { get; set; }
        public KeyItem Iron_Key { get; set; }
        public KeyItem Forgotten_Key { get; set; }
        public KeyItem Key_to_the_Kings_Passage { get; set; }
        public KeyItem Undead_Lockaway_Key { get; set; }
        public KeyItem Eternal_Sanctum_Key { get; set; }
        public KeyItem Dragon_Stone { get; set; }
        public KeyItem Scorching_Iron_Scepter { get; set; }
        public KeyItem Tower_Key { get; set; }
        public KeyItem Garrison_Ward_Key { get; set; }
        public KeyItem Dragon_Talon { get; set; }
        public KeyItem Heavy_Iron_Key { get; set; }
        public KeyItem Frozen_Flower { get; set; }
        public KeyItem Key_to_the_Embedded { get; set; }
        public KeyItem King_Ring { get; set; }


        public ToolItem Aged_Feather { get; set; }
        public ToolItem Champion_Tablet { get; set; }
        public ToolItem Dragon_Head_Stone { get; set; }
        public ToolItem Dragon_Torso_Stone { get; set; }
        public ToolItem Hello_Carving { get; set; }
        public ToolItem Thank_You_Carving { get; set; }
        public ToolItem Im_Sorry_Carving { get; set; }
        public ToolItem Very_Good_Carving { get; set; }

        public ConsumableItem Pharros_Lockstone { get; set; }
        public ConsumableItem Fragrant_Branch_of_Yore { get; set; }

        public static ScramblerData_Items Static { get; }

        static ScramblerData_Items()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\items.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ScramblerData_Items>(File.OpenRead(json_filepath), options);
        }
    }

    public class KeyItem
    {
        public int ID { get; set; }
        public List<string> Maps { get; set; }
    }

    public class ToolItem
    {
        public int ID { get; set; }
        public List<string> Maps { get; set; }
    }

    public class ConsumableItem
    {
        public int ID { get; set; }
        public List<string> Maps { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
