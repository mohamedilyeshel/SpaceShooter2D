using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShots : Laser
{
    private void OnEnable()
    {
        StartCoroutine(hide());
    }

    private void Update()
    {
        laserMvment();
    }

    public override void laserMvment()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.decreaseHealth();
            this.gameObject.SetActive(false);
        }
    }
}
