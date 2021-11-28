using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAtRightEyeScreenSpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(5, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenCVFaceDetection.NormalizedRighteyePositions.Count == 0)
            return;

        transform.position = new Vector3(OpenCVFaceDetection.NormalizedRighteyePositions[0].x, OpenCVFaceDetection.NormalizedRighteyePositions[0].y, -(OpenCVFaceDetection.Facesize.x / 2));
        transform.localScale = OpenCVFaceDetection.Rightsize;
    }
}
