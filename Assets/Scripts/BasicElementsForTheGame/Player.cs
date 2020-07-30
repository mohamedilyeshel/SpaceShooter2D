using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField]
    private int _defaultSpeed;
    private int _speed;
    [SerializeField]
    private int _increasedSpeed;

    [Header("Player Action")]
    [SerializeField]
    private float _canFire = 0;

    [Header ("PowerUps Active Or Not")]
    [SerializeField]
    private bool isTripleShot = false;
    [SerializeField]
    private bool isSpeed = false;
    public int isSheild;

    // Start is called before the first frame update
    void Start()
    {
        _speed = _defaultSpeed;
        PoolingManager.Instance.spawnBullets(this.gameObject); // Spawning the bullets
    }

    // Update is called once per frame
    void Update()
    {
        Mouvements();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            laserShoot();
        }

        if (transform.Find("player_sheild").gameObject.activeInHierarchy == true && isSheild == 0)
        {
            activeAnimationSheild();
        }
    }

    private void laserShoot()
    {
        if(isTripleShot == false)
        {
            _canFire = Time.time + PoolingManager.Instance.laserFireRate;
            PoolingManager.Instance.shootBullets(this.gameObject, isTripleShot);
        }
        else
        {
            _canFire = Time.time + PoolingManager.Instance.tripleShotRate;
            PoolingManager.Instance.shootBullets(this.gameObject, isTripleShot);
        }
    }

    private void Mouvements()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horiz, vert, 0) * (_speed + SpeedIncreaseNow()) * Time.deltaTime);

        if(transform.position.x > 11.5)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        if(transform.position.x < -11.5)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
        if(transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        if (transform.position.y < -4.94)
        {
            transform.position = new Vector3(transform.position.x, -4.94f, 0);
        }
    }

    private int SpeedIncreaseNow()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return _increasedSpeed;
        }
        return 0;
    }

    public void runPowerUp(PowerUp powerScript, int i)
    {
        switch (powerScript.currentPowerUp)
        {
            case PowerUp.powerUpsTypes.TripleShot:
                isTripleShot = true;
                break;
            case PowerUp.powerUpsTypes.Speed:
                isSpeed = true;
                speedBoost(powerScript);
                break;
            case PowerUp.powerUpsTypes.Sheild:
                isSheild = 3;
                activeAnimationSheild();
                break;
            case PowerUp.powerUpsTypes.ammoCollect:
                PoolingManager.Instance.RefillAmmo();
                UIManager.Instance.ammouCountUi();
                break;
        }
        StartCoroutine(cooldownPowerUp(powerScript, SpawnManager.Instance.powerUps[i].powerUpCooldown));
    }

    IEnumerator cooldownPowerUp(PowerUp type, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        switch(type.currentPowerUp)
        {
            case PowerUp.powerUpsTypes.TripleShot:
                isTripleShot = false;
                break;
            case PowerUp.powerUpsTypes.Speed:
                isSpeed = false;
                speedBoost(type);
                break;
            case PowerUp.powerUpsTypes.Sheild:
                isSheild = 0;
                activeAnimationSheild();
                break;
        }
    }

    private void speedBoost(PowerUp powerSpeed)
    {
        if(isSpeed == true)
        {
            _speed += powerSpeed.speedAdd;
        }
        else
        {
            _speed = _defaultSpeed;
        }
    }

    private void activeAnimationSheild()
    {
        if (isSheild != 0)
        {
            transform.Find("player_sheild").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("player_sheild").gameObject.SetActive(false);
        }
        UIManager.Instance.SheildUIUpdate(isSheild);
    }

    public void decreaseHealth()
    {
        if (isSheild == 0)
        {
            GameManager.Instance.health--;
            UIManager.Instance.healthUIUpdate();
            GameManager.Instance.PlayerHurt();
        }
        else
        {
            isSheild--;
            UIManager.Instance.SheildUIUpdate(isSheild);
            return;
        }           
    }
}
