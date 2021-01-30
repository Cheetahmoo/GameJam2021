using System;
using System.Collections.Generic;
using Engine.DataTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.Controllers
{
    public class RoomController : MonoBehaviour
    {
        public Room CurrentRoom { get; private set; }
        public Room[] Rooms { get; private set; }

        public GameObject roomParent;

        [Tooltip("Prefabs for all Tiles in the Game")]
        public GameObject[] tilePrefs;

        private Dictionary<int, GameObject> roomsGO;
        
        //Controllers
        public EnemyController EC;

        private void Start()
        {
            roomsGO = new Dictionary<int, GameObject>();
            Rooms = CreateRooms(GameManger.NumRooms);
            var lastPos = Vector2.zero;
            //AssignNeighbors(Rooms);

            int y = 0;
            int numLeft = 0;
            while (numLeft < Rooms.Length)
            {
                
                for (int i = 0; i < GameManger.ROOM_SIZE; i++)
                {
                    if (numLeft >= Rooms.Length)
                        break;

                    var pos = MapController.GetRoomsOffset(Rooms[numLeft]);
                    Debug.LogError("Last Pos: " + lastPos + ", Offest: " + pos + ", New Pos: " + (pos + lastPos));
                    pos = pos + lastPos;
                    var rmGo = Instantiate(roomParent, transform);

                    rmGo.transform.position = pos;
                    roomsGO.Add(Rooms[numLeft].Id, rmGo);
                    lastPos = pos;
                    numLeft++;
                }
                y++;
            }

            CurrentRoom = Rooms[0];
        }


        void Update()
        {
            if (Input.GetKeyDown("w"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Up);
                if (r != null)
                    UpdateCurrRoom(r);
            }else if (Input.GetKeyUp("s"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Down);
                if (r != null)
                    UpdateCurrRoom(r);
            }else if (Input.GetKeyUp("a"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Left);
                if (r != null)
                    UpdateCurrRoom(r);
            }
            else if (Input.GetKeyUp("d"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Right);
                if (r != null)
                    UpdateCurrRoom(r);
            }
        }
        private void UpdateCurrRoom(Room room)
        {
            Debug.Log("UpdateCurrRoom");
            
            var go = roomsGO[CurrentRoom.Id];
            go.GetComponent<SpriteRenderer>().color = Color.black;

            go = roomsGO[room.Id];
            go.GetComponent<SpriteRenderer>().color = Color.gray;
            CurrentRoom = room;
        }


        private void AssignNeighbors(Room[] rms)
        {
            
            for (int i = 0; i < rms.Length; i++)
            {
                int neighIndex = -1;
                while (neighIndex <= 0 || neighIndex >= rms.Length)
                {
                    neighIndex = Random.Range(-GameManger.NEIGHBOR_MAX_DISTATNCE, GameManger.NEIGHBOR_MAX_DISTATNCE)+i;
                }

                Room.Door door = GetRandomDoor();
                
                //Loop until a free door is found on rms[i] and the opposit door is oppen on the new neighor
                try
                {
                    if (!rms[i].HasNeighborTo(door) && !rms[neighIndex].HasNeighborTo(Room.GetOppDoor(door)))
                    {
                        rms[i].AddNeighborRoom(door, rms[neighIndex]);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                    throw new Exception("Room door#: " + (int)door + ", HasNeighborTo: " + rms[i].HasNeighborTo(door) + ", "+ Room.GetOppDoor(door) +" new Room: " + rms[neighIndex].HasNeighborTo(Room.GetOppDoor(door)));
                }
            }
        }

        /// <summary>
        /// Creates rooms and Ties links to each other
        /// </summary>
        private Room[] CreateRooms(int numRooms)
        {
            Room.Door lastDoor = Room.Door.Right;
            List<Room> rms = new List<Room>();
            for (int i = 0; i < numRooms; i++)
            {
                Room.Door newDoor = lastDoor;
                while (lastDoor == newDoor)
                {
                    newDoor = GetRandomDoor();
                }

                if (CurrentRoom == null)
                {
                    CurrentRoom = new Room(0, Vector2.zero, null, Room.Door.Left);
                    rms.Add(CurrentRoom);
                }
                else
                {
                    rms.Add(CurrentRoom.CreateNeighborRoom(i, Vector2.zero, newDoor, i == numRooms));
                    CurrentRoom = rms[i];
                }
            }

            return rms.ToArray();
        }
        private Room.Door GetRandomDoor()
        {
            int num = Random.Range(0, 4);
            switch (num)
            {
                case 0:
                    return Room.Door.Up;
                case 1:
                    return Room.Door.Down;
                case 2:
                    return Room.Door.Left;
                default:
                    return Room.Door.Right;
            }
        }


        public void DisplayRoom(Room rm)
        {
            if (!rm.IsActived())
            {
                rm.Activate(17);
            }

            if (roomsGO.ContainsKey(rm.Id))
                throw new Exception("GameObject already is tied to this Rooms id: " + rm.Id);

            var rmGO = Instantiate(roomParent, this.transform);

            foreach (var tile in rm.Layout)
            {
                var tGo = Instantiate(tilePrefs[(int)tile.Type], rmGO.transform);
                tGo.transform.position = new Vector2(tile.x, tile.y);
            }
        }
        
        
    }
}