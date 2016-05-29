using UnityEngine;
using System.Collections;

public class TouchColl : MonoBehaviour {
    // === 外部パラメータ ======================================

    // ===  内部パラメータ ======================================


    // === キャッシュ ==========================================
    [System.NonSerialized]    public SphereCollider sphere;	//

    GameObject player;
    PlayerCtrl playerCtrl;
    Animator playerAnim;

    void Awake()
    {
        sphere = GetComponent<SphereCollider>();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Enable(bool _b)
    {
        if (_b){
            sphere.enabled   = true;
            sphere.isTrigger = true;
        }else{
            sphere.enabled   = false;
            sphere.isTrigger = false;
        }

    }

    
}
