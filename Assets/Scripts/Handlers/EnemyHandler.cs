using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("GameObjects")]
    public Path Path;
    public Transform Platform;
    public Transform House;


    [Space(10)]
    [Header("Data")]
    public float CoolDownTime;
    public int CurrentPoint;
}
