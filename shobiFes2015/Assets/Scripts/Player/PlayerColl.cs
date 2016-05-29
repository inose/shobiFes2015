using UnityEngine;
using System.Collections;

public class PlayerColl : MonoBehaviour {

    PlayerCtrl playerCtrl;
    private bool hit = false;

	void Awake () {
        playerCtrl = GetComponent<PlayerCtrl>();
	}

    void OnTriggerEnter(Collider other) {
        
        if (other.tag == "EnemyAttack") {   //口にあたった.
            GameObject manager = GameObject.Find("Manager");
            GameObject objs = GameObject.Find("Objects");
            manager.BroadcastMessage("GameOver", SendMessageOptions.DontRequireReceiver);
            objs.BroadcastMessage("GameOver", SendMessageOptions.DontRequireReceiver);
		}

        if (other.tag == "TouchRange" && !hit){
            hit = true;
            other.SendMessageUpwards("Attack_A");   //強制的に攻撃させる.
            playerCtrl.TouchEnemy(other.gameObject);
        }

        if (other.tag == "ClearPosition" && playerCtrl.clearItem){
            playerCtrl.GameClear();
        }

        if (other.tag == "KeyItem")
        {
            /*
            playerCtrl.messageText.text = "You found a friend!" +"         "+ "go home in a hurry!";
            playerCtrl.messageText.color = Color.yellow;
            playerCtrl.Sound_KeyItem();
            Destroy(other.gameObject);
            //Debug.Log(other.name);
            //playerCtrl.clearItem = true;
            if (other.name.CompareTo("Friend") == 0)
            {
                playerCtrl.clearItem = true;
            }
             */
        }


	}


    public void GameOver()
    {
       // enabled = false;

    }
}
