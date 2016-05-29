using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {

    // === 内部パラメータ ======================================
    private bool gameOver;

    void Awake()
    {
        gameOver = false;
    }
	// Use this for initialization
	void Start () {
        if (Application.loadedLevelName == "Title")
        {
            InvokeRepeating("Stop", 1.0f, 0.0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
        { Application.Quit(); }

        if (Input.GetButtonDown("ESC")) //ゲームパッド.11ボタン.
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel("Title");
        }


        if (Application.loadedLevelName == "Main")
        {
            if (gameOver)
            {
                if (Input.GetButtonUp("Jump"))
                {
                    Time.timeScale = 1.0f;
                    Application.LoadLevel("Title");
                }
            }
        }
        else if (Application.loadedLevelName == "Title")
        {
            if (Input.GetButtonUp("Jump")) Application.LoadLevel("Opening");            
        }
        else if (Application.loadedLevelName == "Opening")
        {
            if (Input.GetButtonUp("Jump")) Application.LoadLevel("StartCamera");
        }
        



       

        
	}

    public void GameOver()  //ブロードキャストメッセージで受信.
    {
        gameOver = true;

    }

    public void Stop()
    {
        BgmManager.Instance.StopImmediately();
    }
}
