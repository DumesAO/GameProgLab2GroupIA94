using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BerriesScript : MonoBehaviour
{
    public Text text;
    public GameObject door;
    public int requiredScore;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);
        text.text = "Berries Collected: 0/" + requiredScore;
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.collider.tag);
        if (col.collider.tag=="Berry")
        {
            CollectBerry();
            Transform berries = col.gameObject.transform.Find("Berries");
            berries.localScale = new Vector3(0, 0, 0);
            col.collider.tag = "Collected Berry";
        }
    }
    void CollectBerry()
    {
        score++;
        text.text = "Berries Collected: "+score+"/"+requiredScore;
        if (score >= requiredScore)
        {
            door.SetActive (true);
        }
    }
}
