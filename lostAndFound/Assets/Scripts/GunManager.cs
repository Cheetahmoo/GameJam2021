using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject primary;
    public GameObject secondary;
    public GunItem gi;
    public bool currGun;
    void Start()
    {
        currGun = false;
        secondary.SetActive(false);
        primary.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0 && !primary.GetComponent<Gun>().isReloading && !secondary.GetComponent<Gun>().isReloading)
        {
            currGun = !currGun;
            if (currGun)
            {
                primary.SetActive(false);
                secondary.SetActive(true);
            }
            else
            {
                secondary.SetActive(false);
                primary.SetActive(true);
            }
        }
    }
}
