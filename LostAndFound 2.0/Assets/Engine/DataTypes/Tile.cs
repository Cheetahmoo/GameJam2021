namespace Engine.DataTypes
{
    public enum TileType
    {
        WallTop,
        WallSide,
        Door,
        Floor
    }

    public class Tile
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public TileType Type { get; private set; }

        //TODO: Define what a tile is

        public Tile(TileType t, int x, int y)
        {
            this.x = x;
            this.y = y;
            Type = t;

        }
    }
}