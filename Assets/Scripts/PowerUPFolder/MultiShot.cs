using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShot : Laser
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(hide());
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetChild(0).gameObject.activeInHierarchy == false)
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public override void laserMvment()
    {
        // I don't need to move the MultiShot Prefab i need just to hide it
    }
}
