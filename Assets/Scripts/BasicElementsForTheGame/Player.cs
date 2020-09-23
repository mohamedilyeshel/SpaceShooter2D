using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    [SerializeField]
    private SpriteRenderer _thruster;
    private bool _canRun = true;

    public enum laserType 
    {
        normalLaser,
        tripleShot,
        multiShot
    }
    [Header("PowerUps Active Or Not")]
    public laserType currentLaserType = laserType.normalLaser;
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
        if(currentLaserType == laserType.normalLaser)
        {
            _canFire = Time.time + PoolingManager.Instance.laserFireRate;
            PoolingManager.Instance.shootBullets(this.gameObject, currentLaserType);
        }
        else
        {
            _canFire = Time.time + PoolingManager.Instance.tripleShotRate;
            PoolingManager.Instance.shootBullets(this.gameObject, currentLaserType);
        }
    }

    private void Mouvements()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horiz, vert, 0) * (_speed + SpeedIncreaseNow()) * Time.deltaTime);

        if (UIManager.Instance.ThrusterLevel() == 0f)
            _canRun = false;
        else if (UIManager.Instance.ThrusterLevel() > 0.5f)
            _canRun = true;

        UIManager.Instance.UpdateThrusterBar(_thruster.enabled);

        if (transform.position.x > 11.5)
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
        if(_canRun == true)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _thruster.enabled = true;
                return _increasedSpeed;
            }
        }

       _thruster.enabled = false;
       return 0;
    }

    public void runPowerUp(PowerUp powerScript, int i)
    {
        switch (powerScript.currentPowerUp)
        {
            case PowerUp.powerUpsTypes.TripleShot:
                currentLaserType = laserType.tripleShot;
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
            case PowerUp.powerUpsTypes.health:
                GameManager.Instance.AddHealth();
                UIManager.Instance.healthUIUpdate();
                break;
            case PowerUp.powerUpsTypes.multiShot:
                currentLaserType = laserType.multiShot;
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
                currentLaserType = laserType.normalLaser;
                break;
            case PowerUp.powerUpsTypes.Speed:
                isSpeed = false;
                speedBoost(type);
                break;
            case PowerUp.powerUpsTypes.Sheild:
                isSheild = 0;
                activeAnimationSheild();
                break;
            case PowerUp.powerUpsTypes.multiShot:
                currentLaserType = laserType.normalLaser;
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
