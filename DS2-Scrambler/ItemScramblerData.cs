using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DS2_Scrambler
{
    public class ItemScramblerData
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

        public static ItemScramblerData Static { get; }

        static ItemScramblerData()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\item_scrambler_data.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ItemScramblerData>(File.OpenRead(json_filepath), options);
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
