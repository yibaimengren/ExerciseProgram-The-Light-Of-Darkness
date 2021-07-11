using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInfo : MonoBehaviour
{
    private static ObjectsInfo _instance;
    public static ObjectsInfo Instance {
        get
        {
            return _instance;
        }
    }

    private Dictionary<int, ObjectInfo> objectInfoDict = new Dictionary<int, ObjectInfo>();

    void Awake()
    {
        _instance = this;
        ReadObjectsInfo();
        //Debug.Log("Count=" + objectInfoDict.Count);
    }

    private void ReadObjectsInfo()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("ObjectsInforList");
        string[] lineArray = textAsset.text.Split('\n');

        for (int i = 1; i < lineArray.Length; i++)
        {
            ObjectInfo obj = new ObjectInfo();
            string[] item = lineArray[i].Split(',');
            obj.id = int.Parse(item[0]);
            obj.name = item[1];
            obj.icon_name = item[2];
            obj.type = (ObjectType)System.Enum.Parse(typeof(ObjectType), item[3]);
            if (obj.type == ObjectType.Drug)
            {
                obj.hp = int.Parse(item[4]);
                obj.mp = int.Parse(item[5]);
                obj.price_sell = int.Parse(item[6]);
                obj.price_buy = int.Parse(item[7]);
            }else if(obj.type == ObjectType.Equip)
            {
                obj.attack = int.Parse(item[4]);
                obj.def = int.Parse(item[5]);
                obj.speed = int.Parse(item[6]);
                obj.dressType = (DressType)System.Enum.Parse(typeof(DressType), item[7]);
                obj.applicationType = (ApplicationType)System.Enum.Parse(typeof(ApplicationType), item[8]);
                obj.price_sell = int.Parse(item[9]);
                obj.price_buy = int.Parse(item[10]);
           
                
            }


            objectInfoDict.Add(obj.id, obj);
        }
    }

    public ObjectInfo GetObjectInfoById(int id)
    {
        ObjectInfo objectInfo;
        objectInfoDict.TryGetValue(id, out objectInfo);
        return objectInfo;
    }
}

public enum ObjectType
{
    Drug,
    Equip,
    Mat
}

public enum DressType { 
    Headgear,
    Armor,
    RightHand,
    LeftHand,
    Shoe,
    Accessory
}

public enum ApplicationType
{
    Swordman,//剑士
    Magician,//魔法师
    Common//通用 
}


public class ObjectInfo
{
    public int id;
    public string name;
    public string icon_name;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int def;
    public int speed;
    public DressType dressType;
    public ApplicationType applicationType;
}


