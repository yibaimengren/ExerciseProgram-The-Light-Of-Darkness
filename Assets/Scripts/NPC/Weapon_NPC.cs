using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_NPC : NPCBase
{
    [System.NonSerialized]
    public bool isOpen = false;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isOpen)
        {
            isOpen = true;
            WeaponShopPanel weaponShop = (WeaponShopPanel)UIManager.Instance.PushPanel(UIPanelType.WeaponShopPanel);
            weaponShop.weapon_Npc = this;
        }

    }
}
