using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Interactable : MonoBehaviour, IInteractable
{
    Renderer rend;
    Collider col;
    Material def;
    [SerializeField] Material glow;

    PlayerController player;

    [HideInInspector] protected SoundManager sound;



    public bool isHovering;
    public bool isInteractable;

    public enum Type
    {
        Door,
        Item,
        Extract,
        Enemy
    }
    public Type type;

    public virtual void Interact()
    {
        if (type == Type.Door)
        {
            rend.enabled = false;
            col.enabled = false;
        }
        else if (type == Type.Extract && StateManager.State == StateManager.GameState.FIGHT) StateManager.NextState();
    }

    public virtual void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        def = rend.materials[0];
        sound = SoundManager.Instance;
    }

    void OnMouseEnter()
    {
        isHovering = true;
        GlowMat();
    }

    void OnMouseExit()
    {
        isHovering = false;
        DefaultMat();
    }

    public void DefaultMat()
    {
        rend.material = def;
    }

    public void GlowMat()
    {
        rend.material = glow;
    }
}
