using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public int HP;
    public int index;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("yo");
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
