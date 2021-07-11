using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对Dictionary的扩展
/// </summary>
public static class DictionaryExtension 
{
    /// <summary>
    /// 尝试根据key得到value，得到了直接返回value，没有得到返回null
    /// this Dictionary<Tkey,Tvalue> dict表示调用此方法的Dictionary对象
    /// </summary>
    public static Tvalue TryGet<Tkey,Tvalue>(this Dictionary<Tkey,Tvalue> dict,Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }
}
