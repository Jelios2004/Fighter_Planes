using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side_Enemy : MonoBehaviour
{

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 2f);

        if (transform.position.x < -30f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            //I hit the Player!
            whatIHit.GetComponent<Player>().LoseALife();
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (whatIHit.tag == "Weapon")
        {
            //I am shot!
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(3);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }
}