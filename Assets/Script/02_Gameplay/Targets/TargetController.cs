using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class TargetController : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private TargetData data;
    [SerializeField] private bool isCaptured = false;
    private List<IAffect> listAffects;
    [SerializeField] private List<MonoBehaviour> affectComponents = new List<MonoBehaviour>();

    public string Id { get => id; set => id = value; }
    public bool IsCaptured { get => isCaptured; set => isCaptured = value; }

    public event Action<TargetController> OnCapturedEvent;
    public event Action<TargetController> OnDespawnedEvent;

    public TargetData Data
    {
        get
        {
            if ((data == null || string.IsNullOrEmpty(data.TargetId)) && !string.IsNullOrEmpty(id))
            {
                if (TargetManager.Instance != null && TargetManager.Instance.library != null)
                {
                    data = TargetManager.Instance.library.GetTarget(id);
                }
            }
            return data;
        }
        set => data = value;
    }

    public void InitTarget()
    {
        isCaptured = false;
        if (TargetManager.Instance != null && TargetManager.Instance.library != null && !string.IsNullOrEmpty(id))
        {
            data = TargetManager.Instance.library.GetTarget(id);
        }
        GetListAffects();
    }

    /// <summary>
    /// Thiết lập quái vật theo ID và nạp đúng Sprite + hành vi tương ứng.
    /// </summary>
    public void Setup(string targetId, TargetData targetData = null)
    {
        id = targetId;
        data = targetData;
        isCaptured = false;

        if (data == null && TargetManager.Instance != null && TargetManager.Instance.library != null)
        {
            data = TargetManager.Instance.library.GetTarget(id);
        }

        // 1. Nạp Sprite đặc thù của quái
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;

            if (data != null && !string.IsNullOrEmpty(data.SpritePath))
            {
                Sprite customSprite = Resources.Load<Sprite>(data.SpritePath);
                if (customSprite != null)
                {
                    sr.sprite = customSprite;
                }
            }
        }
        transform.localScale = Vector3.one;

        // 2. Thiết lập hành vi đặc thù (Tấn công hoặc Tắt đèn)
        ConfigureBehaviors();

        InitTarget();
    }

    /// <summary>
    /// Phân chia hành vi rõ ràng:
    /// - Quỷ Máu (QuyMau): Tấn công người chơi, gây sát thương Run Health.
    /// - The Shy (TheShy): Tắt toàn bộ ánh sáng trong phòng của người chơi.
    /// </summary>
    public void ConfigureBehaviors()
    {
        TargetAttack attack = GetComponent<TargetAttack>();
        TargetTurnOffLight turnOffLight = GetComponent<TargetTurnOffLight>();

        string behavior = data != null ? data.BehaviorType : "";

        // 1. Quỷ Máu (Chuyên tấn công người chơi)
        if (id == "QuyMau" || behavior == "Attack")
        {
            if (turnOffLight != null)
            {
                turnOffLight.CancelEffect();
                turnOffLight.enabled = false;
            }

            if (attack == null) attack = gameObject.AddComponent<TargetAttack>();
            attack.enabled = true;
            attack.damage = 2; // Gây 2 sát thương Run Health
            attack.attackSpeed = 2.5f; // Đánh mỗi 2.5 giây
            attack.timeStartAttacking = 3f; // Tấn công sau 3 giây nếu không chụp
            attack.ApplyEffect();

            Debug.Log($"<color=red>[TargetController] Kích hoạt Quỷ Máu '{data?.TargetName ?? id}': Hành vi Tấn Công Người Chơi!</color>");
        }
        // 2. The Shy (Chuyên tắt hết ánh sáng trong scene)
        else if (id == "TheShy" || behavior == "TurnOffLight")
        {
            if (attack != null)
            {
                attack.CancelEffect();
                attack.enabled = false;
            }

            if (turnOffLight == null) turnOffLight = gameObject.AddComponent<TargetTurnOffLight>();
            turnOffLight.enabled = true;
            turnOffLight.timeToStart = 0.5f; // Tắt đèn ngay sau 0.5 giây
            turnOffLight.Duration = 18f; // Thời gian tắt đèn
            turnOffLight.ApplyEffect();

            Debug.Log($"<color=yellow>[TargetController] Kích hoạt The Shy '{data?.TargetName ?? id}': Hành vi Tắt Toàn Bộ Ánh Sáng!</color>");
        }
        else
        {
            // Các loại khác
            if (attack != null) attack.enabled = true;
            if (turnOffLight != null) turnOffLight.enabled = false;
        }

        GetListAffects();
    }

    public void GetListAffects()
    {
        IAffect[] affects = GetComponents<IAffect>();
        listAffects = affects != null ? affects.ToList() : new List<IAffect>();
        affectComponents = affects != null ? affects.OfType<MonoBehaviour>().ToList() : new List<MonoBehaviour>();
    }

    private void Start()
    {
        InitTarget();
    }

    /// <summary>
    /// Kích hoạt khi người chơi chụp trúng mục tiêu: hủy tấn công, bật lại đèn, chạy hiệu ứng tan biến và ẩn quái.
    /// </summary>
    public void OnCaptured()
    {
        if (isCaptured) return;
        isCaptured = true;

        CancelAllAffects();
        OnCapturedEvent?.Invoke(this);

        // Hiệu ứng biến mất: Scale nảy nhẹ rồi mờ dần
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && gameObject.activeInHierarchy)
        {
            transform.DOScale(transform.localScale * 1.25f, 0.15f).OnComplete(() =>
            {
                sr.DOFade(0f, 0.35f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    OnDespawnedEvent?.Invoke(this);
                });
            });
        }
        else
        {
            gameObject.SetActive(false);
            OnDespawnedEvent?.Invoke(this);
        }
    }

    /// <summary>
    /// Quái vật tự biến mất khi hết thời gian tồn tại.
    /// </summary>
    public void Despawn()
    {
        CancelAllAffects();

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && gameObject.activeInHierarchy)
        {
            sr.DOFade(0f, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnDespawnedEvent?.Invoke(this);
            });
        }
        else
        {
            gameObject.SetActive(false);
            OnDespawnedEvent?.Invoke(this);
        }
    }

    /// <summary>
    /// Hủy tất cả các hiệu ứng đang chạy (ngừng tấn công và bật sáng lại đèn).
    /// </summary>
    public void CancelAllAffects()
    {
        if (listAffects != null)
        {
            foreach (var affect in listAffects)
            {
                affect?.CancelEffect();
            }
        }

        var attack = GetComponent<TargetAttack>();
        if (attack != null) attack.CancelEffect();

        var lightOff = GetComponent<TargetTurnOffLight>();
        if (lightOff != null) lightOff.CancelEffect();
    }
}
