using DG.Tweening;
using UnityEngine;

public class I_HiddenObstacle : ItemInteract_DOTween
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public override bool Interact()
    {
        col.enabled = false; // Tắt va chạm để bấm được vật ẩn bên trong

        // Vừa đẩy vật trượt xuống, vừa mờ dần
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(transform.localPosition.y - 0.4f, 0.3f).SetEase(Ease.InBack))
           .Join(spriteRenderer.DOFade(0f, 0.3f))
           .OnComplete(() => gameObject.SetActive(false));

        return true;
    }
}
