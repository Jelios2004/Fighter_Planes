using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D coinMesh)
    {
        if (coinMesh.tag == "Player")
        {
            Destroy(this.gameObject);
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(1);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(coin);
    }

}