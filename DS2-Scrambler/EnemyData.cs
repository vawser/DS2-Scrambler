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
    public class EnemyData
    {
        // *** Core ***
        Regulation RegulationData;

        // *** Strings ***
        public string ParamScramblePath;
        public string EnemyScramblePath;
        public string ItemScramblePath;
        public string OutputPath;

        // *** Dictionaries ***
        public Dictionary<string, List<Enemy>> Per_Map_Enemy_Dict = new Dictionary<string, List<Enemy>>();

        // Generator ID List per Map of 'Boss' enemies, i.e. Last Giant
        public Dictionary<string, List<int>> Per_Map_Boss_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 853, 857 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 803, 2500 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 800 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 845, 846, 847, 848, 8000, 8001, 8002, 8003, 8004 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Miniboss' enemies, i.e. Pursuer encounters
        public Dictionary<string, List<int>> Per_Map_Miniboss_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 3350 } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 2255 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 8500 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 4000, 4010, 4020, 4030, 6031 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };


        // Generator ID List per Map of 'Boss Minion' enemies, i.e. Skeleton Lord skeletons
        public Dictionary<string, List<int>> Per_Map_Boss_Minion_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 8000, 8001, 8002, 8003, 8004, 8005, 8010, 8011, 8012, 8013, 8014, 8015, 2510, 2511, 2520, 2521, 2522, 2530, 2531, 2532, 2533, 2534 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 8010, 8011, 8012, 8013, 8014 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };


        // Generator ID List per Map of 'NG Plus' enemies, i.e. Falconers in NG+2
        public Dictionary<string, List<int>> Per_Map_NG_Plus_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 3040, 3050, 3211, 3230, 4000, 4001, 4002, 4003, 7000, 7010 } },
            // Majula
            { "m10_04_00_00", new List<int>{ 3101, 3230, 4000, 1000, 1001, 1002, 1010, 1011, 1020, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 2000 } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 2500, 2504, 2600, 6000, 6001, 6002, 7002, 9000 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 1180, 1181, 2200, 3552, 3553, 3590, 3591, 5110 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 1650, 5100, 6009, 6110, 6310, 6510, 6511, 8100, 8101 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };


        // Generator ID List per Map of 'Character' enemies, i.e. Milibeth
        public Dictionary<string, List<int>> Per_Map_Character_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 15, 210, 215, 220 } },
            // Majula
            { "m10_04_00_00", new List<int>{ 45, 47, 60, 85, 100, 105, 107, 108, 110, 115, 120, 122, 135, 150, 152, 205, 260, 270, 400, 401, 402, 405, 410, 413, 414, 415 } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 60, 65, 85, 260 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 55, 58, 65, 265, 310, 911 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 90, 160, 205 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 90, 95, 145, 165, 167, 147, 912 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Character' gravestones
        public Dictionary<string, List<int>> Per_Map_CharacterGravestone_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 16, 17, 211, 212, 216, 217, 221, 222} },
            // Majula
            { "m10_04_00_00", new List<int>{ 46, 86, 101, 106, 111, 116, 121, 136, 151, 207, 261 } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 61, 66, 86, 261 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 56, 58, 66, 266, 311 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 91, 161 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 91, 96, 146, 166 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Character' summons
        public Dictionary<string, List<int>> Per_Map_CharacterSummon_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 526, 527, 528 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 526, 527, 528, 529 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 540 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 546, 547, 548, 549, 550 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Character' invaders/hostile phantoms
        public Dictionary<string, List<int>> Per_Map_CharacterInvaders_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 520, 521, 923 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 520, 521, 926 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 530, 531, 532, 7000, 7001, 7002, 7003, 9000, 9001, 9004 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 540, 541, 932 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Fixed' enemies, i.e. Dyna and Tillo, Player Character entity
        public Dictionary<string, List<int>> Per_Map_Skipped_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 430, 9500, 9000 } },
            // Majula
            { "m10_04_00_00", new List<int>{ 9000 } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ 2520, 2521, 2522 } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ 1000, 9000 } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        // Generator ID List per Map of 'Petrified' enemies
        public Dictionary<string, List<int>> Per_Map_Petrified_ID_Dict = new Dictionary<string, List<int>>
        {
            // Things Betwixt
            { "m10_02_00_00", new List<int>{ 3500 } },
            // Majula
            { "m10_04_00_00", new List<int>{ } },
            // Forest of Fallen Giants
            { "m10_10_00_00", new List<int>{ } },
            // Brightstone Cove Tseldora
            { "m10_14_00_00", new List<int>{ } },
            // Aldia's Keep
            { "m10_15_00_00", new List<int>{ 2500, 8600, 8601 } },
            // Lost Bastille
            { "m10_16_00_00", new List<int>{ 9900 } },
            // Earthen Peak
            { "m10_17_00_00", new List<int>{ } },
            // No Man's Wharf
            { "m10_18_00_00", new List<int>{ } },
            // Iron Keep
            { "m10_19_00_00", new List<int>{ } },
            // Huntman's Copse
            { "m10_23_00_00", new List<int>{ } },
            // The Gutter
            { "m10_25_00_00", new List<int>{ } },
            // Dragon Aerie
            { "m10_27_00_00", new List<int>{ } },
            // Path to the Shaded Woods
            { "m10_29_00_00", new List<int>{ } },
            // Unseen Path to Heide's
            { "m10_30_00_00", new List<int>{ } },
            // Heide's Tower of Flame
            { "m10_31_00_00", new List<int>{ } },
            // Shaded Woods
            { "m10_32_00_00", new List<int>{ } },
            // Doors of Pharros
            { "m10_33_00_00", new List<int>{ } },
            // Grave of Saints
            { "m10_34_00_00", new List<int>{ } },
            // Giant's Memories
            { "m20_10_00_00", new List<int>{ } },
            // Shrine of Amana
            { "m20_11_00_00", new List<int>{ } },
            // Drangleic Castle
            { "m20_21_00_00", new List<int>{ } },
            // Undead Crypt
            { "m20_24_00_00", new List<int>{ } },
            // Dragon Memories
            { "m20_26_00_00", new List<int>{ } },
            // Dark Chasm of Old
            { "m40_03_00_00", new List<int>{ } },
            // Shulva
            { "m50_35_00_00", new List<int>{ } },
            // Brume Tower
            { "m50_36_00_00", new List<int>{ } },
            // Eleum Loyce
            { "m50_37_00_00", new List<int>{ } },
            // Memory of the King
            { "m50_38_00_00", new List<int>{ } }
        };

        public EnemyData(Regulation reg, string output_path)
        {
            RegulationData = reg;
            OutputPath = output_path;

            ParamScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";
            EnemyScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Enemy-Scramble\\";
            ItemScramblePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\Item-Scramble\\";

            Console.WriteLine("EnemyData");

            foreach (ParamWrapper wrapper in reg.regulationParamWrappers)
            {
                if (wrapper.Name.Contains("generatorparam_"))
                {
                    Console.WriteLine($"{wrapper.Name}");
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    List<Enemy> map_enemies = new List<Enemy>();

                    foreach (PARAM.Row generator_row in wrapper.Rows)
                    {
                        uint regist_id = (uint)generator_row["GeneratorRegistParam"].Value;

                        ParamWrapper regist_params = reg.regulationParamWrappers.Where(r => r.Name == $"generatorregistparam_{map_id}").ToList()[0];
                        PARAM.Row regist_row = regist_params.Rows.Where(r => r.ID == regist_id).ToList()[0];

                        ParamWrapper location_params = reg.regulationParamWrappers.Where(r => r.Name == $"generatorlocation_{map_id}").ToList()[0];
                        PARAM.Row location_row = location_params.Rows.Where(r => r.ID == generator_row.ID).ToList()[0];

                        Enemy new_enemy = new Enemy(generator_row, regist_row, location_row);
                        map_enemies.Add(new_enemy);
                    }

                    Per_Map_Enemy_Dict.Add(map_id, map_enemies);
                }
            }

            // Assign categories
            foreach(KeyValuePair<string, List<Enemy>> entry in Per_Map_Enemy_Dict)
            {
                foreach(Enemy enemy in entry.Value)
                {
                    // Example
                    if(enemy.GeneratorID == 100)
                    {
                        enemy.EnemyCategory = EnemyCategory.Boss;
                    }
                }
            }
        }

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
