using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerU
{
    public GameObject powerUpPrefab;
    public float powerUpCooldown;
    public enum itemType
    {
        Commun, // 0
        Rare, // 1
        Epic // 2
    }
    public itemType _typeOfItem;
}
