using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DS2_Scrambler
{
    public enum EnemyCategory {
        Basic,
        Boss,
        BossBasic,
        Character,
        Invader,
        Summon,
        NewGamePlus,
        Other
    }

    public class Enemy
    {
        PARAM.Row EnemyGeneratorRow;
        PARAM.Row EnemyRegistRow;

        public string ChrID { get; set; }
        public EnemyCategory EnemyCategory { get; set; }

        // GeneratorRegistParam
        public uint RegistID { get; set; }
        public int EnemyParamID { get; set; }
        public int LogicParamID { get; set; }
        public int DefaultLogicParamID { get; set; }
        public ushort SpawnState { get; set; }
        public byte DrawGroup { get; set; }

        // GeneratorParam
        public uint GeneratorID { get; set; }
        public uint GeneratorRegistParamID { get; set; }
        public byte AggroGroup { get; set; }
        public uint ApperanceEventID { get; set; }

        // GeneratorLocationParam
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }


        public Enemy(PARAM.Row generator_row, PARAM.Row regist_row, PARAM.Row location_row)
        {
            EnemyGeneratorRow = generator_row;
            EnemyRegistRow = regist_row;

            ChrID = EnemyRegistRow.ID.ToString().Substring(0, 4);

            RegistID = (uint)EnemyRegistRow.ID;
            EnemyParamID = (int)EnemyRegistRow["EnemyParamID"].Value;
            LogicParamID = (int)EnemyRegistRow["LogicParamID"].Value;
            DefaultLogicParamID = (int)EnemyRegistRow["DefaultLogicParamID"].Value;
            SpawnState = (ushort)EnemyRegistRow["SpawnState"].Value;
            DrawGroup = (byte)EnemyRegistRow["DrawGroup"].Value;

            GeneratorID = (uint)EnemyGeneratorRow.ID;
            GeneratorRegistParamID = (uint)EnemyGeneratorRow["GeneratorRegistParam"].Value;
            AggroGroup = (byte)EnemyGeneratorRow["AggroGroup"].Value;
            ApperanceEventID = (uint)EnemyGeneratorRow["ApperanceEventID"].Value;

            PositionX = (float)location_row["PositionX"].Value;
            PositionY = (float)location_row["PositionY"].Value;
            PositionZ = (float)location_row["PositionZ"].Value;
            RotationX = (float)location_row["RotationX"].Value;
            RotationY = (float)location_row["RotationY"].Value;
            RotationZ = (float)location_row["RotationZ"].Value;

            EnemyCategory = EnemyCategory.Basic; // Default
        }
    }
}
