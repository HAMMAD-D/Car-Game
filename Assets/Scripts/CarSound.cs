using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    public float minSpeed;

    public float maxSpeed;

    private float currentSpeed;
    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;
    public AudioSource runAudio;

    private Rigidbody rigidbody;
    private CarController reference;

    // Start is called before the first frame update
    void Start()
    {
        reference = GameObject.Find("Car").GetComponent<CarController>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        currentSpeed = rigidbody.velocity.magnitude;
        if (currentSpeed >= minSpeed)
        {
            if (runAudio.pitch <= 3 && reference.engineRunning == true)
            {
                runAudio.pitch += 0.01f;
            }
            else if (reference.engineRunning == false)
            {
                runAudio.pitch -= 0.01f;
                if (runAudio.pitch <= -0.01f)
                {
                    runAudio.pitch = 0.01f;
                }
            }
        }
    }
}