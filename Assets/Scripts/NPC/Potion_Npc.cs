using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Npc : NPCBase
{
    [System.NonSerialized]
    public bool isOpen = false;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isOpen)
        {
            isOpen = true;
            DrugPanel drugPanel = (DrugPanel)UIManager.Instance.PushPanel(UIPanelType.DrugPanel);
            drugPanel.potion_Npc = this;
        }

    }
}
