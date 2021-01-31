using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    public float rotationOffset;
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (sr)
        {
            if (mousePos.x < transform.position.x)
            {
                sr.flipY = true;
            }
            else
            {
                sr.flipY = false;
            }
        }
        if (GetComponent<Gun>().playerControlled)
        {
            TurnToFace(mousePos, rotationOffset);
        }
    }

    void TurnToFace(Vector3 target, float offset = 0)
    {
        Vector3 aimDir = target - transform.position;
        float theta = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(theta + offset, Vector3.forward);
        transform.rotation = targetRotation;
    }
}
