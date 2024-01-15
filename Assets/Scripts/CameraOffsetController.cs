using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOffsetController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private float smoothingFactor = 1f;

    public float maxYOffset = 5.0f;
    // Start is called before the first frame update
    void Start()
    {

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
            StartCoroutine(OffsetDownwards());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(OffsetUpwards());
    }


    private IEnumerator OffsetDownwards()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        while (true)
        {
            transposer.m_TrackedObjectOffset.y -= smoothingFactor / 10;
            if (transposer.m_TrackedObjectOffset.y <= -maxYOffset)
            {
                break;
            }
        }
        yield return null;
    }

    private IEnumerator OffsetUpwards()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        while (true)
        {
            transposer.m_TrackedObjectOffset.y += smoothingFactor;
            if (transposer.m_TrackedObjectOffset.y >= 0)
            {
                transposer.m_TrackedObjectOffset.y = 0;
                break;
            }
        }
        yield return null;

    }
}
