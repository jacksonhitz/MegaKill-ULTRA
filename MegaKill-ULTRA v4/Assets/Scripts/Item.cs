using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool available;
    public Material def;
    public Material glow;

    public Renderer rend;
    Coroutine scaleCoroutine;
    Vector3 originalScale;
    float scaleDuration = 0.5f;
    bool isHovering;
    public bool thrown;

    void Awake()
    {
        originalScale = transform.localScale;
        available = true;

        rend = GetComponentInChildren<Renderer>();
        def = rend.material;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            thrown = false;
            if (collision.gameObject.CompareTag("NPC"))
            {
                collision.gameObject.GetComponent<Enemy>()?.Hit();
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider collider = GetComponent<Collider>();
            if (collider == null)
            {
                collider = GetComponentInParent<Collider>();
            }
            if (collider != null)
            {
                Physics.IgnoreCollision(collision.collider, collider);
            }
        }
    }

    public void Enable()
    {
        transform.SetParent(null);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.enabled = true;

        Vector3 randomDirection = new Vector3(
            Random.Range(-.1f, .1f),
            Random.Range(0.25f, .5f),
            Random.Range(-.1f, .1f)
        );

        rb.AddForce(randomDirection * 5f, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-20f, 20f),
            Random.Range(-20f, 20f),
            Random.Range(-20f, 20f)
        );

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    void OnMouseEnter()
    {
        Debug.Log("hovering");
        if (available)
        {
            rend.material = glow;
            isHovering = true;
            if (scaleCoroutine == null)
            {
                scaleCoroutine = StartCoroutine(PulseScale());
            }
        }
    }

    void OnMouseExit()
    {
        rend.material = def;
        isHovering = false;
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }
        StartCoroutine(ResetScale());
    }

    IEnumerator PulseScale()
    {
        Vector3 maxScale = originalScale * 1.2f;
        Vector3 minScale = originalScale * 1f;
        bool expanding = true;

        while (isHovering)
        {
            float elapsedTime = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = expanding ? maxScale : minScale;

            while (elapsedTime < scaleDuration && isHovering)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
            expanding = !expanding;
        }
    }

    IEnumerator ResetScale()
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, originalScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
