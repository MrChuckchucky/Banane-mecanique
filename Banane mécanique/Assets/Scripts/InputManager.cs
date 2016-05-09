using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    public float tolerance;

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
        /*if(Input.GetAxis("HorizontalLeftPlayer1") >= tolerance || Input.GetAxis("HorizontalLeftPlayer1") <= -tolerance)
        {
            Debug.Log("hlp1");
        }
        if (Input.GetAxis("VerticalLeftPlayer1") >= tolerance || Input.GetAxis("VerticalLeftPlayer1") <= -tolerance)
        {
            Debug.Log("vlp1");
        }
        if (Input.GetAxis("HorizontalRightPlayer1") >= tolerance || Input.GetAxis("HorizontalRightPlayer1") <= -tolerance)
        {
            Debug.Log("hrp1");
        }
        if (Input.GetAxis("VerticalRightPlayer1") >= tolerance || Input.GetAxis("VerticalRightPlayer1") <= -tolerance)
        {
            Debug.Log("vrp1");
        }
        if (Input.GetAxis("HorizontalLeftPlayer2") >= tolerance || Input.GetAxis("HorizontalLeftPlayer2") <= -tolerance)
        {
            Debug.Log("hlp2");
        }
        if (Input.GetAxis("VerticalLeftPlayer2") >= tolerance || Input.GetAxis("VerticalLeftPlayer2") <= -tolerance)
        {
            Debug.Log("vlp2");
        }
        if (Input.GetAxis("HorizontalRightPlayer2") >= tolerance || Input.GetAxis("HorizontalRightPlayer2") <= -tolerance)
        {
            Debug.Log("hrp2");
        }
        if (Input.GetAxis("VerticalRightPlayer2") >= tolerance || Input.GetAxis("VerticalRightPlayer2") <= -tolerance)
        {
            Debug.Log("vrp2");
        }*/

        if(Input.GetAxis("VerticalLeftPlayer1") <= tolerance && Input.GetAxis("VerticalLeftPlayer1") >= -tolerance && Input.GetAxis("HorizontalLeftPlayer1") <= tolerance && Input.GetAxis("HorizontalLeftPlayer1") >= -tolerance)
        {
            P1LeftArm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            P1LeftArm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis("VerticalLeftPlayer1") * -1 + P1LeftArm.transform.position.x, P1LeftArm.transform.position.y, Input.GetAxis("HorizontalLeftPlayer1") * -1 + P1LeftArm.transform.position.z);
            P1LeftArm.transform.LookAt(look);
        }

        if (Input.GetAxis("VerticalRightPlayer1") <= tolerance && Input.GetAxis("VerticalRightPlayer1") >= -tolerance && Input.GetAxis("HorizontalRightPlayer1") <= tolerance && Input.GetAxis("HorizontalRightPlayer1") >= -tolerance)
        {
            P1RightArm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            P1RightArm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis("VerticalRightPlayer1") + P1RightArm.transform.position.x, P1RightArm.transform.position.y, Input.GetAxis("HorizontalRightPlayer1") + P1RightArm.transform.position.z);
            P1RightArm.transform.LookAt(look);
        }

        if (Input.GetAxis("VerticalLeftPlayer2") <= tolerance && Input.GetAxis("VerticalLeftPlayer2") >= -tolerance && Input.GetAxis("HorizontalLeftPlayer2") <= tolerance && Input.GetAxis("HorizontalLeftPlayer2") >= -tolerance)
        {
            P2LeftArm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            P2LeftArm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis("VerticalLeftPlayer2") * -1 + P2LeftArm.transform.position.x, P2LeftArm.transform.position.y, Input.GetAxis("HorizontalLeftPlayer2") * -1 + P2LeftArm.transform.position.z);
            P2LeftArm.transform.LookAt(look);
        }

        if (Input.GetAxis("VerticalRightPlayer2") <= tolerance && Input.GetAxis("VerticalRightPlayer2") >= -tolerance && Input.GetAxis("HorizontalRightPlayer2") <= tolerance && Input.GetAxis("HorizontalRightPlayer2") >= -tolerance)
        {
            P2RightArm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            P2RightArm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(Input.GetAxis("VerticalRightPlayer2") + P2RightArm.transform.position.x, P2RightArm.transform.position.y, Input.GetAxis("HorizontalRightPlayer2") + P2RightArm.transform.position.z);
            P2RightArm.transform.LookAt(look);
        }
    }
}
