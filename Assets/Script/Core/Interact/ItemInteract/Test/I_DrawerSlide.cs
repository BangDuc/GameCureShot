using DG.Tweening;
using UnityEngine;

public class I_DrawerSlide : ItemInteract_DOTween
{
    [SerializeField] private Vector3 slideOffset = new Vector3(0.5f, 0, 0); // Kéo ngang sang phải 0.5 đơn vị
    [SerializeField] private float slideDuration = 0.3f;

    private Vector3 initialPosition;
    private bool isOpened = false;

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.localPosition;
    }

    public override bool Interact()
    {
        isOpened = !isOpened;
        Vector3 targetPos = isOpened ? initialPosition + slideOffset : initialPosition;

        transform.DOKill();
        transform.DOLocalMove(targetPos, slideDuration).SetEase(Ease.OutCubic);

        return true;
    }
}
