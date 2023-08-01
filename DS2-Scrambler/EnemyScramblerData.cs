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
        public HashSet<int> BlacklistAccessory { get; set; }

        public HashSet<int> BlacklistBullet { get; set; }

        public HashSet<int> BlacklistGoods { get; set; }

        public HashSet<int> BlacklistProtector { get; set; }

        public HashSet<int> BlacklistWeapon { get; set; }

        public HashSet<ushort>[] WepTypeGroups { get; set; }

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
