using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyMouvementTypes
    {
        Vertical,
        LeftHorizontal,
        RightHorizontal,
        Diagonale
    }

    private int _speed;
    private Animator _enemyAnimator;
    [SerializeField]
    private AnimationClip _deathAnimation;

    [Header("Enemy Abilities")]
    [SerializeField]
    private SpriteRenderer _enemySheild;
    public bool canFireFromBehind = false;

    [Header("Enemy Mouvement System")]
    public EnemyMouvementTypes _currentMouvementType;
    [SerializeField]
    private bool _stopMoving = false;
    public bool _activeZigZag;
    [Range(0.1f,1f)]
    [SerializeField]
    private float _zigZagAmplitude = 0.1f;
    private float _switch = 0.0f;
    private float _maxX = 1f;
    private float _minX = -1f;

    [Header("Enemy Bullets")]
    public bool canShoot;
    private bool canKillOnCollide = true;
    public GameObject bulletPrefab;
    public List<GameObject> enemyBullets = new List<GameObject>();
    public GameObject behindBulletPrefab;
    public List<GameObject> enemyBehinBullets = new List<GameObject>();
    public int enemyBulletAmount;
    private float _canShootFromBehind = 0;

    private void OnEnable()
    {
        if(canShoot == true)
            StartCoroutine(EnemyShoot(enemyBullets));

        SpawnManager.Instance.EnemyMouvementType(this);
    }

    private void OnDisable()
    {
        if (canKillOnCollide == false)
        {
            _enemyAnimator.SetBool("EnemyDead", false);
            canKillOnCollide = true;
            _stopMoving = false;
        }
    }

    private void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _speed = Random.Range(3,7);
    }

    // Update is called once per frame
    void Update()
    {
        enemyTransform();

        if(canFireFromBehind == true)
            SmartEnemy();
    }

    public void ActiveSheild(bool canActive)
    {
        _enemySheild.enabled = canActive;
    }

    public void ChooseRandomMouvementType()
    {
        var i = Random.Range(0, 4);
        var y = 0;
        foreach (EnemyMouvementTypes type in System.Enum.GetValues(typeof(EnemyMouvementTypes)))
        {
            if(i == y)
            {
                _currentMouvementType = type;
                break;
            }
            y++;
        }
    }

    IEnumerator EnemyShoot(List<GameObject> bulletList)
    {
         while(true)
         {
            if(canKillOnCollide == true && enemyBulletAmount != 0)
            {
                PoolingManager.Instance.shoot(bulletList, transform.position);
            }
            yield return new WaitForSeconds(6.0f);
         }
    }

    public void EnemyShootBehind(List<GameObject> bulletList)
    {
         if (canKillOnCollide == true && enemyBulletAmount != 0)
         {
             PoolingManager.Instance.shoot(bulletList, transform.position);
         }
    }

    private void ZigZagMouvement()
    {
        transform.Translate(new Vector3(Mathf.Lerp(_minX, _maxX, _switch), 0, 0) * _speed * Time.deltaTime);
        _switch += 0.1f * _zigZagAmplitude;
        if (_switch >= 1)
        {
            _switch = 0.0f;
            float temp = _maxX;
            _maxX = _minX;
            _minX = temp;
        }
    }

    public void SmartEnemy()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 3f, 1 << 9);
        if(hitInfo.collider != null)
        {
            if(hitInfo.transform.tag == "Player")
            {
                if(Time.time > _canShootFromBehind)
                {
                    EnemyShootBehind(enemyBehinBullets);
                    _canShootFromBehind = Time.time + 2f;
                }
            }
        }
    }

    private void enemyTransform()
    {
        if(_stopMoving == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if(_activeZigZag == true)
            {
                ZigZagMouvement();
            }
        }
        
        switch(_currentMouvementType)
        {
            case EnemyMouvementTypes.Vertical:
                if (transform.position.y < -6.4)
                {
                    transform.position = new Vector3(Random.Range(-9, 9), 6.4f, 0);
                    SpawnManager.Instance.EnemyMouvementType(this);
                }
                break;
            case EnemyMouvementTypes.RightHorizontal:
                if (transform.position.x < -11.5)
                {
                    transform.position = new Vector3(11.91f, Random.Range(-0.75f, -5.28f), 0);
                    SpawnManager.Instance.EnemyMouvementType(this);
                }
                break;
            case EnemyMouvementTypes.LeftHorizontal:
                if (transform.position.x > 11.5)
                {
                    transform.position = new Vector3(-11.91f, Random.Range(-0.75f, -5.28f), 0);
                    SpawnManager.Instance.EnemyMouvementType(this);
                }
                break;
            case EnemyMouvementTypes.Diagonale:
                if (transform.position.y < -7.4)
                {
                    transform.position = new Vector3(Random.Range(10.87f, 16.43f), Random.Range(11.44f, 5.88f), 0);
                    SpawnManager.Instance.EnemyMouvementType(this);
                }
                break;
        }

        if(GameManager.Instance.CalculateDistanceBetweenPlayerAndB(this.transform) < 2f)
        {
            var direction = GameManager.Instance.CalculateDirectionBetweenPlayerAndB(this.transform);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            if (canKillOnCollide == true)
            {
                 Player p = other.GetComponent<Player>();
                 p.decreaseHealth();
                 StartCoroutine(EnemyDeadAnimation());
            }
            else
            {
                ActiveSheild(false);
            }
        }
        else if(other.CompareTag("Laser"))
        {
            if (_enemySheild.enabled == false)
            {
                StartCoroutine(EnemyDeadAnimation());
                other.gameObject.SetActive(false);
                GameManager.Instance.increaseScore(Random.Range(5, 11));
            }
            else
            {
                ActiveSheild(false);
            }
        }
    }

    IEnumerator EnemyDeadAnimation()
    {
        _enemyAnimator.SetBool("EnemyDead",true);
        canKillOnCollide = false;
        _stopMoving = true;
        SpawnManager.Instance.enemiesStillAlive--;
        ActiveSheild(false);
        ActiveOrDeactiveCollider();
        AudioManager.Instance.ExplosionPlay();
        yield return new WaitForSeconds(_deathAnimation.length);
        ActiveOrDeactiveCollider();
        this.gameObject.SetActive(false);
    }

    private void ActiveOrDeactiveCollider()
    {
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        if(coll != null)
        {
            if (coll.enabled == true)
                coll.enabled = false;
            else
                coll.enabled = true;
        }
    }

}
