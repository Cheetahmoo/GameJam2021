using System.Diagnostics;
using Engine.DataTypes;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Engine.Controllers
{
    public class MapController
    {
        public static void DisplayMap(Room[] rooms)
        {

            /*var lastPos = Vector2.zero;
            //AssignNeighbors(Rooms);

            int y = 0;
            int numLeft = 0;
            while (numLeft < rooms.Length)
            {

                for (int i = 0; i < GameManger.ROOM_SIZE; i++)
                {
                    if (numLeft >= rooms.Length)
                        break;

                    var pos = MapController.GetRoomsOffset(rooms[numLeft]);
                    pos = pos + lastPos;
                    var rmGo = Instantiate(roomParent, transform);
                    rmGo.name = "Room: " + rooms[numLeft].Id;

                    rmGo.transform.position = pos;
                    roomsGO.Add(rooms[numLeft].Id, rmGo);
                    lastPos = pos;
                    numLeft++;
                }

                y++;
            }*/
        }
        
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