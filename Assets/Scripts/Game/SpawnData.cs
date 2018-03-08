using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData : MonoBehaviour {

    public GameObject Balloon;
    public float StartRate;
    public float EndRate;

    public override string ToString()
    {
        return "Name: " + Balloon.name + "; Start Rate: " + StartRate + "; End Rate: " + EndRate + "; Change Rate: " + (EndRate - StartRate);
    }
}
