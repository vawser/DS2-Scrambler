using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DS2_Scrambler
{
    public class ParamScramblerData
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

        public static ParamScramblerData Static { get; }

        static ParamScramblerData()
        {
            string json_filepath = AppContext.BaseDirectory + "\\Assets\\param_scrambler_data.json";

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            Static = JsonSerializer.Deserialize<ParamScramblerData>(File.OpenRead(json_filepath), options);
        }
    }
}
