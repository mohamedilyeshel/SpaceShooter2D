using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoSinglton<PoolingManager>
{
    [Header("Ammount Of Bullets")]
    public int bulletsAmmount;
    [SerializeField]
    private int _bulletCollected;
    private bool _canShoot =false;

    [Header("Normal Bullets Behavior")]
    [SerializeField]
    private GameObject _laser;
    public float laserFireRate = 0.15f;
    [SerializeField]
    private GameObject _bulletContainer;
    [SerializeField]
    private List<GameObject> Bullets = new List<GameObject>();

    [Header("TripleShot Bullets Behavior")]
    [SerializeField]
    private GameObject _tripleShot;
    public float tripleShotRate = 0.40f;
    [SerializeField]
    private GameObject _tripleContainer;
    [SerializeField]
    private List<GameObject> tripleBullets = new List<GameObject>();

    [Header("MultiShot Bullets Behavior")]
    [SerializeField]
    private GameObject _multiShotPrefab;
    [SerializeField]
    private GameObject _multiShotContainer;
    [SerializeField]
    private List<GameObject> _multishotBullets = new List<GameObject>();

    public void RefillAmmo()
    {
        bulletsAmmount += _bulletCollected;
    }

    public void spawnBullets(GameObject p)
    {
        for(int i = 0; i < bulletsAmmount; i++)
        {
            float y = p.transform.position.y;
            Spawn_Bullets(_laser, new Vector3(p.transform.position.x, y + 0.8f, 0), _bulletContainer, Bullets);
            Spawn_Bullets(_tripleShot, p.transform.position, _tripleContainer, tripleBullets);
            Spawn_Bullets(_multiShotPrefab, p.transform.position, _multiShotContainer, _multishotBullets);
        }
    }

    public void Spawn_Bullets(GameObject prefabToSpawn,Vector3 positionToSpawn, GameObject Container, List<GameObject> bulletList)
    {
        var bullet = Instantiate(prefabToSpawn, positionToSpawn, Quaternion.identity, Container.transform);
        bulletList.Add(bullet);
        bullet.SetActive(false);
    }

    public void shootBullets(GameObject p, Player.laserType laserType)
    {
        if(bulletsAmmount > 0)
        {
            if (laserType == Player.laserType.normalLaser)
            {
                float y = p.transform.position.y + 0.8f;
                float x = p.transform.position.x;
                Vector3 pos = new Vector3(x, y, 0);
                shoot(Bullets, pos);
                _canShoot = true;
            }
            else
            {
                Vector3 pos = p.transform.position;
                if (laserType == Player.laserType.tripleShot)
                {
                    if(bulletsAmmount < 3)
                    {
                        _canShoot = false;
                        AudioManager.Instance.LaserShotAudioPlay(_canShoot);
                        return;
                    }
                    else
                    {
                        _canShoot = true;
                        shoot(tripleBullets, pos);
                    }
                }
                else
                {
                    if(bulletsAmmount < 5)
                    {
                        _canShoot = false;
                        AudioManager.Instance.LaserShotAudioPlay(_canShoot);
                        return;
                    }
                    else
                    {
                        _canShoot = true;
                        shoot(_multishotBullets, pos);
                    }
                }
            }
            AudioManager.Instance.LaserShotAudioPlay(_canShoot);
            decreaseAmmo(laserType);
        }
        else
        {
            _canShoot = false;
            AudioManager.Instance.LaserShotAudioPlay(_canShoot);
        }
    }

    public void shoot(List<GameObject> bulletList, Vector3 pos)
    {
        foreach (var b in bulletList)
        {
            if (b.activeInHierarchy == false)
            {
                b.transform.position = pos;
                b.SetActive(true);
                return;
            }
        }
    }

    public void decreaseAmmo(Player.laserType laserType)
    {
        if (laserType == Player.laserType.normalLaser)
            bulletsAmmount--;
        else if (laserType == Player.laserType.tripleShot)
            bulletsAmmount -= 3;
        else
            bulletsAmmount -= 5;

        if (bulletsAmmount < 0)
        {
            bulletsAmmount = 0;
        }
        UIManager.Instance.ammouCountUi();
    }
}
