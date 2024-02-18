using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(BloopManager))]
public class GameUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image nextBloopImage;
    [SerializeField] private TextMeshProUGUI nextBloopText;
    private BloopManager bloopManager;

    private void Awake()
    {
        BloopManager.onNextBloopIndexSet += UpdateNextBloopImage;
    }

    private void OnDestroy()
    {
        BloopManager.onNextBloopIndexSet -= UpdateNextBloopImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        //bloopManager = GetComponent<BloopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        nextBloopText.text = bloopManager.GetNextBloopName();
    }

    private void UpdateNextBloopImage()
    {
        if (bloopManager == null)
            bloopManager = GetComponent<BloopManager>();

        nextBloopImage.sprite = bloopManager.GetNextBloopSprite();

    }
}
