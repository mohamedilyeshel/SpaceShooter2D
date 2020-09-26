using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private CircleCollider2D _bossCollider;
    [SerializeField]
    private GameObject _player;

    [Header("Boss State")]
    public int _health;
    [SerializeField]
    private int _ammountOfDamageTakenByLaser;
    [SerializeField]
    private int _ammountDamageTakenByRocket;
    [SerializeField]
    private bool _bossDead;
    [SerializeField]
    private Animator _bossanimator;
    [SerializeField]
    private AnimationClip _bossDiedAnimation;

    [Header("Boss Mouvement")]
    [SerializeField]
    private List<Transform> _waypoints = new List<Transform>();
    [SerializeField]
    private float _speed;
    private Transform _destination;

    [Header("Boss Shots Behavior")]
    [SerializeField]
    private GameObject _bossShotPrefab;
    [SerializeField]
    private GameObject _bossShotsContainer;
    [SerializeField]
    private List<GameObject> _bossShots = new List<GameObject>();
    [SerializeField]
    private int _shotsAmmount;
    [SerializeField]
    private float _bossFireRate = 0.5f;
    private float _canFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        _bossCollider = GetComponent<CircleCollider2D>();

        _destination = _waypoints[0];

        for(int i = 0; i < _shotsAmmount; i++)
        {
            PoolingManager.Instance.Spawn_Bullets(_bossShotPrefab, _bossShotsContainer.transform.position, _bossShotsContainer, _bossShots);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateBossToPlayerAndShoot();

        if(_health < 50)
        {
            if(transform.position == _waypoints[0].position)
            {
                _destination = _waypoints[1];
            }
            else if(transform.position == _waypoints[1].position)
            {
                _destination = _waypoints[0];
            }

            transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
        }
    }

    public IEnumerator DeadBoss()
    {
        if (_bossDead == false)
        {
            SpawnManager.Instance.enemiesStillAlive--;
            _bossDead = true;
            _bossanimator.SetTrigger("bossDead");
            AudioManager.Instance.ExplosionPlay();
            yield return new WaitForSeconds(_bossDiedAnimation.length);
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void RotateBossToPlayerAndShoot()
    {
        if (_bossCollider.enabled == true && _player != null)
        {
            Vector2 direction = GameManager.Instance.CalculateDirectionBetweenPlayerAndB(this.transform);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);

            if(Time.time > _canFire)
            {
                _canFire = Time.time + _bossFireRate;
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        var pos = _bossShotsContainer.transform.position;
        PoolingManager.Instance.shoot(_bossShots, pos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Laser"))
        {
            _health -= _ammountOfDamageTakenByLaser;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Rocket"))
        {
            _health -= _ammountDamageTakenByRocket;
            collision.gameObject.SetActive(false);
        }
    }
}
