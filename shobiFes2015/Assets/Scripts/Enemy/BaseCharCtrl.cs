using UnityEngine;
using System.Collections;

public class BaseCharCtrl : MonoBehaviour
{
    // === 外部パラメータ（インスペクタ表示） =====================
    public Vector3 velocityMin = new Vector3(-100.0f, -100.0f, -100.0f);	//速度上限.
    public Vector3 velocityMax = new Vector3(+100.0f, +100.0f, -100.0f);
    public float gravity = 20f;


    // === 外部パラメータ ======================================
    [System.NonSerialized]    public float dir = 1.0f;
    [System.NonSerialized]    public float speed = 6.0f;

    // === キャッシュ ==========================================
    [System.NonSerialized]    public Animator animator;
    [System.NonSerialized]    public CharacterController charCtrl;

    // === 内部パラメータ ======================================
    private Vector3 gravityAdd = Vector3.down;    //重力加算分.

    private float maxRotSpeed = 200.0f;
    private float minTime = 0.1f;
    private float _velocity;

    // === コード（Monobehaviour基本機能の実装） ================
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        charCtrl = GetComponent<CharacterController>();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate()
    {
        // キャラクタ個別の処理
        FixedUpdateCharacter();
        Gravity(gravity);
    }

    protected virtual void FixedUpdateCharacter()
    {    }


    // === コード（基本アクション） =============================
    //向いてる方向へ移動.
    public virtual void ActionMove()
    {
        Vector3 direction;
        direction = transform.forward;
        direction *= speed;
        charCtrl.Move(direction * Time.deltaTime);
    }

    //対象オブジェクトへ移動.
    public virtual void ActionMove(GameObject go)
    {
        ActionLookup(go);
        Vector3 direction;
        direction = transform.forward;
        direction *= speed;
        charCtrl.Move(direction * Time.deltaTime);
    }


    //重力付加.
    public virtual void Gravity(float _y)
    {
        gravityAdd.y += (Physics.gravity.y * Time.fixedDeltaTime * 5);
        gravityAdd.y = Mathf.Clamp(gravityAdd.y, velocityMin.y, velocityMax.y);
        charCtrl.Move(gravityAdd * Time.fixedDeltaTime);
        if (charCtrl.isGrounded)
        {
            gravityAdd = Vector3.down;
        }

    }


    //相手へ向く
    public void ActionLookup(GameObject go)
    {
        Vector3 newRotation = Quaternion.LookRotation(go.transform.position - transform.position).eulerAngles;    //goの方向を取得.
        Vector3 angles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angles.x, Mathf.SmoothDampAngle(angles.y, newRotation.y, ref _velocity, minTime, maxRotSpeed), angles.z);
    }

    //座標へ向く(x,z).
    public void ActionLookup(Vector2 _p)
    {
        _p.x += transform.position.x;
        _p.y += transform.position.z;
        Vector3 pos = new Vector3(_p.x, 0.0f, _p.y);
        Vector3 newRotation = Quaternion.LookRotation(pos - transform.position).eulerAngles;    //座標の方向を取得.
        Vector3 angles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angles.x, Mathf.SmoothDampAngle(angles.y, newRotation.y, ref _velocity, minTime, maxRotSpeed), angles.z);
    }

    //相手との距離(結果の数値は２乗である).
    public bool ActionLook(GameObject go, float near)
    {
        if (near >= 0)
        {
            if ((transform.position - go.transform.position).sqrMagnitude > near)
            {
                return true;
            }
        }
        else
        {
            near *= -1;
            if ((transform.position - go.transform.position).sqrMagnitude < near)
            {
               
                return true;
            }
        }
        return false;	//Playerに近づきすぎたらfalse
    }

    //相手へ近づく.nearより離れていたらgoのオブジェクトへ移動する.
    public bool ActionMoveToNear(GameObject go, float near)
    {
        
        if (ActionLook(go, near))
        {
           return true;
        }
        ActionMove(go);
        return false;	//Playerに近づきすぎたらfalse
    }

    // === コード（その他） ====================================

    //X軸速度取得.
    public float GetVelX()
    {
        return charCtrl.velocity.x;
    }

    //Y軸速度取得.
    public float GetVelY()
    {
        return charCtrl.velocity.y;
    }

    //Z軸速度取得.
    public float GetVelZ()
    {
        return charCtrl.velocity.z;
    }

    //速度取得.
    public Vector3 GetVel()
    {
        return charCtrl.velocity;
    }

    //X座標取得.
    public float GetPosX()
    {
        return transform.position.x;
    }

    //Y座標取得.
    public float GetPosY()
    {
        return transform.position.y;
    }

    //Z座標取得.
    public float GetPosZ()
    {
        return transform.position.z;
    }

    //座標取得.
    public Vector3 GetPos()
    {
        return charCtrl.transform.position; ;
    }

}
