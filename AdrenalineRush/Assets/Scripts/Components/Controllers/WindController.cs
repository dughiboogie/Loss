using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public Material[] materials;
    public float windSpeed = .5f;

    private void Update()
    {
        if(materials[0].GetFloat("_WindSpeed") != windSpeed) {
            foreach(Material material in materials) {
                material.SetFloat("_WindSpeed", windSpeed);
            }
        }
    }
}
