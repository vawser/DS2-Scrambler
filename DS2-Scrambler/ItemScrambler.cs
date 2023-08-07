using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

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
        public bool T_Ensure_Lifegems = false;
        public bool T_Retain_Shop_Spread = false;

        public List<PARAM.Row> Unassigned_Weapons;
        public List<PARAM.Row> Unassigned_Armor;
        public List<PARAM.Row> Unassigned_Spells;
        public List<PARAM.Row> Unassigned_Rings;

        public List<PARAM.Row> Assigned_Itemlots;

        // TODO: add cheat-sheet generation so important items can be peeked at - At minimum this should pinpoint keys

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

        public Regulation Scramble_Loot(bool scrambleLoot, bool scrambleEnemyDrops, bool scrambleShops, bool scrambleBossTrades, bool includeBossTreasure, bool includeCharacterTreasure, bool includeCovenantTreasure, bool includeBirdTreasure, bool includeEventTreasure, bool ignoreKeys, bool ignoreTools, bool ensureLifegems, bool retainShopSpread)
        {
            T_Ignore_Keys = ignoreKeys;
            T_Ignore_Tools = ignoreTools;
            T_Include_Boss_Treasure = includeBossTreasure;
            T_Include_Character_Treasure = includeCharacterTreasure;
            T_Include_Covenant_Treasure = includeCovenantTreasure;
            T_Include_Bird_Treasure = includeBirdTreasure;
            T_Include_Event_Treasure = includeEventTreasure;
            T_Ensure_Lifegems = ensureLifegems;
            T_Retain_Shop_Spread = retainShopSpread;

            if (scrambleLoot)
            {
                Util.PrintLine("Item Scramble: Structured Loot");

                if (!T_Ignore_Keys)
                {
                    ScrambleKeyItems();
                }

                if (!T_Ignore_Tools)
                {
                    ScrambleToolItems();
                }

                Util.PrintLine("Item Scramble: Random Loot");

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
                    ScrambleEnemyDrops(Data.ItemlotParam_Chr.Rows.Where(row => 
                    row.ID >= ScramblerData_Items.Static.Enemy_Drop_Itemlot_Start && 
                    row.ID <= ScramblerData_Items.Static.Enemy_Drop_Itemlot_End
                    ).ToList());
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
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Soldier_Key.ID
                ).ToList(), Data.Row_List_Soldier_Key, 0, 1);
            }

            // Dull Ember
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Dull_Ember.ID
            ).ToList(), Data.Row_List_Dull_Ember, 0, 1);

            // Aldia Key
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Aldia_Key.ID
                ).ToList(), Data.Row_List_Aldia_Key, 0, 1);
            }

            // Ashen Mist Heart
            if (T_Include_Event_Treasure || T_Include_Character_Treasure) // Only include this if Character/Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Ashen_Mist_Heart.ID
                ).ToList(), Data.Row_List_Ashen_Mist_Heart, 0, 1);
            }

            // Giant's Kinship
            if (T_Include_Event_Treasure || T_Include_Boss_Treasure) // Only include this if Boss/Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Giants_Kinship.ID
                ).ToList(), Data.Row_List_Giants_Kinship, 0, 1);
            }

            // Rotunda Lockstone
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Rotunda_Lockstone.ID
                ).ToList(), Data.Row_List_Rotunda_Lockstone, 0, 1);
            }

            // Lenigrasts Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Lenigrasts_Key.ID
            ).ToList(), Data.Row_List_Lenigrasts_Key, 0, 1);

            // House Key
            if (T_Include_Character_Treasure) // Only include this if Character Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.House_Key.ID
                ).ToList(), Data.Row_List_House_Key, 0, 1);
            }

            // Antiquated Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Antiquated_Key.ID
            ).ToList(), Data.Row_List_Antiquated_Key, 0, 1);

            // Brightstone Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Brightstone_Key.ID
            ).ToList(), Data.Row_List_Brightstone_Key, 0, 1);

            // Bastille Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Bastille_Key.ID
            ).ToList(), Data.Row_List_Bastille_Key, 0, 1);

            // Tseldora Den Key
            if (T_Include_Character_Treasure) // Only include this if Character Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Tseldora_Den_Key.ID
                ).ToList(), Data.Row_List_Tseldora_Den_Key, 0, 1);
            }

            // Fang Key
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Fang_Key.ID
                ).ToList(), Data.Row_List_Fang_Key, 0, 1);
            }

            // Iron Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Iron_Key.ID
            ).ToList(), Data.Row_List_Iron_Key, 0, 1);

            // Forgotten Key
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Forgotten_Key.ID
                ).ToList(), Data.Row_List_Forgotten_Key, 0, 1);
            }

            // Key to the Kings Passage
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Key_to_the_Kings_Passage.ID
            ).ToList(), Data.Row_List_Key_to_the_Kings_Passage, 0, 1);

            // Undead Lockaway Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Undead_Lockaway_Key.ID
            ).ToList(), Data.Row_List_Undead_Lockaway_Key, 0, 1);

            // Eternal Sanctum Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Eternal_Sanctum_Key.ID
            ).ToList(), Data.Row_List_Eternal_Sanctum_Key, 0, 1);

            // Dragon Stone
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Dragon_Stone.ID
            ).ToList(), Data.Row_List_Dragon_Stone, 0, 1);

            // Scorching Iron Scepter
            if (T_Include_Event_Treasure) // Only include this if Event Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Scorching_Iron_Scepter.ID
                ).ToList(), Data.Row_List_Scorching_Iron_Scepter, 0, 1);
            }

            // Tower Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Tower_Key.ID
            ).ToList(), Data.Row_List_Tower_Key, 0, 1);

            // Garrison Ward Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Garrison_Ward_Key.ID
            ).ToList(), Data.Row_List_Garrison_Ward_Key, 0, 1);

            // Dragon Talon
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Dragon_Talon.ID
            ).ToList(), Data.Row_List_Dragon_Talon, 0, 1);

            // Heavy Iron Key
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Heavy_Iron_Key.ID
            ).ToList(), Data.Row_List_Heavy_Iron_Key, 0, 1);

            // Frozen Flower
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.Frozen_Flower.ID
            ).ToList(), Data.Row_List_Frozen_Flower, 0, 1);

            // Key to the Embedded
            if (T_Include_Boss_Treasure) // Only include this if Boss Treasure is randomised
            {
                AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Key_to_the_Embedded.ID
                ).ToList(), Data.Row_List_Key_to_the_Embedded, 0, 1);
            }

            // King Ring
            AssignKeyToItemlot(Data.ItemParam.Rows.Where(row =>
            row.ID == ScramblerData_Items.Static.King_Ring.ID
            ).ToList(), Data.Row_List_King_Ring, 0, 1);
        }

        public void AssignKeyToItemlot(List<PARAM.Row> item, List<PARAM.Row> rows, int slot, int amount)
        {
            if(rows.Count < 1)
            {
                Util.PrintLine("AssignKeyToItemlot: passed rows is empty.");
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
                    !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, chosen_row)
                )
                    can_assign = true;
            }

            Util.PrintLine($"Assign Key itemlot: {item[0].ID}");

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
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Aged_Feather.ID
                ).ToList(), Data.Row_List_Aged_Feather, 0, 1);
            }

            // Champion's Tablet
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Champion_Tablet.ID
                ).ToList(), Data.Row_List_Champion_Tablet, 0, 1);
            }

            // Dragon Head Stone
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Dragon_Head_Stone.ID
                ).ToList(), Data.Row_List_Dragon_Head_Stone, 0, 1);
            }

            // Dragon Torso Stone
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Dragon_Torso_Stone.ID
                ).ToList(), Data.Row_List_Dragon_Torso_Stone, 0, 1);
            }

            // Hello Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Hello_Carving.ID
                ).ToList(), Data.Row_List_Hello_Carving, 0, 1);
            }

            // Thank You Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Thank_You_Carving.ID
                ).ToList(), Data.Row_List_Thank_You_Carving, 0, 1);
            }

            // I'm Sorry Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Im_Sorry_Carving.ID
                ).ToList(), Data.Row_List_Im_Sorry_Carving, 0, 1);
            }

            // Very Good! Carving
            if (T_Include_Covenant_Treasure) // Only include this if Covenant Treasure is randomised
            {
                AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                row.ID == ScramblerData_Items.Static.Very_Good_Carving.ID
                ).ToList(), Data.Row_List_Very_Good_Carving, 0, 1);
            }

            // Pharros' Lockstone
            foreach (string map in ScramblerData_Items.Static.Pharros_Lockstone.Maps)
            {
                if (Data.map_itemlots.ContainsKey(map))
                {
                    List<PARAM.Row> rows = Data.map_itemlots[map];

                    AssignToolToItemlot(Data.ItemParam.Rows.Where(row => 
                    row.ID == ScramblerData_Items.Static.Pharros_Lockstone.ID).ToList(), 
                    rows, 0,
                    ScramblerData_Items.Static.Pharros_Lockstone.Min,
                    ScramblerData_Items.Static.Pharros_Lockstone.Max
                    );
                }
            }

            // Fragrant Branch of Yore
            foreach (string map in ScramblerData_Items.Static.Fragrant_Branch_of_Yore.Maps)
            {
                if (Data.map_itemlots.ContainsKey(map))
                {
                    List<PARAM.Row> rows = Data.map_itemlots[map];

                    AssignToolToItemlot(Data.ItemParam.Rows.Where(row =>
                    row.ID == ScramblerData_Items.Static.Fragrant_Branch_of_Yore.ID).ToList(),
                    rows.Where(row => 
                    !ScramblerData_Items.Static.Blacklist_Itemlots_for_Keys.Contains(row.ID)).ToList(), 
                    0,
                    ScramblerData_Items.Static.Fragrant_Branch_of_Yore.Min,
                    ScramblerData_Items.Static.Pharros_Lockstone.Max
                    );
                }
            }

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
                Util.PrintLine("AssignToolToItemlot: passed rows is empty.");
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
                    !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, chosen_row) &&
                    (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, chosen_row))
                )
                    can_assign = true;
            }

            Util.PrintLine($"Assign Tool itemlot: {item[0].ID}");

            // Add the item to the itemlot
            rows[index][$"item_lot_{slot}"].Value = item[0].ID;

            if (max_amount > 0)
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
                Util.PrintLine("AssignBossSoulToItemlot: passed rows is empty.");
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
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, chosen_row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, chosen_row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, chosen_row))
                )
                    can_assign = true;
            }

            Util.PrintLine($"Assign Boss Soul itemlot: {item[0].ID}");

            // Add the item to the itemlot
            rows[index][$"item_lot_{slot}"].Value = item[0].ID;
            rows[index][$"amount_lot_{slot}"].Value = amount;
            rows[index][$"reinforcement_lot_{slot}"].Value = 0;
            rows[index][$"infusion_lot_{slot}"].Value = 0;

            // Add the itemlot to the assigned list
            Assigned_Itemlots.Add(rows[index]);
        }

        public void ScrambleBossRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Boss Reward itemlot: {row.ID}");
                    SelectUniqueItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleCharacterRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Character Reward itemlot: {row.ID}");
                    SelectItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleCovenantRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Covenant Reward itemlot: {row.ID}");
                    SelectUniqueItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleBirdRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Bird Reward itemlot: {row.ID}");
                    SelectItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleEventRewards(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Event Reward itemlot: {row.ID}");
                    SelectItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleMapLoot(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Map Treasure itemlot: {row.ID}");
                    SelectItemForItemlot(row, 10, false);
                }
            }
        }

        public void ScrambleEnemyDrops(List<PARAM.Row> itemlots)
        {
            foreach (PARAM.Row row in itemlots)
            {
                // Do not change the already changed, or those that have keys/tools/boss souls.
                if (!Assigned_Itemlots.Contains(row) &&
                !HasMatchingLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Enemy Drop itemlot: {row.ID}");
                    SelectItemForItemlot(row, 60510000, true);
                }
            }
        }

        public void ScrambleShops()
        {
            AssignShopItemlot(Data.Shoplot_List_Vengarl);
            AssignShopItemlot(Data.Shoplot_List_Agdayne);
            AssignShopItemlot(Data.Shoplot_List_Gilligan_InitialStage);
            AssignShopItemlot(Data.Shoplot_List_Wellager);
            AssignShopItemlot(Data.Shoplot_List_Grandahl);
            AssignShopItemlot(Data.Shoplot_List_Gavlan);
            AssignShopItemlot(Data.Shoplot_List_Melentia);
            AssignShopItemlot(Data.Shoplot_List_Rat_King);
            AssignShopItemlot(Data.Shoplot_List_Maughlin);
            AssignShopItemlot(Data.Shoplot_List_Chloanne);
            AssignShopItemlot(Data.Shoplot_List_Rosabeth);
            AssignShopItemlot(Data.Shoplot_List_Lenigrast);
            AssignShopItemlot(Data.Shoplot_List_McDuff);
            AssignShopItemlot(Data.Shoplot_List_Carhillion);
            AssignShopItemlot(Data.Shoplot_List_Carhillion_InitialStage);
            AssignShopItemlot(Data.Shoplot_List_Straid);
            AssignShopItemlot(Data.Shoplot_List_Licia);
            AssignShopItemlot(Data.Shoplot_List_Licia_InitialStage);
            AssignShopItemlot(Data.Shoplot_List_Felkin);
            AssignShopItemlot(Data.Shoplot_List_Navlaan);
            AssignShopItemlot(Data.Shoplot_List_Magerold);
            AssignShopItemlot(Data.Shoplot_List_Ornifex);
            AssignShopItemlot(Data.Shoplot_List_Shalquoir);
            AssignShopItemlot(Data.Shoplot_List_Gren);
            AssignShopItemlot(Data.Shoplot_List_Cromwell);
            AssignShopItemlot(Data.Shoplot_List_Targray);

            UpdateSecondStageShopEntries(Data.Shoplot_List_Gilligan_InitialStage, Data.Shoplot_List_Gilligan_SecondStage);
            UpdateSecondStageShopEntries(Data.Shoplot_List_Carhillion_InitialStage, Data.Shoplot_List_Carhillion_SecondStage);
            UpdateSecondStageShopEntries(Data.Shoplot_List_Licia_InitialStage, Data.Shoplot_List_Licia_SecondStage);

            if (T_Ensure_Lifegems)
            {
                Random rand = new Random();
                int roll = rand.Next(100);

                // Melentia
                if (roll >= 0 && roll < 33)
                {
                    List<PARAM.Row> rows = Data.Shoplot_List_Melentia.Where(row => row.ID == 75400600).ToList();
                    rows[0]["equip_id"].Value = 60010000;
                    rows[0]["quantity"].Value = 255;
                }

                // Felkin
                if (roll >= 33 && roll < 66)
                {
                    List<PARAM.Row> rows = Data.Shoplot_List_Felkin.Where(row => row.ID == 77000600).ToList();
                    rows[0]["equip_id"].Value = 60010000;
                    rows[0]["quantity"].Value = 255;
                }

                // Shalquior
                if (roll >= 66)
                {
                    List<PARAM.Row> rows = Data.Shoplot_List_Shalquoir.Where(row => row.ID == 77700402).ToList();
                    rows[0]["equip_id"].Value = 60010000;
                    rows[0]["quantity"].Value = 255;
                }
            }
        }

        public void UpdateSecondStageShopEntries(List<PARAM.Row> initialShopRows, List<PARAM.Row> secondShopRows)
        {
            // Update second stage shop
            for (int i = 0; i < initialShopRows.Count; i++)
            {
                PARAM.Row original_row = initialShopRows[i];
                PARAM.Row free_row = secondShopRows[i];

                free_row["equip_id"].Value = original_row["equip_id"].Value;
                free_row["quantity"].Value = original_row["quantity"].Value;
            }
        }

        public void AssignShopItemlot(List<PARAM.Row> rows)
        {
            Random rand = new Random();
            
            foreach(PARAM.Row row in rows)
            {
                if(!HasMatchingShopLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingShopLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingShopLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign shoplot: {row.ID}");
                    SelectItemForShoplot(row);
                }
            }
        }

        public void ScrambleBossSoulTrades()
        {
            AssignBossTradeItemlot(Data.Shoplot_List_Straid_Boss);
            AssignBossTradeItemlot(Data.Shoplot_List_Ornifex_Boss);

            // Duplicate Ornifex's assignments to the free rows, but se the price_rate to 0.
            for(int i = 0; i < Data.Shoplot_List_Ornifex_Boss.Count; i++)
            {
                PARAM.Row original_row = Data.Shoplot_List_Ornifex_Boss[i];
                PARAM.Row free_row = Data.Shoplot_List_Ornifex_Boss_Free[i];

                free_row["equip_id"].Value = original_row["equip_id"].Value;
                free_row["Unk00"].Value = original_row["Unk00"].Value;
                free_row["enable_flag"].Value = original_row["enable_flag"].Value;
                free_row["disable_flag"].Value = original_row["disable_flag"].Value;
                free_row["material_id"].Value = original_row["material_id"].Value;
                free_row["duplicate_item_id"].Value = original_row["duplicate_item_id"].Value;
                free_row["Unk01"].Value = original_row["Unk01"].Value;
                free_row["price_rate"].Value = 0;
                free_row["quantity"].Value = original_row["quantity"].Value;
            }
        }

        public void AssignBossTradeItemlot(List<PARAM.Row> rows)
        {
            Random rand = new Random();

            foreach (PARAM.Row row in rows)
            {
                if (!HasMatchingShopLot(ScramblerData_Items.Static.Blacklist_Itemlot_Contents, row) &&
                (!T_Ignore_Keys || T_Ignore_Keys && !HasMatchingShopLot(ScramblerData_Items.Static.Category_Key_Item, row)) &&
                (!T_Ignore_Tools || T_Ignore_Tools && !HasMatchingShopLot(ScramblerData_Items.Static.Category_Tool_Item, row)))
                {
                    Util.PrintLine($"Assign Boss Trade shoplot: {row.ID}");
                    SelectUniqueItemForShoplot(row);
                }
            }
        }

        public void SelectUniqueItemForItemlot(PARAM.Row row, int EmptyItemID, bool isEnemyDrop)
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
                        Util.PrintLine($"Selected unique item for treasure: Weapon");

                        AddItemToItemlot(row, slot, Unassigned_Weapons, Data.Row_List_Weapons, isEnemyDrop);
                    }
                    // Armor
                    else if (roll >= 25 && roll <= 50)
                    {
                        Util.PrintLine($"Selected unique item for treasure: Armor");

                        AddItemToItemlot(row, slot, Unassigned_Armor, Data.Row_List_Armor, isEnemyDrop);
                    }
                    // Spells
                    else if (roll >= 50 && roll <= 75)
                    {
                        Util.PrintLine($"Selected unique item for treasure: Spell");

                        AddItemToItemlot(row, slot, Unassigned_Spells, Data.Row_List_Spells, isEnemyDrop);
                    }
                    // Rings
                    else if (roll >= 75)
                    {
                        Util.PrintLine($"Selected unique item for treasure: Ring");

                        AddItemToItemlot(row, slot, Unassigned_Rings, Data.Row_List_Rings, isEnemyDrop);
                    }
                }
            }

            Assigned_Itemlots.Add(row);
        }

        public void SelectItemForItemlot(PARAM.Row row, int EmptyItemID, bool isEnemyDrop)
        {
            Random rand = new Random();

            for (int slot = 0; slot <= 9; slot++)
            {
                int roll = rand.Next(100);

                int item_lot_value = (int)row[$"item_lot_{slot}"].Value;

                // Remove any preset reinforcement/infusions
                row[$"reinforcement_lot_{slot}"].Value = 0;
                row[$"infusion_lot_{slot}"].Value = 0;

                // Item lot slot is not empty
                if (item_lot_value != EmptyItemID)
                {
                    // Weapon
                    if (roll >= 0 && roll <= 10)
                    {
                        Util.PrintLine($"Selected item for treasure: Weapon");

                        AddItemToItemlot(row, slot, Unassigned_Weapons, Data.Row_List_Weapons, isEnemyDrop);

                        // Set infusion and reinforcement in rare cases
                        if(rand.Next(100) < 15)
                        {
                            row[$"reinforcement_lot_{slot}"].Value = rand.Next(1, 5);
                            row[$"infusion_lot_{slot}"].Value = rand.Next(1, 8);
                        }
                    }
                    // Armor
                    else if (roll >= 10 && roll <= 20)
                    {
                        Util.PrintLine($"Selected item for treasure: Armor");

                        AddItemToItemlot(row, slot, Unassigned_Armor, Data.Row_List_Armor, isEnemyDrop);
                    }
                    // Spells
                    else if (roll >= 20 && roll <= 30)
                    {
                        Util.PrintLine($"Selected item for treasure: Spell");

                        AddItemToItemlot(row, slot, Unassigned_Spells, Data.Row_List_Spells, isEnemyDrop);
                    }
                    // Rings
                    else if (roll >= 30 && roll <= 40)
                    {
                        Util.PrintLine($"Selected item for treasure: Ring");

                        AddItemToItemlot(row, slot, Unassigned_Rings, Data.Row_List_Rings, isEnemyDrop);
                    }
                    // Item: Ammunition
                    else if (roll >= 40 && roll <= 45)
                    {
                        Util.PrintLine($"Selected item for treasure: Ammunition");

                        AddItemToItemlot(row, slot, null, Data.Row_List_Ammunition, isEnemyDrop, 10, 50);
                    }
                    // Item: Material
                    else if (roll >= 45 && roll <= 50)
                    {
                        Util.PrintLine($"Selected item for treasure: Material");

                        AddItemToItemlot(row, slot, null, Data.Row_List_Materials, isEnemyDrop, 1, 3);
                    }
                    // Item: Soul
                    else if (roll >= 50 && roll <= 60)
                    {
                        Util.PrintLine($"Selected item for treasure: Soul");

                        AddItemToItemlot(row, slot, null, Data.Row_List_Soul_Consumables, isEnemyDrop, 1, 3);
                    }
                    // Item: Throwable
                    else if (roll >= 60 && roll <= 70)
                    {
                        Util.PrintLine($"Selected item for treasure: Throwable");

                        AddItemToItemlot(row, slot, null, Data.Row_List_Throwable_Consumable, isEnemyDrop, 1, 3);
                    }
                    // Item: Spice/Bird
                    else if (roll >= 70 && roll <= 80)
                    {
                        Util.PrintLine($"Selected item for treasure: Spice/Bird");

                        if (roll >= 75)
                            AddItemToItemlot(row, slot, null, Data.Row_List_Bird_Consumables, isEnemyDrop, 1, 3);
                        else
                            AddItemToItemlot(row, slot, null, Data.Row_List_Spell_Upgrades, isEnemyDrop, 1, 3);
                    }
                    // Item: HP/Cast
                    else if (roll >= 80 && roll <= 90)
                    {
                        Util.PrintLine($"Selected item for treasure: HP/Cast");

                        if (roll >= 85)
                            AddItemToItemlot(row, slot, null, Data.Row_List_HP_Consumables, isEnemyDrop, 1, 3);
                        else
                            AddItemToItemlot(row, slot, null, Data.Row_List_Cast_Consumables, isEnemyDrop, 1, 3);
                    }
                    // Item: Misc
                    else if (roll >= 90)
                    {
                        Util.PrintLine($"Selected item for treasure: Misc");
                        AddItemToItemlot(row, slot, null, Data.Row_List_Misc_Consumable, isEnemyDrop, 1, 5);
                    }
                }
            }

            Assigned_Itemlots.Add(row);
        }

        public void AddItemToItemlot(PARAM.Row row, int slot, List<PARAM.Row> unassigned_list, List<PARAM.Row> fallback_list, bool isEnemyDrop, int min = 0, int max = 0)
        {
            Random rand = new Random();

            PARAM.Row value = null;

            // Use this list first so each weapon appears at least once.
            if (unassigned_list != null && unassigned_list.Count > 0)
            {
                value = unassigned_list[rand.Next(unassigned_list.Count)];

                row[$"item_lot_{slot}"].Value = value.ID;
                Util.PrintLine($"Adding to itemlot from unassigned: {value.ID}");
            }
            // After all weapons are present, allow duplicates
            else
            {
                value = fallback_list[rand.Next(fallback_list.Count)];

                row[$"item_lot_{slot}"].Value = value.ID;
                Util.PrintLine($"Adding to itemlot from any: {value.ID}");
            }

            if (min > 0 && max > 0)
                row[$"amount_lot_{slot}"].Value = rand.Next(min, max);
            else
                row[$"amount_lot_{slot}"].Value = 1;

            // For enemy drop lots
            if (isEnemyDrop)
            {
                // If it is not a guaranteed drop, randomise the chance
                if ((float)row[$"chance_lot_{slot}"].Value != 100)
                {
                    row[$"chance_lot_{slot}"].Value = (float)rand.Next(1, 25);
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

        public bool HasMatchingShopLot(List<int> list, PARAM.Row row)
        {
            bool match = false;

            for (int slot = 0; slot <= 9; slot++)
            {
                if (list.Contains((int)row[$"equip_id"].Value))
                    match = true;
            }

            return match;
        }

        public void SelectUniqueItemForShoplot(PARAM.Row row)
        {
            Random rand = new Random();

            int roll = rand.Next(100);

            int item_lot_value = (int)row[$"equip_id"].Value;

            // If Retain Shop Spread in on, fix the rolls so the current item is swapped for the same type
            if (T_Retain_Shop_Spread)
            {
                if (Data.Row_List_Weapons.Any(r => r.ID == item_lot_value))
                    roll = 0;

                if (Data.Row_List_Spells.Any(r => r.ID == item_lot_value))
                    roll = 50;

                if (Data.Row_List_Rings.Any(r => r.ID == item_lot_value))
                    roll = 75;
            }

            // Weapon
            if (roll >= 0 && roll < 50)
            {
                Util.PrintLine($"Selected unique item for shop: Weapon");
                AddItemToShoplot(row, Data.Row_List_Weapons, 255, 255);
            }
            // Spells
            else if (roll >= 50 && roll < 75)
            {
                Util.PrintLine($"Selected unique item for shop: Spell");
                AddItemToShoplot(row, Data.Row_List_Spells, 255, 255);
            }
            // Rings
            else if (roll >= 75)
            {
                Util.PrintLine($"Selected unique item for shop: Ring");
                AddItemToShoplot(row, Data.Row_List_Rings, 255, 255);
            }
        }

        public void SelectItemForShoplot(PARAM.Row row)
        {
            Random rand = new Random();

            int roll = rand.Next(100);
            int inf_roll = rand.Next(100);

            int item_lot_value = (int)row[$"equip_id"].Value;

            // If Retain Shop Spread in on, fix the rolls so the current item is swapped for the same type
            if (T_Retain_Shop_Spread)
            {
                if (Data.Row_List_Weapons.Any(r => r.ID == item_lot_value))
                    roll = 0;

                if (Data.Row_List_Armor.Any(r => r.ID == item_lot_value))
                    roll = 10;

                if (Data.Row_List_Spells.Any(r => r.ID == item_lot_value))
                    roll = 20;

                if (Data.Row_List_Rings.Any(r => r.ID == item_lot_value))
                    roll = 30;

                if (Data.Row_List_Ammunition.Any(r => r.ID == item_lot_value))
                    roll = 40;

                if (Data.Row_List_Materials.Any(r => r.ID == item_lot_value))
                    roll = 45;

                if (Data.Row_List_Soul_Consumables.Any(r => r.ID == item_lot_value))
                    roll = 55;

                if (Data.Row_List_Throwable_Consumable.Any(r => r.ID == item_lot_value))
                    roll = 60;

                if (Data.Row_List_Bird_Consumables.Any(r => r.ID == item_lot_value))
                    roll = 65;

                if (Data.Row_List_Spell_Upgrades.Any(r => r.ID == item_lot_value))
                    roll = 70;

                if (Data.Row_List_HP_Consumables.Any(r => r.ID == item_lot_value))
                    roll = 75;

                if (Data.Row_List_Cast_Consumables.Any(r => r.ID == item_lot_value))
                    roll = 85;

                if (Data.Row_List_Flask_Upgrades.Any(r => r.ID == item_lot_value))
                    roll = 90;

                if (Data.Row_List_Misc_Consumable.Any(r => r.ID == item_lot_value))
                    roll = 95;
            }

            // Weapon
            if (roll >= 0 && roll < 10)
            {
                Util.PrintLine($"Selected item for shop: Weapon");

                AddItemToShoplot(row, Data.Row_List_Weapons, 255, 255);
            }
            // Armor
            else if (roll >= 10 && roll < 20)
            {
                Util.PrintLine($"Selected item for shop: Armor");

                AddItemToShoplot(row, Data.Row_List_Armor, 255, 255);
            }
            // Spells
            else if (roll >= 20 && roll < 30)
            {
                Util.PrintLine($"Selected item for shop: Spell");

                AddItemToShoplot(row, Data.Row_List_Spells, 1, 3);
            }
            // Rings
            else if (roll >= 30 && roll < 40)
            {
                Util.PrintLine($"Selected item for shop: Ring");

                AddItemToShoplot(row, Data.Row_List_Rings, 1, 1);
            }
            // Item: Ammunition
            else if (roll >= 40 && roll < 45)
            {
                Util.PrintLine($"Selected item for shop: Ammunition");

                AddItemToShoplot(row, Data.Row_List_Ammunition, 255, 255);
            }
            // Item: Material
            else if (roll >= 45 && roll < 55)
            {
                Util.PrintLine($"Selected item for shop: Material");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Materials, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Materials, 5, 10);
            }
            // Item: Soul
            else if (roll >= 55 && roll < 60)
            {
                AddItemToShoplot(row, Data.Row_List_Soul_Consumables, 3, 5);
            }
            // Item: Throwable
            else if (roll >= 60 && roll < 65)
            {
                Util.PrintLine($"Selected item for shop: Throwable");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Throwable_Consumable, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Throwable_Consumable, 25, 100);
            }
            // Item: Bird
            else if (roll >= 65 && roll < 70)
            {
                Util.PrintLine($"Selected item for shop: Bird");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Bird_Consumables, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Bird_Consumables, 1, 3);
            }
            // Item: Spice
            else if (roll >= 70 && roll < 75)
            {
                Util.PrintLine($"Selected item for shop: Spice");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Spell_Upgrades, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Spell_Upgrades, 1, 3);
            }
            // Item: HP
            else if (roll >= 75 && roll < 85)
            {
                Util.PrintLine($"Selected item for shop: HP");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_HP_Consumables, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_HP_Consumables, 3, 10);
            }
            // Item: Cast
            else if (roll >= 85 && roll < 90)
            {
                Util.PrintLine($"Selected item for shop: Cast");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Cast_Consumables, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Cast_Consumables, 3, 10);
            }
            // Item: Flask
            else if (roll >= 90 && roll < 95)
            {
                Util.PrintLine($"Selected item for shop: Flask");

                AddItemToShoplot(row, Data.Row_List_Flask_Upgrades, 1, 1);
            }
            // Item: Misc
            else if (roll >= 95)
            {
                Util.PrintLine($"Selected item for shop: Misc");

                if (inf_roll >= 80)
                    AddItemToShoplot(row, Data.Row_List_Misc_Consumable, 255, 255);
                else
                    AddItemToShoplot(row, Data.Row_List_Flask_Upgrades, 1, 1);
            }
        }

        public void AddItemToShoplot(PARAM.Row row, List<PARAM.Row> itemlist, int min, int max)
        {
            Random rand = new Random();

            PARAM.Row value = null;

            value = itemlist[rand.Next(itemlist.Count)];

            Util.PrintLine($"Adding to shoplot: {value.ID}");

            row[$"equip_id"].Value = value.ID;

            if (min > 0 && max > 0)
                row[$"quantity"].Value = rand.Next(min, max);
            else if (min == 255 && max == 255)
                row[$"quantity"].Value = 255;
            else
                row[$"quantity"].Value = 1;
        }
    }
}
