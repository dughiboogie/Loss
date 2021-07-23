using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPosition;
    [SerializeField] private new Camera camera;
    [SerializeField] private float parallaxValue;

    private void Awake()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Move smoothly layers
        float distance = camera.transform.position.x * parallaxValue;
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        // Move background sprites to always be seen
        float relativeDistance = camera.transform.position.x * (1 - parallaxValue);

        if(relativeDistance > startPosition + length) {
            startPosition += length;
        }
        else if(relativeDistance < startPosition - length) {
            startPosition -= length;
        }
    }
}
