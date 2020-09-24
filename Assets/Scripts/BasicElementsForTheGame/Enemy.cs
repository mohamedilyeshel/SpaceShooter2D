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
    public int enemyBulletAmount;

    private void OnEnable()
    {
        if(canShoot == true)
            StartCoroutine(EnemyShoot());
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

    IEnumerator EnemyShoot()
    {
         while(true)
        {
            if(canKillOnCollide == true && enemyBulletAmount != 0)
            {
                PoolingManager.Instance.shoot(enemyBullets, transform.position);
            }
            yield return new WaitForSeconds(6.0f);
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
                }
                break;
            case EnemyMouvementTypes.RightHorizontal:
                if (transform.position.x < -11.5)
                {
                    transform.position = new Vector3(11.91f, Random.Range(-0.75f, -5.28f), 0);
                }
                break;
            case EnemyMouvementTypes.LeftHorizontal:
                if (transform.position.x > 11.5)
                {
                    transform.position = new Vector3(-11.91f, Random.Range(-0.75f, -5.28f), 0);
                }
                break;
            case EnemyMouvementTypes.Diagonale:
                if (transform.position.y < -7.4)
                {
                    transform.position = new Vector3(Random.Range(10.87f, 16.43f), Random.Range(11.44f, 5.88f), 0);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            if(canKillOnCollide == true)
            {
                Player p = other.GetComponent<Player>();
                p.decreaseHealth();
                StartCoroutine(EnemyDeadAnimation());
            }
        }
        else if(other.CompareTag("Laser"))
        {
            StartCoroutine(EnemyDeadAnimation());
            other.gameObject.SetActive(false);
            GameManager.Instance.increaseScore(Random.Range(5,11));
        }
    }

    IEnumerator EnemyDeadAnimation()
    {
        _enemyAnimator.SetBool("EnemyDead",true);
        canKillOnCollide = false;
        _stopMoving = true;
        SpawnManager.Instance.enemiesStillAlive--;
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
