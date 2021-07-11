using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    private Camera miniMapCamera;

    public float maxValue = 6;
    public float minValue = 2;
    public float zoomSpeed = 0.5f;
    public Slider slider;

    void Awake()
    {
        miniMapCamera = GameObject.FindWithTag("MiniCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// 放大小地图视野
    /// </summary>
    public void Enlarge()
    {
        slider.value -= zoomSpeed;
    }
    /// <summary>
    /// 缩小小地图视野
    /// </summary>
    public void Narrow()
    {
        slider.value += zoomSpeed;
    }
    /// <summary>
    /// 当直接修改slider时
    /// </summary>
    public void OnSliderValueChanged(float percent)
    {
        if (miniMapCamera)
        {
            miniMapCamera.orthographicSize = minValue + (maxValue - minValue) * percent;
        }          
    }
}
