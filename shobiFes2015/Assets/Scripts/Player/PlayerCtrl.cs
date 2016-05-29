using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class PlayerCtrl : BaseCharCtrl{
    // === パラメータ（インスペクタ表示） =====================
    public bool DebugLight = false;

    // === 外部パラメータ =====================================
    [System.NonSerialized] public  bool    lightEnable = false; //HandLight
    public AudioClip SE_Heart;
    public AudioClip SE_HeartFast;
    public AudioClip SE_LightOn;
    public AudioClip SE_Walk;
    public AudioClip SE_Run;
    public AudioClip SE_GameClear;
    public AudioClip SE_GameOver;
    public AudioClip SE_KeyItem;

    // アニメーションのハッシュ名
    public readonly static int ANISTS_LightS = Animator.StringToHash("Base Layer.Light_Start");
    public readonly static int ANISTS_LightL = Animator.StringToHash("Base Layer.Light_Loop");


    // === 内部パラメータ =====================================
    [System.NonSerialized] public    bool        deathFlag    = false;
    [System.NonSerialized] public    GameObject  killedObject;
    [System.NonSerialized] public    Vector3     killedPos;

    public    Text        gameOverText; //Text用変数.
    public    Text        lantenText; //Text用変数.

    public    bool        clearItem = true;
    public    float         lantenMeter = 100;    //ランタンの残量.
    public    float       lantenSubSecond = 0.4f;      //何秒でゲージを減らすか.
    private   float       lantenSub = 0;

    private float     maxRotSpeed  = 200.0f;
    private float     minTime = 0.1f;
    private float     _velocity;

    private AudioSource sound = null;
    private AudioSource soundHeart = null;
    

    // === コード（Monobehaviour基本機能の実装） ==============
    protected override void Awake() {
        base.Awake();

        EnemyManager.ReSet();
        BgmManager.Instance.DebugMode = false;
        RenderSettings.ambientIntensity = -7.3f;
        if (DebugLight) { 
            RenderSettings.ambientIntensity = 1f; 
        }
        lantenSub = lantenSubSecond * 60;   //フレーム＊秒数.

        AudioSource[] s = gameObject.GetComponents<AudioSource>();
        foreach (AudioSource audio in s)
        {
            if (sound != null)
            {
                soundHeart = audio;
                soundHeart.clip = SE_Heart;
                soundHeart.loop = true;
                soundHeart.Play();
            }
            else
                sound = audio;
        }
    }


	protected override void Start () {
        base.Start();

        InvokeRepeating("LockOn", 0.0f, 2.0f);
	}

	
	protected override void FixedUpdate (){

        //現在のステート取得.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //ゲージ表示
        int m = (int)lantenMeter;
        string str = m.ToString();
        lantenText.text = str + "%";


        if (deathFlag)
        {
            ActionLookup(killedObject);
            transform.position = killedPos;
        }
        else  if (stateInfo.nameHash == ANISTS_LightS || stateInfo.nameHash == ANISTS_LightL)
            {
                if (lantenMeter > 0)
                {
                    lantenMeter -= lantenSubSecond;
                }
                //死んでないときランタンがついてたら残量を減らす
              
            }
        
	}

    //======サウンドSE====================================
    public void Sound_Idle()
    {
        sound.pitch = 1.0f;
        soundHeart.Play();
    }
   
    public void Sound_LightOn()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_LightOn);
    }
    public void Sound_Run()
    {
        sound.pitch = 1.0f;
        if (charCtrl.isGrounded)
        {
           // sound.PlayOneShot(SE_Run);
        }
    }
    public void Sound_Walk()
    {
        sound.pitch = 1.0f;
        if (charCtrl.isGrounded)
        {
           // sound.PlayOneShot(SE_Walk);
        }
    }
    public void Sound_GameOver()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_GameOver);
    }
    public void Sound_GameClear()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_GameClear);
    }
    public void Sound_KeyItem()
    {
        sound.pitch = 1.0f;
        sound.PlayOneShot(SE_KeyItem);
    }
    



    // === コード(その他) =====================================

    //自分が動いているか.
    public bool GetMoving() {
		if (GetVelX () < 1.0f && GetVelY () < 1.0f && GetVelZ () < 1.0f && 
		     GetVelX () > -1.0f && GetVelY () > -1.0f && GetVelZ () > -1.0f) {
			return false;
		}
		return true;
	}

    public void LockOn()
    {
        if (EnemyManager.LockOn())
        {
            BgmManager.Instance.Play("NightWind");
                if(soundHeart.clip == SE_Heart){
                    soundHeart.clip = SE_HeartFast;
                    soundHeart.Play();
                }
        }
        else
        {
            BgmManager.Instance.Play("Field1");
                if(soundHeart.clip == SE_HeartFast){
                    soundHeart.clip = SE_Heart;
                    soundHeart.Play();
                }
        }
    }


    public void TouchEnemy(GameObject go)
    {
        killedObject = go;
        killedPos = transform.position;
        deathFlag = true;
        BgmManager.Instance.Stop();
        soundHeart.Stop();
    }

    public void GameOver()
    {
        gameOverText.text = "GameOver";
        Sound_GameOver();
        Time.timeScale = 0;        
    }



    public void GameClear()
    {
        BgmManager.Instance.StopImmediately();
        Application.LoadLevel("Ending");

    }

}
