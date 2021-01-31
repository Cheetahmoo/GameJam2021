using System;
using Engine.DataTypes;
using UnityEngine;

namespace Engine.Controllers
{
    public class TileController: MonoBehaviour
    {
        private static TileController instance;

        public GameObject DoorWallTop;

        public string WallName;

        private void Start()
        {
            if (instance != null)
                throw new Exception("There should only be 1 TileController, Please Remove one");

            instance = this;
        }

        public static void RenderRoom(Tile[,] rmLayout, GameObject[,] go)
        {
            Transform tr = go[0, 0].GetComponentInParent<Transform>();
            
            foreach (var tile in rmLayout)
            {
                if (tile.Type == TileType.WallTop)
                {
                    RenderWall(go[tile.x, tile.y], tile, rmLayout, tr);
                }
                else if(tile.Type == TileType.Door)
                {
                    RenderDoor(go[tile.x, tile.y], tile, rmLayout, tr);
                }
            }
        }
        private static void RenderDoor(GameObject go, Tile t, Tile[,] rmLayout, Transform tr)
        {
            string name = "Door_";

            if (t.x - 1 < 0 || t.x + 1 >= rmLayout.GetLength(0))
                name += "H";
            else
                return;//nothing needs to be done;
            
            
            var sp = Resources.Load<GameObject>("Tiles/" + name);

            if (!sp)
                throw new MissingMemberException("Door for " + name + " is missing");

            var pos = go.transform.position;
            Destroy(go);
            go = Instantiate(sp, tr);
            go.transform.position = pos;
        }
        
        
        private static void RenderWall(GameObject gameObject, Tile t, Tile[,] rmLayout, Transform tr)
        {
            //Get each tile in up, down left and right
            var name = GetConnectedName(t, rmLayout);
            var sp = Resources.Load<GameObject>("Tiles/" + name);
            
            if (!sp)
                throw new MissingMemberException("Sprite for " + name + " is missing");

            var pos = gameObject.transform.position;
            Destroy(gameObject);
            gameObject = Instantiate(sp, tr);
            gameObject.transform.position = pos;

            //throw new System.NotImplementedException();
        }


        private static string GetConnectedName(Tile t, Tile[,] rmLayout)
        {
            string wallName = instance.WallName + "_";

            if (t.x + 1 < rmLayout.GetLength(0) && t.Type == rmLayout[t.x + 1, t.y].Type)
            {
                wallName += "R";
                if (t.y - 1 > -1 && rmLayout[t.x, t.y - 1].Type == TileType.Door)
                    return instance.WallName + "_Door_H";
            }

            if (t.x - 1 > -1 && t.Type == rmLayout[t.x - 1, t.y].Type)
                wallName += "L";

            if (t.y + 1 < rmLayout.GetLength(1) && t.Type == rmLayout[t.x, t.y + 1].Type)
                wallName += "U";

            if (t.y - 1 > -1 && t.Type == rmLayout[t.x, t.y - 1].Type)
                wallName += "D";

            return wallName;
        }
    }
}