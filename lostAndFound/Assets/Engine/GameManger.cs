using UnityEngine;

namespace Engine
{
    public class GameManger : MonoBehaviour
    {
        public const int ROOM_SIZE_WIDTH = 17;
        public const int ROOM_SIZE_HEIGHT = 11;
        public const int NEIGHBOR_MAX_DISTATNCE = 4;
        public static int NumRooms { get; private set; } = 15;
    }
}