using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEnder : MonoBehaviour
{
    public bool CutSceneEnd = false;

    public static CutSceneEnder instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CutCam1"))
        {
            CutSceneEnd = true;
        }
    }
}
