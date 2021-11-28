using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAtLeftEyeScreenSpace : MonoBehaviour
{

    void Start()
    {
        transform.position = new Vector3(-5, 0, -1);
    }

    void Update()
    {
        if (OpenCVFaceDetection.NormalizedLefteyePositions.Count == 0)
            return;

        transform.position = new Vector3(OpenCVFaceDetection.NormalizedLefteyePositions[0].x, OpenCVFaceDetection.NormalizedLefteyePositions[0].y, -(OpenCVFaceDetection.Facesize.x/2));
        transform.localScale = OpenCVFaceDetection.Leftsize;
    }
}
