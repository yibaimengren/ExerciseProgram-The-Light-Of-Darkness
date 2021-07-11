using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : BasePanel
{
    private PlayerStatus playerStatus;

    public Transform gridParent;
    public Text coinText;
    private GameObject itemPrefab;
    private Dictionary<int, int> itemSiblingIndexDict;//第一个int是id，第二个int是位置索引
    new void Awake()
    {
        base.Awake();
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        itemPrefab = (GameObject)Resources.Load("InventoryPanel/InventoryItem");
        InventoryManager.Instance.inventoryPanel = this;
        SyncUpdate();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        coinText.text = playerStatus.coin.ToString();
    }

    public void OnClickCloseButton()
    {
        UIManager.Instance.PopPanel();
    }
    /// <summary>
    /// 同步所有物品，第一次打开此面板时使用
    /// </summary>
    private void SyncUpdate()
    {
        foreach(KeyValuePair<int, int> pair in InventoryManager.Instance.ItemDict)
        {
            AddNewItem(pair.Key, pair.Value);
        }
    }
    /// <summary>
    /// 更新物品数量
    /// </summary>
    /// <param name="id"></param>
    public void UpdateItemCount(int id,int count)
    {
        int index;
        if( itemSiblingIndexDict.TryGetValue(id,out index))
        {
            InventoryGrid inventoryGrid = gridParent.GetChild(index).GetComponent<InventoryGrid>();

            if(count <= 0)//如果数量小于等于0
            {
                Destroy(inventoryGrid.transform.Find("InventoryItem(Clone)").gameObject);
                itemSiblingIndexDict.Remove(id);
                inventoryGrid.Clear();
            }
            else
                inventoryGrid.SetCount(count);//更新数量显示
        }
    }

    public bool AddNewItem(int id,int count)
    {
        if (itemSiblingIndexDict == null) itemSiblingIndexDict = new Dictionary<int, int>();

        int index = 0;
        for(int i =0; i < gridParent.childCount; i++)//遍历所有Grid，如果找到某个是空的，就往这里添加物品
        {
            if (gridParent.GetChild(i).Find("InventoryItem(Clone)") == null)
            {
                index = i;
                GameObject go = Instantiate(itemPrefab, gridParent.GetChild(i));
                InventoryItem inventoryItem = go.GetComponent<InventoryItem>();
          
                inventoryItem.SetItemById(id);
                itemSiblingIndexDict.Add(id, i);
                inventoryItem.SetParent(go.transform.parent);

                //更新数量显示
                InventoryGrid inventoryGrid = gridParent.GetChild(i).GetComponent<InventoryGrid>();
                inventoryGrid.SetCount(count);

                return true;
            }
        }
        return false;
    }

    public void UpdateSiblingIndex(int id,int index)
    {
        itemSiblingIndexDict[id] = index;
    }
}
