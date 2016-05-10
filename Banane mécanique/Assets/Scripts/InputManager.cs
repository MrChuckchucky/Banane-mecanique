using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public float tolerance;
    public float backAngles;
    public float frontAngles;

    GameObject P1LeftArm;
    GameObject P1RightArm;
    GameObject P2LeftArm;
    GameObject P2RightArm;
    // Use this for initialization
    void Start ()
    {
        P1LeftArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(1).gameObject;
        P1RightArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).gameObject;
        P2LeftArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(1).gameObject;
        P2RightArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        MoveArmLeft(P1LeftArm, "LeftPlayer1");
        MoveArmRight(P1RightArm, "RightPlayer1");
        MoveArmLeft(P2LeftArm, "LeftPlayer2");
        MoveArmRight(P2RightArm, "RightPlayer2");
    }

    void MoveArmRight(GameObject arm, string input)
    {
        string input1 = "Vertical" + input;
        string input2 = "Horizontal" + input;
        if (Input.GetAxis(input1) <= tolerance && Input.GetAxis(input1) >= -tolerance && Input.GetAxis(input2) <= tolerance && Input.GetAxis(input2) >= -tolerance)
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis(input1) + arm.transform.position.x, arm.transform.position.y, Input.GetAxis(input2) + arm.transform.position.z);
            arm.transform.LookAt(look);
            if(arm.transform.localEulerAngles.y > backAngles && arm.transform.localEulerAngles.y < 360 - frontAngles)
            {
                if (Mathf.Abs(arm.transform.localEulerAngles.y - (360 - frontAngles)) < Mathf.Abs(arm.transform.localEulerAngles.y - backAngles))
                {
                    arm.transform.localEulerAngles = new Vector3(0, 360 - frontAngles, 0);
                }
                else
                {
                    arm.transform.localEulerAngles = new Vector3(0, backAngles, 0);
                }
            }
        }
    }
    void MoveArmLeft(GameObject arm, string input)
    {
        string input1 = "Vertical" + input;
        string input2 = "Horizontal" + input;
        if (Input.GetAxis(input1) <= tolerance && Input.GetAxis(input1) >= -tolerance && Input.GetAxis(input2) <= tolerance && Input.GetAxis(input2) >= -tolerance)
        {
            arm.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis(input1) * -1 + arm.transform.position.x, arm.transform.position.y, Input.GetAxis(input2) * -1 + arm.transform.position.z);
            arm.transform.LookAt(look);
            if(arm.transform.localEulerAngles.y > frontAngles && arm.transform.localEulerAngles.y < 360 - backAngles)
            {
                if(Mathf.Abs(arm.transform.localEulerAngles.y - frontAngles) < Mathf.Abs(arm.transform.localEulerAngles.y - (360 - backAngles)))
                {
                    arm.transform.localEulerAngles = new Vector3(0, frontAngles, 0);
                }
                else
                {
                    arm.transform.localEulerAngles = new Vector3(0, 360 - backAngles, 0);
                }
            }
        }
    }
}
