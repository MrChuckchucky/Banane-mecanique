using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InputManager : MonoBehaviour
{
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    public float tolerance;
    public float backAngles;
    public float frontAngles;
    public float rotateSpeed;

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
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        MoveArmLeft(P1LeftArm, 1);
        MoveArmRight(P1RightArm, 1);
        MoveArmLeft(P2LeftArm, 2);
        MoveArmRight(P2RightArm, 2);

        if((XInput.instance.getButton(1, 'L') == ButtonState.Released || XInput.instance.getButton(1, 'R') == ButtonState.Released) && (XInput.instance.getButton(2, 'L') == ButtonState.Released || XInput.instance.getButton(2, 'R') == ButtonState.Released))
        {
            if (XInput.instance.getButton(1, 'L') == ButtonState.Pressed && XInput.instance.getButton(2, 'L') == ButtonState.Pressed)
            {
                GameObject.FindGameObjectWithTag("Player").transform.eulerAngles += new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
            }
            if (XInput.instance.getButton(1, 'R') == ButtonState.Pressed && XInput.instance.getButton(2, 'R') == ButtonState.Pressed)
            {
                GameObject.FindGameObjectWithTag("Player").transform.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime, 0);
            }
        }
    }

    void MoveArmRight(GameObject arm, int index)
    {
        if(XInput.instance.getYStickRight(index) <= tolerance && XInput.instance.getYStickRight(index) >= -tolerance && XInput.instance.getXStickRight(index) <= tolerance && XInput.instance.getXStickRight(index) >= -tolerance)
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(XInput.instance.getYStickRight(index) * -1 + arm.transform.position.x, arm.transform.position.y, XInput.instance.getXStickRight(index) + arm.transform.position.z);
            arm.transform.LookAt(look);
            if (arm.transform.localEulerAngles.y > backAngles && arm.transform.localEulerAngles.y < 360 - frontAngles)
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
    void MoveArmLeft(GameObject arm, int index)
    {
        if (XInput.instance.getYStickLeft(index) <= tolerance && XInput.instance.getYStickLeft(index) >= -tolerance && XInput.instance.getXStickLeft(index) <= tolerance && XInput.instance.getXStickLeft(index) >= -tolerance)
        {
            arm.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            arm.transform.eulerAngles = new Vector3(0, 0, 0);
            Vector3 look = new Vector3(XInput.instance.getYStickLeft(index) + arm.transform.position.x, arm.transform.position.y, XInput.instance.getXStickLeft(index) * -1 + arm.transform.position.z);
            arm.transform.LookAt(look);
            if (arm.transform.localEulerAngles.y > frontAngles && arm.transform.localEulerAngles.y < 360 - backAngles)
            {
                if (Mathf.Abs(arm.transform.localEulerAngles.y - frontAngles) < Mathf.Abs(arm.transform.localEulerAngles.y - (360 - backAngles)))
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
