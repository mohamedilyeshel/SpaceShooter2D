using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSinglton<SpawnManager>
{
    [Header ("Enemy Part Spawn")]
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private int _enemyCounter;
    public List<GameObject> _enemies = new List<GameObject>();
    [SerializeField]
    private float _enemySpawnTime = 5.0f;

    [Header ("Power Ups Part Spawn")]    
    [SerializeField]
    private GameObject _powerUpContainer;
    public List<PowerU> powerUps = new List<PowerU>();

    [Header("")]
    [SerializeField]
    private bool _spawnDone = false;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemies();
        SpawnPowerUps(); 
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(SpawnAmmoPowerUP());
    }

    public void SpawnPowerUps()
    {
        for(int i = 0; i < powerUps.Count; i++)
        {
            var pUp = Instantiate(powerUps[i].powerUpPrefab, transform.position, Quaternion.identity, _powerUpContainer.transform);
            powerUps[i].powerUpPrefab = pUp;
            pUp.SetActive(false);
        }
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < _enemyCounter; i++)
        {
            GameObject enemy = Instantiate(_enemy, new Vector3(Random.Range(-9, 9), 6.4f, 0), Quaternion.identity, _enemyContainer.transform);
            _enemies.Add(enemy);

            Enemy enem = enemy.GetComponent<Enemy>();     
            for(int y = 0; y < enem.enemyBulletAmount; y++)
            {
                PoolingManager.Instance.Spawn_Bullets(enem.bulletPrefab, enem.transform.position, enem.transform.GetChild(0).gameObject, enem.enemyBullets);
            }
            enemy.SetActive(false);
        }
    }

    // Items can be splited to Commun with 0.96 chance to spawn - Rare with 0.03 chance to spawn - Epic with 0.01 chance to spawn

    public IEnumerator SpawnPowerUp()
    {
        while(_spawnDone == false)
        {
            var i = SpawnChances();
            if (powerUps[i].powerUpPrefab.activeInHierarchy == false && Time.time > 15 && i != 3)
            {
                AddThePowerUP(powerUps[i].powerUpPrefab);
                yield return new WaitForSeconds(30.0f);
            }
            else
            {
                yield return null;
            }
        }
    }

    private int SpawnChances()
    {
        int itemID = -1;
        while(itemID == -1)
        {
            int i = Random.Range(0, powerUps.Count);
            float c = Random.Range(0f, 1f);
            int type = -1;

            if (c < 0.2f)
            {
                type = 2; // Epic Item
            }
            else if (c > 0.2f && c < 0.4f)
            {
                type = 1; // Rare Item
            }
            else if (c > 0.4f)
            {
                type = 0; // Commun Item
            }

            if ((int)powerUps[i]._typeOfItem == type)
            {
                itemID = i;
            }                
        }

        return itemID;
    }

    public IEnumerator SpawnAmmoPowerUP()
    {
        while(_spawnDone == false)
        {
            AddThePowerUP(powerUps[3].powerUpPrefab);
            yield return new WaitForSeconds(10.0f);
        }
    }

    private void AddThePowerUP(GameObject prefab)
    {
        prefab.transform.position = new Vector3(Random.Range(-9, 10), 7.0f, 0);
        prefab.SetActive(true);
    }

    public IEnumerator SpawnEnemy()
    {
        while(_spawnDone == false)
        {
            foreach(var e in _enemies)
            {
                if(e.activeInHierarchy == false)
                {
                    Enemy en = e.GetComponent<Enemy>();

                    if(en != null)
                    {
                        en.ChooseRandomMouvementType();
                        EnemyMouvementType(en);
                    }

                    e.SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    public void EnemyMouvementType(Enemy e)
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        switch (e._currentMouvementType)
        {
            case Enemy.EnemyMouvementTypes.Vertical:
                pos = new Vector3(Random.Range(-9, 9), 6.4f, 0);
                rot = Quaternion.identity;
                break;
            case Enemy.EnemyMouvementTypes.RightHorizontal:
                pos = new Vector3(11.91f, Random.Range(-0.75f,-5.28f), 0);
                rot = Quaternion.AngleAxis(-90, Vector3.forward);
                break;
            case Enemy.EnemyMouvementTypes.LeftHorizontal:
                pos = new Vector3(-11.91f, Random.Range(-0.75f, -5.28f), 0);
                rot = Quaternion.AngleAxis(90, Vector3.forward);                
                break;
            case Enemy.EnemyMouvementTypes.Diagonale:
                pos = new Vector3(Random.Range(10.87f, 16.43f), Random.Range(11.44f, 5.88f), 0);
                rot = Quaternion.AngleAxis(-45, Vector3.forward);
                break;
        }
        e.transform.position = pos;
        e.transform.rotation = rot;
    }

    public void ChangeSpawnTime()
    {
        if (_enemySpawnTime > 1)
            _enemySpawnTime--;
        else
            GameManager.Instance.ChangeTimeBool();
    }

    public void IsPlayerDead()
    {
        _spawnDone = true;
        Destroy(_enemyContainer);
        Destroy(_powerUpContainer);
    }
}
