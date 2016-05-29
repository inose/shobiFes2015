using UnityEngine;
using System.Collections;

public class StartCamera : MonoBehaviour {

    public float LightIntensity = -5.6f;
    public GameObject target;
    private Vector3 targetPos;
    private Vector3 pos;
    private Vector3 distance; 
    private float   sTime;

    void Awake()
    {
        // Debug.Log(RenderSettings.ambientIntensity);
        RenderSettings.ambientIntensity = LightIntensity;
        sTime = -2.0f;
        pos = transform.position;
        targetPos = target.transform.position;


        distance.x = Mathf.Abs(pos.x) - Mathf.Abs(targetPos.x);
        distance.y = Mathf.Abs(pos.y) - Mathf.Abs(targetPos.y);
        distance.z = Mathf.Abs(pos.z) - Mathf.Abs(targetPos.z);
        distance = new Vector3(Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z));
        if (targetPos.x < pos.x) distance.x *= -1.0f;
        if (targetPos.y < pos.y) distance.y *= -1.0f;
        if (targetPos.z < pos.z) distance.z *= -1.0f;

        float  d = (1f / Time.deltaTime);
        if( d > 0.03)        distance /= (1f / 0.02f);
        else                 distance /= (1f / d);
    }

	// Use this for initialization
	void Start () {

	}

   
	
	// Update is called once per frame
	void Update () {

        sTime += Time.deltaTime;
        
        if (sTime < 0.0f)
        {
            return;
        }

        if (Mathf.Abs(transform.position.y) - Mathf.Abs(targetPos.y) < 5) {
            Application.LoadLevel("Main");
        }
        else {
            transform.position += distance;
        }
       
        
	}
}
