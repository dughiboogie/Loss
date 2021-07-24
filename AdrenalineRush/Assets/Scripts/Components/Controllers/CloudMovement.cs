using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private BoxCollider2D cloudsRange;
    private float cloudsRangeWidth, cloudsRangeHeight;
    private float cloudsRangeExtension;

    public GameObject[] cloudsPrefabs;

    private List<GameObject> currentClouds;

    [Header("Procedural cloud generation variables")]
    public int maxCloudsNumber = 30;
    public float movementValueMin = .1f;
    public float movementValueMax = 1.5f;
    public float maxScale = 2f;

    private void Awake()
    {
        cloudsRange = GetComponent<BoxCollider2D>();

        cloudsRangeWidth = cloudsRange.size.x;
        cloudsRangeHeight = cloudsRange.size.y;
        cloudsRangeExtension = cloudsRangeWidth / 2;

        currentClouds = new List<GameObject>();

        for(int i = 0; i < maxCloudsNumber; i++) {

            currentClouds.Add(InstantiateCloud(false));
        }

    }

    private void Update()
    {
        for(int i = 0; i < maxCloudsNumber; i++) {

            // check if object is out of bounds, in case destroy and instantiate a new one
            if(currentClouds[i].transform.localPosition.x >= cloudsRangeExtension) {
                Destroy(currentClouds[i]);

                currentClouds[i] = InstantiateCloud(true);
            }

            // move cloud
            float randomMovementValue = Random.Range(movementValueMin, movementValueMax);
            Vector2 currentPosition = currentClouds[i].transform.localPosition;
            Vector2 newPosition = new Vector2(currentPosition.x + randomMovementValue, currentPosition.y);

            currentClouds[i].transform.localPosition = Vector2.Lerp(currentPosition, newPosition, Time.deltaTime);

        }
    }

    private GameObject InstantiateCloud(bool runtimeCloud)
    {
        Vector2 currentCloudCoordinates = GenerateRandomCoordinates();
        if(runtimeCloud) {
            currentCloudCoordinates.x = (- cloudsRangeExtension);
        }

        int prefabIndex = Random.Range(0, cloudsPrefabs.Length);

        GameObject currentCloud = Instantiate(cloudsPrefabs[prefabIndex], transform, false);
        currentCloud.transform.localPosition = currentCloudCoordinates;

        Vector2 currentCloudScale = GenerateRandomScale();
        currentCloud.transform.localScale = currentCloudScale;

        return currentCloud;
    }

    private Vector2 GenerateRandomCoordinates()
    {
        float xCoord = Random.Range(-cloudsRangeExtension, cloudsRangeExtension);
        float yCoord = Random.Range(0f, cloudsRangeHeight);

        return new Vector2(xCoord, yCoord);
    }

    private Vector2 GenerateRandomScale()
    {
        float scaleValue = Random.Range(1f, maxScale);

        return new Vector2(scaleValue, scaleValue);
    }

}
