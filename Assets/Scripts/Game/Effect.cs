using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

    public enum EffectType
    {
        Area, Shoot
    }

    public EffectType effectType;
    public float timeDestroy = 1f;
    public float maxDistance = 10f;
    public float speed = 2f;

    private Vector2 startPosition;
    private float timer;
    private float distance;
    private float randomDir;

    void Awake()
    {
        startPosition = transform.position;

        if (effectType == EffectType.Shoot)
        {
            randomDir = Random.Range(0f, 360f);
            transform.eulerAngles = new Vector3(0, 0, randomDir);
        }
    }

    void Update () {

        if (effectType == EffectType.Area)
        {
            timer += Time.deltaTime;

            if (timer >= timeDestroy)
            {
                Destroy(this.gameObject);
            }
        }
        else if (effectType == EffectType.Shoot)
        {
            transform.Translate((Vector2.up/60f) * speed);

            if (maxDistance <= Vector2.Distance(startPosition, transform.position))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
