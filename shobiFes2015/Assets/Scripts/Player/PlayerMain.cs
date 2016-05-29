using UnityEngine;
using System.Collections;


public class PlayerMain : MonoBehaviour {
    
    // === 外部パラメータ =====================================    
    [System.NonSerialized] public bool deathFlag = false;
    private bool groundIf = false;   //接地確認.

    // === キャッシュ ==========================================
    PlayerCtrl playerCtrl;

    // === コード（Monobehaviour基本機能の実装） ================
    void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
       
    }
    void Start(){
        InvokeRepeating("GroundIf", 0.5f, 0.3f);
    }

    void Update()
    {

        if (deathFlag)
        {            
            return;
        }

        playerCtrl.lightEnable = false;

        if (playerCtrl.GetVelX() < 0.5f && playerCtrl.GetVelY() < 0.5f && playerCtrl.GetVelZ() < 0.5f &&
            playerCtrl.GetVelX() > -0.5f && playerCtrl.GetVelY() > -0.5f && playerCtrl.GetVelZ() > -0.5f)
        {
            if (Input.GetAxis("Horizontal") == 0.00000f && Input.GetAxis("Vertical") == 0.00000f && playerCtrl.lantenMeter >= 1) {
                if(groundIf)    playerCtrl.lightEnable = true;
            }			
		} 
        //if (Input.GetButtonDown("Jump"))             Application.CaptureScreenshot(i+"image.png");
        if (Input.GetButtonDown("Jump"))    playerCtrl.animator.SetTrigger("Jump");   	
    }

    public void GameOver()
    {        
        deathFlag = true;
    }


    public void GroundIf()
    {
       // Debug.DrawRay(transform.position, new Vector3(0, -1, 0), new Color(255, 0, 0), 0.5f, true);
        groundIf = Physics.Raycast(transform.position, new Vector3(0, -1, 0), 0.6f);
                      
    }

}
