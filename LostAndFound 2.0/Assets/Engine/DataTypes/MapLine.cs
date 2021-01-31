namespace Engine.DataTypes
{
    public class MapLine
    {
        private int room1ID;
        private int room2ID;

        public readonly string ID;
        public readonly string ID_revers;

        public MapLine(int room1ID, int room2ID)
        {
            this.room1ID = room1ID;
            this.room2ID = room2ID;
            ID = room1ID + "-" + room2ID;
            ID_revers =  room2ID+ "-" + room1ID;
        }

        public bool ConnectedToRoom(int id)
        {
            if (id == room1ID)
                return true;
            else if (id == room2ID)
                return true;

            return false;
        }
    }
}