using DG.Tweening;
using System.Collections;
using UnityEngine;


    public class I_HangingObject : ItemInteract_DOTween
    {
        public override bool Interact()
        {
            transform.DOKill(true);
            // Lắc trục Z góc 8 độ rồi giảm dần (đung đưa như quả lắc)
            transform.DOShakeRotation(0.8f, new Vector3(0, 0, 10f), vibrato: 10, randomness: 0)
                     .SetEase(Ease.OutQuad);

            return true;
        }

    }