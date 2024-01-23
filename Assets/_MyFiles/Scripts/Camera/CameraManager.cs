using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [Header("Camera")]
    [SerializeField] Transform _camTransform;
    [SerializeField] float camHeight = 5f;
    [SerializeField] float armLength = 5f;
    [SerializeField, Range(0.1f, 5f)] float followSpeed;

    [Header("Look At")]
    [SerializeField] bool lookAt;

    [Header("Clamp")]
    [SerializeField] bool clampX;
    [SerializeField] float minX, maxX;

    private Vector3 refVelocity = Vector3.zero;

    private void Update()
    {
        if (followTarget == null)
        {
            if (Player.Instance == null) return;

            followTarget = Player.Instance.transform;
            return;
        }

        Vector3 camInterpPosition = new Vector3(followTarget.position.x, camHeight, armLength);
        _camTransform.position = Vector3.SmoothDamp(_camTransform.position, camInterpPosition, ref refVelocity, followSpeed);



        if(lookAt == true)
        {
            _camTransform.LookAt(followTarget);
        }

        if (clampX == true)
        {
            _camTransform.position = new Vector3(Mathf.Clamp(_camTransform.position.x, minX, maxX),
                                                             _camTransform.position.y,
                                                             _camTransform.position.z);
        }

    }



}
