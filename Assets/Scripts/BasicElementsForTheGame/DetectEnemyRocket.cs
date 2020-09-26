using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemyRocket : MonoBehaviour
{
    private bool _doneEnemyDetected;
    [SerializeField]
    private Rocket _rocket;

    private void OnEnable()
    {
        _doneEnemyDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if((other.CompareTag("Enemy") || other.CompareTag("Boss")) && _doneEnemyDetected == false)
        {
            _doneEnemyDetected = true;
            if(_rocket != null)
            {
                _rocket.EnemyDetected(other.transform);
            }
        }
    }
}
