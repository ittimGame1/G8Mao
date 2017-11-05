using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenAdjust : MonoBehaviour
{
    public float targetWidth = 1080f;
    public float targetHeight = 1920f;
    Camera cam;
	
	void Start ()
    {
        cam = GetComponent<Camera>();
        cam.aspect = targetWidth / targetHeight;
	}
}
