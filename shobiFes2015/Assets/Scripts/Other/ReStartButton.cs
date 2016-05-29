using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class ReStartButton : MonoBehaviour {
    //public  GUIStyle GuiStyle;
    private Text text;

    void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {    
	}

    public void OnGUI()
    {
            int sw, sh;
            int w,h;
            Font f;
            w = Screen.width;
            h = Screen.height;
            sw = w / 4;
            sh  = 50;
            if (GUI.Button(new Rect(w - sw, h - sh, sw, sh), "continue"))
            {
                BgmManager.Instance.StopImmediately();
               // soundHeart.Stop();
                Time.timeScale = 1.0f;
                Application.LoadLevel("Title");
            }
             
        
    }

}
