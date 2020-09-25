using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : Laser
{
    [SerializeField]
    private bool _bulletBehind;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(hide());
    }

    // Update is called once per frame
    void Update()
    {
        laserMvment();
    }

    public override void laserMvment()
    {
        if(_bulletBehind == false)
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        else
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.decreaseHealth();
            this.gameObject.SetActive(false);
        }
    }
}
