using DG.Tweening;
using UnityEngine;

public class I_LeverOrHinge : ItemInteract_DOTween
{
    [SerializeField] private float openedAngle = -60f;
    [SerializeField] private float closedAngle = 0f;
    [SerializeField] private float rotateDuration = 0.35f;

    private bool isOpen = false;

    public override bool Interact()
    {
        isOpen = !isOpen;
        float targetAngle = isOpen ? openedAngle : closedAngle;

        transform.DOKill();
        transform.DORotate(new Vector3(0, 0, targetAngle), rotateDuration)
                 .SetEase(isOpen ? Ease.OutBack : Ease.OutBounce);

        return true;
    }
}
