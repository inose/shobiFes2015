using UnityEngine;
using System.Collections;

public enum ENEMYAISTS // --- 敵のAIステート ---
{
    ACTIONSELECT,		// アクション選択（思考）
    WAIT,				// 一定時間（止まって）待つ
    RUNTOPLAYER,		// 走ってプレイヤーに近づく
    FORMCHANGE,         //形態を変える.
    FORMRETURN,         //形態を変える.
    CRYTOPLAYER,        //威嚇.    
    CALLENEMY,          //仲間を呼ぶ.
    PATROL,				// 巡回 
}


public class EnemyMain : MonoBehaviour
{

    // === 外部パラメータ（インスペクタ表示） =====================
    public int debug_SelectAIState = -1;

    // === 外部パラメータ ======================================
    [System.NonSerialized]
    public ENEMYAISTS aiState = ENEMYAISTS.ACTIONSELECT;

    // === キャッシュ ==========================================
    protected EnemyCtrl enemyCtrl;
    protected GameObject player;
    protected PlayerCtrl playerCtrl;

    // === 内部パラメータ ======================================
    protected float aiActionTimeLength = 0.0f;
    protected float aiActionTImeStart  = 0.0f;
    protected float distanceToPlayer   = 0.0f;
    protected float distanceToPlayerPrev = 0.0f;

    // === コード（Monobehaviour基本機能の実装） ================
    public virtual void Awake()
    {
        enemyCtrl = GetComponent<EnemyCtrl>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void FixedUpdate()
    {
        if (CheckAction())
        {
            FixedUpdateAI();
            EndEnemyCommonWork();
        }
    }

    public virtual void FixedUpdateAI() { }


    // === コード（基本AI動作処理） =============================

    public void EndEnemyCommonWork()
    {
        // アクションのリミット時間をチェック
        float time = Time.fixedTime - aiActionTImeStart;
        if (time > aiActionTimeLength)
        {
            aiState = ENEMYAISTS.ACTIONSELECT;
        }
    }

    //特定のアクションをしていたら他の処理を行わない.
    public bool CheckAction()
    {
        // 状態チェック
        /*
                AnimatorStateInfo stateInfo = enemyCtrl.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.tagHash == EnemyController.ANITAG_ATTACK ||
                    stateInfo.nameHash == EnemyController.ANISTS_DMG_A ||
                    stateInfo.nameHash == EnemyController.ANISTS_DMG_B ||
                    stateInfo.nameHash == EnemyController.ANISTS_Dead ||
                    stateInfo.nameHash == EnemyController.ANISTS_ApDeath ||
                    stateInfo.nameHash == EnemyController.ANISTS_Ability_A)
                {

                    return false;
                }
         */
        return true;
    }

    //ステートを選択するためのランダム値を返す
    public int SelectRandomAIState()
    {
#if UNITY_EDITOR
        if (debug_SelectAIState >= 0)
        {
            return debug_SelectAIState;
        }
#endif
        return Random.Range(0, 100 + 1);
    }

    public void SetAIState(ENEMYAISTS sts, float t)
    {
        aiState = sts;
        aiActionTImeStart = Time.fixedTime;
        aiActionTimeLength = t;
    }

    // === コード（AIスクリプトサポート関数） ====================
    //Playerとの距離を返す
    public float GetDistancePlayer()
    {
        distanceToPlayerPrev = distanceToPlayer;
        distanceToPlayer = Vector3.Distance(transform.position, playerCtrl.transform.position);
        return distanceToPlayer;
    }

}
