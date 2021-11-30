using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class OpenCVFaceDetection : MonoBehaviour
{
    public static List<Vector2> NormalizedFacePositions { get; private set; }
    public static Vector3 Facesize;
    public static List<Vector2> NormalizedLefteyePositions { get; private set; }
    public static Vector3 Leftsize;
    public static List<Vector2> NormalizedRighteyePositions { get; private set; }
    public static Vector3 Rightsize;
    public static List<Vector2> NormalizedMouthPositions { get; private set; }
    public static Vector3 Mouthssize;

    public static Vector2 CameraResolution;

    /// <summary>
    /// Downscale factor to speed up detection.
    /// </summary>
    private const int DetectionDownScale = 1;
    private const int MovementScale = 5;
    private const float MovementScaleMouthY = 9 / 2;
    private const float ProximityScale = 120 / 7;
    private const float ProximityScaleMouthX = 240 / 7;
    private bool _ready;
    private int _maxFaceDetectCount = 2;
    private CvCircle[] _faces;
    private CvCircle[] _lefteye;
    private CvCircle[] _righteye;
    private CvBox[] _mouth;


    void Start()
    {
        int camWidth = 0, camHeight = 0;
        int result = OpenCVInterop.Init(ref camWidth, ref camHeight);           //Get size of frames gotten from camera, number of pixels in image
        if (result < 0)
        {
            if (result == -1)
            {
                Debug.LogWarningFormat("[{0}] Failed to find neuronals networks definitions.", GetType());
            }
            else if (result == -2)
            {
                Debug.LogWarningFormat("[{0}] Failed to open camera stream.", GetType());
            }

            return;
        }

        CameraResolution = new Vector2(camWidth, camHeight);
        // Arrays to store variables
        _faces = new CvCircle[_maxFaceDetectCount];
        _lefteye = new CvCircle[_maxFaceDetectCount];
        _righteye = new CvCircle[_maxFaceDetectCount];
        _mouth = new CvBox[_maxFaceDetectCount];

        NormalizedFacePositions = new List<Vector2>();
        NormalizedLefteyePositions = new List<Vector2>();
        NormalizedRighteyePositions = new List<Vector2>();
        NormalizedMouthPositions = new List<Vector2>();

        OpenCVInterop.SetScale(DetectionDownScale);
        _ready = true;
    }

    void OnApplicationQuit()
    {
        if (_ready)
        {
            OpenCVInterop.Close();
        }
    }

    void Update()
    {
        if (!_ready)
            return;

        int detectedFaceCount = 0;
        int detectedLefteyeCount = 0;
        int detectedRighteyeCount = 0;
        int detectedMouthCount = 0;
        unsafe
        {
            fixed (CvCircle* outFaces = _faces, outlefteye = _lefteye, outrighteye = _righteye)
            {
                fixed (CvBox* outmouth = _mouth)
                {
                    OpenCVInterop.Detect(outFaces, outlefteye, outrighteye, outmouth, _maxFaceDetectCount, ref detectedFaceCount, ref detectedLefteyeCount, ref detectedRighteyeCount, ref detectedMouthCount);
                }
            }
        }

        NormalizedFacePositions.Clear();
        NormalizedLefteyePositions.Clear();
        NormalizedRighteyePositions.Clear();
        NormalizedMouthPositions.Clear();
        for (int i = 0; i < detectedFaceCount; i++)
        {
            float size = _faces[i].Radius * DetectionDownScale * ProximityScale / CameraResolution.x;
            NormalizedFacePositions.Add( new Vector2( ((_faces[i].X * DetectionDownScale) - (CameraResolution.x / 2))*(2*MovementScale/CameraResolution.x), ((_faces[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2*MovementScale / CameraResolution.y) ) );
            Facesize = new Vector3(size, size, size); 
            //Debug.Log("Face = " + new Vector2(((_faces[i].X * DetectionDownScale) - (CameraResolution.x / 2)) * (2 * MovementScale / CameraResolution.x), ((_faces[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScale / CameraResolution.y)));
            //Debug.Log("FaceSize=" +  _faces[i].Radius*DetectionDownScale*FaceScale / CameraResolution.x);
        }
        for (uint i = 0; i < detectedLefteyeCount; i++)
        {
            float size = _lefteye[i].Radius * DetectionDownScale * ProximityScale / CameraResolution.x;
            NormalizedLefteyePositions.Add(new Vector2( ( ( _lefteye[i].X* DetectionDownScale ) - (CameraResolution.x / 2) ) * (2 * MovementScale / CameraResolution.x), (( _lefteye[i].Y* DetectionDownScale) - (CameraResolution.y / 2))*(-2 * MovementScale / CameraResolution.y)) );
            Leftsize = new Vector3(size, size, size);
            //Debug.Log("Lefteye = " + new Vector2(((_lefteye[i].X * DetectionDownScale) - (CameraResolution.x / 2)) * (2 * MovementScale / CameraResolution.x), (( _lefteye[i].Y* DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScale / CameraResolution.y)) );
            //Debug.Log("EyeSize=" + _lefteye[i].Radius * DetectionDownScale * ProximityScale / CameraResolution.x);
        }
        for (uint i = 0; i < detectedRighteyeCount; i++)
        {
            float size = _lefteye[i].Radius * DetectionDownScale * ProximityScale / CameraResolution.x;
            NormalizedRighteyePositions.Add(new Vector2( ( ( _righteye[i].X * DetectionDownScale ) - (CameraResolution.x / 2) ) * (2 * MovementScale / CameraResolution.x), (( _righteye[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScale / CameraResolution.y)) );
            Rightsize = new Vector3(size, size, size);
            //Debug.Log("RightEye = " + new Vector2(((_lefteye[i].X * DetectionDownScale) - (CameraResolution.x / 2)) * (2 * MovementScale / CameraResolution.x), ((_lefteye[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScale / CameraResolution.y)));
            //Debug.Log("RightSize=" + _righteye[i].Radius * DetectionDownScale * ProximityScale / CameraResolution.x);
        }
        for (uint i = 0; i < detectedMouthCount; i++)
        {
            float sizex = _mouth[i].Width * DetectionDownScale * ProximityScaleMouthX / CameraResolution.x;
            float sizey = _mouth[i].Height * DetectionDownScale * ProximityScale / CameraResolution.x;
            NormalizedMouthPositions.Add(new Vector2(((_mouth[i].X * DetectionDownScale) - (CameraResolution.x / 2)) * (2 * MovementScale / CameraResolution.x), ((_mouth[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScaleMouthY / CameraResolution.y)));
            Mouthssize = new Vector3(sizex, sizey, sizey);
            //Debug.Log("Mouth = " + new Vector2(((_mouth[i].X * DetectionDownScale) - (CameraResolution.x / 2)) * (2 * MovementScale / CameraResolution.x), ((_mouth[i].Y * DetectionDownScale) - (CameraResolution.y / 2)) * (-2 * MovementScaleMouthY / CameraResolution.y)));
            Debug.Log("Mouthssize=" + new Vector3(sizex, sizey, sizey));
        }
    }
}
// Define the functions which can be called from the .dll.
internal static class OpenCVInterop
{
    [DllImport("OpenCV")]
    internal static extern int Init(ref int outCameraWidth, ref int outCameraHeight);

    [DllImport("OpenCV")]
    internal static extern int Close();

    [DllImport("OpenCV")]
    internal static extern int SetScale(int downscale);

    [DllImport("OpenCV")]
    internal unsafe static extern void Detect(CvCircle* outFaces, CvCircle* outlefteye, CvCircle* outrighteye, CvBox* outmouth, int maxOutFacesCount, ref int outDetectedFacesCount, ref int outDetectedLefteyeCount, ref int outDetectedRighteyeCount, ref int outDetectedMouth);
}

// Define the structure to be sequential and with the correct byte size (3 ints = 4 bytes * 3 = 12 bytes)
[StructLayout(LayoutKind.Sequential, Size = 12)]
public struct CvCircle
{
    public int X, Y, Radius;
}
[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct CvBox
{
    public int X, Y, Width, Height;
};