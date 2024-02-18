/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineDisplay : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject deadline;
    [SerializeField] private Transform bloopParent;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.Game)
            StartCheckingForNearbyBloops();
        else
            StopCheckingForNearbyBloops();
    }

    private void StartCheckingForNearbyBloops()
    {
        StartCoroutine(CheckForNearbyBloopsCoroutine());
    }

    private void StopCheckingForNearbyBloops()
    {
        HideDeadline();

        StopCoroutine(CheckForNearbyBloopsCoroutine());
    }
    IEnumerator CheckForNearbyBloopsCoroutine()
    {
        while (true)
        {
            bool foundNearbyBloop = false;

            for (int i = 0; i < bloopParent.childCount; i++)
            {
                if (!bloopParent.GetChild(i).GetComponent<Bloop>().HasCollided())
                    continue;

                float distance = Mathf.Abs(bloopParent.GetChild(i).position.y - deadline.transform.position.y);

                if (distance <= 1)
                {
                    ShowDeadline();
                    foundNearbyBloop = true;
                    break;
                }
            }

            if(!foundNearbyBloop)
                HideDeadline() ;

            yield return new WaitForSeconds(1);
        }
    }

    private void ShowDeadline() => deadline.SetActive(true);

    private void HideDeadline() => deadline.SetActive(false);


}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineDisplay : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject deadline;
    [SerializeField] private Transform bloopParent;
    [SerializeField] private LineRenderer lineRenderer;

    private SpriteRenderer deadlineSpriteRenderer;
    private bool blinking;
    private bool lineBlinking;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
        deadlineSpriteRenderer = deadline.GetComponent<SpriteRenderer>();

        if (deadlineSpriteRenderer == null || lineRenderer == null)
        {
            Debug.LogError("SpriteRenderer or LineRenderer not found on the Deadline object!");
            enabled = false; // Disable this script if the components are missing
        }
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.Game)
            StartCheckingForNearbyBloops();
        else
            StopCheckingForNearbyBloops();
    }

    private void StartCheckingForNearbyBloops()
    {
        StartCoroutine(CheckForNearbyBloopsCoroutine());
    }

    private void StopCheckingForNearbyBloops()
    {
        HideDeadline();

        StopCoroutine(CheckForNearbyBloopsCoroutine());
    }

    private IEnumerator CheckForNearbyBloopsCoroutine()
    {
        while (true)
        {
            bool foundNearbyBloop = false;

            for (int i = 0; i < bloopParent.childCount; i++)
            {
                if (!bloopParent.GetChild(i).GetComponent<Bloop>().HasCollided())
                    continue;

                float distance = Mathf.Abs(bloopParent.GetChild(i).position.y - deadline.transform.position.y);

                if (distance <= 1)
                {
                    ShowDeadline();
                    foundNearbyBloop = true;
                    StartBlinking();
                    break;
                }
            }

            if (!foundNearbyBloop)
            {
                HideDeadline();
                StopBlinking();
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void ShowDeadline()
    {
        deadline.SetActive(true);
    }

    private void HideDeadline()
    {
        deadline.SetActive(false);
    }

    private void StartBlinking()
    {
        if (!blinking)
        {
            blinking = true;
            StartCoroutine(Blink());
        }
    }

    private void StopBlinking()
    {
        blinking = false;
        StopCoroutine(Blink());

        if (deadlineSpriteRenderer != null)
        {
            deadlineSpriteRenderer.color = Color.white; // Reset the color when blinking stops
        }
    }

    private void StartLineBlinking()
    {
        if (!lineBlinking)
        {
            lineBlinking = true;
            StartCoroutine(BlinkLine());
        }
    }

    private void StopLineBlinking()
    {
        lineBlinking = false;
        StopCoroutine(BlinkLine());

        // Reset line renderer color when blinking stops
        Color initialColor = Color.white; // Change this to your initial color
        lineRenderer.startColor = initialColor;
        lineRenderer.endColor = initialColor;
    }

    private IEnumerator BlinkLine()
    {
        Color initialColor = Color.white; // Change this to your initial color
        Color blinkColor = Color.red; // Change this to your blinking color
        bool toggle = false;

        while (lineBlinking)
        {
            if (toggle)
            {
                lineRenderer.startColor = blinkColor;
                lineRenderer.endColor = blinkColor;
            }
            else
            {
                lineRenderer.startColor = initialColor;
                lineRenderer.endColor = initialColor;
            }

            toggle = !toggle;
            yield return new WaitForSeconds(0.5f); // Change this to set the blinking speed
        }
    }

    private IEnumerator Blink()
    {
        while (blinking)
        {
            deadlineSpriteRenderer.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    }
}

