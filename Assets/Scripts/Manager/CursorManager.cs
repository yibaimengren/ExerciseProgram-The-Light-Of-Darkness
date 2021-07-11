using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager instance;

    public static CursorManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Texture2D normal;
    public Texture2D attack;
    public Texture2D pick;
    public Texture2D lockTarget;
    public Texture2D npcTalk;

    void Awake()
    {
        instance = this;
    }
    public void SetNormalCursor()
    {
        if (PlayerStatus.Instance.isAim)
        {
            SetAimCursor();
            return;
        }
            

        Cursor.SetCursor(normal, Vector2.zero, CursorMode.Auto);
        
    }

    public void SetNPCTalkCursor()
    {
        Cursor.SetCursor(npcTalk, Vector2.zero, CursorMode.Auto);
    }

    public void SetAttackCursor()
    {
        if (PlayerStatus.Instance.isAim)
            return;

        Cursor.SetCursor(attack, Vector2.zero, CursorMode.Auto);
    }

    public void SetAimCursor()
    {
        Cursor.SetCursor(lockTarget, Vector2.zero, CursorMode.Auto);
    }
}
