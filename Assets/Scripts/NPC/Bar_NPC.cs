using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_NPC : NPCBase
{
    [System.NonSerialized]
    public bool isOpen = false;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isOpen)
        {
            isOpen = true;
            QuestPanel questPanel = (QuestPanel)UIManager.Instance.PushPanel(UIPanelType.QuestPanel);
            questPanel.bar_Npc = this;     
        }
            
    }
}
