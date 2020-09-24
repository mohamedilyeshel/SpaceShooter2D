using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave.asset", menuName = "Wave/Normal Enemies")]
public class WaveSystem : ScriptableObject
{
    public int numberOfEnemiesToSpawn;
    public float cooldownToStartNextWave;
}
