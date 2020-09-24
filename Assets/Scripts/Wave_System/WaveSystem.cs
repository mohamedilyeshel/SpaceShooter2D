using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave.asset", menuName = "Wave/Normal Enemies")]
public class WaveSystem : ScriptableObject
{
    public bool enemiesCanShoot;
    public bool canEnemiesZigzag;
    public int numberOfEnemiesToSpawn;
    public float cooldownToStartNextWave;
}
