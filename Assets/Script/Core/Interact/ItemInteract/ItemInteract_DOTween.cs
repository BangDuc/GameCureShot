using UnityEngine;
using Core.Interact;
using DG.Tweening;

[RequireComponent(typeof(PolygonCollider2D))]
public class ItemInteract_DOTween : MonoBehaviour, IInteractable
{
    [Header("DOTween Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.08f;
    [SerializeField] private float hoverDuration = 0.18f;

    protected Vector3 initialScale;

    protected Collider2D col;

    protected virtual void Awake()
    {
        initialScale = transform.localScale;
        col = GetComponent<Collider2D>();
    }

    public virtual bool CanInteract()
    {
        Debug.Log("CanInteract " + this.gameObject);
        return true;
    }

    public virtual bool Interact()
    {
        Debug.Log("Click " + this.gameObject);
        return true;
    }

    public void OnHoverEnter()
    {
        transform.DOKill();
        transform.DOScale(initialScale * hoverScaleMultiplier, hoverDuration).SetEase(Ease.OutBack);
        Debug.Log("OnHoverEnter " + this.gameObject);
    }

    public void OnHoverExit()
    {
        transform.DOKill();
        transform.DOScale(initialScale, hoverDuration).SetEase(Ease.InQuad);
        
        Debug.Log("OnHoverExit " + this.gameObject);
    }

    protected void PlayLockedFeedback()
    {
        transform.DOKill(true);
        transform.DOShakePosition(0.25f, new Vector3(0.08f, 0, 0), vibrato: 15, randomness: 90, fadeOut: true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
