using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2_Scrambler
{
    public class Position
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }

        public Position(float x, float y, float z, float rX, float rY, float rZ)
        {
            PosX = x;
            PosY = y;
            PosZ = z;
            RotX = rX;
            RotY = rY;
            RotZ = rZ;
        }
    }
}
