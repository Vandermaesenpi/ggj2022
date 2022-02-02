using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderDepth : MonoBehaviour
{
    public int offset;
    public SpriteRenderer rend;

    void Update()
    {
        float lowerY = GM.I.cam.transform.position.y - GM.I.cam.cameraBounds.z;
        float upperY = GM.I.cam.transform.position.y + GM.I.cam.cameraBounds.w;
        float zOrderIncrement = (upperY - lowerY) / 100f;
        rend.sortingOrder = (100 - (int)((transform.position.y - lowerY) / zOrderIncrement)) * 2 + offset;
    }
}
