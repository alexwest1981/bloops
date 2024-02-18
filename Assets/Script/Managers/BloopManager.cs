using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;

public class BloopManager : MonoBehaviour
{
    public static BloopManager Instance;

    [Header(" Elements ")]
    [SerializeField] private SkinDataSO skinData;
    [SerializeField] private Transform bloopParent;
    [SerializeField] private LineRenderer bloopDropLine;
    private Bloop currentBloop;


    [Header(" Settings ")]
    [SerializeField] private float bloopYSpawnPos;
    [SerializeField] private float spawnDelay;
    private bool canControl;
    private bool isControlling;


    [Header(" Next Bloop Settings ")]
    private int nextBloopIndex;

    [Header(" Debug ")]
    [SerializeField] private bool enableGizmos;

    [Header(" Actions")]
    public static Action onNextBloopIndexSet;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        MergeManager.onMergeProcessed += MergeProcessedCallback;

        ShopManager.onSkinSelected += SkinSelectedCallback;
        SetNextBloopIndex();
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;

        ShopManager.onSkinSelected -= SkinSelectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNextBloopIndex();

        canControl = true;
        HideLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.IsGameState())
            return;

        if (canControl)
            ManagePlayerInput();
    }

    private void SkinSelectedCallback(SkinDataSO skinDataSO)
    {
        skinData = skinDataSO;
    }
    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDownCallback();

        else if (Input.GetMouseButton(0))
        {
            if (isControlling)
                MouseDragCallback();
            else
                MouseDownCallback();
        }
            

        else if (Input.GetMouseButtonUp(0) && isControlling)
            MouseUpCallback();
    }

    private void MouseDownCallback()
    {
        if (!IsClickDetected())
            return;

        DisplayLine();

        PlaceLineAtClickedPosition();

        SpawnBloop();
        isControlling = true;
    }

    private bool IsClickDetected()
    {
        Vector2 mousePos = Input.mousePosition;

        return mousePos.y > Screen.height /10;
    }

    private void MouseDragCallback()
    {
        PlaceLineAtClickedPosition();

        if (currentBloop != null)
        {
            currentBloop.MoveTo(GetSpawnPosition());
        }
    }

    private void MouseUpCallback()
    {
        HideLine();

        if(currentBloop != null)
            currentBloop.EnablePhysics();

        canControl = false;
        isControlling = false;
        StartControlTimer();
    }

    private void SpawnBloop()
    {
        Vector2 spawnPosition = GetSpawnPosition();
        Bloop bloopToInstantiate = skinData.GetSpawnablePrefabs()[nextBloopIndex];

        currentBloop = Instantiate(
            bloopToInstantiate, 
            spawnPosition, 
            Quaternion.identity, 
            bloopParent);

        SetNextBloopIndex();
    }

    private void SetNextBloopIndex()
    {
        nextBloopIndex = UnityEngine.Random.Range(0, skinData.GetSpawnablePrefabs().Length);
        onNextBloopIndexSet?.Invoke();
    }

    public string GetNextBloopName()
    {
        return skinData.GetSpawnablePrefabs()[nextBloopIndex].name;
    }

    public Sprite GetNextBloopSprite()
    {
        return skinData.GetSpawnablePrefabs()[nextBloopIndex].GetSprite();
    }

    public Bloop[] GetSmallBloops()
    {
        List<Bloop> smallBloops = new List<Bloop>();

        for (int i = 0; i < bloopParent.childCount; i++)
        {
            Bloop bloop = bloopParent.GetChild(i).GetComponent<Bloop>();

            int bloopTypeInt = (int)(bloop.GetBloopType());

            if (bloopTypeInt < 3)
                smallBloops.Add(bloop);
        }
        return smallBloops.ToArray();
    }

    public Bloop[] GetMediumBloops()
    {
        List<Bloop> mediumBloops = new List<Bloop>();

        for (int i = 0; i < bloopParent.childCount; i++)
        {
            Bloop bloop = bloopParent.GetChild(i).GetComponent<Bloop>();

            int bloopTypeInt = (int)(bloop.GetBloopType());

            if (bloopTypeInt < 5)
                mediumBloops.Add(bloop);
        }
        return mediumBloops.ToArray();
    }

    public Bloop[] GetLargeBloops()
    {
        List<Bloop> largeBloops = new List<Bloop>();

        for (int i = 0; i < bloopParent.childCount; i++)
        {
            Bloop bloop = bloopParent.GetChild(i).GetComponent<Bloop>();

            int bloopTypeInt = (int)(bloop.GetBloopType());

            if (bloopTypeInt < 6)
                largeBloops.Add(bloop);
        }
        return largeBloops.ToArray();
    }

    private Vector2 GetClickedWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 clickedWorldPosition = GetClickedWorldPosition();
        clickedWorldPosition.y = bloopYSpawnPos;
        return clickedWorldPosition;
    }

    private void PlaceLineAtClickedPosition()
    {
        bloopDropLine.SetPosition(0, GetSpawnPosition());
        bloopDropLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15);
    }

    private void HideLine()
    {
        bloopDropLine.enabled= false;
    }

    private void StartControlTimer()
    {
        Invoke("StopControlTimer", spawnDelay);
    }

    private void StopControlTimer()
    {
        canControl = true;
    }

    private void DisplayLine()
    {
        bloopDropLine.enabled = true;
    }

    private void MergeProcessedCallback(BloopType bloopType, Vector2 spawnPosition)
    {
        for (int i = 0; i < skinData.GetObjectPrefabs().Length; i++)
        {
            if (skinData.GetObjectPrefabs()[i].GetBloopType() == bloopType)
            {
                SpawnMergedBloop(skinData.GetObjectPrefabs()[i], spawnPosition);
                break;
            }
        }
    }   

    private void SpawnMergedBloop(Bloop bloop, Vector2 spawnPosition)
    {
        Bloop bloopInstance = Instantiate(bloop, spawnPosition, Quaternion.identity, bloopParent);
        bloopInstance.EnablePhysics();
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (!enableGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-50, bloopYSpawnPos, 0), new Vector3(50, bloopYSpawnPos, 0));
    }
    
#endif
}
