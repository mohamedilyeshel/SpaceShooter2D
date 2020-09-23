using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public enum _LaserDir
    {
        Up,
        Right,
        Left
    }

    [Header("Bullet State")]
    public int _speed = 8;
    [SerializeField]
    private _LaserDir _laserDirection;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        if(transform.parent.name == "BulletsContainer")
            StartCoroutine(hide());
    }

    private void OnDisable()
    {
        if (transform.parent.tag == "MultiShot")
            transform.localPosition = Vector3.zero;
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
        switch(_laserDirection)
        {
            case _LaserDir.Up:
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                break;
            case _LaserDir.Right:
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
                break;
            case _LaserDir.Left:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                break;
        }
    }
}
