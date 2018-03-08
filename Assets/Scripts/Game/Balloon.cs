using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    public enum BalloonType
    {
        Soft, Hard
    };

    public BalloonType balloonType;
    public Transform limitCelling;
    public float shell = 0.5f;
    public bool undestroyable;
    public int lifeBonus;
    public GameObject specialEffect;

    public int score;
    public float spawnRate = 1f;
    public int beginSpawnAtScore = 0;
    public float incrementSpawn;
    public int incrementSpawnEveryScore;
    public int maxSpawnRate;

    private Rigidbody2D rb2d;
    private int hitIce;
    private int numberRequiredHitIce;
    private GameObject ice;

    private GameObject baseGameObject;
    private MainController mc;
    private CircleCollider2D triggerCollider;

    private bool commited;
    private int playerRegistered;


    // Use this for initialization
    void Awake () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;

        baseGameObject = GameObject.FindWithTag("Base");
        mc = baseGameObject.GetComponent<MainController>();

        limitCelling = limitCelling ? limitCelling : GameObject.FindWithTag("Celling").transform;

        if(balloonType == BalloonType.Soft)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
            triggerCollider.radius = 0.5f;
            triggerCollider.isTrigger = true;
        }
        

        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.tag == "Ice")
            {
                hitIce = numberRequiredHitIce = 4;
                ice = transform.GetChild(i).gameObject;
                break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < limitCelling.position.y - shell)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.freezeRotation = true;
        }

        if (balloonType == BalloonType.Soft)
        {
            if(GameObject.FindWithTag("Emoji") && playerRegistered != GameObject.FindWithTag("Emoji").GetInstanceID())
            {
                Physics2D.IgnoreCollision(GameObject.FindWithTag("Emoji").GetComponent<Collider2D>(), GetComponent<Collider2D>());
                playerRegistered = GameObject.FindWithTag("Emoji").GetInstanceID();
            }
        }

        if(mc.life <= 0)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.gravityScale = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(balloonType == BalloonType.Soft && other.gameObject.tag == "Emoji" && !undestroyable && numberRequiredHitIce == 0 && !commited)
        {
            PopBalloon();
            mc.LifeBonus(lifeBonus);
        }

        if (other.gameObject.tag == "Effect" && !commited)
        {
            PopBalloon();
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (balloonType == BalloonType.Hard && coll.gameObject.tag == "Emoji" && !undestroyable && numberRequiredHitIce == 0 && !commited)
        {
            PopBalloon();
            mc.LifeBonus(lifeBonus);
        }

        if (coll.gameObject.tag == "Emoji" && numberRequiredHitIce > 0)
        {
            numberRequiredHitIce--;
            Color currentColor = ice.GetComponent<SpriteRenderer>().color;
            ice.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, IceTransparent(numberRequiredHitIce));
        }
    }

    float IceTransparent(int value)
    {
        return (float)value / (float)hitIce;
    }

    void PopBalloon()
    {
        commited = true;
        if (specialEffect)
        {
            Instantiate(specialEffect, transform.position, Quaternion.identity);
        }
        mc.AddScore(score);
        Destroy(this.gameObject);
    }
}
