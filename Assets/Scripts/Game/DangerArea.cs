using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour {
    private GameObject baseGameObject;
    private MainController mc;

    // Use this for initialization
    void Awake () {
        baseGameObject = GameObject.FindWithTag("Base");
        mc = baseGameObject.GetComponent<MainController>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Balloon")
        {
            mc.LifeBonus(-1);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "Emoji")
        {
            mc.LifeBonus(-1);
            Destroy(coll.gameObject);
        }
    }
}
