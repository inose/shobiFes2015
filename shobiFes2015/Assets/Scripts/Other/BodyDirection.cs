using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/BodyDirection")]

public class BodyDirection : MonoBehaviour {
    public enum RotationAxes { StickXAndY = 0, StickX = 1, StickY = 2 }
    public RotationAxes axes = RotationAxes.StickXAndY;
    public float sensitivityX = 15F;	//感度.
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -30F;   //上下に顔をふる限界角度.
    public float maximumY = 60F;

    private float rotationY = 0F;

    private bool deathFlag = false;
    void Update()
    {
        if (deathFlag)
        {
            return;
        }

        if (axes == RotationAxes.StickXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal_RStick") * sensitivityX;

            rotationY += Input.GetAxis("Vertical_RStick") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.StickX)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal_RStick") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Vertical_RStick") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    public void GameOver()
    {
        deathFlag = true;
    }


}
