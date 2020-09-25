using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Power Up State")]
    private float _speed;
    private int i = -1;
    private bool _movingToPlayer = false;

    public enum powerUpsTypes
    {
        TripleShot,
        Speed,
        Sheild,
        ammoCollect,
        health,
        multiShot,
        negativeEffect
    }
    public powerUpsTypes currentPowerUp;

    [Header("If this is a speed power up")]
    public int speedAdd;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(3, 5);        
    }

    private void OnEnable()
    {
        _movingToPlayer = false;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        Mouvements();

        if(Input.GetKeyDown(KeyCode.C))
        {
            _movingToPlayer = true;
        }

        if (currentPowerUp == powerUpsTypes.health && GameManager.Instance.health == 3)
        {
            _movingToPlayer = false;
        }

        if (currentPowerUp == powerUpsTypes.ammoCollect && PoolingManager.Instance.bulletsAmmount == 15)
        {
            _movingToPlayer = false;
        }

        if (_movingToPlayer == true)
        {
            GoTowardsPlayer();
        }
    }

    void Mouvements()
    {
        if (transform.position.y <= -7.19f)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
    }

    public void GoTowardsPlayer()
    {
        Vector2 direction = GameManager.Instance.CalculateDirectionBetweenPlayerAndB(this.transform);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "LaserPower")
        {
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }

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

            if(currentPowerUp == powerUpsTypes.health && GameManager.Instance.health == 3)
            {
                return;
            }

            if(currentPowerUp == powerUpsTypes.ammoCollect && PoolingManager.Instance.bulletsAmmount == 15)
            {
                return;
            }

            Player p = collision.transform.GetComponent<Player>();
            p.runPowerUp(this, i);
            AudioManager.Instance.PowerUpPlay();
            gameObject.SetActive(false);
        }
    }
}
