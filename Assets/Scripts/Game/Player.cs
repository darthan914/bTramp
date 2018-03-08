using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D rb2d;

    private GameObject baseGameObject;
    private MainController mc;

    void Awake () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;

        baseGameObject = GameObject.FindWithTag("Base");
        mc = baseGameObject.GetComponent<MainController>();
    }

    void Update()
    {
        if(mc.life <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch()
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }
}
