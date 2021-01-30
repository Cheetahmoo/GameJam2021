using Engine.DataTypes;
using UnityEngine;

namespace Engine.Controllers
{
    public class MapController
    {
        public static Vector2 GetRoomsOffset(Room rm)
        {
            var pos = new Vector2();

            if (rm.HasNeighborTo(Room.Door.Up) && rm.GetNeighbor(Room.Door.Up).Id < rm.Id)
                pos.y -= 1;

            else if (rm.HasNeighborTo(Room.Door.Down) && rm.GetNeighbor(Room.Door.Down).Id < rm.Id)
                pos.y += 1;

            else if (rm.HasNeighborTo(Room.Door.Right) && rm.GetNeighbor(Room.Door.Right).Id < rm.Id)
                pos.x -= 1;

            else if (rm.HasNeighborTo(Room.Door.Left) && rm.GetNeighbor(Room.Door.Left).Id < rm.Id)
                pos.x += 1;

            return pos;
        }
    }
}