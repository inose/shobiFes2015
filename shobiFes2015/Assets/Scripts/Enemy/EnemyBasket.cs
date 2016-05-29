using UnityEngine;
using System.Collections;

public class EnemyBasket : MonoBehaviour {

	
    // === 外部パラメータ（インスペクタ表示） =====================
    public Vector3      spawnRange         = new Vector3(0,0,0);   //生成範囲.
    public float        initSpawnTime      = 2.0f;
    public float        activeRange        = 50.0f;     //プレイヤーとの距離に応じて呼び出すまたはfarmへ戻す.
    public GameObject[]   enemyPrfb;
    public GameObject     farm;                         //farmの場所.      

    // === 内部パラメータ ======================================
    ArrayList inFarm = new ArrayList();
    private Vector3    grRange;
    private Vector3 grPos;
    private float spawnTime;
    private int maxPrfb;
    
    // === コード（Monobehaviour基本機能の実装） ================
    public void Awake()
    {
        grRange = spawnRange;
        spawnTime = initSpawnTime;
        activeRange *= activeRange;

        int i = 0;
        foreach (GameObject prfb in enemyPrfb)
        {
            if (prfb != null)
            {               
                inFarm.Add(true);
                i++;
            }
        }
        maxPrfb = i;
        InvokeRepeating("returnFarm", 1.0f, 2.0f);
    }

    public void FixedUpdate()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime < 0.0f)
        {
            grPos.x = Random.Range(-grRange.x, grRange.x);
            grPos.y = Random.Range(-grRange.y, grRange.y);
            grPos.z = Random.Range(-grRange.z, grRange.z);
            if (EnemyManager.IfEnemyGenerated()) { //最大出現数を超えてない.
				if(ActionLook(gameObject,EnemyManager.player, -activeRange)){ //プレイヤーが籠の近くにいる.
                    
                    for (int i = 0; i < maxPrfb; i++)
                    {
                        bool b = (bool)inFarm[i];
                        if (b)
                        {
                            enemyPrfb[i].active = true;
                            enemyPrfb[i].transform.position = gameObject.transform.position + grPos;
                            inFarm[i] = false;
                            EnemyManager.AddEnemy();
                            break;
                        }
                        else i++;
                    }
				}
            }
            spawnTime = initSpawnTime;
        }        
    }

    public void returnFarm()
    {

        for (int i = 0; i < maxPrfb; i++)
        {
            bool b = (bool)inFarm[i];
            if (!b) 
            {
                if (ActionLook(enemyPrfb[i], EnemyManager.player, activeRange))
                {
                    enemyPrfb[i].transform.position = farm.transform.position;
                    enemyPrfb[i].active = false;
                    inFarm[i] = true;
                    EnemyManager.SubEnemy();
                }
            }
        }
                    
    }

    //２乗で計算.
	public bool ActionLook(GameObject my, GameObject go, float near)
	{
		if (near >= 0)
		{
            if ((my.transform.position - go.transform.position).sqrMagnitude > near)
			{
				return true;
			}
		}
		else
		{
			near *= -1;
            if ((my.transform.position - go.transform.position).sqrMagnitude < near)
			{
				
				return true;
			}
		}
		return false;	
    }


}
