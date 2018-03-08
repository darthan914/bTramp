using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public enum SpawnType
    {
        ByRateBalloon, Randomize
    };

    public SpawnType spawnType = SpawnType.ByRateBalloon;
    public bool singleSpawn;

    public Vector2Int areaSpawn = new Vector2Int(1, 1);

    public List<GameObject> balloon;

    private float timer;
    private GameObject baseGameObject;
    private MainController mc;

    private List<SpawnData> listSpawnData = new List<SpawnData>();
    private float numberRateAdd;

    void Awake()
    {
        baseGameObject = GameObject.FindWithTag("Base");
        mc = baseGameObject.GetComponent<MainController>();

        SpawnBalloon(areaSpawn);
        if (singleSpawn)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        
        if(mc.life > 0)
        {
            timer += 1 / 50f * mc.Speed();

            if (timer > 1)
            {
                SpawnBalloon(new Vector2Int(areaSpawn.x, 1));
                timer = 0;
            }
        }
        
    }

    void SpawnBalloon(Vector2 area)
    {
        if(spawnType == SpawnType.ByRateBalloon)
        {
            listSpawnData.Clear();

            for (int i = 0; i < balloon.Count; i++)
            {
                listSpawnData.Add(new SpawnData()
                {
                    Balloon = balloon[i],
                    StartRate = numberRateAdd,
                    EndRate = numberRateAdd + Rate(balloon[i].GetComponent<Balloon>())
                });

                numberRateAdd += Rate(balloon[i].GetComponent<Balloon>());
            }
            // listSpawnData.Sort((p1, p2) => p1.StartRate.CompareTo(p2.StartRate));

            // Row
            for(int row = 0; row < area.y; row++)
            {
                // Column
                for (int col = 0; col < area.x; col++)
                {
                    float rateRange = Random.Range(0f, numberRateAdd);
                    Vector3 location = new Vector3(transform.position.x + col, transform.position.y - row, 0);

                    for (int i = 0; i < listSpawnData.Count; i++)
                    {
                        if (listSpawnData[i].StartRate <= rateRange && rateRange < listSpawnData[i].EndRate)
                        {
                            Instantiate(listSpawnData[i].Balloon, location, Quaternion.identity);
                            break;
                        }
                    }
                }
                    
            }


            numberRateAdd = 0;
        }
        else if (spawnType == SpawnType.Randomize)
        {
            // Row
            for (int row = 0; row < area.y; row++)
            {
                // Column
                for (int col = 0; col < area.x; col++)
                {
                    int random = Random.Range(0, balloon.Count);
                    Vector3 location = new Vector3(transform.position.x + col, transform.position.y - row, 0);
                    Instantiate(balloon[random], location, Quaternion.identity);
                }

            }
        }


    }

    float Rate(Balloon balloonData)
    {
        float rate;

        if(balloonData.beginSpawnAtScore <= mc.score + mc.startScore)
        {
            rate = Mathf.Min(balloonData.spawnRate + (balloonData.incrementSpawn * Mathf.Floor(((mc.score + mc.startScore) - balloonData.beginSpawnAtScore) / balloonData.incrementSpawnEveryScore) ), balloonData.maxSpawnRate <= 0 ? Mathf.Infinity : balloonData.maxSpawnRate);
        }
        else
        {
            rate = 0;
        }

        return rate;
    }


}
