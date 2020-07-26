using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : Laser
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(hide());

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeInHierarchy == false)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        laserMvment();
    }
}
