using UnityEngine;
using System.Collections;

public class ArrowCtrl : MonoBehaviour {

	public GameObject[] keyItemList;
	public GameObject[] destroyObjectList;
	public static Vector3      destination = Vector3.zero;
	public GameObject	arrowPrfb;
	
	protected float maxRotSpeed = 200.0f;
	protected float minTime = 0.1f;
	protected float _velocity;
	
	// === キャッシュ ==========================================
	GameObject player;
	PlayerCtrl playerCtrl;
	
	void Awake()
	{
		player     = GameObject.FindGameObjectWithTag("Player");
		playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
	}
	void Start () {
		InvokeRepeating ("CheckItem",0.0f, 1.0f);	//1秒ごとに関連付けられているItemをチェック
	}
	
	void Update()
	{

		//ActionLookup(new Vector2(destination.x,destination.z));
		//transform.LookAt(destination);
		
		
	}
	
	void CheckItem () {
		// 登録されているリストから生存状態を確認
		// （1秒に1回でもよい）
		bool flag = true;
		foreach (GameObject keyItem in keyItemList){
			if (keyItem != null) {
				flag = false;
				destination = keyItem.transform.position;
				//ActionLookup(keyItem);
				//                transform.LookAt(keyItem.transform);
			}
		} 
		
		// 全てのKeyItemが回収されてるか？
		if (flag) {
			//	Debug.Log(string.Format(">>> flag"+flag));
			// 登録されている破壊物リストのオブジェクトを削除
			foreach (GameObject destroyObject in destroyObjectList) {
				//Destroy(destroyObject,1.0f);
			}
			  CancelInvoke("CheckItem");

		} else {
			Instantiate(arrowPrfb, transform.position, transform.rotation);
		}

	}
	

	

	
	
}
