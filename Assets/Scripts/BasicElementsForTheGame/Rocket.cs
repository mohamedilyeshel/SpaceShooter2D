using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Laser
{
    private bool _detected;
    private Transform _enemyDetected;

    private void OnEnable()
    {
        StartCoroutine(hide());
    }

    private void OnDisable()
    {
        _detected = false;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {        
        laserMvment();

        if(_detected == true)
            GoToEnemy();
    }

    public void EnemyDetected(Transform enemy)
    {
        _detected = true;
        _enemyDetected = enemy;
    }

    private void GoToEnemy()
    {
        Vector3 direction = _enemyDetected.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 4f * Time.deltaTime);
    }
}
