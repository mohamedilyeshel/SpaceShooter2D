using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoSinglton<PoolingManager>
{
    [Header("Ammount Of Bullets")]
    public int bulletsAmmount;
    [SerializeField]
    private int _bulletCollected;

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
        }
    }

    public void Spawn_Bullets(GameObject prefabToSpawn,Vector3 positionToSpawn, GameObject Container, List<GameObject> bulletList)
    {
        var bullet = Instantiate(prefabToSpawn, positionToSpawn, Quaternion.identity, Container.transform);
        bulletList.Add(bullet);
        bullet.SetActive(false);
    }

    public void shootBullets(GameObject p, bool isTripleEnable)
    {
        if(bulletsAmmount > 0)
        {
            if (isTripleEnable == false)
            {
                float y = p.transform.position.y + 0.8f;
                float x = p.transform.position.x;
                Vector3 pos = new Vector3(x, y, 0);
                shoot(Bullets, pos);               
            }
            else
            {
                Vector3 pos = p.transform.position;
                shoot(tripleBullets, pos);
            }
            AudioManager.Instance.LaserShotAudioPlay(bulletsAmmount);
            decreaseAmmo(isTripleEnable);
        }
        else
        {
            AudioManager.Instance.LaserShotAudioPlay(bulletsAmmount);
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

    public void decreaseAmmo(bool isTripleEnable)
    {
        if (isTripleEnable == false)
            bulletsAmmount--;
        else
            bulletsAmmount -= 3;

        UIManager.Instance.ammouCountUi();
    }
}
