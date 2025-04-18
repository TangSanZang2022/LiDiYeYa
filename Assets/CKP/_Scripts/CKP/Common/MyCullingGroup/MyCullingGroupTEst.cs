using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCullingGroupTEst : MonoBehaviour
{
    CullingGroup cullingGroup;
    BoundingSphere[] boundingSpheres = new BoundingSphere[1];
    // Start is called before the first frame update
    void Start()
    {
        cullingGroup = new CullingGroup();
        for (int i = 0; i < boundingSpheres.Length; i++)
        {
            boundingSpheres[i] = new BoundingSphere(transform.position,2);
        }
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(boundingSpheres.Length);
        cullingGroup.SetBoundingDistances(new float[] { 20});
        cullingGroup.targetCamera = Camera.main;
        cullingGroup.SetDistanceReferencePoint(Camera.main.transform);
        cullingGroup.onStateChanged = StateChangeFunc;
    }

    private void StateChangeFunc(CullingGroupEvent sphere)
    {
        int[] resultIndices = new int[2];
        int num = 0;
        num = cullingGroup.QueryIndices(true, resultIndices,0);
        Debug.Log("AAA"+num);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        cullingGroup.Dispose();
    }
}
