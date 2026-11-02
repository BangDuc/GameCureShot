using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField]public List<Light2D> List_Light;


    public void GetListLight()
    {
        Light2D[] lights = GetComponentsInChildren<Light2D>();
        List_Light= lights.ToList();

    }
    private void Start()
    {
        GetListLight();
    }
    [ContextMenu("Off All")]
    public void OffAllLight()
    {
        foreach (Light2D light in List_Light) 
        {
            light.gameObject.SetActive(false);
        } 
    }
    [ContextMenu("On All")]
    public void OnAllLight()
    {
        foreach (Light2D light in List_Light)
        {
            light.gameObject.SetActive(true);
        }
    }


}
