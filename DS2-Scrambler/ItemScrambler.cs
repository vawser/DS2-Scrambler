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
using static SoulsFormats.MSBE.Event;
using System.Reflection;
using Org.BouncyCastle.Crypto;
using static SoulsFormats.LUAINFO;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

namespace DS2_Scrambler
{
    public class ItemScrambler
    {
        public Random rand;
        public Regulation regulation;
        public ScramblerData Data;

        public bool T_Ignore_Keys = false;
        public bool T_Ignore_Tools = false;
        public bool T_Include_Boss_Treasure = false;
        public bool T_Include_Character_Treasure = false;
        public bool T_Include_Covenant_Treasure = false;
        public bool T_Include_Bird_Treasure = false;
        public bool T_Include_Event_Treasure = false;

        public List<PARAM.Row> Unassigned_Weapons;
        public List<PARAM.Row> Unassigned_Armor;
        public List<PARAM.Row> Unassigned_Spells;
        public List<PARAM.Row> Unassigned_Rings;

        public List<PARAM.Row> Assigned_Itemlots;

        // TODO: add cheat-sheet generation so important items can be peeked at - At minimum this should pinpoint keys
        // TODO: add 'bucket' system for item assignment so the items are distributed fairly across all maps - this is to prevent bias towards the first maps in the randomisation order
        // TODO: add check to merchants so infinite items always have a price to buy
        // TODO: add option to retain lifegems at early-game merchant

        public ItemScrambler(Random random, Regulation reg, ScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;

            Unassigned_Weapons = new List<PARAM.Row>(Data.Row_List_Weapons);
            Unassigned_Armor = new List<PARAM.Row>(Data.Row_List_Armor);
            Unassigned_Spells = new List<PARAM.Row>(Data.Row_List_Spells);
            Unassigned_Rings = new List<PARAM.Row>(Data.Row_List_Rings);

            Assigned_Itemlots = new List<PARAM.Row>();
        }

        public Regulation Scramble_Loot(bool scrambleLoot, bool scrambleEnemyDrops, bool scrambleShops, bool scrambleBossTrades, bool includeBossTreasure, bool includeCharacterTreasure, bool includeCovenantTreasure, bool includeBirdTreasure, bool includeEventTreasure, bool ignoreKeys, bool ignoreTools)
        {
            T_Ignore_Keys = ignoreKeys;
            T_Ignore_Tools = ignoreTools;
            T_Include_Boss_Treasure = includeBossTreasure;
            T_Include_Character_Treasure = includeCharacterTreasure;
            T_Include_Covenant_Treasure = includeCovenantTreasure;
            T_Include_Bird_Treasure = includeBirdTreasure;
            T_Include_Event_Treasure = includeEventTreasure;

            if (scrambleLoot)
            {
                Console.WriteLine("Item Scramble: Structured Loot");

                if (!T_Ignore_Keys)
                {
                    ScrambleKeyItems();
                }

                if (!T_Ignore_Tools)
                {
                    ScrambleToolItems();
                }

                Console.WriteLine("Item Scramble: Random Loot");

                if (T_Include_Boss_Treasure)
                {
                    ScrambleBossRewards(Data.Itemlot_List_Boss_Drops);
                    ScrambleBossSoulItems();
                }
                if (T_Include_Character_Treasure)
                {
                    ScrambleCharacterRewards(Data.Itemlot_List_NPC_Rewards);
                }
                if (T_Include_Covenant_Treasure)
                {
                    ScrambleCovenantRewards(Data.Itemlot_List_Covenant_Rewards);
                }
                if (T_Include_Bird_Treasure)
                {
                    ScrambleBirdRewards(Data.Itemlot_List_Bird_Rewards);
                }
                if (T_Include_Event_Treasure)
                {
                    ScrambleEventRewards(Data.Itemlot_List_Event_Rewards);
                }

                ScrambleMapLoot(Data.Itemlot_List_Things_Betwixt);
                ScrambleMapLoot(Data.Itemlot_List_Majula);
                ScrambleMapLoot(Data.Itemlot_List_Forest_of_Fallen_Giants);
                ScrambleMapLoot(Data.Itemlot_List_Brightstone_Cove_Tseldora);
                ScrambleMapLoot(Data.Itemlot_List_Aldias_Keep);
                ScrambleMapLoot(Data.Itemlot_List_Lost_Bastille);
                ScrambleMapLoot(Data.Itemlot_List_Earthen_Peak);
                ScrambleMapLoot(Data.Itemlot_List_No_Mans_Wharf);
                ScrambleMapLoot(Data.Itemlot_List_Iron_Keep);
                ScrambleMapLoot(Data.Itemlot_List_Huntmans_Copse);
                ScrambleMapLoot(Data.Itemlot_List_The_Gutter);
                ScrambleMapLoot(Data.Itemlot_List_Dragon_Aerie);
                ScrambleMapLoot(Data.Itemlot_List_Path_to_the_Shaded_Woods);
                ScrambleMapLoot(Data.Itemlot_List_Unseen_Path_to_Heide);
                ScrambleMapLoot(Data.Itemlot_List_Heides_Tower_of_Flame);
                ScrambleMapLoot(Data.Itemlot_List_Shaded_Woods);
                ScrambleMapLoot(Data.Itemlot_List_Doors_of_Pharros);
                ScrambleMapLoot(Data.Itemlot_List_Grave_of_Saints);
                ScrambleMapLoot(Data.Itemlot_List_Giants_Memory);
                ScrambleMapLoot(Data.Itemlot_List_Shrine_of_Amana);
                ScrambleMapLoot(Data.Itemlot_List_Drangleic_Castle);
                ScrambleMapLoot(Data.Itemlot_List_Undead_Crypt);
                ScrambleMapLoot(Data.Itemlot_List_Dragon_Memories);
                ScrambleMapLoot(Data.Itemlot_List_Chasm_of_the_Abyss);
                ScrambleMapLoot(Data.Itemlot_List_Shulva);
                ScrambleMapLoot(Data.Itemlot_List_Brume_Tower);
                ScrambleMapLoot(Data.Itemlot_List_Things_Betwixt);
                ScrambleMapLoot(Data.Itemlot_List_Eleum_Loyce);

                if (scrambleEnemyDrops)
                {
                    ScrambleEnemyDrops(Data.ItemlotParam_Chr.Rows.Where(row => row.ID >= 10000000 && row.ID <= 89800000).ToList());
                }

                if (scrambleShops)
                {
                    ScrambleShops();
                }

                if (scrambleBossTrades)
                {
                    ScrambleBossSoulTrades();
                }
            }

            return regulation;
        }

        public void ScrambleKeyItems()
        {
            // Soldier Key
            if (T_Include_Event_Treasure || T_Include_Boss_Treasure) // Only include this if Boss/Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50600000).ToList(), Data.Row_List_Soldier_Key, 0, 1);

            // Dull Ember
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50990000).ToList(), Data.Row_List_Dull_Ember, 0, 1);

            // Aldia Key
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 51030000).ToList(), Data.Row_List_Aldia_Key, 0, 1);

            // Ashen Mist Heart
            if (T_Include_Event_Treasure || T_Include_Character_Treasure) // Only include this if Character/Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50910000).ToList(), Data.Row_List_Ashen_Mist_Heart, 0, 1);

            // Giant's Kinship
            if (T_Include_Event_Treasure || T_Include_Boss_Treasure) // Only include this if Boss/Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50900000).ToList(), Data.Row_List_Giants_Kinship, 0, 1);

            // Rotunda Lockstone
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50890000).ToList(), Data.Row_List_Rotunda_Lockstone, 0, 1);

            // Lenigrasts Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50870000).ToList(), Data.Row_List_Lenigrasts_Key, 0, 1);

            // House Key
            if (T_Include_Character_Treasure) // Only include this if Character Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50860000).ToList(), Data.Row_List_House_Key, 0, 1);

            // Antiquated Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50840000).ToList(), Data.Row_List_Antiquated_Key, 0, 1);

            // Brightstone Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50830000).ToList(), Data.Row_List_Brightstone_Key, 0, 1);

            // Bastille Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50800000).ToList(), Data.Row_List_Bastille_Key, 0, 1);

            // Tseldora Den Key
            if (T_Include_Character_Treasure) // Only include this if Character Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50930000).ToList(), Data.Row_List_Tseldora_Den_Key, 0, 1);

            // Fang Key
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50850000).ToList(), Data.Row_List_Fang_Key, 0, 1);

            // Iron Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50810000).ToList(), Data.Row_List_Iron_Key, 0, 1);

            // Forgotten Key
            if(T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50820000).ToList(), Data.Row_List_Forgotten_Key, 0, 1);

            // Key to the Kings Passage
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50610000).ToList(), Data.Row_List_Key_to_the_Kings_Passage, 0, 1);

            // Undead Lockaway Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50970000).ToList(), Data.Row_List_Undead_Lockaway_Key, 0, 1);

            // Eternal Sanctum Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52300000).ToList(), Data.Row_List_Eternal_Sanctum_Key, 0, 1);

            // Dragon Stone
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52650000).ToList(), Data.Row_List_Dragon_Stone, 0, 1);

            // Scorching Iron Scepter
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53100000).ToList(), Data.Row_List_Scorching_Iron_Scepter, 0, 1);

            // Tower Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52400000).ToList(), Data.Row_List_Tower_Key, 0, 1);

            // Eye of the Priestess
            // AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53600000).ToList(), Data.Row_List_Eye_of_the_Priestess, 0, 1);

            // Garrison Ward Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52500000).ToList(), Data.Row_List_Garrison_Ward_Key, 0, 1);

            // Dragon Talon
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52000000).ToList(), Data.Row_List_Dragon_Talon, 0, 1);

            // Heavy Iron Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52100000).ToList(), Data.Row_List_Heavy_Iron_Key, 0, 1);

            // Frozen Flower
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 52200000).ToList(), Data.Row_List_Frozen_Flower, 0, 1);

            // Key to the Embedded
            if (T_Include_Boss_Treasure) // Only include this if Boss Treasure is randomised
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 1980000).ToList(), Data.Row_List_Key_to_the_Embedded, 0, 1);
        }

        public void AssignKeyToItemlot(List<PARAM.Row> item, List<PARAM.Row> rows, int slot, int amount)
        {
            if(rows.Count < 1)
            {
                Console.WriteLine("AssignKeyToItemlot: passed rows is empty.");
                return;
            }

            Random rand = new Random();
            bool can_assign = false;

            int index = 0;

            // Only use the itemlot if it has not been previously used
            while (!can_assign)
            {
                index = rand.Next(rows.Count);

                PARAM.Row chosen_row = rows[index];

                // Do not assign if the itemlot has already been used, or it has a never-change item in it.
                if (!Assigned_Itemlots.Contains(chosen_row) &&
                    !HasMatchingLot(Data.Itemlot_Info_Never_Change, chosen_row)
                )
                    can_assign = true;
            }

            // Add the item to the itemlot
            rows[index][$"item_lot_{slot}"].Value = item[0].ID;
            rows[index][$"amount_lot_{slot}"].Value = amount;

            // Add the itemlot to the assigned list
            Assigned_Itemlots.Add(rows[index]);
        }

        public void ScrambleToolItems()
        {
            // Aged Feather
            if (T_Include_Character_Treasure) // Only include this if Character Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60355000).ToList(), Data.Itemlot_List_Dragon_Aerie, 0, 1);

            // Champion's Tablet
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 50940000).ToList(), Data.Itemlot_List_Drangleic_Castle, 0, 1);

            // Dragon Head Stone
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60405000).ToList(), Data.Itemlot_List_Iron_Keep, 0, 1);

            // Dragon Torso Stone
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60406000).ToList(), Data.Itemlot_List_Iron_Keep, 0, 1);

            // Hello Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60470000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1);

            // Thank You Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60480000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1);

            // I'm Sorry Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60490000).ToList(), Data.Itemlot_List_The_Gutter, 0, 1);

            // Very Good! Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60500000).ToList(), Data.Itemlot_List_Drangleic_Castle, 0, 1);

            // Pharros' Lockstone
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Things_Betwixt, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Majula, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Brightstone_Cove_Tseldora, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Aldias_Keep, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Lost_Bastille, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Earthen_Peak, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_No_Mans_Wharf, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Iron_Keep, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Huntmans_Copse, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_The_Gutter, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Dragon_Aerie, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Heides_Tower_of_Flame, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Shaded_Woods, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Doors_of_Pharros, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Grave_of_Saints, 0, 1, 2);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Shrine_of_Amana, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Drangleic_Castle, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Undead_Crypt, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Shulva, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60536000).ToList(), Data.Itemlot_List_Eleum_Loyce, 0, 1);

            // Fragrant Branch of Yore
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Things_Betwixt.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Majula.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Heides_Tower_of_Flame.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Lost_Bastille.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Lost_Bastille.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_The_Gutter.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Shaded_Woods.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Shaded_Woods.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Shaded_Woods.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Drangleic_Castle.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Drangleic_Castle.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Shrine_of_Amana.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Shrine_of_Amana.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Brightstone_Cove_Tseldora.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Iron_Keep.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Dragon_Aerie.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60537000).ToList(), Data.Itemlot_List_Huntmans_Copse.Where(row => !Data.Itemlot_Info_Forbidden_For_Keys.Contains(row.ID)).ToList(), 0, 1);

            // Smelter Wedge
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53200000).ToList(), Data.Itemlot_List_Brume_Tower, 0, 1);

            // *** Estus Flask Shard
            // 1
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Things_Betwixt, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Majula, 0, 1);

            // 2
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Brightstone_Cove_Tseldora, 0, 1);

            // 3
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Aldias_Keep, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Lost_Bastille, 0, 1);

            // 4
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Earthen_Peak, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_No_Mans_Wharf, 0, 1);

            // 5
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Iron_Keep, 0, 1);

            // 6
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Huntmans_Copse, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_The_Gutter, 0, 1);

            // 7
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Dragon_Aerie, 0, 1);

            // 8
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Heides_Tower_of_Flame, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Shaded_Woods, 0, 1);

            // 9
            if (rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Doors_of_Pharros, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Grave_of_Saints, 0, 1);

            // 10
            AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Shrine_of_Amana, 0, 1);

            // 11
            if(rand.Next(100) >= 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Drangleic_Castle, 0, 1);
            else
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60525000).ToList(), Data.Itemlot_List_Undead_Crypt, 0, 1);

            // *** Sublime Bone Dust
            // 1
            int roll = rand.Next(100);

            if (roll >= 0 && roll < 25)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Things_Betwixt, 0, 1);
            else if (roll >= 25 && roll < 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Majula, 0, 1);
            else if (roll >= 50 && roll < 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1);
            else if (roll >= 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Brightstone_Cove_Tseldora, 0, 1);

            // 2
            roll = rand.Next(100);

            if (roll >= 0 && roll < 25)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Aldias_Keep, 0, 1);
            else if (roll >= 25 && roll < 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Lost_Bastille, 0, 1);
            else if (roll >= 50 && roll < 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Earthen_Peak, 0, 1);
            else if (roll >= 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_No_Mans_Wharf, 0, 1);

            // 3
            roll = rand.Next(100);

            if (roll >= 0 && roll < 25)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Iron_Keep, 0, 1);
            else if (roll >= 25 && roll < 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Huntmans_Copse, 0, 1);
            else if (roll >= 50 && roll < 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_The_Gutter, 0, 1);
            else if (roll >= 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Heides_Tower_of_Flame, 0, 1);

            // 4
            roll = rand.Next(100);

            if (roll >= 0 && roll < 25)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Dragon_Aerie, 0, 1);
            else if (roll >= 25 && roll < 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Shaded_Woods, 0, 1);
            else if (roll >= 50 && roll < 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Doors_of_Pharros, 0, 1);
            else if (roll >= 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Grave_of_Saints, 0, 1);

            // 5
            roll = rand.Next(100);

            if (roll >= 0 && roll < 25)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Shrine_of_Amana, 0, 1);
            else if (roll >= 25 && roll < 50)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Drangleic_Castle, 0, 1);
            else if (roll >= 50 && roll < 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Undead_Crypt, 0, 1);
            else if (roll >= 75)
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 60526000).ToList(), Data.Itemlot_List_Forest_of_Fallen_Giants, 0, 1);

        }

        public void AssignToolToItemlot(List<PARAM.Row> item, List<PARAM.Row> rows, int slot, int amount, int max_amount = 0)
        {
            if (rows.Count < 1)
            {
                Console.WriteLine("AssignToolToItemlot: passed rows is empty.");
                return;
            }

            Random rand = new Random();
            bool can_assign = false;

            int index = 0;

            // Only use the itemlot if it has not been previously used
            while (!can_assign)
            {
                index = rand.Next(rows.Count);

                PARAM.Row chosen_row = rows[index];

                // Do not assign if the itemlot has already been used, or it has a never-change item in it or an existing key itemlot
                if (!Assigned_Itemlots.Contains(chosen_row) &&
                    !HasMatchingLot(Data.Itemlot_Info_Never_Change, chosen_row) &&
                    (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, chosen_row))
                )
                    can_assign = true;
            }

            // Add the item to the itemlot
            rows[index][$"item_lot_{slot}"].Value = item[0].ID;

            if(max_amount > 0)
                rows[index][$"amount_lot_{slot}"].Value = rand.Next(amount, max_amount);
            else
                rows[index][$"amount_lot_{slot}"].Value = amount;

            // Add the itemlot to the assigned list
            Assigned_Itemlots.Add(rows[index]);
        }

        public void ScrambleBossSoulItems()
        {
            // Soul of Nadalia, Bride of Ash
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 53300000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the Pursuer
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64000000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the Last Giant
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64010000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Dragonrider Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64020000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Old Dragonslayer Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64030000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Flexile Sentry Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64040000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Ruin Sentinel Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64050000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the Lost Sinner
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64060000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Executioner's Chariot Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64070000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Skeleton Lord's Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64080000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Covetous Demon Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64090000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Mytha, the Baneful Queen Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64100000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Smelter Demon Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64110000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Old Iron King Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64120000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Royal Rat Vanguard Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64130000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the Rotten
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64140000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Scorpioness Najka Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64150000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Royal Rat Authority Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64160000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the Duke's Dear Freja
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64170000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Looking Glass Knight Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64180000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Demon of Song Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64190000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of Velstadt
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64200000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of the King
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64210000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Guardian Dragon Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64220000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Ancient Dragon Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64230000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Giant Lord Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64240000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Soul of Nashandra
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64250000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Throne Defender Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64260000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Throne Watcher Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64270000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Darklurker Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64280000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Belfry Gargoyle Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64290000).ToList(), Data.Itemlot_List_Vanilla, 0, 1);

            // Old Witch Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64300000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Old King Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64310000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Old Dead One Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64320000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Old Paledrake Soul
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64330000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Sinh, the Slumbering Dragon
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64500000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of the Fume Knight
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64510000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Aava, the King's Pet
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64520000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Elana, Squalid Queen
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64530000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Nadalia, Bride of Ash
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64540000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Alsanna, Silent Oracle
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64550000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Sir Alonne
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64560000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of the Ivory King
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64580000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Zallen, the King's Pet
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64590000).ToList(), Data.Itemlot_List_DLC, 0, 1);

            // Soul of Lud, the King's Pet
            AssignBossSoulToItemlot(Data.ItemParam.Rows.Where(row => row.ID == 64610000).ToList(), Data.Itemlot_List_DLC, 0, 1);
        }

        public void AssignBossSoulToItemlot(List<PARAM.Row> item, List<PARAM.Row> rows, int slot, int amount)
        {
            if (rows.Count < 1)
            {
                Console.WriteLine("AssignBossSoulToItemlot: passed rows is empty.");
                return;
            }

            Random rand = new Random();
            bool can_assign = false;

            int index = 0;

            // Only use the itemlot if it has not been previously used
            while (!can_assign)
            {
                index = rand.Next(rows.Count);

                PARAM.Row chosen_row = rows[index];

                // Do not assign if the itemlot has already been used, or it has a never-change item in it or an existing key itemlot or an existing tool itemlot
                if (!Assigned_Itemlots.Contains(chosen_row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, chosen_row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, chosen_row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, chosen_row))
                )
                    can_assign = true;
            }

            // Add the item to the itemlot
            rows[index][$"item_lot_{slot}"].Value = item[0].ID;
            rows[index][$"amount_lot_{slot}"].Value = amount;

            // Add the itemlot to the assigned list
            Assigned_Itemlots.Add(rows[index]);
        }

        public void ScrambleBossRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectUniqueItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleCharacterRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleCovenantRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectUniqueItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleBirdRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleEventRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleMapLoot(List<PARAM.Row> itemlots)
        {
            foreach(PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if(!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectItemForItemlot(row, 10, false);
            }
        }

        public void ScrambleEnemyDrops(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(Data.Itemlot_Info_Never_Change, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(Data.ID_List_Keys, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(Data.ID_List_Tools, row)))
                    SelectItemForItemlot(row, 60510000, true);
            }
        }

        public void ScrambleShops()
        {

        }

        public void ScrambleBossSoulTrades()
        { 

        }

        public void SelectUniqueItemForItemlot(PARAM.Row row, int EmptyItemID, bool applyEnemyChanceReduction)
        {
            Random rand = new Random();

            for (int slot = 0; slot <= 9; slot++)
            {
                int roll = rand.Next(100);

                int item_lot_value = (int)row[$"item_lot_{slot}"].Value;

                // Item lot slot is not empty
                if (item_lot_value != EmptyItemID)
                {
                    // Weapon
                    if (roll >= 0 && roll <= 25)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Weapons, Data.Row_List_Weapons, applyEnemyChanceReduction);
                    }
                    // Armor
                    else if (roll >= 25 && roll <= 50)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Armor, Data.Row_List_Armor, applyEnemyChanceReduction);
                    }
                    // Spells
                    else if (roll >= 50 && roll <= 75)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Spells, Data.Row_List_Spells, applyEnemyChanceReduction);
                    }
                    // Rings
                    else if (roll >= 75)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Rings, Data.Row_List_Rings, applyEnemyChanceReduction);
                    }
                }
            }

            Assigned_Itemlots.Add(row);
        }

        public void SelectItemForItemlot(PARAM.Row row, int EmptyItemID, bool applyEnemyChanceReduction)
        {
            Random rand = new Random();

            for (int slot = 0; slot <= 9; slot++)
            {
                int roll = rand.Next(100);

                int item_lot_value = (int)row[$"item_lot_{slot}"].Value;

                // Item lot slot is not empty
                if (item_lot_value != EmptyItemID)
                {
                    // Weapon
                    if (roll >= 0 && roll <= 20)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Weapons, Data.Row_List_Weapons, applyEnemyChanceReduction);
                    }
                    // Armor
                    else if (roll >= 10 && roll <= 20)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Armor, Data.Row_List_Armor, applyEnemyChanceReduction);
                    }
                    // Spells
                    else if (roll >= 20 && roll <= 30)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Spells, Data.Row_List_Spells, applyEnemyChanceReduction);
                    }
                    // Rings
                    else if (roll >= 30 && roll <= 40)
                    {
                        AddItemToItemlot(row, slot, Unassigned_Rings, Data.Row_List_Rings, applyEnemyChanceReduction);
                    }
                    // Item: Ammunition
                    else if (roll >= 40 && roll <= 45)
                    {
                        AddItemToItemlot(row, slot, null, Data.Row_List_Ammunition, applyEnemyChanceReduction, 10, 50);
                    }
                    // Item: Material
                    else if (roll >= 45 && roll <= 50)
                    {
                        AddItemToItemlot(row, slot, null, Data.Row_List_Materials, applyEnemyChanceReduction, 1, 3);
                    }
                    // Item: Soul/Spices
                    else if (roll >= 50 && roll <= 60)
                    {
                        AddItemToItemlot(row, slot, null, Data.Row_List_Soul_Consumables, applyEnemyChanceReduction, 1, 3);
                    }
                    // Item: Throwable
                    else if (roll >= 60 && roll <= 70)
                    {
                        AddItemToItemlot(row, slot, null, Data.Row_List_Throwable_Consumable, applyEnemyChanceReduction, 1, 3);
                    }
                    // Item: Spice/Bird
                    else if (roll >= 70 && roll <= 80)
                    {
                        if(roll >= 75)
                            AddItemToItemlot(row, slot, null, Data.Row_List_Bird_Consumables, applyEnemyChanceReduction, 1, 3);
                        else
                            AddItemToItemlot(row, slot, null, Data.Row_List_Spell_Upgrades, applyEnemyChanceReduction, 1, 3);
                    }
                    // Item: HP/Cast
                    else if (roll >= 80 && roll <= 90)
                    {
                        if(roll >= 85)
                            AddItemToItemlot(row, slot, null, Data.Row_List_HP_Consumables, applyEnemyChanceReduction, 1, 3);
                        else
                            AddItemToItemlot(row, slot, null, Data.Row_List_Cast_Consumables, applyEnemyChanceReduction, 1, 3);
                    }
                    // Item: Misc
                    else if (roll >= 90)
                    {
                        AddItemToItemlot(row, slot, null, Data.Row_List_Misc_Consumable, applyEnemyChanceReduction, 1, 5);
                    }
                }
            }

            Assigned_Itemlots.Add(row);
        }

        public void AddItemToItemlot(PARAM.Row row, int slot, List<PARAM.Row> unassigned_list, List<PARAM.Row> fallback_list, bool applyEnemyChanceReduction, int min = 0, int max = 0)
        {
            Random rand = new Random();

            PARAM.Row value = null;

            // Use this list first so each weapon appears at least once.
            if (unassigned_list != null && unassigned_list.Count > 0)
            {
                value = unassigned_list[rand.Next(unassigned_list.Count)];

                row[$"item_lot_{slot}"].Value = value.ID;
            }
            // After all weapons are present, allow duplicates
            else
            {
                value = fallback_list[rand.Next(fallback_list.Count)];

                row[$"item_lot_{slot}"].Value = value.ID;
            }

            if (min > 0 && max > 0)
                row[$"amount_lot_{slot}"].Value = rand.Next(min, max);
            else
                row[$"amount_lot_{slot}"].Value = 1;

            // For enemy drop lots
            if (applyEnemyChanceReduction)
            {
                // If the chance is 100
                if ((float)row[$"chance_lot_{slot}"].Value == 100)
                {
                    // And the item is not a 'item', then reduce the chance to a more suitable level
                    if (!Data.Row_List_Items.Any(row => row.ID == value.ID))
                        row[$"chance_lot_{slot}"].Value = (float)rand.Next(1, 10);
                }
            }

            if (unassigned_list != null && value != null)
                unassigned_list.Remove(value);
        }

        public bool HasMatchingLot(List<int> list, PARAM.Row row)
        {
            bool match = false;

            for (int slot = 0; slot <= 9; slot++)
            {
                if (list.Contains((int)row[$"item_lot_{slot}"].Value))
                    match = true;
            }

            return match;
        }
    }
}
