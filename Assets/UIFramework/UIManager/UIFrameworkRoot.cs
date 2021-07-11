using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFrameworkRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       UIManager.Instance.PushPanel(UIPanelType.MainPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
