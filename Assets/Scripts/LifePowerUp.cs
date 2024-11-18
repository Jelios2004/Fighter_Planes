using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            GameObject.Find("Player(Clone)").GetComponent<Player>().AddALife();
            Destroy(this.gameObject);
        }
    }
}
