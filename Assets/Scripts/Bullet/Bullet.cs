using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDMG;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthManager>().TakeDamage(bulletDMG);
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            other.GetComponent<HealthManager>().TakeDamage(bulletDMG);
            Destroy(gameObject);
        }
        else if (other.tag == "Object")
        {
            other.GetComponent<HealthManager>().TakeDamage(bulletDMG);
            Destroy(gameObject);
        }
        else if (other.tag == "Outpost")
        {
            other.GetComponent<HealthManager>().TakeDamage(bulletDMG);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
}
