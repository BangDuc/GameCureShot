using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CampfireLightFlicker : MonoBehaviour
{
    private Light2D light2D;
    public float minIntensity = 1.2f;
    public float maxIntensity = 2.2f;
    public float speed = 5.0f;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (light2D != null)
        {
            light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * speed, 0.0f));
        }
    }
}