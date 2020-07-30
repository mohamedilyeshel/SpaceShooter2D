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

    public IEnumerator SpawnPowerUp()
    {
        while(_spawnDone == false)
        {
            int i = Random.Range(0, powerUps.Count);
            if (powerUps[i].powerUpPrefab.activeInHierarchy == false && Time.time > 1 && i != 3)
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
                    EnemyPosition(e);
                    e.SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    public void EnemyPosition(GameObject e)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), 6.4f, 0);
        e.transform.position = pos;
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
