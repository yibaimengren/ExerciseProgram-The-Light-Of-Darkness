using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }
    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.blocksRaycasts = true;       
    }
    /// <summary>
    /// 界面不显示，退出此界面
    /// </summary>
    public virtual void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
