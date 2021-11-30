using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAtMouthScreenSpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(5, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenCVFaceDetection.NormalizedMouthPositions.Count == 0)
            return;

        transform.position = new Vector3(OpenCVFaceDetection.NormalizedMouthPositions[0].x, OpenCVFaceDetection.NormalizedMouthPositions[0].y, -(OpenCVFaceDetection.Facesize.x / 2));
        transform.localScale = OpenCVFaceDetection.Mouthssize;
    }
}
