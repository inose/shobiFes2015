using UnityEngine;
using System.Collections;

public static class EnemyManager{
  
    // === 外部パラメータ ======================================
    public static  int        initTotalEnemy = 0; 
    public static  int        initMaxEnemy   = 120;
    public static  int        totalEnemy = 0;
    public static  int        maxEnemy   = 0;
    [System.NonSerialized] public static  bool       call       = false;
    [System.NonSerialized] public static  bool       target     = false;
    [System.NonSerialized] public static  Vector3    callPoint  = new Vector3(0,0,0);
    [System.NonSerialized] public static  float      ID         = 0.0f;

    [System.NonSerialized] public static GameObject player = null;

    public static void ReSet()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        totalEnemy = initTotalEnemy;
        maxEnemy   = initMaxEnemy;
        call       =  false;
        target     =  false;
        callPoint  = new Vector3(0,0,0);
        ID         = 0.0f;
   }
    
    public static void AddEnemy()
    {
        totalEnemy++;
    }

    public static void SubEnemy()
    {
        totalEnemy--;
        if (totalEnemy < 0) totalEnemy = 0;
    }

    public static bool IfEnemyGenerated()
    {
        if (totalEnemy < maxEnemy){
            return true;
        }
        return false;
    }

    public static void CallEnemy(Vector3 _callPoint, float _ID)
    {
        call = true;
        callPoint = _callPoint;
        ID = _ID;
    }

    public static void StopCallEnemy(float _ID)
    {
        //同じ敵かどうか.
        if (ID == _ID)
        {
            call = false;
            callPoint = Vector3.zero;
        }
    }

    public static Vector3 GetCallPoint()
    {
        if (call){           
            return callPoint;
        }        
        
        return Vector3.zero;
    }

    public static void SetTarget()
    {
        //プレイヤーを発見している.
        target = true;
    }

    public static bool LockOn()
    {
        if (target){
            target = false;
            return true;
        }
        return false;
    }



}