using System;
using System.Collections;
using System.Collections.Generic;
using Engine;
using Engine.Controllers;
using Engine.DataTypes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    
    [Tooltip("Image Render for the Map tooken")]
    public GameObject mapPointPref;
    public GameObject mapPref;
    public GameObject LinePref;

    public Vector2 Offset;
    public float posMul;

    public Vector2 lineOffest;
    
    public GameObject Map;
    private Dictionary<int, GameObject> mapPoints;
    private Dictionary<string, GameObject> mapLines;
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        mapPoints = new Dictionary<int, GameObject>();
        mapLines = new Dictionary<string, GameObject>();
        
        Map = Instantiate(mapPref, this.transform);
        Map.SetActive(false);

        GameManger.RegisterOnStart(OnStart,0);
    }

    void OnStart()
    {
        Debug.Log("OnStart");
        var rmC = FindObjectOfType<RoomController>();
        rmC.RegistarOnRoomsCreaded(OnRoomsCreated);
        rmC.RegistarOnRoomsChanged(OnRoomChanged);
    }

    void OnRoomsCreated(Room startRM)
    {
        Debug.Log("Room Reated");
        
        Queue<Room> foundRooms = new Queue<Room>();
        foundRooms.Enqueue(startRM);
        //Map Out the Rooms
        while (foundRooms.Count > 0)
        {
            Room crm = foundRooms.Dequeue();
            for (int j = 0; j < 4; j++)
            {
                Room rm = crm.GetNeighbor((Room.Door)j);
                if (rm != null)
                {

                    if (mapPoints.ContainsKey(rm.Id) == false)
                    {
                        CreateMapPoint(rm);
                        foundRooms.Enqueue(rm);
                    }

                    createLineConnection(crm,rm);
                }
            }
        }
        
        mapPoints[startRM.Id].GetComponent<Image>().color = Color.red;
        
        if(Map.transform.GetChild(1).name == "NoMap")
            Destroy(Map.transform.GetChild(1).gameObject);
    }
    private void createLineConnection(Room crm, Room rm)
    {
        MapLine ml = new MapLine(crm.Id, rm.Id);
        if (!mapLines.ContainsKey(ml.ID) && !mapLines.ContainsKey(ml.ID_revers))
        {
            var go = Instantiate(LinePref, Map.transform);
            var lr = go.GetComponent<LineRenderer>();

            var pos = Camera.main.ScreenToWorldPoint((crm.Position * posMul) + Offset + lineOffest);
            var pos2 = Camera.main.ScreenToWorldPoint((rm.Position * posMul) + Offset + lineOffest);

            pos.z = 0;
            pos2.z = 0;
            
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            go.name = "LINE: "+ ml.ID;
            mapLines.Add(ml.ID, go);
        }
    }

    private void CreateMapPoint(Room rm)
    {
        var go = Instantiate(mapPointPref, Map.transform);
        var pos = rm.Position * posMul;

        var i = go.GetComponent<Image>();
        var color = new Color(1, Random.Range(100, 150), Random.Range(50, 100));
        i.color = color;
        i.GraphicUpdateComplete();

        go.transform.localPosition = pos+Offset;
        go.name = "Room: "+ rm.Id;
        mapPoints.Add(rm.Id, go);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if(Map.activeSelf)
                Map.SetActive(false);
            else
                Map.SetActive(true);
        }
        
    }

    void OnRoomChanged(Room pre, Room Nrm)
    {
        if (mapPoints.ContainsKey(pre.Id))
        {
            mapPoints[pre.Id].GetComponent<Image>().color = Color.white;
            mapPoints[Nrm.Id].GetComponent<Image>().color = Color.red;
        }
    }
}
