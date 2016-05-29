using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {
    GameObject player;
    PlayerCtrl playerCtrl;
    protected float maxRotSpeed = 200.0f;
    protected float minTime     = 0.1f;
    protected float _velocity;
    protected bool deathFlag    = false;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (deathFlag)
        {
            Lookup(playerCtrl.killedObject);
        }


	}

    public void GameOver()
    {
        deathFlag = true;

    }



    public void Lookup(GameObject go)
    {

        //transform.LookAt(go.transform.position);
        Vector3 newRotation = Quaternion.LookRotation(go.transform.position - transform.position).eulerAngles;    //goの方向を取得.
        Vector3 angles = transform.rotation.eulerAngles;
        //transform.rotation = Quaternion.Euler(angles.x, Mathf.SmoothDampAngle(angles.y, newRotation.y, ref _velocity, minTime, maxRotSpeed), angles.z);
        transform.rotation = Quaternion.Euler(Mathf.SmoothDampAngle(angles.x, newRotation.x, ref _velocity, minTime, maxRotSpeed), angles.y, angles.z);
    }
}
