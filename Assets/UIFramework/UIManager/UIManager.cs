using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    static private UIManager _instance;

    static public UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }
    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
                canvasTransform = GameObject.Find("Canvas").transform;
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict;//储存所有面板的prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板身上的BasePanel组件
    private Stack<BasePanel> panelStack;

    private UIManager() 
    {
        ParseUIPanelTypeJson();
    }
    /// <summary>
    /// 把某个界面入栈并显示
    /// </summary>
    /// <param name="basePanel"></param>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        //判断一下栈里面是否有面板
        if(panelStack.Count > 0)
        {
            BasePanel basePanel = panelStack.Peek();
            basePanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        //Debug.Log("count=" + panelStack.Count);
        return panel;
        
    }
    /// <summary>
    /// 把某个界面出栈并显示
    /// </summary>
    /// <param name="basePanel"></param>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count == 0)
            return;

        BasePanel basePanel = panelStack.Peek();
        basePanel.OnExit();
        panelStack.Pop();

        if (panelStack.Count > 0)
            panelStack.Peek().OnResume();
    }

    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }
        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);

        BasePanel panel = panelDict.TryGet(panelType);

        if (panel == null)
        {
            string path = panelPathDict.TryGet(panelType) + "/" + panelType.ToString();
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(path),CanvasTransform);
            panel = go.GetComponent<BasePanel>();
            panelDict.Add(panelType, panel);
        }
        return panel;
    }

    [System.Serializable]
    private class UITypeJson
    {
        public List<UIPanelInfo> panelList;
    }
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();
        TextAsset asset = Resources.Load<TextAsset>("UIPanelType");

        UITypeJson typeJson = JsonUtility.FromJson<UITypeJson>(asset.text);
        
        foreach(UIPanelInfo infor in typeJson.panelList)
        {
            panelPathDict.Add(infor.panelType, infor.path);
        }
    }

    public void Test()
    {
        Debug.Log("length=" + panelPathDict.Count);
        foreach(KeyValuePair<UIPanelType,string> valuePair in panelPathDict)
        {
            Debug.Log("type=" + valuePair.Key + "   path=" + valuePair.Value);
        }
    }
}
