using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameController gc;

    private void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        gc.CheckPointCount(this);
    }
}
