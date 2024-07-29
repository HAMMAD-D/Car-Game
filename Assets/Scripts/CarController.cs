using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarWheelCollider colliders;
    [SerializeField] private WheelMeshes wheelMeshes;
    private float forwardInput;
    private float steerForce = 50f;
    private float steerInput;
    private float motorForce = 1000f;
    private float brakeForce = 500f;
    private Rigidbody carRb;

    public GameObject centerOfMassOfCar;

    public AnimationCurve steeringCurve;
    public AudioSource runEngine;
    public AudioSource idleEngine;
    public bool engineRunning;
    private CarSound reference;

    // Start is called before the first frame update
    void Start()
    {
        reference = GameObject.Find("CarMovingSound").GetComponent<CarSound>();
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMassOfCar.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        InputSystem();
        BreakingInput();
        AddMotorForce();
        SteeringWheel();
        CallingWheelUpdateFunction();
    }

    void AddMotorForce()
    {
        if (Input.GetKey(KeyCode.W))
        {
            engineRunning = true;
        }
        else
        {
            engineRunning = false;
        }

        colliders.backLeftWheelCollider.motorTorque = motorForce * forwardInput;
        colliders.backRightWheelCollider.motorTorque = motorForce * forwardInput;
    }

    void SteeringWheel()
    {
        colliders.frontLeftWheelCollider.steerAngle = steerForce * steerInput;
        colliders.frontRightWheelCollider.steerAngle = steerForce * steerInput;
    }

    void InputSystem()
    {
        forwardInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void BreakingInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            colliders.frontLeftWheelCollider.brakeTorque = brakeForce;
            colliders.frontRightWheelCollider.brakeTorque = brakeForce;
            colliders.backLeftWheelCollider.brakeTorque = brakeForce;
            colliders.backRightWheelCollider.brakeTorque = brakeForce;
            reference.runAudio.pitch -= 0.01f;
        }
        else
        {
            colliders.frontLeftWheelCollider.brakeTorque = 0f;
            colliders.frontRightWheelCollider.brakeTorque = 0f;
            colliders.backLeftWheelCollider.brakeTorque = 0f;
            colliders.backRightWheelCollider.brakeTorque = 0f;
        }
    }

    void UpdateWHeelAccordingToRotation(WheelCollider wCollider, MeshRenderer wheelMeshRenderer)
    {
        Vector3 position;
        Quaternion rotation;
        wCollider.GetWorldPose(out position, out rotation);
        wheelMeshRenderer.transform.position = position;
        wheelMeshRenderer.transform.rotation = rotation;
    }

    void CallingWheelUpdateFunction()
    {
        UpdateWHeelAccordingToRotation(colliders.frontLeftWheelCollider, wheelMeshes.frontLeftWheel);
        UpdateWHeelAccordingToRotation(colliders.frontRightWheelCollider, wheelMeshes.frontRightWheel);
        UpdateWHeelAccordingToRotation(colliders.backLeftWheelCollider, wheelMeshes.rearLeftWheel);
        UpdateWHeelAccordingToRotation(colliders.backRightWheelCollider, wheelMeshes.rearRightWheel);
    }
}

[System.Serializable]
class CarWheelCollider
{
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider backLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
};

[System.Serializable]
class WheelMeshes
{
    public MeshRenderer frontLeftWheel;
    public MeshRenderer frontRightWheel;
    public MeshRenderer rearLeftWheel;
    public MeshRenderer rearRightWheel;
};