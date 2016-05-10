using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public int playerIndex;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Death();
        }
    }

    void OnCollisionEnter()
    {
        Death();
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
