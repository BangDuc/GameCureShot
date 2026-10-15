using DG.Tweening;
using System.Collections;
using UnityEngine;
    public class I_PushButton : ItemInteract_DOTween
    {   

        public override bool Interact()
        {
            transform.DOKill(true);
            // Nén nhẹ theo trục Y (thụt xuống) rồi đàn hồi về vị trí cũ trong 0.2s
            transform.DOPunchScale(new Vector3(0f, 0.1f, 0f), 0.2f, vibrato: 1, elasticity: 0.3f);

            return true;
        }
    }