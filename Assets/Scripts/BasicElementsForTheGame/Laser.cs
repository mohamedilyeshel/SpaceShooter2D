using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Bullet State")]
    public int _speed = 8;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(transform.parent.tag != "TripleShot")
            StartCoroutine(hide());   
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.tag != "TripleShot")
            laserMvment();
    }

    public IEnumerator hide()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

    public virtual void laserMvment()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
