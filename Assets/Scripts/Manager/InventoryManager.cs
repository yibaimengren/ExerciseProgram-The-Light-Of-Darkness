using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private Dictionary<int, int> itemDict;//第一个int是id，第二个int是数量

    public Dictionary<int, int> ItemDict {
        get
        {
            if (itemDict == null)
                itemDict = new Dictionary<int, int>();
            return itemDict;
        }
    }

    public InventoryPanel inventoryPanel;
    private const int max = 20;
    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            AddItem(Random.Range(2001, 2022), 10);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            AddItem(1001, 10);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            DeleteItem(Random.Range(2001, 2022),5);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            DeleteItem(1001, 5);

        //if (itemDict != null)
        //    Debug.Log("count=" + itemDict.Count);
    }

    /// <summary>
    /// 添加addNum个id物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="addNum"></param>
    public void AddItem(int id,int addNum)
    {
        if (itemDict == null) itemDict = new Dictionary<int, int>();

        int count = 0;
        if(itemDict.TryGetValue(id,out count))//如果已有该物品，则仅增加数量
        {
            itemDict[id] = count + addNum;
            if (inventoryPanel != null)
                inventoryPanel.UpdateItemCount(id,count + addNum);//将数字更新到面板上
        }
        else//如果该物品之前没有，则添加
        {
            itemDict.Add(id, addNum); 
            if (inventoryPanel != null)
                inventoryPanel.AddNewItem(id, addNum);    
        }
    }
    /// <summary>
    /// 删除delNum个id物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="delNum"></param>
    public void DeleteItem(int id, int delNum)
    {
        if (itemDict == null) itemDict = new Dictionary<int, int>();

        int count = 0;
        if( itemDict.TryGetValue(id, out count))
        {
            itemDict[id] -= delNum;
            if (inventoryPanel != null)
                inventoryPanel.UpdateItemCount(id, count - delNum);//将数字更新到面板上

            if (itemDict[id] <= 0)
                itemDict.Remove(id);
        }    
    }

    public int GetNumById(int id)
    {
        int num = -1;
        itemDict.TryGetValue(id, out num);
        return num;
    }

    public void UpdateSiblingIndex(int id, int index)
    {
        inventoryPanel.UpdateSiblingIndex(id, index);
    }

}
