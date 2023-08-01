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
using System.Reflection;
using Org.BouncyCastle.Crypto;
using static SoulsFormats.MSBD.Event;

namespace DS2_Scrambler
{
    public class EnemyScrambler
    {
        public Random rand;
        public Regulation regulation;
        public ScramblerData Data;

        public bool T_IncludeCharacters = false;

        public EnemyScrambler(Random random, Regulation reg, ScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;
        }

        // TODO: add basic support for Location scramble using new method

        public Regulation Scramble_Enemies(bool scrambleEnemyLocation, bool enableEnemySharedAggro, bool includeCharacters, bool enable_Things_Betwixt, bool enable_Majula, bool enable_Forest_of_Fallen_Giants, bool enable_Brightstone_Cove_Tseldora, bool enable_Aldias_Keep, bool enable_Lost_Bastille, bool enable_Earthen_Peak, bool enable_No_mans_Wharf, bool enable_Iron_Keep, bool enable_Huntmans_Copse, bool enable_Gutter, bool enable_Dragon_Aerie, bool enable_Path_to_Shaded_Woods, bool enable_Unseen_Path_to_Heides, bool enable_Heides_Tower_of_Flame, bool enable_Shaded_Woods, bool enable_Doors_of_Pharros, bool enable_Grave_of_Saints, bool enable_Giant_Memories, bool enable_Shrine_of_Amana, bool enable_Drangleic_Castle, bool enable_Undead_Crypt, bool enable_Dragon_Memories, bool enable_Dark_Chasm_of_Old, bool enable_Shulva, bool enable_Brume_Tower, bool enable_Eleum_Loyce, bool enable_Memory_of_the_King)
        {
            T_IncludeCharacters = includeCharacters;

            if (enableEnemySharedAggro)
                ApplyEnemyAggressionMod();

            if (scrambleEnemyLocation)
            {
                if (enable_Things_Betwixt)
                {
                    ScrambleEnemyLocation("m10_02_00_00", new List<int> 
                    { 
                        15, 16, 17, // Milibeth
                        210, 211, 212, // Strowen
                        215, 216, 217, // Griant
                        220, 221, 222, // Morrel
                        320, // Dyna and Tillo
                        3500, // Petrified Hollow
                        9000, // Player
                        9500, // Player
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_02_00_00", new List<int>
                        {
                            // None
                        }, 
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Majula)
                {
                    ScrambleEnemyLocation("m10_04_00_00", new List<int>
                    {
                        45, 46, 47, // Laddersmith Gilligan
                        60, // Benhart of Jugo
                        85, 86, // Cartographer Cale
                        100, 101, // Crestfallen Saulden
                        105, 106, 107, 108, // Maughlin the Armourer
                        110, 111, // Stone Trader Cholanne
                        115, 116, // Rosabeth of Melfia
                        120, 121, 122, // Blacksmith Lenigrast
                        135, 136, // Carhllion of the Fold
                        150, 151, 152, // Licia of Lindelt
                        205, 207, // Emerald Herald
                        260, 261, // Merchant Hag Melentia
                        270, // Sweet Shalquior
                        400, 401, 402, 405, // Emerald Herald
                        410, 413, 414, 415, // Sweet Shalquior
                        9000 // Voice Marker
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_04_00_00", new List<int>
                        {
                            45,// Laddersmith Gilligan
                            60, // Benhart of Jugo
                            85, // Cartographer Cale
                            100, // Crestfallen Saulden
                            105, 107, 108, // Maughlin the Armourer
                            110, // Stone Trader Cholanne
                            115,// Rosabeth of Melfia
                            120, 122, // Blacksmith Lenigrast
                            135, // Carhllion of the Fold
                            150, 152, // Licia of Lindelt
                            205, // Emerald Herald
                            260,  // Merchant Hag Melentia
                            270, // Sweet Shalquior
                            400, 401, 402, 405, // Emerald Herald
                            410, 413, 414, 415 // Sweet Shalquior
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Forest_of_Fallen_Giants)
                {
                    ScrambleEnemyLocation("m10_10_00_00", new List<int>
                    {
                        60, 61, // Benhart of Jugo
                        65, 66, // Mil Mannered PAte
                        85, 86, // Cartographer Cale
                        260, 261, // Merchant Hag Melentia

                        520, // Royal Sorcerer Navlaan
                        521, // Armorer Dennis
                        923, // The Forlorn

                        526, // Mild Mannered Pate
                        527, // Sellsword Luet
                        528, // Ruined Alfis

                        853, // The Pursuer
                        857 // The Last Giant
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_10_00_00", new List<int>
                        {
                            60, // Benhart of Jugo
                            65, // Mil Mannered PAte
                            85, // Cartographer Cale
                            260 // Merchant Hag Melentia
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Brightstone_Cove_Tseldora)
                {
                    ScrambleEnemyLocation("m10_14_00_00", new List<int>
                    {
                        55, 56, 57, 58, // Creighton the Wanderer
                        65, 66, // Mild Mannered Pate
                        265, 266, // Weaponsmith Ornifex
                        310, 311, // Cromwell the Pardoner
                        
                        520, // Bowman Guthry
                        521, // Royal Sorcerer Navlaan
                        926, // The Forlorn

                        526, // Benhart of Jugo
                        527, // Ashen Knight Boyd
                        528, // Manhunter O'Harrah
                        529, // Bashful Ray

                        // Prowling Congregation
                        2500, 2510, 2511, 2520, 2521, 2522,
                        2530, 2531, 2532, 2533, 2534,

                        803, // The Duke's Dear Freja
                        911, // Aldia
                        1000, // The Duke's Dear Freja
                        9000 // Voice Marker
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_14_00_00", new List<int>
                        {
                            55, // Creighton the Wanderer
                            65, // Mild Mannered Pate
                            265, // Weaponsmith Ornifex
                            310 // Cromwell the Pardoner
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Aldias_Keep)
                {
                    ScrambleEnemyLocation("m10_15_00_00", new List<int>
                    {
                        90, 91, // Lucatiel of Mirrah
                        160, 161, // Royal Sorcerer Navlaan
                        205, // Emerald Herald

                        530, // Royal Sorcerer Navlaan
                        531, // Aslatiel of Mirrah

                        540, // Sellsword Luet
                        800, // Guardian Dragon

                        2500, // Petrified Ogre
                        8600, 8601, // Petrified Hollows
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_15_00_00", new List<int>
                        {
                            90, // Lucatiel of Mirrah
                            160, // Royal Sorcerer Navlaan
                            205, // Emerald Herald
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Lost_Bastille)
                {
                    ScrambleEnemyLocation("m10_16_00_00", new List<int>
                    {
                        90, 91, // Lucatiel of Mirrah
                        95, 96, // Bell Keeper
                        145, 146, 147, // Straid of Olaphis
                        165, 166, 167, // Steady Hand McDuff

                        540, // Vorgel the Sinner
                        541, // Bell Keeper
                        932, // The Forlorn

                        546, // Lucatiel of Mirrah
                        547, // Pilgrim Bellclaire
                        548, // Felicia the Brave
                        549, // Sellsword Luet
                        550, // Masterless Glencour

                        845, 846, 847, // Ruin Sentinels
                        848, 8100, 8101, // Lost Sinner
                        8000, 8001, 8002, 8003, 8004, 8010, 8011, 8012, 8013, 8014, // Belfry Gargoyles
                        912, // Aldia

                        9900 // Petrified Hollow
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_16_00_00", new List<int>
                        {
                            90, // Lucatiel of Mirrah
                            95, // Bell Keeper
                            145, 147, // Straid of Olaphis
                            165, 167 // Steady Hand McDuff
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Earthen_Peak)
                {
                    ScrambleEnemyLocation("m10_17_00_00", new List<int>
                    {
                        45, 46, 47, // Laddersmith Gilligan
                        65, 66, // Mild Mannered Pate
                        90, 91, // Lucatiel of Mirrah
                        195, 196, // Stone Trader Chloanne
                        245, 246, // Lonesome Gavlan

                        556, // Jester Thomas
                        935, // Forlorn

                        600, // Devotee Scarlett
                        605, // Bashful Ray

                        800, // Covetous Demon
                        801, // Mytha

                        9001 // Voice MArker
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_17_00_00", new List<int>
                        {
                            45, 47, // Laddersmith Gilligan
                            65, // Mild Mannered Pate
                            90, // Lucatiel of Mirrah
                            195, // Stone Trader Chloanne
                            245 // Lonesome Gavlan
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_No_mans_Wharf)
                {
                    ScrambleEnemyLocation("m10_18_00_00", new List<int>
                    {
                        90, 91, // Lucatiel of Mirrah
                        135, 136, // Carhillion of the Fold
                        245, 246, // Lonesome Gavlan

                        938, // The Forlorn

                        556, // Lucatiel of Mirrah
                        567, // Bradley of the Old Guard

                        844, // Flexible Sentry
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_18_00_00", new List<int>
                        {
                            90, // Lucatiel of Mirrah
                            135, // Carhillion of the Fold
                            245, // Lonesome Gavlan
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Iron_Keep)
                {
                    ScrambleEnemyLocation("m10_19_00_00", new List<int>
                    {
                        95, 96, // Bell Keeper
                        175, 176, // Magerold of Lanafir

                        570, // Bell Keeper
                        571, // Oliver the Collector
                        572, // Fencer Sharron
                        573, // Armorer Dennis
                        941, // The Forlorn
                        7007, // Mad Warrior Phantom

                        576, // Lucatiel of Mirrah
                        577, // Manhunter O'Harrah
                        578, // Drifter SWordsman AIdel

                        806, // Smelter Demon
                        807, // Old Iron King

                        913, // Aldia
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_19_00_00", new List<int>
                        {
                             95, // Bell Keeper
                             175, // Magerold of Lanafir
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Huntmans_Copse)
                {
                    ScrambleEnemyLocation("m10_23_00_00", new List<int>
                    {
                        55, 56, // Crieghton the Wanderer
                        155, 156, // Felkin the Outcast
                        275, 276, // Titchy Gren

                        580, // Merciless Roenna
                        8000, // Greatsword Phantom
                        8001, // Greatsword Phantom

                        586, // Bashful Ray
                        587, // Creighton the Wanderer

                        816, // Executioner's Chariot
                        //6000, 6001, 6002, 6003, 6004, 6005, 6006, // EC - Skeletons

                        839, 840, 841, // Skeleton Lords
                        5210, 211, 5212, 5213, 5214, 5220, 5221, 5222, 5223, 5224, 
                        5225, 5226, 5227, 5228, 5229, 5230, 5231, // Skeleton Lord - Skeletons

                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_23_00_00", new List<int>
                        {
                            55, // Crieghton the Wanderer
                            155, // Felkin the Outcast
                            275, // Titchy Gren
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Gutter)
                {
                    ScrambleEnemyLocation("m10_25_00_00", new List<int>
                    {
                        40, 41, // Darkdiver Grandahl
                        90, 91, // Lucatiel of Mirrah

                        590, // Melinda the Butcher
                        591, // Royal Sorcerer Navlaan
                        592, // Woodland Child Victor
                        593, // Woodland Child Gully
                        594, // Gutter Denizen
                        947, // The Forlorn

                        596, // Lucatiel of Mirrah
                        597, // Lone Hunter Schmidt
                        598, // Bashful Ray

                        851, // The Rotten
                        914, // Aldira

                        8000, // Petrified Hollow
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_25_00_00", new List<int>
                        {
                            40, // Darkdiver Grandahl
                            90, // Lucatiel of Mirrah
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Dragon_Aerie)
                {
                    ScrambleEnemyLocation("m10_27_00_00", new List<int>
                    {
                        205, // Emerald Herald

                        600, // Royal Sorcerer Navlaan

                        601, // Dragonfang Villard

                        // Black Dragon Knights
                        602, 603, 
                        4000, 4001, 4005, 4009, 4010, 4011, 4020, 4030, 4031, 4032, 4033, 
                        4036, 4037, 4038, 4039, 4040, 4041, 4042,

                        604, // Bashful Ray
                        606, // Vengarl
                        607, // Melinda the Butcher
                        608, // Transcendent Edde
                        609, // Fencer Sharron

                        800, // Ancient Dragon
                        916, // Aldia

                        //1100, 1101, 1102, // Guardian Dragons

                        6000, // Petrified Hollow
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_27_00_00", new List<int>
                        {
                            205, // Emerald Herald
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Path_to_Shaded_Woods)
                {
                    ScrambleEnemyLocation("m10_29_00_00", new List<int>
                    {
                        60, 61, // Benhart of Jugo
                        115, 116, 117, // Rosabeth of Melfia
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_29_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Unseen_Path_to_Heides)
                {
                    ScrambleEnemyLocation("m10_30_00_00", new List<int>
                    {
                        90, 91, 566, // Lucatiel of Mirrah

                        9000, // Petrified Hollow
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_30_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Heides_Tower_of_Flame)
                {
                    ScrambleEnemyLocation("m10_31_00_00", new List<int>
                    {
                        150, 151, // Licia of Lindelt
                        315, 316, // BLue Sentinel Targray

                        616, // Masterless Glencour
                        617, // Devotee Scarlett
                        
                        842, // Dragonrider
                        843, // Old Dragonslyaer

                        // 1808, // Guardian Dragon
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_31_00_00", new List<int>
                        {
                            150, // Licia of Lindelt
                            315, // BLue Sentinel Targray
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Shaded_Woods)
                {
                    ScrambleEnemyLocation("m10_32_00_00", new List<int>
                    {
                        255, 257, // Vengarl
                        265, 266, // Weaponsmith Ornifex
                        325, 326, // Manscorpion Tark

                        956, // Forlorn

                        626, // Manscorpion Tark
                        627, // Bradley of the Old Guard

                        824, // Scorpioness Najka

                        2040, 2041, 2103, 2210, 2310, 2500, // Petrified Lion Warrior
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_32_00_00", new List<int>
                        {
                            255, // Vengarl
                            265, // Weaponsmith Ornifex
                            325, // Manscorpion Tark
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Doors_of_Pharros)
                {
                    ScrambleEnemyLocation("m10_33_00_00", new List<int>
                    {
                        245, 246, // Lonesome Gavlan
                        285, 286, // Rat King

                        630, 631, // Bowman Guthry

                        852, // Royal Rat Authority
                        2800, 2810, 2820, 2830, // Mongrel Rats
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_33_00_00", new List<int>
                        {
                            245, // Lonesome Gavlan
                            285, // Rat King
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Grave_of_Saints)
                {
                    ScrambleEnemyLocation("m10_34_00_00", new List<int>
                    {
                        285, 286, // Rat King

                        640, 641, // Rhoy the Explorer

                        // Rat Swarm
                        850, 
                        9000, 9001, 9002, 9003, 9004, 9005, 9006, 9007, 9008, 9009, 9010, 9011
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m10_34_00_00", new List<int>
                        {
                            285, // Rat King
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Giant_Memories)
                {
                    ScrambleEnemyLocation("m20_10_00_00", new List<int>
                    {
                        30, 32, // Captain Drummond
                        60, 61, // Benheart of Jugo

                        656, // Benhart of Jugo
                        657, // Captain Drummond

                        800, // Giant Lord
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_10_00_00", new List<int>
                        {
                            30, // Captain Drummond
                            60, // Benheart of Jugo
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Shrine_of_Amana)
                {
                    ScrambleEnemyLocation("m20_11_00_00", new List<int>
                    {
                        20, 21, 25, 26, // Milfanito

                        660, 3100, // Peculiar Kindalur
                        965, // The Forlorn
                        6020, 6021, 6022, // Butcher Phantom

                        666, // Felicia the Brave
                        667, // Lone Hunter Schmidt

                        802, // Demon of Song

                        5300, // Petrified Hollow
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_11_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Drangleic_Castle)
                {
                    ScrambleEnemyLocation("m20_21_00_00", new List<int>
                    {
                        1, // Chancellor Wellager
                        40, 41, // Darkdiver Grandahl
                        60, 61, // Benhart of Jugo
                        205, 400, // Emerald Herald
                        235, // Nashandra (human)

                        670, // Royal Sorcerer Navlaan
                        671, // Nameless Usurper
                        968, // Forlorn
                        1020, // Drakekeeper Knight Phantom
                        3100, // Washing Pole Phantom

                        676, 678, // Benhart of Jugo
                        677, // Vengarl
                        679, // Ashen Knight Boyd
                        680, // Bradley of the Old Guard
                        681, // Pilgrim Bellclaire
                        682, // Bashful Ray
                        683, // Sellsword Luet

                        825, 500, 5001, // Looking Glass Knight
                        859, // Nashandra
                        860, // Throne Defender
                        862, // Throne Watcher
                        863, 864, // Twin Dragonriders
                        910, 917, // Aldia

                        305, 306, // Imprisoned Milfanito
                        1300, 1306, // Stone Soldier (for the kill doors)
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_21_00_00", new List<int>
                        {
                            1, // Chancellor Wellager
                            40, // Darkdiver Grandahl
                            60, // Benhart of Jugo
                            205, 400, // Emerald Herald
                            235, // Nashandra (human)
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Undead_Crypt)
                {
                    ScrambleEnemyLocation("m20_24_00_00", new List<int>
                    {
                        205, // Emerald Herald
                        240, 241, // Grave Warden Agdayne

                        680, // Nameless Usurper
                        971, // Forlorn
                        
                        686, // Grave Warden Agdayne
                        690, // Deovtee Scarlett

                        230, // Vendrick
                        800, // Velstadt
                        915, // Aldia
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_24_00_00", new List<int>
                        {
                            205, // Emerald Herald
                            240, // Grave Warden Agdayne
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Dragon_Memories)
                {
                    ScrambleEnemyLocation("m20_26_00_00", new List<int>
                    {
                        1000, // Voice Marker
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m20_26_00_00", new List<int>
                        {

                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Dark_Chasm_of_Old)
                {
                    ScrambleEnemyLocation("m40_03_00_00", new List<int>
                    {
                        811, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2100, // Darklurker
                        1000, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1012, // Intruders
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m40_03_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Shulva)
                {
                    ScrambleEnemyLocation("m50_35_00_00", new List<int>
                    {
                        700, // Jester Thomas
                        710, // Abbess Feeva
                        711, // Ruined Alfis
                        712, // Steelheart Ellie
                        713, // Transcendent Edde
                        714, // Rapacious Andrei
                        715, // Benhart of Jugo

                        720, // Rockshield Baldyr
                        721, // Rhoy the Explorer
                        722, // Stewart the Explorer
                        723, // Courth the Explorer
                        974, // Forlorn

                        900, // Elana
                        8500, 8501, 8502, 8503, 8504, 8505, 8506, 8057, 8508, // Elana Summons
                        901, 9000 // Sinh
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_35_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Brume_Tower)
                {
                    ScrambleEnemyLocation("m50_36_00_00", new List<int>
                    {
                        337, // Voice Marker

                        730, // Quicksword Rachel
                        731, // Maldron the Assassin
                        732, 9500, 9502, // Evangelist Scion Waslo
                        735,736, 737, 738, // Swashbuckling Matelas
                        977, // Forlorn
                        7000, 7001, 7002, 7003, 7004, // Prowlers

                        740, // Carhillion of the Fold
                        741, // Steelheart Ellie
                        742, 744, // Steel-willed Lorrie
                        743, 745, // Drifter Swordsman Aidel

                        903, // Fume Knight
                        904, // Sir Alonne
                        905, // Blue Smelter Demon
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_36_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Eleum_Loyce)
                {
                    ScrambleEnemyLocation("m50_37_00_00", new List<int>
                    {
                        340, 342, 343, 344, // Alsanna

                        760, // Castaway Witch Donna
                        761, // Hexer Nicholai
                        762, // Holy Knight Aurheim
                        980, // Forlorn
                        
                        770, // Lucatiel of Mirrah
                        771, // Twiggy Shei
                        772, // Masterless Glencour
                        773, // Steelheart Ellie
                        774, // Vengarl
                        775, // Abbess Feeva
                        776, // Manhunter O'Harra
                        777, // Sellsword Luet
                        5320, // Teeth-chattering Dan

                        7505, 7506, 7600, 7602, 7603, // Phantom

                        907, // Lud
                        908, // Burnt Ivory King
                        4110, 4111, 4112, 4113, 4114, 4115, 4116, 4117, 4118, 
                        4119, 4120, 4121, // Charred Loyce Knights
                        909, 910, // Aava

                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_37_00_00", new List<int>
                        {
                            // None
                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }

                if (enable_Memory_of_the_King)
                {
                    ScrambleEnemyLocation("m50_38_00_00", new List<int>
                    {
                        1000, // Vendirck
                        2000, // Velstadt
                    });

                    if (T_IncludeCharacters)
                    {
                        ScrambleCharacterLocation("m50_38_00_00", new List<int>
                        {

                        },
                        new List<Position>
                        {
                            // None
                        });
                    }
                }
            }

            return regulation;
        }

        public void ScrambleEnemyLocation(string map_id, List<int> Ignored_Generator_ID_List)
        {
            Console.WriteLine($"ScrambleEnemyLocation - {map_id}");

            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name == $"generatorlocation_{map_id}")
                {
                    param_rows = param_rows.Where(r => 
                    !Ignored_Generator_ID_List.Contains(r.ID)
                    ).ToList();

                    AssignNewLocations(param_rows);
                }
            }
        }

        public void ScrambleCharacterLocation(string map_id, List<int> Character_Generator_ID_List, List<Position> additionaLocations)
        {
            Console.WriteLine($"ScrambleCharacterLocation - {map_id}");

            if (Character_Generator_ID_List.Count > 0)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    PARAM param = wrapper.Param;
                    List<PARAM.Row> param_rows = param.Rows;

                    if (wrapper.Name == $"generatorlocation_{map_id}")
                    {
                        param_rows = param_rows.Where(r =>
                        Character_Generator_ID_List.Contains(r.ID)
                        ).ToList();

                        AssignNewLocations(param_rows);
                    }
                }
            }
        }

        public void AssignNewLocations(List<PARAM.Row> param_rows)
        {
            List<Position> positionList = new List<Position>();

            // Build position list
            foreach (PARAM.Row row in param_rows)
            {
                positionList.Add(
                    new Position(
                        (float)row["PositionX"].Value,
                        (float)row["PositionY"].Value,
                        (float)row["PositionZ"].Value,
                        (float)row["RotationX"].Value,
                        (float)row["RotationY"].Value,
                        (float)row["RotationZ"].Value
                    )
                );
            }

            // Shuffle positions
            positionList.Shuffle(rand);
            int index = 0;

            // Assign new positions
            foreach (PARAM.Row row in param_rows)
            {
                row["PositionX"].Value = positionList[index].PosX;
                row["PositionY"].Value = positionList[index].PosY;
                row["PositionZ"].Value = positionList[index].PosZ;

                row["RotationX"].Value = positionList[index].RotX;
                row["RotationY"].Value = positionList[index].RotY;
                row["RotationZ"].Value = positionList[index].RotZ;

                index = index + 1;
            }
        }
        #region Enemy Type

        Dictionary<string, List<int>> enemyDictPerMap = new Dictionary<string, List<int>>();

        public void ScrambleEnemyTypes()
        {
           
            //Console.WriteLine("Dragonrider Test");
            //ChangeEnemyType("m10_31_00_00", 61100000, 842, "m10_02_00_00", 30000000, 1600);
        }

        public void ChangeEnemyType(string target_map_id, int target_generator_id, int target_regist_id, string source_map_id, int source_generator_id, int source_regist_id)
        {
            List<ParamWrapper> target_regist_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorregistparam_{target_map_id}")).ToList();
            List<ParamWrapper> target_generator_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorparam_{target_map_id}")).ToList();

            Console.WriteLine($"target_regist_wrapper: {target_regist_wrapper.Count}");
            Console.WriteLine($"target_generator_wrapper: {target_generator_wrapper.Count}");

            List<ParamWrapper> source_regist_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorregistparam_{source_map_id}")).ToList();
            List<ParamWrapper> source_generator_wrapper = regulation.regulationParamWrappers.Where(d => d.Name == ($"generatorparam_{source_map_id}")).ToList();

            Console.WriteLine($"source_regist_wrapper: {source_regist_wrapper.Count}");
            Console.WriteLine($"source_generator_wrapper: {source_generator_wrapper.Count}");

            List<PARAM.Row> target_regist_rows_id_verification = target_regist_wrapper[0].Rows;

            List<PARAM.Row> target_regist_rows = target_regist_wrapper[0].Rows.Where(r => r.ID == target_generator_id).ToList();
            List<PARAM.Row> target_generator_rows = target_generator_wrapper[0].Rows.Where(row => row.ID == target_regist_id).ToList();

            Console.WriteLine($"target_regist_rows: {target_regist_rows.Count}");
            Console.WriteLine($"target_generator_rows: {target_generator_rows.Count}");

            List<PARAM.Row> source_regist_rows = source_regist_wrapper[0].Rows.Where(r => r.ID == source_generator_id).ToList();
            List<PARAM.Row> source_generator_rows = source_generator_wrapper[0].Rows.Where(row => row.ID == source_regist_id).ToList();

            Console.WriteLine($"source_regist_rows: {source_regist_rows.Count}");
            Console.WriteLine($"source_generator_rows: {source_generator_rows.Count}");

            PARAM.Row target_regist = target_regist_rows[0];
            PARAM.Row target_generator = target_generator_rows[0];

            Console.WriteLine($"target_regist: {target_regist.ID}");
            Console.WriteLine($"target_generator: {target_generator.ID}");

            PARAM.Row source_regist = source_regist_rows[0];
            PARAM.Row source_generator = source_generator_rows[0];

            Console.WriteLine($"source_regist: {source_regist.ID}");
            Console.WriteLine($"source_generator: {source_generator.ID}");

            // Change Dragonrider
            int new_id = target_regist.ID;

            bool isUnique = false;
            while (!isUnique)
            {
                new_id = new_id + 1;
                isUnique = IsUniqueRowID(target_regist_rows_id_verification, new_id);
                Console.WriteLine($"Unique ID Search result: {new_id} - {isUnique}");
            }

            target_regist.ID = new_id;
            target_regist["EnemyParamID"].Value = target_regist["EnemyParamID"].Value;
            target_regist["LogicParamID"].Value = target_regist["LogicParamID"].Value;
            target_regist["DefaultLogicParamID"].Value = target_regist["DefaultLogicParamID"].Value;
            target_regist["SpawnState"].Value = 0; // Zero it out to be safe

            target_generator["GeneratorRegistParam"].Value = target_regist.ID;
        }

        public bool IsUniqueRowID(List<PARAM.Row> verification_rows, int new_id)
        {
            bool isUnique = false;

            foreach (PARAM.Row row in verification_rows)
            {
                if (row.ID == new_id)
                {
                    isUnique = false;
                }
            }

            return isUnique;
        }
        #endregion

        #region Enemy Tweaks
        public void ApplyEnemyAggressionMod()
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                PARAM param = wrapper.Param;
                List<PARAM.Row> param_rows = param.Rows;

                if (wrapper.Name.Contains("generatorparam"))
                {
                    string map_id = wrapper.Name.Replace("generatorparam_", "");

                    // Ignore boss rows
                    foreach (int row_id in Data.Per_Map_Boss_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    // Ignore character
                    foreach (int row_id in Data.Per_Map_Character_Dict[map_id])
                    {
                        param_rows = param_rows.Where(row => row.ID != row_id).ToList();
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        row["AggroGroup"].Value = 2;
                        row["Leash Distance"].Value = 10000;
                    }
                }
            }
        }

        #endregion

        #region Util
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

        #endregion

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
