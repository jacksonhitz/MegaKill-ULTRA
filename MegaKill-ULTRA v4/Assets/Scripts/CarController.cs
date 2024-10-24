using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{ 

    public float horzInput;
    public float vertInput;
    float currentBrakeForce;
    float steerAngle;
    public bool isBraking;

    public WheelCollider fl;
    public WheelCollider fr;
    public WheelCollider bl;
    public WheelCollider br;

    public Transform flT;
    public Transform frT;
    public Transform blT;
    public Transform brT;

    public float motorForce;
    public float brakeForce;
    public float maxSteer;

    public PlayerController player;

    public BoxCollider boxCollider;

    void FixedUpdate()
    {
        if (player.currentState == PlayerController.State.driving && player.currentCar == transform)
        {
            Inputs();
        }

        Motor();
        Steer();
        // Wheels();
    }

    void Inputs()
    {
        horzInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
        isBraking |= Input.GetKey(KeyCode.Q);
    }

    void Motor()
    {
        if (!isBraking)
        {
            fl.motorTorque = vertInput * motorForce;
            fr.motorTorque = vertInput * motorForce;
            bl.motorTorque = vertInput * motorForce;
            br.motorTorque = vertInput * motorForce;
        }

        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBrake();
    }

    void ApplyBrake()
    {
        fl.brakeTorque = currentBrakeForce;
        fr.brakeTorque = currentBrakeForce;
        bl.brakeTorque = currentBrakeForce;
        br.brakeTorque = currentBrakeForce;

        if (!isBraking)
        {
            fl.brakeTorque = 0f;
            fr.brakeTorque = 0f;
            bl.brakeTorque = 0f;
            br.brakeTorque = 0f;
        }
    }

    void Steer()
    {
        steerAngle = maxSteer * horzInput;
        fl.steerAngle = steerAngle;
        fr.steerAngle = steerAngle;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC") && player.currentCar == transform)
        {
            NPC npc = collision.gameObject.GetComponent<NPC>();
            npc.Hit();
        }
    }

    void Wheels()
    {
        Single(fl, flT);
        Single(fr, frT);
        Single(bl, blT);
        Single(br, brT);
    }

    void Single(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
