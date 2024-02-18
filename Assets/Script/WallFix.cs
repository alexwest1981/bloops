using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFix : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Transform rightWall;
    [SerializeField] private Transform leftWall;

    // Start is called before the first frame update
    void Start()
    {
        /*float aspectRatio = (float)Screen.height / Screen.width;

        Camera mainCamera = Camera.main;

        float halfHorizontalFov = mainCamera.orthographicSize / aspectRatio;

        rightWall.transform.position = new Vector3(halfHorizontalFov + .5f, 0, 0);
        leftWall.transform.position = new Vector3(halfHorizontalFov - .5f, 0, 0);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
