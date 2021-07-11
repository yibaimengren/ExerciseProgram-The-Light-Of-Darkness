
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentBar : MonoBehaviour
{
    private static ExperimentBar _instance;
    public Image expImg;
    public Text expText;
    public static ExperimentBar Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
        PlayerStatus.Instance.AddExperiment(0);//一开始先更新经验条的显示
    }
    /// <summary>
    /// 更新经验条的显示
    /// </summary>
    /// <param name="current">当前获得的经验</param>
    /// <param name="total">升级所需经验</param>
    public void UpdateExperimentBarShow(float fillAmoutt,string text)
    {
        expImg.fillAmount = fillAmoutt;
        expText.text = text;
    }


}
