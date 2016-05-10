using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public float speed;

	// Use this for initialization
	void Start ()
    {
        transform.GetChild(0).LookAt(transform.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.eulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
	}
}