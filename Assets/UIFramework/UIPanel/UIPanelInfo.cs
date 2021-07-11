using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [System.NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;
    public string path;

    public void OnAfterDeserialize()
    {
        panelType = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
    }

    public void OnBeforeSerialize()
    {
        //throw new System.NotImplementedException();
    }
}
