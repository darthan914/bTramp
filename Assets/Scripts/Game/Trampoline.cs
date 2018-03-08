using UnityEngine;


public class Trampoline : MonoBehaviour {

    public float speed = 0.4f;
    public float shell = 0.5f;
    public float force = 500f;
    public float forceTilt = 200f;
    public Transform limitLeft;
    public Transform limitRight;

    public GameObject player;

    public bool autoPlay;

    private bool playerLaunched;
    private float timer;

    private GameObject baseGameObject;
    private MainController mc;

    private float touchOnHoldTime;
    private float timerMove;
    private int screenWidth;

    private Vector3 lastPositionTouchToWorld;
    private Vector2 touchOrigin;
    private float swipeSensitive = 20f;
    
    private RaycastHit2D[] hits;
    private float lowestObject = 5f;
    private Transform storedObject;
    private float direction;

    // Use this for initialization
    void Awake () {
        baseGameObject = GameObject.FindWithTag("Base");
        mc = baseGameObject.GetComponent<MainController>();
    }

    void FixedUpdate()
    {
        if (mc.life > 0)
        {
            if (limitLeft.position.x + shell < transform.position.x && Input.GetAxis("Horizontal") < 0)
            {
                transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed, 0f));
            }

            if (limitRight.position.x - shell > transform.position.x && Input.GetAxis("Horizontal") > 0)
            {
                transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed, 0f));
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];

                if (touch.phase == TouchPhase.Began)
                {
                    touchOrigin = touch.position;
                }

                else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = touch.position;

                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;

                    touchOrigin.x = -1;

                    if (Mathf.Abs(x) > swipeSensitive || Mathf.Abs(y) > swipeSensitive)
                    {
                        if (y > 0) Launch();
                    }

                }

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    lastPositionTouchToWorld = Camera.main.ScreenToWorldPoint(touch.position);
                }

                transform.position = new Vector3(lastPositionTouchToWorld.x, transform.position.y);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }

            if(autoPlay)
            {
                storedObject = null;
                lowestObject = 5f;
                direction = 0;

                hits = Physics2D.BoxCastAll(transform.position, new Vector2(9f, 5f), 0f, Vector2.up);

                for (int i = 0; i < hits.Length; i++)
                {
                    if(hits[i].transform.gameObject.tag == "Emoji" || hits[i].transform.gameObject.tag == "Balloon")
                    {
                        if (hits[i].transform.position.y + hits[i].rigidbody.velocity.y < lowestObject)
                        {
                            storedObject = hits[i].transform;
                            lowestObject = hits[i].transform.position.y;
                        }
                    }
                }

                float random = Random.Range(-1f, 1f);

                if(storedObject)
                {
                    if (storedObject.position.x > transform.position.x + 1.5f + random || storedObject.position.x < transform.position.x - 1.5f + random)
                    {
                        if (storedObject.position.x < transform.position.x)
                        {
                            direction = -1f;
                        }
                        else if (storedObject.position.x > transform.position.x)
                        {
                            direction = 1f;
                        }
                    }
                }

                if (limitLeft.position.x + shell < transform.position.x && direction < 0)
                {
                    transform.Translate(new Vector3(direction * speed, 0f));
                }

                if (limitRight.position.x - shell > transform.position.x && direction > 0)
                {
                    transform.Translate(new Vector3(direction * speed, 0f));
                }
            }
        }
    }

    void Update()
    {
        if (mc.life > 0)
        {
            if (GameObject.FindWithTag("Emoji") && !playerLaunched)
            {
                timer += Time.deltaTime;
                if (timer >= 5)
                {
                    Launch();
                }
            }

            if (GameObject.FindWithTag("Emoji") == null)
            {
                CreatePlayer();
                playerLaunched = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Emoji")
        {
            float dist = coll.gameObject.transform.position.x - transform.position.x;
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dist * forceTilt, Mathf.Abs(force)));
        }

        if (coll.gameObject.tag == "Balloon")
        {
            Destroy(coll.gameObject);
        }
    }

    public void CreatePlayer()
    {
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + player.GetComponent<CircleCollider2D>().radius);
        GameObject obj = Instantiate(player, spawnLocation, Quaternion.identity);
        obj.transform.SetParent(this.transform);
    }

    void Launch()
    {
        GameObject.FindWithTag("Emoji").GetComponent<Player>().Launch();
        GameObject.FindWithTag("Emoji").transform.SetParent(GameObject.FindWithTag("Base").transform);
        playerLaunched = true;
        timer = 0;
    }

    
}
