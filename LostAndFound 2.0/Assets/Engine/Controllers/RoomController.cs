﻿using System;
using System.Collections.Generic;
using Engine.DataTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.Controllers
{
    public class RoomController : MonoBehaviour
    {
        private Action<Room> onRoomsCreatedCallback;
        private Action<Room, Room> onRoomsChangedCallback;

        private Room _crrRoom;
        public Room CurrentRoom
        {
            get { return _crrRoom; }
            private set
            {
                Debug.Log("Current Room has Changed to: " + value.Id + ", Last room was: " + _crrRoom?.Id);
                onRoomsChangedCallback(_crrRoom, value);
                _crrRoom = value;
            }
        }
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

            GameManger.RegisterOnStart(OnStart, 1);
            onRoomsChangedCallback += CurrentRoomChanged;
        }

        private void OnStart()
        {
            Rooms = CreateRooms(GameManger.NumRooms);
            AssignNeighbors(Rooms);
            SetRoomPositions(Rooms);
            //TODO add room connections

            _crrRoom = Rooms[0];
            onRoomsCreatedCallback.Invoke(CurrentRoom);
            DisplayRoom(CurrentRoom);
        }

        void Update()
        {
            if (Input.GetKeyDown("w"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Up);
                if (r != null)
                    UpdateCurrRoom(r);
            }
            else if (Input.GetKeyUp("s"))
            {
                var r = CurrentRoom.GetNeighbor(Room.Door.Down);
                if (r != null)
                    UpdateCurrRoom(r);
            }
            else if (Input.GetKeyUp("a"))
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


        /// <param name="lrm">Last Room</param>
        private void CurrentRoomChanged(Room lrm, Room nrm)
        {

            //Debug.Log("Current Room has Changed to: " + CurrentRoom.Id);
            if (lrm != null && roomsGO.ContainsKey(lrm.Id))
            {
                roomsGO[lrm.Id].SetActive(false);
            }

            if (nrm != null && roomsGO.ContainsKey(nrm.Id) == false)
            {
                DisplayRoom(nrm);
            }
            else if (nrm != null)
            {
                roomsGO[nrm.Id].SetActive(true);
            }
        }

        private void UpdateCurrRoom(Room room)
        {
            CurrentRoom = room;
        }


        private void AssignNeighbors(Room[] rms)
        {

            for (int i = 0; i < rms.Length; i++)
            {
                int neighIndex = -1;
                while (neighIndex <= 0 || neighIndex >= rms.Length)
                {
                    neighIndex = Random.Range((int)-GameManger.NEIGHBOR_MAX_DISTANCE, (int)GameManger.NEIGHBOR_MAX_DISTANCE) + i;
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
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    throw new Exception("Room door#: " + (int)door + ", HasNeighborTo: " + rms[i].HasNeighborTo(door) + ", " +
                                        Room.GetOppDoor(door) + " new Room: " + rms[neighIndex].HasNeighborTo(Room.GetOppDoor(door)));
                }
            }
        }

        private void SetRoomPositions(Room[] rms)
        {
            Vector2 lastPos = Vector2.zero;
            //float minRange = GameManger.NEIGHBOR_MIN_DISTANCE;
            float range = GameManger.NEIGHBOR_MAX_DISTANCE;

            for (int i = 1; i < rms.Length; i++)
            {
                Vector2 newPos = Vector2.zero;
                int infCheck = 0;
                while (isOverlap(newPos, rms[i].Id) || newPos.x <= 0 || newPos.y <= 0 || newPos.y > 10 || newPos.x > 15)
                {
                    newPos = new Vector2(Random.Range(-range, range + 1), Random.Range(-range, range + 1));
                    newPos += lastPos;

                    infCheck++;
                    if (infCheck > 5000)
                        throw new Exception("Never Ending Loop Detected");
                }
                
                rms[i].SetPosition(newPos);
                lastPos = rms[i].Position;
            }
        }
        private bool isOverlap(Vector2 newPos, int id = -1)
        {
            if (id >= 0)
            {
                for (int i = 0; i < id; i++)
                {
                    if (Vector2.Distance(newPos, Rooms[i].Position) < GameManger.NEIGHBOR_MIN_DISTANCE)
                        return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Creates rooms and Ties links to each other
        /// </summary>
        private Room[] CreateRooms(int numRooms)
        {
            Room.Door lastDoor = Room.Door.Right;
            List<Room> rms = new List<Room>();
            Room room = CurrentRoom;
            for (int i = 0; i < numRooms; i++)
            {
                Room.Door newDoor = lastDoor;
                while (lastDoor == newDoor)
                {
                    newDoor = GetRandomDoor();
                }

                if (room == null)
                {
                    room = new Room(0, Vector2.zero, null, Room.Door.Left);
                    rms.Add(room);
                }
                else
                {
                    rms.Add(room.CreateNeighborRoom(i, Vector2.zero, newDoor, i == numRooms));
                    room = rms[i];
                }

                lastDoor = Room.GetOppDoor(newDoor);
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
                rm.Activate(GameManger.ROOM_SIZE_WIDTH, GameManger.ROOM_SIZE_HEIGHT);
            }

            if (roomsGO.ContainsKey(rm.Id))
                throw new Exception("GameObject already is tied to this Rooms id: " + rm.Id);

            var rmGO = Instantiate(roomParent, this.transform);
            rmGO.name = "Room: " + rm.Id;
            roomsGO.Add(rm.Id, rmGO);
            GameObject[,] tileGOs = new GameObject[rm.Layout.Length, rm.Layout.Length];

            foreach (var tile in rm.Layout)
            {
                var tGo = Instantiate(tilePrefs[(int)tile.Type], rmGO.transform);
                tGo.transform.position = new Vector2(tile.x, tile.y);
                tileGOs[tile.x, tile.y] = tGo;
            }

            TileController.RenderRoom(rm.Layout, tileGOs);
        }


        public void RegistarOnRoomsCreaded(Action<Room> callback)
        {
            onRoomsCreatedCallback += callback;
        }
        public void RegistarOnRoomsChanged(Action<Room,Room> callback)
        {
            onRoomsChangedCallback += callback;
        }
    }
}