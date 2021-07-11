using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    public Text numText;
    private ObjectInfo objectInfo;

    public void SetCount(int count)
    {
        numText.text = count.ToString();
        numText.gameObject.SetActive(true);      
    }

    public void Clear()
    {
        numText.gameObject.SetActive(false);
    }
}
