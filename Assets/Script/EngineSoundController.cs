using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundController : MonoBehaviour
{
    public AudioSource carSoundSource;
    public EngineController carEngineController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        carSoundSource.pitch = Mathf.Lerp(0.5f, 3f, Mathf.InverseLerp(0f, 1200f, Mathf.Abs(carEngineController.GetEngineRPM())));
        //print(carSoundSource.pitch);
    }
}
