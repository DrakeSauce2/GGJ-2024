using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAttachComponent : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform attachPoint;
    [Space]
    [SerializeField] RectTransform _UIObject;

    [Header("Camera")]
    [SerializeField] private Camera _Cam;

    [Header("Animation")]
    [SerializeField] Image baseImage;
    [SerializeField] Sprite sprite_1;
    [SerializeField] Sprite sprite_2;

    public void StartFingerPoint()
    {
        _UIObject.gameObject.SetActive(true);

        StartCoroutine(PointDown());
    }

    private void Update()
    {
        if (_UIObject && _Cam)
        {
            _UIObject.position = _Cam.WorldToScreenPoint(attachPoint.position);
        }
        
    }

    private IEnumerator PointDown()
    {
        baseImage.sprite = sprite_1;

        yield return new WaitForSeconds(.3f);

        baseImage.sprite = sprite_2;

        yield return new WaitForSeconds(.3f);

        StartCoroutine(PointDown());
    }

}
