using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave.asset", menuName = "Wave/Normal Enemies")]
public class WaveSystem : ScriptableObject
{
    [Header("Normal Wave")]
    public bool enemiesCanShoot;
    public bool canEnemiesZigzag;
    public int sheildedEnemies;
    public bool enemiesCanShootFromBehind;
    public bool VerticaleEnemiesDodge;
    public int numberOfEnemiesToSpawn;
    public float cooldownToStartNextWave;

    [Header("Boss Wave")]
    public bool bossWave;
}
