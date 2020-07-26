using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;
    private Animator _explodeAnimator;
    [SerializeField]
    private AnimationClip _exploisionAnimation;

    private void Start()
    {
        _explodeAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            CircleCollider2D coll = GetComponent<CircleCollider2D>();
            coll.enabled = false;
            _explodeAnimator.SetTrigger("OnExplode");
            StartCoroutine(finishExplode());
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator finishExplode()
    {
        AudioManager.Instance.ExplosionPlay();
        yield return new WaitForSeconds(_exploisionAnimation.length);
        Destroy(this.gameObject,0.5f);
        SpawnManager.Instance.StartSpawning();
    }
}
