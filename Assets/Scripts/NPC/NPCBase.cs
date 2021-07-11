using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    void OnMouseEnter()
    {
        CursorManager.Instance.SetNPCTalkCursor();
    }

    void OnMouseExit()
    {
        CursorManager.Instance.SetNormalCursor();
    }
}
