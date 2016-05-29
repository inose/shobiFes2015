using UnityEngine;
using System.Collections;

public class movie : MonoBehaviour
{
    public MovieTexture video;
    private AudioSource ses;
    private bool        flag;
    public  float switchTime;   //動画が終わってから何秒で切り替えるか.
    private float clipTime;
    public enum Type { Title = 0, StartCamera = 1}
    public Type stage = Type.Title;


    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = video as MovieTexture;
        ses = GetComponent<AudioSource>();
        video.Play();
        clipTime = ses.clip.length;
        flag = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            return;
        }
        if (ses.time  > clipTime + switchTime){
            flag = true;
            if (stage == Type.Title)
                Application.LoadLevel("Title");
            if (stage == Type.StartCamera)
                Application.LoadLevel("StartCamera");
        }



    }
}
