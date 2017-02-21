
namespace LayerDefinitions
{
    public class Layers
    {
        public static int Floor = (1 << 8);
        public static int NonFloor = ~(1 << 8);
        public static int Wall = (1 << 9);
        public static int NonWall = ~(1 << 9);
        public static int Player = (1 << 10);
        public static int NonPlayer = ~(1 << 10);
        public static int Enemy = (1 << 11);
        public static int NonEnemy = ~(1 << 11);
    }
}

