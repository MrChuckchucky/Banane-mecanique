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
    public float armMoveSpeed;
    public float coolDown;

    float cdArmLeftP1;
    float cdArmRightP1;
    float cdArmLeftP2;
    float cdArmRightP2;

    GameObject P1LeftArm;
    GameObject P1RightArm;
    GameObject P2LeftArm;
    GameObject P2RightArm;
    GameObject bullet;
    // Use this for initialization
    void Start ()
    {
        P1LeftArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(1).gameObject;
        P1RightArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).gameObject;
        P2LeftArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(1).gameObject;
        P2RightArm = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(0).gameObject;

        cdArmLeftP1 = cdArmRightP1 = cdArmLeftP2 = cdArmRightP2 = 0;
        bullet = Resources.Load("Bullet") as GameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        cdArmLeftP1 -= Time.deltaTime;
        cdArmRightP1 -= Time.deltaTime;
        cdArmLeftP2 -= Time.deltaTime;
        cdArmRightP2 -= Time.deltaTime;
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

        Rotate();
        MoveArms();
    }

    void Rotate()
    {
        if ((XInput.instance.getButton(1, 'L') == ButtonState.Released || XInput.instance.getButton(1, 'R') == ButtonState.Released) && (XInput.instance.getButton(2, 'L') == ButtonState.Released || XInput.instance.getButton(2, 'R') == ButtonState.Released))
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
    void MoveArms()
    {
        MoveArmLeft(P1LeftArm, 1);
        MoveArmRight(P1RightArm, 1);
        MoveArmLeft(P2LeftArm, 2);
        MoveArmRight(P2RightArm, 2);
    }
    void MoveArmRight(GameObject arm, int index)
    {
        if(XInput.instance.getYStickRight(index) <= tolerance && XInput.instance.getYStickRight(index) >= -tolerance && XInput.instance.getXStickRight(index) <= tolerance && XInput.instance.getXStickRight(index) >= -tolerance)
        {
        }
        else
        {
            Vector3 look = new Vector3(XInput.instance.getYStickRight(index) * -1 + arm.transform.position.x, arm.transform.position.y, XInput.instance.getXStickRight(index) * -1 + arm.transform.position.z);
            Vector3 forward = transform.forward;
            Vector3 toOther = arm.transform.position - look;
            float angle = Vector3.Angle(forward, toOther);
            if(look.x < 0)
            {
                angle *= -1;
            }
            arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * armMoveSpeed);
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
            CanShoot("Right", index);
        }
    }
    void MoveArmLeft(GameObject arm, int index)
    {
        if (XInput.instance.getYStickLeft(index) <= tolerance && XInput.instance.getYStickLeft(index) >= -tolerance && XInput.instance.getXStickLeft(index) <= tolerance && XInput.instance.getXStickLeft(index) >= -tolerance)
        {
        }
        else
        {
            Vector3 look = new Vector3(XInput.instance.getYStickLeft(index) + arm.transform.position.x, arm.transform.position.y, XInput.instance.getXStickLeft(index) + arm.transform.position.z);
            Vector3 forward = transform.forward;
            Vector3 toOther = arm.transform.position - look;
            float angle = Vector3.Angle(forward, toOther);
            if (look.x < 0)
            {
                angle *= -1;
            }
            arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * armMoveSpeed);
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
            CanShoot("Left", index);
        }
    }
    void CanShoot(string side, int index)
    {
        if(side == "Right")
        {
            if (index == 1)
            {
                if(cdArmRightP1 <= 0)
                {
                    Shoot(P1RightArm, index);
                    cdArmRightP1 = coolDown;
                }
            }
            else if (index == 2)
            {
                if (cdArmRightP2 <= 0)
                {
                    Shoot(P2RightArm, index);
                    cdArmRightP2 = coolDown;
                }
            }
        }
        else if(side == "Left")
        {
            if (index == 1)
            {
                if (cdArmLeftP1 <= 0)
                {
                    Shoot(P1LeftArm, index);
                    cdArmLeftP1 = coolDown;
                }
            }
            else if (index == 2)
            {
                if (cdArmLeftP2 <= 0)
                {
                    Shoot(P2LeftArm, index);
                    cdArmLeftP2 = coolDown;
                }
            }
        }
    }
    void Shoot(GameObject arm, int index)
    {
        GameObject shoot = Instantiate(bullet, arm.transform.position, Quaternion.identity) as GameObject;
        shoot.transform.LookAt(arm.transform.GetChild(0).position);
        shoot.GetComponent<BulletScript>().playerIndex = index;
    }
}
