using UnityEngine;

public class PositionAtFaceScreenSpace : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(0,0,0);
    }

    void Update()
    {
        if (OpenCVFaceDetection.NormalizedFacePositions.Count == 0)
            return;

        transform.position = new Vector3(OpenCVFaceDetection.NormalizedFacePositions[0].x, OpenCVFaceDetection.NormalizedFacePositions[0].y, 0);
        transform.localScale = OpenCVFaceDetection.Facesize;
    }
}