using Bang.Lib.Singleton;
using DG.Tweening;
using UnityEngine;

public class TweenManager : Singleton<TweenManager>
{






    public void SnapToSlot(Transform target, Transform slotTransform, float duration = 0.3f)
    {
        if (target == null || slotTransform == null) return;

        Vector3 targetPos = new Vector3(slotTransform.position.x, slotTransform.position.y, target.position.z);

        target.DOKill();
        target.DOMove(targetPos, duration).SetEase(Ease.OutBack);
        target.DORotate(slotTransform.eulerAngles, duration * 0.8f);
    }
    public void InvalidShake(Transform target, float duration =0.4f , float strength= 0.3f )
    {
        target.DOShakePosition(duration, strength: new Vector3(strength, 0, 0), vibrato: 10, randomness: 90);
    }

    public void GetShotInGame(Transform target)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        sr.DOColor(Color.yellow, 0.2f).SetLoops(4, LoopType.Yoyo);
    }



}
