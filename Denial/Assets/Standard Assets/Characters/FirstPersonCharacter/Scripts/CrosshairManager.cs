using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private PlayerInteractionsHelper PlayerInteractionsHelper;
    
    public LayerMask clickableLayer;
    public int interactionDistance = 3;

        // Swap Cursors per object
    public Sprite pointer;
    public Sprite target;
    public Sprite doorway;

    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerInteractionsHelper.Init();
        image = GetComponent<Image>();
        image.sprite = pointer;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCrosshair();
        HandleInteraction();
    }

    void HandleCrosshair()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, interactionDistance, clickableLayer.value))
        {
            if (hit.collider.gameObject.tag.Equals(nameof(Clickables.Doorway)))
                image.sprite = doorway;
            else
                image.sprite = target;
        }
        else
        {
           image.sprite = pointer;
        }
    }

    void HandleInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, interactionDistance, clickableLayer.value)
            && Input.GetMouseButton(0))
        {
            PlayerInteractionsHelper.HandleInteraction(hit.collider.gameObject);
        }
    }
}
