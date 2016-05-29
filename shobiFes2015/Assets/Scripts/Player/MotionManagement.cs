using UnityEngine;
using System.Collections;

public class MotionManagement : StateMachineBehaviour {

    // === パラメータ（インスペクタ表示） =====================
    public bool motionBreakEnable = false;   //現在のアニメーションを途中で中断できるか否か.

    // === キャッシュ ==========================================
    PlayerCtrl playerCtrl;
    
    // アニメーションのハッシュ名
    public readonly static int Hash_LightS      = Animator.StringToHash("Base Layer.Light_Start");
    public readonly static int Hash_LightL      = Animator.StringToHash("Base Layer.Light_Loop");

    // === コード（Monobehaviour基本機能の実装） ================
    void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }


    //Start
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("MotionBreak", motionBreakEnable);
        animator.ResetTrigger("Idle");
        
  
    }

    //Update
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //モーション中断処理.    enum型でどの中断方法を有効にするかインスペクターで設定させる.
        //自機に動きがあり、モーション中断がOnになっているならIdleへ. 
        if (playerCtrl.GetMoving() && motionBreakEnable)
        {
            animator.ResetTrigger("Light_Start");
            animator.Play("Idle");
        }

        //HandLightON
        //自機に動きが無く、ライトフラグがONになっているか.
        if (!playerCtrl.GetMoving() && playerCtrl.lightEnable)
        {
            animator.SetTrigger("Light_Start");
            
        }

        //HandLightOff
        //ライトモーションの時、スイッチが離されたか.
        if (!playerCtrl.lightEnable)
        {
            if (stateInfo.nameHash == Hash_LightS || stateInfo.nameHash == Hash_LightL)
            {
                 //animator.ResetTrigger(stateInfo.shortNameHash);
                 //animator.Play("Idle");
                animator.ResetTrigger("Light_Start");
                 animator.SetTrigger("Idle");
            }
        }

 
    }
}
