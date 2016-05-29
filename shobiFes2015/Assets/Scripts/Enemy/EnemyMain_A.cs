using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//白→未成熟、無害.
//赤→成熟、攻撃＋威嚇.
//オレンジ→特殊、仲間を呼ぶ.
//虎色→亜種、明るいところに近寄れる.
public class EnemyMain_A : EnemyMain
{

	// === 外部パラメータ（インスペクタ表示） =====================
    [System.NonSerialized]public int aiifPATROL;
	[System.NonSerialized]public int aiIfRUNTOPLAYER;
    [System.NonSerialized]public int aiIfCRYTOPLAYER;
    [System.NonSerialized]public int aiIfCALLENEMY;
	
    public float LostRange              = 50.0f;   //修正→破壊はせずにロックオンだけはずす
    public float eyesightRange          = 9.0f;    //視界限界距離.    //プレイヤーを捉えられる範囲.
    public float lightStopRange         = 3.0f;    //ライトで止まる距離.   
    public float findRange              = 4.0f;    //絶対に感知する距離.
    public float callRange              = 30.0f;    //仲間の遠吠えに気づく距離.
    public float lightSeeRange          = 10.0f;    //タイガープヨンがライトに気づく距離、半径.
    public float eyesAngle              = 60.0f;      //視野角度.
    public float formChangeOfFindTime   = 7.0f;        //ターゲットを見つけてから形態変化するまでの時間.

    public TextMesh  headUpText;    //!マーク.

    public enum EnemyType { White = 0, Red = 1, Blue = 2, Tiger = 3 }
    public EnemyType puyonType = EnemyType.White;

    [System.NonSerialized] public int   layerMask              = 1 << 15;   //レイヤー"Player"の番号=15.

    // === 内部パラメータ========================================
    private int     tempRUNTOPLAYER = 0;
    private float   followTime      = 0.0f;      //ターゲットを追っている時間
    private bool    formChange      = false;    //形態変化（前/後）.
    private bool       headUpMark   = false;
    private bool       lantenFlag   = false;
    private bool       stopCall     = false;
    private GameObject target       = null;    //標的発見.
    

    // === コード（Monobehaviour基本機能の実装） ================
    public override void Awake()
    { base.Awake(); }

    public override void Start()
    {
        base.Start();
        if (puyonType != EnemyType.White) {
            InvokeRepeating("Call", 1.0f, 1.0f);
        }

       
        InvokeRepeating("GetLightFrag", 0.5f, 0.5f);

        //2乗処理.距離計算が2乗ででるため.
        LostRange       *= LostRange;
        eyesightRange   *= eyesightRange;
        lightStopRange  *= lightStopRange;
        findRange       *= findRange;
        lightSeeRange   *= lightSeeRange;

        layerMask        = ~layerMask;

        switch (puyonType)
        {
            case EnemyType.White:
                aiifPATROL = 70;
                aiIfRUNTOPLAYER = 0;
                aiIfCRYTOPLAYER = 0;
                aiIfCALLENEMY = 0;
                break;
            case EnemyType.Red:
                aiifPATROL = 70;
                aiIfRUNTOPLAYER = 70;
                aiIfCRYTOPLAYER = 20;
                aiIfCALLENEMY = 0;
                break;
            case EnemyType.Blue:
                aiifPATROL = 70;
                aiIfRUNTOPLAYER = 40;
                aiIfCRYTOPLAYER = 0;
                aiIfCALLENEMY = 40;
                break;
            case EnemyType.Tiger:
                aiifPATROL = 0;
                aiIfRUNTOPLAYER = 100;
                aiIfCRYTOPLAYER = 0;
                aiIfCALLENEMY = 0;
                formChangeOfFindTime = 0.0f; 
                break;

            default:
                break;
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

	// === コード（AI思考処理） =================================
	public override void FixedUpdateAI () {
       
        //プレイヤーに気付いた.
        if (target != null && !headUpMark){
            headUpText.text = "!";
            headUpMark = true;
        }
        else if(target == null){
            headUpMark = false;
        }
       
        //ぷよんごとの設定.
        if (puyonType == EnemyType.Red || puyonType == EnemyType.Blue){
            Sensor();
        }
        else if (puyonType == EnemyType.Tiger) {
            //ランタンOn、ライト感知内のとき
            if (lantenFlag && enemyCtrl.ActionLook(player, -lightSeeRange))
            {
                Sensor();
                enemyCtrl.TouchRangeON();
                if (target == null)
                {
                    target = player;  
                }
            }
            else
            {
                enemyCtrl.TouchRangeOFF();
                if (target != null)
                {
                    target = null;
                    if( aiState == ENEMYAISTS.RUNTOPLAYER){
                        SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                    }
                }
            }
        }

		// AIステート.
		//Debug.Log (string.Format(">>> aists {0}",aiState));
		switch (aiState) {
		case ENEMYAISTS.ACTIONSELECT	: // 思考の起点
			// アクションの選択
			int n = SelectRandomAIState();	       
            if(!formChange){                
                /* 通常形態AI. */
                if(target != null)
                {
                   
                    if (n < aiIfRUNTOPLAYER) {
                    //追いかける
				    SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
                    } else
		    	    if (n < aiIfRUNTOPLAYER + 20) {
                    //形態変化.
                    SetAIState(ENEMYAISTS.FORMCHANGE, 10.0f);
		    	    }else{
                    //一定時間止まる.
				    SetAIState(ENEMYAISTS.WAIT,0.0f + Random.Range(0.0f,1.0f));
                    }
                }
                else
                {
                    if (n < aiifPATROL&& puyonType != EnemyType.Tiger) {
                    //巡回.
				    SetAIState(ENEMYAISTS.PATROL,1.0f + Random.Range(0.0f,2.0f));
                    //SetAIState(ENEMYAISTS.FORMCHANGE, 10.0f);
                    }else{
                    //一定時間止まる.
				    SetAIState(ENEMYAISTS.WAIT,1.0f + Random.Range(0.0f,1.0f));
                    }
                }  
            }
            else
            { 
              if(target != null){
                if (n < aiIfRUNTOPLAYER) {
                //追いかける
				SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
                } else
		    	if (n < aiIfRUNTOPLAYER + aiIfCRYTOPLAYER) {
                //威嚇する.
				SetAIState(ENEMYAISTS.CRYTOPLAYER,1.0f);	
		    	}else
		    	if (n < aiIfRUNTOPLAYER + aiIfCRYTOPLAYER + aiIfCALLENEMY) {
                //callする.
				SetAIState(ENEMYAISTS.CALLENEMY,1.0f);
				}else{
                //一定時間止まる.
				SetAIState(ENEMYAISTS.WAIT,0.0f + Random.Range(0.0f,1.0f));
                }
              }
              else{
                if (puyonType != EnemyType.Tiger) SetAIState(ENEMYAISTS.FORMRETURN, 10.0f);
              }
            }
    		break;

		case ENEMYAISTS.WAIT			: // 休憩
            if (target != null){
                enemyCtrl.ActionLookup(target);
            }
			break;

		case ENEMYAISTS.RUNTOPLAYER		: // 近寄る
			if (enemyCtrl.ActionMoveToNear(player,-4)) {	//距離2まで近づく、そしてそれより近いか？.
                if (puyonType != EnemyType.Blue)
                {
                    Attack_A();
                }
			}
			break;

        case ENEMYAISTS.FORMCHANGE      : // 形態変化.
            enemyCtrl.FormChange();
            formChange = true;
            SetAIState(ENEMYAISTS.WAIT,6.0f);
            break;

        case ENEMYAISTS.FORMRETURN: // 形態変化.
            enemyCtrl.FormReturn();
            formChange = false;
            SetAIState(ENEMYAISTS.WAIT, 10.0f);
            break;

        case ENEMYAISTS.CRYTOPLAYER     : // 威嚇.
            enemyCtrl.ActionCry();
            SetAIState(ENEMYAISTS.WAIT, 4.5f);  //威嚇アニメーション再生時間が2.5倍スピードで4.5秒.
            break;

        case ENEMYAISTS.CALLENEMY       : // 仲間を呼ぶ.
            enemyCtrl.CallEnemy();
            SetAIState(ENEMYAISTS.WAIT, 6.6f);
            break;

		case ENEMYAISTS.PATROL			: // 巡回
            //向きをランダムで変える.
            Vector2 d = new Vector2(Random.Range(-10.0f, 10.0f),Random.Range(-10.0f, 10.0f));
            enemyCtrl.ActionLookup(d);
            enemyCtrl.ActionMove();
			break;
		}
	}

 //------------------------------------------------------------------------------------------
	void Attack_A() {
		enemyCtrl.ActionAttack("Attack");
		SetAIState(ENEMYAISTS.WAIT,4.0f);
	}

 //------------------------------------------------------------------------------------------
    public bool FindTarget()
    {
        if (enemyCtrl.ActionLook(player, -eyesightRange) && Vector3.Angle(playerCtrl.GetPos() - transform.position, transform.forward) <= eyesAngle) //視界範囲内/プレイヤ-が見える.
        {
            if (!Physics.Linecast(transform.position, playerCtrl.GetPos(), layerMask))//障害物なし.
            {
                return true;
            }

        }
        return false;
    } 

//------------------------------------------------------------------------------------------
    public void Call()
    {
        if (puyonType == EnemyType.Blue && stopCall)
        {
            StopCall();
        }
        stopCall = !stopCall;

        if (target != null) {
            return;
        }

        Vector3 point = EnemyManager.GetCallPoint();
        if (point != Vector3.zero){
            if(enemyCtrl.GetPosX() < point.x + callRange &&
                enemyCtrl.GetPosX() > point.x - callRange &&
                 enemyCtrl.GetPosZ() < point.z + callRange &&
                  enemyCtrl.GetPosZ() > point.z - callRange)
            {
                target = player;
                if (!formChange) SetAIState(ENEMYAISTS.FORMCHANGE, 10.0f);
            }
        }

    }

//------------------------------------------------------------------------------------------
    public void StopCall()
    {
        EnemyManager.StopCallEnemy(enemyCtrl.callID);
      
    }


//------------------------------------------------------------------------------------------
    //~秒ごとにプレイヤーがランタンつけてるか見る.
    public void GetLightFrag() 
    {
        if (playerCtrl.lightEnable == true)
            lantenFlag = true;
        else
            lantenFlag = false;
    }

//------------------------------------------------------------------------------------------
    //センサー.
    public void Sensor()
    {
        //ランタンがついていて、ライト設定範囲内か.
        if (enemyCtrl.ActionLook(player, -lightStopRange) && lantenFlag && puyonType != EnemyType.Tiger)
        {
            if(aiIfRUNTOPLAYER != -1){
                tempRUNTOPLAYER = aiIfRUNTOPLAYER;
       
                SetAIState(ENEMYAISTS.WAIT, 2.0f);
                aiIfRUNTOPLAYER = -1;   //プレイヤーに近づく%を0に.
            }
        }
        else if(aiIfRUNTOPLAYER == -1){
            aiIfRUNTOPLAYER = tempRUNTOPLAYER;
            if (aiState == ENEMYAISTS.CRYTOPLAYER)
            {
                SetAIState(ENEMYAISTS.WAIT, 4.5f);
            }
            else if (aiState == ENEMYAISTS.CALLENEMY)
            {
                SetAIState(ENEMYAISTS.WAIT, 6.6f);
            }
            else
            SetAIState(ENEMYAISTS.WAIT, 2.0f);
        }


        //プレイヤーとの距離ごとの設定.
        if (enemyCtrl.ActionLook(player, -findRange))
        {
            //距離が近すぎるからターゲット強制発見.
            target = player;
            if (!formChange) SetAIState(ENEMYAISTS.FORMCHANGE, 10.0f);
        }
        else if (enemyCtrl.ActionLook(player, -eyesightRange)){
            //視野距離内.
            if (FindTarget()) target = player;      //ターゲットロックON. 
        }
        else if (enemyCtrl.ActionLook(player, LostRange)){
            //活動範囲外.
            target = null;
            followTime = 0.0f;
        }


        //ターゲットがいる場合.
        if (target != null){
            EnemyManager.SetTarget();
            followTime += Time.deltaTime;   //追跡時間.
            if (followTime > formChangeOfFindTime && !formChange) SetAIState(ENEMYAISTS.FORMCHANGE, 10.0f);
        }
    }
	
}
