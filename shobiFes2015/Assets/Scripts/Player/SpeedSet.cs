using UnityEngine;
using System.Collections;

public class SpeedSet : StateMachineBehaviour {

    // === パラメータ（インスペクタ表示） =====================
    public string speed = "Speed";

    // === キャッシュ ==========================================
    PlayerCtrl playerCtrl;

    // === コード（Monobehaviour基本機能の実装） ================
    void Awake() {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        Vector3 velocity = new Vector3(Mathf.Abs(playerCtrl.GetVelX()), 0.0f, Mathf.Abs(playerCtrl.GetVelZ()));
        float   vel      = velocity.x + velocity.z;

        animator.SetFloat(speed, vel);
        
    }
}
