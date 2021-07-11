UI框架使用方法：
1.将面板prefab放在一个Resources文件夹内
2.将所有面板的名字和路径存放进UIFramework/Resources文件夹下的UIPanelType.json中
3.将每个面板prefab的名字加入UIFramework/UIPanel文件夹下的UIPanelType枚举类型中
4.每个面板根节点需要添加CanvasGrounp组件
5.每个面板根节点需要添加一个继承自BasePanel的自定义脚本，可以重写BasePansel的四个状态函数来修改显示/暂停/恢复/关闭面板时的效果，
比如插入DoTween动画
6.把UIFrameworkRoot挂在Canvas上

脚本：
1.如果想要显示面板（入栈），就调用 UIManager.Instance.PushPanel(panelType);
2.如果想要关闭面板（出栈），就调用 UIManager.Instance.PopPanel();

