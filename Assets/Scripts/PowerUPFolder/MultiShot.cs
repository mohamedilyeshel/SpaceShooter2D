using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShot : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(hide());
    }

    public IEnumerator hide()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }

}
