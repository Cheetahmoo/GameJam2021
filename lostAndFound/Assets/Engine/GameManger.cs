using UnityEngine;

namespace Engine
{
    public class GameManger : MonoBehaviour
    {
        public const int ROOM_SIZE = 15;
        public const int NEIGHBOR_MAX_DISTATNCE = 4;
        public static int NumRooms { get; private set; } = 15;
    }
}