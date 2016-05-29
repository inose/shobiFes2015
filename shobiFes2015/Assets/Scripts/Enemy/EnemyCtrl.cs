using UnityEngine;
using System.Collections;
//test

public class EnemyCtrl : BaseCharCtrl{

	// === 外部パラメータ（インスペクタ表示） =====================
	[System.NonSerialized] public float initHpMax	= 5.0f;
	public float 	 initSpeed 	    = 2.0f;
    public AudioClip SE_FormChange;
    public AudioClip SE_FormEnd;
    public AudioClip SE_Cry;
    public AudioClip SE_CallEnemy;
    public AudioClip SE_Attack;
    public AudioClip SE_Eat;

    // ===  内部パラメータ ======================================
    private AudioSource sound;
    [System.NonSerialized] public float callID = 0.0f;

	// === キャッシュ ==========================================
    TouchColl       touchColl;

	// === コード（Monobehaviour基本機能の実装） ================
	protected override void Awake () {
		base.Awake ();

        touchColl = GetComponentInChildren<TouchColl>();
        sound     = gameObject.GetComponent<AudioSource>();
    }

	protected override void Start () {
		base.Start ();

        speed 	= initSpeed;
	}

	protected override void Update (){
		base.Update ();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

	protected override void FixedUpdateCharacter () {
        base.FixedUpdateCharacter();
        animator.SetFloat("Speed", ( Mathf.Abs(GetVelX()) + Mathf.Abs(GetVelZ()) ) );
	}

    public override void ActionMove(){
        base.ActionMove();      
    }

	public void ActionAttack(string atkname) {
        sound.Stop();
		animator.SetTrigger (atkname);
	}

    public void ActionCry()
    {
        animator.SetTrigger("Cry");
    }

    public void FormChange()
    {
        animator.SetTrigger("FormChange");
    }

    public void FormReturn()
    {
        animator.SetTrigger("FormReturn");
    }

    public void CallEnemy()
    {
        animator.SetTrigger("CallEnemy");
        callID = Time.time;
        EnemyManager.CallEnemy(transform.position, callID);
    }




    //======サウンドSE====================================animatorのアニメーションのとこにイベントとしてつっこむ.
    public void Sound_Idle()
    {
        sound.pitch = 1.0f;
    }
    public void Sound_Move()
    {
        sound.pitch = 1.0f;
    }
    public void Sound_FormChange()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_FormChange);
    }
    public void Sound_FormChange2()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_FormEnd);
    }
    public void Sound_Cry()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_Cry);
    }
    public void Sound_CallEnemy()
    {
        sound.pitch = 0.7f;
        sound.PlayOneShot(SE_CallEnemy);
    }
    public void Sound_Attack()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_Attack);
    }
    public void Sound_Eat()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_Eat);
    }


//==========================================

    public void GameOver()
    {
        //Destroy(gameObject);
        //enabled = false;
    }

    public void TouchRangeON()
    {
        touchColl.Enable(true);
    }

    public void TouchRangeOFF()
    {
        touchColl.Enable(false);
    }



}

