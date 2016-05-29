using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadText : MonoBehaviour {

	private TextMesh text;
    private float timer;
    void Awake()
    {
        text = gameObject.GetComponent<TextMesh>();
        timer = 5.0f;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (text.text.CompareTo("") != 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {

                text.text = "";
                timer = 5.0f;
            }

        }

    }

    public void OnGUI()
    {
    }
}

