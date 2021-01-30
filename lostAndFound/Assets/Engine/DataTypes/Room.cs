using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Engine.DataTypes
{
    public class Room
    {
        // -------Mapping data-------
        public enum Door { Up, Down, Left, Right}

        //Location of the room
        public Vector2 Position { get; private set; }
        public int Id { get; private set; }

        private Room[] neighbors;

        private bool activated = false;

        // -------Looks, Layout, enemies-------

        public Tile[,] Layout { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        /// <summary>
        /// If this room is the end of the level
        /// </summary>
        public readonly bool IsEnd = false;

        private Action<Room> reachedEndCallBack;
        private Action<Room> onActivateCallBack;
        private Action<Room> onPlayerEnterCallback;
        private Action<Room> onPlayerExitCallback;

        public void Activate(int size)
        {
            //Make sure the room is not already activated
            if (activated)
                throw new InvalidOperationException("Can't Active: Room has Already been Activated");

            PopulateLayout(size);
            
            
            onActivateCallBack?.Invoke(this);
            activated = true;
        }
        private void PopulateLayout(int size)
        {
            Layout = new Tile[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (x == 0 || x == size - 1 || y == 0 || y == size - 1)
                        Layout[x, y] = new Tile(TileType.RoomBoarder, x, y);
                    else
                        Layout[x, y] = new Tile(TileType.Floor, x, y);
                }
            }

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (HasNeighborTo((Door)i))
                {
                    switch (i)
                    {
                        case 0:
                            Layout[0, size / 2] = new Tile(TileType.Door, 0, size / 2);
                            break;
                        case 2:
                            Layout[size-1, size / 2] =
                                new Tile(TileType.Door, size-1, size / 2);
                            break;
                        case 3:
                            Layout[size / 2, 0] = new Tile(TileType.Door, size / 2, 0);
                            break;
                        case 4:
                            Layout[size / 2, size-1] =
                                new Tile(TileType.Door, size / 2, size-1);
                            break;

                    }
                }
            }
        }

        //This is all of the mapping construction of a room.
        #region Construction
        /// <param name="id">ID of new room</param>
        /// <param name="pos">Position of the Room</param>
        /// <param name="parent">Room that Creating a Connection to this new room</param>
        /// <param name="door">What door is being used to get to this room</param>
        public Room(int id, Vector2 pos, Room parent, Door door, bool isEnd = false)
        {
            if(parent == null)
                Debug.LogWarning("Parent is Null");

            this.IsEnd = isEnd;

            Id = id;
            Position = pos;

            neighbors = new Room[4];
            
            neighbors[(int) GetOppDoor(door)] = parent;
        }
        
        ///<summary>
        /// Get opposite door so that if the player uses the right door to enter this room
        /// that means the the left door is used to get back to the parent room
        ///</summary>
        public static Door GetOppDoor(Door door)
        {
            if (door == Door.Up)
                return Door.Down;
            else if (door == Door.Down)
                return Door.Up;

            else if (door == Door.Right)
                return Door.Left;
            else
                return Door.Right;
        }

        /// <summary>
        /// Checks too see if a neighbor is already using a Door
        /// </summary>
        public bool HasNeighborTo(Door door)
        {
            return neighbors[(int)door] != null;
        }

        /// <summary>
        /// This will create a new room that is Connected to the existing room.
        /// </summary>
        /// <param name="id">Id for new room</param>
        /// <param name="pos">Position of new room</param>
        /// <param name="door">Door used to get to new room</param>
        public Room CreateNeighborRoom(int id, Vector2 pos, Door door, bool end = false)
        {
            if (activated == false && neighbors[(int)door] == null)
            {
                neighbors[(int)door] = new Room(id, pos, this, door, end);
            }
            else
                throw new InvalidOperationException("Can't Add Room: This room has Already been Accessed by the player.");

            return neighbors[(int)door];
        }
        
        /// <summary>
        /// Use to Connected an existing room to this room.
        /// </summary>
        /// <param name="rm">Room to connect To</param>
        public void AddNeighborRoom(Door dr, Room rm)
        {
            if (activated == false)
            {
                ConnectToExisting(dr, rm);
                
            }
            else
                throw new InvalidOperationException("Can't Add Room: This room has Already been Accessed by the player.");
        }

        /// <summary>
        /// Connects Current room and an existing room too gather.
        /// </summary>
        /// <param name="dr">Door used to get to the room</param>
        /// <param name="rm">room to connect too</param>
        private void ConnectToExisting(Door dr, Room rm)
        {
            Door oppDoor = GetOppDoor(dr);
            
            //Maker sure the Door is not in use already
            if (rm.HasNeighborTo(oppDoor))
            {
                throw new InvalidOperationException("Can't Connect Room: The Room(" + rm.Id + ") is already using this Door-> " + oppDoor);
            }

            if (neighbors[(int)dr] != null)
            {
                throw new InvalidOperationException("Can't Connect Room: This Room (" + Id + ") is already using this Door-> " + dr);
            }

            rm.neighbors[(int)oppDoor] = this;
            neighbors[(int)dr] = rm;
        }

        /// <summary>
        /// Gets all Connections that this room has.
        /// </summary>
        /// <returns>Array of Doors</returns>
        public Door[] GetDoorConnections()
        {
            List<Door> connections = new List<Door>();
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null)
                {
                    connections.Add((Door)i);
                }
            }

            return connections.ToArray();
        }

        public Room GetNeighbor(Door dr)
        {
            return neighbors[(int)dr];
        }
        #endregion


        #region Utilities
        
        public void RegisterCallBack_OnEnter(Action<Room> callback)
        {
            onPlayerEnterCallback += callback;
        }
        public void RegisterCallBack_OnExit(Action<Room> callback)
        {
            onPlayerExitCallback += callback;
        }
        public void RegisterCallBack_OnReachedEnd(Action<Room> callback)
        {
            reachedEndCallBack += callback;
        }
        public void RegisterCallBack_OnActivate(Action<Room> callback)
        {
            onActivateCallBack += callback;
        }
        

        #endregion


        public bool IsActived()
        {
            return activated;
        }
    }
}