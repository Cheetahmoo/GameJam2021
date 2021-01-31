using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Engine
{
    public class GameManger : MonoBehaviour
    {
        private static GameManger instance;
        
        public const int ROOM_SIZE_WIDTH = 23;
        public const int ROOM_SIZE_HEIGHT = 13;
        public const float NEIGHBOR_MAX_DISTANCE = 4f;
        public const float NEIGHBOR_MIN_DISTANCE = 1.3f;
        public static int NumRooms { get; private set; } = 15;

        private List<Action> onStartCallBacks;

        void Awake()
        {
            onStartCallBacks = new List<Action>();
            if (instance != null)
                throw new Exception("There should only be one GameManger");

            instance = this;
        }

        private void Start()
        {
            StartCoroutine(startGame());
        }

        private IEnumerator startGame()
        {

            //returning 0 will make it wait 1 frame
            yield return 0;

            foreach (var action in onStartCallBacks)
            {
                action.Invoke();
            }
        }

        public static void RegisterOnStart(Action onStart, int p = -1)
        {
            if (p >= 0)
                instance.onStartCallBacks.Insert(p, onStart);
            else
                instance.onStartCallBacks.Add(onStart);
        }
    }
}