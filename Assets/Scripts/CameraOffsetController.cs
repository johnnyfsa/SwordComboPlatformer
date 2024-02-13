using System.Collections;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOffsetController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private Vector3 smoothingFactor;

    public Vector3 maxOffset;
    // Start is called before the first frame update
    void Start()
    {
        smoothingFactor = maxOffset / 10;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (transposer)
        {
            StartCoroutine(OffsetForward());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(OffsetBackward());
    }


    private IEnumerator OffsetForward()
    {

        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        while (true)
        {
            transposer.m_TrackedObjectOffset += smoothingFactor;
            if (math.abs(transposer.m_TrackedObjectOffset.y) >= math.abs(maxOffset.y) && math.abs(transposer.m_TrackedObjectOffset.x) >= math.abs(maxOffset.x))
            {
                break;
            }

        }
        yield return null;
    }

    private IEnumerator OffsetBackward()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        while (true)
        {
            transposer.m_TrackedObjectOffset -= smoothingFactor;
            if (transposer.m_TrackedObjectOffset.y <= 0 && transposer.m_TrackedObjectOffset.x <= 0)
            {
                transposer.m_TrackedObjectOffset.y = 0;
                transposer.m_TrackedObjectOffset.x = 0;
                break;
            }

        }
        yield return null;

    }
}
