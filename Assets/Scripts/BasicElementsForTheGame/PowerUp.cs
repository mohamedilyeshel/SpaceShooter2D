using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Power Up State")]
    private float _speed;
    private int i = -1;

    public enum powerUpsTypes
    {
        TripleShot,
        Speed,
        Sheild,
        ammoCollect,
        health
    }
    public powerUpsTypes currentPowerUp;

    [Header("If this is a speed power up")]
    public int speedAdd;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Mouvements();
    }

    void Mouvements()
    {
        if (transform.position.y <= -7.19f)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            int y = 0;
            while(i == -1 && y < SpawnManager.Instance.powerUps.Count)
            {
                if(SpawnManager.Instance.powerUps[y].powerUpPrefab == this.gameObject)
                {
                    i = y;
                }
                else
                {
                    y++;
                }
            }

            Player p = collision.transform.GetComponent<Player>();
            p.runPowerUp(this,i);
            AudioManager.Instance.PowerUpPlay();
            gameObject.SetActive(false);
        }
    }
}
