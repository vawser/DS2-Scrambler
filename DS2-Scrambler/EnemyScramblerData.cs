using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DS2_Scrambler
{
    public class EnemyScramblerData
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

        public static EnemyScramblerData Static { get; }

        static EnemyScramblerData()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\enemy_scrambler_data.json";
 
            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<EnemyScramblerData>(File.OpenRead(json_filepath), options);
        }
    }
}
