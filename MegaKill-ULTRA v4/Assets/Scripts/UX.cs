using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UX : MonoBehaviour
{
    private Camera cam;
    public Vector3 offset;
    public Canvas canvas; 
    public TextMeshProUGUI score;
    public TextMeshProUGUI popup;
    public TextMeshProUGUI ammo;

    public Slider healthBar;
    public Slider focusBar;

    private GameManager gameManager;
    private PlayerController player;
    private float initialFoV;
    private Vector3 initialCanvasScale;

    public GameObject ammoIcon;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
        gameManager = FindObjectOfType<GameManager>();
        player = FindAnyObjectByType<PlayerController>();

        if (cam != null)
        {
            initialFoV = cam.fieldOfView;
        }
        
        if (canvas != null)
        {
            initialCanvasScale = canvas.transform.localScale;
        }
    }

    void Update()
    {
        UpdateCanvasFoVScaling();
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        healthBar.value = health / maxHealth;
    }

    public void UpdateFocus(float focus, float maxFocus)
    {
        focusBar.value = focus / maxFocus;
    }

    public void UpdateAmmo()
    {
        switch (player.currentWeapon)
        {
            case PlayerController.WeaponState.Revolver:
                ammoIcon.SetActive(true);
                ammo.text = player.gun.bullets.ToString();
                break;
            case PlayerController.WeaponState.Shotgun:
                ammoIcon.SetActive(true);
                ammo.text = player.gun.shells.ToString();
                break;
            default:
                ammoIcon.SetActive(false);
                break;
        }
    }

    void UpdateCanvasFoVScaling()
    {
        if (cam != null && canvas != null)
        {
            float currentFoV = cam.fieldOfView;
            float scaleFactor = currentFoV / initialFoV;
            canvas.transform.localScale = initialCanvasScale * scaleFactor;
        }
    }

    public void PopUp(int newScore)
    {
        popup.gameObject.SetActive(true);

        popup.text = "+" + newScore;

        float randomX = Random.Range(-1000f, 1000f);
        float randomY = Random.Range(-350f, 350f);

        popup.rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        StartCoroutine(ShowPopup());
    }

    IEnumerator ShowPopup()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = popup.rectTransform.anchoredPosition;
        Vector3 targetPosition = startPosition + new Vector3(0, 50, 0);

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            popup.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        popup.gameObject.SetActive(false);
    }

    public void Score()
    {
        // score.text = "SCORE: " + gameManager.score;
    }
}
