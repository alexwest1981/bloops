using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;
    public float shakeDuration = 0f;
    public float shakeMagnitude = 0.01f;
    public float dampingSpeed = 1.0f;
    public bool isShaking = true; // Variabel för att kontrollera om kameran ska skaka

    Vector3 initialPosition;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = camTransform.localPosition;
    }

    void Update()
    {
        if (isShaking && shakeDuration > 0) // Kontrollera om kameran ska skaka och om skakningsvaraktigheten är större än 0
        {
            camTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = initialPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void ToggleShake(bool shakeEnabled)
    {
        isShaking = shakeEnabled; // Aktivera eller inaktivera kameraskakningen baserat på värdet
    }
}