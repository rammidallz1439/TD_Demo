using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs
{
}


#region Configs

[Serializable]
public class WaveData
{
    public List<Wave> Waves;
}

[Serializable]
public class Wave
{
    public List<EnemyData> EnemyData;
    public int WaveTime;
}


[Serializable]
public class EnemyData
{
    public EnemyScriptable EnemyScriptable;
    public float MaxCount;
    public float TimeToStart;

}

[System.Serializable]
public class GridData
{
    public List<bool> grid;
    public int rows;
    public int columns;
}

#endregion

#region Enums
public enum EnemyType
{
    None = 0,
    Minion = 1,
    Elite = 2,
    Boss = 3
}

public enum BulletType
{
    None = 0,
    Bullet = 1,
    Rocket = 2,
    Laser = 3
}

#endregion