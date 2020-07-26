using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _speed;
    private Animator _enemyAnimator;
    [SerializeField]
    private AnimationClip _deathAnimation;
    private bool _stopMoving = false;
    private bool _cankill = true;

    [Header("Enemy Bullets")]
    public GameObject bulletPrefab;
    public List<GameObject> enemyBullets = new List<GameObject>();
    public int enemyBulletAmount;

    private void OnEnable()
    {
        StartCoroutine(EnemyShoot());
    }

    private void OnDisable()
    {
        if (_cankill == false)
        {
            _enemyAnimator.SetBool("EnemyDead", false);
            _cankill = true;
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

    IEnumerator EnemyShoot()
    {
         while(true)
        {
            if(_cankill == true)
            {
                PoolingManager.Instance.shoot(enemyBullets, transform.position);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }

    private void enemyTransform()
    {
        if(_stopMoving == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (transform.position.y < -6.4)
        {
            transform.position = new Vector3(Random.Range(-9, 9), 6.4f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            if(_cankill == true)
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
        _cankill = false;
        _stopMoving = true;
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
