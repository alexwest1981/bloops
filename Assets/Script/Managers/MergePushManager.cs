using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePushManager : MonoBehaviour
{

    [Header(" Settings")]
    [SerializeField] private float pushRadius;
    [SerializeField] private Vector2 minMaxPushMagnitude;
    [SerializeField] private float pushMagnitude;
    private Vector2 pushPosition;
    [SerializeField] private CameraShake cameraShake;

    [Header(" Debug")]
    [SerializeField] private bool enableGizmos;

    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;
        SettingsManager.onPushMagnitudeChanged += PushMagnitudeChangedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
        SettingsManager.onPushMagnitudeChanged -= PushMagnitudeChangedCallback;
    }
    

 

    private void MergeProcessedCallback(BloopType bloopType, Vector2 mergePos)
    {
        pushPosition = mergePos;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mergePos, pushRadius);

        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out Bloop bloop))
            {
                Vector2 force = ((Vector2)bloop.transform.position - mergePos).normalized;
                force *= pushMagnitude;


                bloop.GetComponent<Rigidbody2D>().AddForce(force);
            }
            if (cameraShake != null)
            {
                cameraShake.TriggerShake(0.3f, 0.05f);
            }
        }
    }

    private void PushMagnitudeChangedCallback(float newPushMagnitude)
    {
        pushMagnitude = Mathf.Lerp(minMaxPushMagnitude.x, minMaxPushMagnitude.y, newPushMagnitude);
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!enableGizmos)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pushPosition, pushRadius);
    }

#endif
}
