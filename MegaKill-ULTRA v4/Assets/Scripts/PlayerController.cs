using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class PlayerController : MonoBehaviour
{
    float horzInput;
    float vertInput;
    public float runSpd;
    public float range; 
    public float reach;
    float gravity = -19.62f;
    public Camera cam;
    public CharacterController controller; 
    Vector3 vel;
    public Transform groundCheck;
    public Transform currentCar;
    float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public enum State { foot, driving }
    public State currentState = State.foot;

    public Gun gun;
    public UX ux;

    bool hasMoved = false;
    bool hasFired = false; 
    bool hasReloaded = false; 
    bool hasBailed = false; 

    int health = 10;

    SoundManager soundManager;

    public bool isDead = false;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }
        
        switch (currentState)
        {
            case State.foot:
                controller.enabled = true;
                Move();
                //HoverCheck();  
                break;
            case State.driving:
                controller.enabled = false;
                Drive();
                break;
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (currentState == State.foot)
            {
                Interact();
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (!hasBailed)
            {
                ux.Police();
                hasBailed = true;
            }
            Bail();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            gun.Shoot();

            if (!hasFired)
            {
                ux.Reload();
                hasFired = true; 
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            gun.Reload();
            if (!hasReloaded)
            {
                ux.Drive();
                hasReloaded = true; 
            }
        }
    }

    void Bail()
    {
        transform.position = transform.position + Vector3.left * 0.2f;
        currentState = State.foot;
    }

    void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && vel.y < 0)
        {
            vel.y = -2f;
        }
        
        horzInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horzInput + transform.forward * vertInput;

        if (!hasMoved && (horzInput != 0 || vertInput != 0))
        {
            ux.Kill();
            hasMoved = true; 
        }

        vel.y += gravity * Time.deltaTime;

        controller.Move(move * runSpd * Time.deltaTime);
        controller.Move(vel * Time.deltaTime);
    }

    void Drive()
    {
        transform.position = currentCar.position + Vector3.up * 0.7f + Vector3.left * 0.2f + Vector3.back * 0.15f;
    }

    void Interact()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.CompareTag("Car"))
            {
                float gap = Vector3.Distance(transform.position, hit.transform.position);
                if (gap <= reach)
                {
                    currentState = State.driving;
                    currentCar = hit.transform;
                    ux.Bail();
                }
            }
        }
    }

    public void Hit()
    {
        health--;

        if (health <= 0)
        {
            isDead = true;
            ux.Died();
            soundManager.Flatline();
            StartCoroutine(Fall());
        }
        else
        {
            ux.Hit();
            soundManager.Heartbeat();
        }
    }

    IEnumerator Fall()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(90f, 0f, 0f));

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }


    void HoverCheck()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.CompareTag("Car"))
            {
                float gap = Vector3.Distance(transform.position, hit.transform.position);
                if (gap <= reach)
                {
                    
                }
            }
        }
    }
}
