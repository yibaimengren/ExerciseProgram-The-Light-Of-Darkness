using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class MainShotcut : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerClickHandler
{
    //快捷栏存储相关信息
    public KeyCode shotKey;
    public Image image;
    private ShotcutType type;
    private SkillInfo skillInfo;
    private SkillIcon skillIcon;
    private ObjectInfo objectInfo;
    private InventoryItem inventoryItem;
    private Image gridBackgound;
    //拖拽功能
    private GameObject skillDragItem;
    private Transform dragIconT;
    //角色信息
    private PlayerStatus playerStatus;
    private PlayerAttack playerAttack;
    public bool IsEmpty
    {
        get {
            if (type == ShotcutType.NONE) return true;
            return false;          
        }
    }
    void Awake()
    {
        skillDragItem = Resources.Load<GameObject>("SkillDragItem");
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
        image.gameObject.SetActive(false);
        gridBackgound = GetComponent<Image>();
    }

    void Update()
    {
        if (!isActive && type == ShotcutType.SKILL)//如果没有激活此快捷栏，就不能使用对应快捷功能
            return;

        if (Input.GetKeyDown(shotKey))//如果按下技能快捷键
        {
            UseItem();
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //如果在格子上按下鼠标右键，就使用药品或技能
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }

    }

    private void UseItem()
    {
        if (type == ShotcutType.OBJECT)//如果使用的是药品
        {
            playerStatus.AddHP(objectInfo.hp);
            playerStatus.AddMP(objectInfo.mp);
            InventoryManager.Instance.DeleteItem(objectInfo.id, 1);
            if (InventoryManager.Instance.GetNumById(objectInfo.id) <= 0)
                Clear();
        }

        if (type == ShotcutType.SKILL)//如果使用的是技能
        {
            playerAttack.UseSkill(skillInfo);
        }
    }

    /// <summary>
    /// 将快捷栏设为一个技能，
    /// </summary>
    /// /// <param name="skillIcon">SkillIcon</param>
    /// <param name="skill">技能信息</param>
    /// <param name="img">技能图标</param>
    public void SetSkill(SkillIcon skillIcon,SkillInfo skill,Image img,bool reSet = true)
    {
        type = ShotcutType.SKILL;
        skillInfo = skill;

        if (reSet && this.skillIcon)
        {
            this.skillIcon.ResetState();   
        }
        this.skillIcon = skillIcon;
        SetSprite(img.sprite);
    }
    /// <summary>
    /// 向快捷栏放入一个物品
    /// </summary>
    /// <param name="obj">物品信息</param>
    /// <param name="img">物品图标</param>
    public void SetObject(ObjectInfo obj,InventoryItem inventoryItem,Image img)
    {
        type = ShotcutType.OBJECT;
        objectInfo = obj;
        SetSprite(img.sprite);
        this.inventoryItem = inventoryItem;
    }
    /// <summary>
    /// 交换两个格子中存储的物体
    /// </summary>
    /// <param name="shotcut1"></param>
    /// <param name="shotcut2"></param>
    public void Swap(MainShotcut shotcut1, MainShotcut shotcut2)
    {
        //临时存储shotcut1中的信息
        ShotcutType shotcutType = shotcut1.type;
        SkillInfo skillInfo = shotcut1.skillInfo;
        SkillIcon skillIcon = shotcut1.skillIcon;
        ObjectInfo objectInfo = shotcut1.objectInfo;
        InventoryItem item = shotcut1.inventoryItem;
        Sprite sp = shotcut1.image.sprite;
        //将shotcut2中的信息存储到shotcut1中
        shotcut1.type = shotcut2.type;
        shotcut1.skillInfo = shotcut2.skillInfo;
        shotcut1.skillIcon = shotcut2.skillIcon;
        shotcut1.objectInfo = shotcut2.objectInfo;
        shotcut1.inventoryItem = shotcut2.inventoryItem;
        shotcut1.SetSprite(shotcut2.image.sprite);
        //将shotcut1中的信息存储到shotcut2中
        shotcut2.type = shotcutType;
        shotcut2.skillInfo = skillInfo;
        shotcut2.skillIcon = skillIcon;
        shotcut2.objectInfo = objectInfo;
        shotcut2.inventoryItem = item;

        shotcut2.SetSprite(sp);
        if(shotcut1.type == ShotcutType.NONE)
            shotcut1.image.gameObject.SetActive(false);
        if (shotcut2.type == ShotcutType.NONE)
            shotcut2.image.gameObject.SetActive(false);
    }

    private void SetSprite(Sprite sprite)
    {
        image.gameObject.SetActive(true);
        image.color = Color.white;
        image.sprite = sprite;
    }

    private bool isActive = true;
    /// <summary>
    /// 启用或禁用此快捷栏
    /// </summary>
    /// <param name="isActive"></param>
    public void SetShotcutActive(bool active)
    {
        //只有放置的是技能才需要启用或者禁用
        if(type == ShotcutType.SKILL)
        {
            if (active)
            {
                image.color = Color.white;
                gridBackgound.color = Color.white;
            }
            else
            {
                //将图片变灰色并且不可以点击
                image.color = Color.gray;
                gridBackgound.color = Color.gray;
            }
        }

        isActive = active;
    }

    public void Clear()
    {
        skillIcon = null;
        skillInfo = null;
        objectInfo = null;
        inventoryItem = null;
        image.gameObject.SetActive(false);
        type = ShotcutType.NONE;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconT)
        {
            dragIconT.position = Input.mousePosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       if(type != ShotcutType.NONE)
        {
            dragIconT = Instantiate(skillDragItem, transform.root).transform;
            dragIconT.position = Input.mousePosition;
            dragIconT.GetComponent<Image>().sprite = image.sprite;
            image.color = Color.gray;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (dragIconT)
        {
            Destroy(dragIconT.gameObject);
            RaycastResult result = eventData.pointerCurrentRaycast;
            if(result.gameObject )               
            {
                if (type== ShotcutType.SKILL && result.gameObject.CompareTag("SkiilIcon"))//如果拖回到了技能栏
                {
                    //重设技能栏图标状态
                    SkillIcon si = result.gameObject.GetComponent<SkillIcon>();
                    if (si == skillIcon)
                    {
                        skillIcon.ResetState();
                        //将快捷栏置空
                        skillIcon = null;
                        image.gameObject.SetActive(false);
                        type = ShotcutType.NONE;
                        return;
                    }
                }
                else if (type == ShotcutType.OBJECT)
                {
                    if(result.gameObject.CompareTag("InventoryGrid"))//如果放到背包的空格子上
                    {
                        inventoryItem.originParent.GetComponent<InventoryGrid>().Clear();
                        inventoryItem.originParent = result.gameObject.transform;
                        Clear();
                    }else if (result.gameObject.CompareTag("InventoryItem")) //如果放到了背包的其他物体上
                    {
                        result.gameObject.transform.parent.GetComponent<InventoryGrid>().Clear();
                        inventoryItem.originParent.GetComponent<InventoryGrid>().Clear();

                        Transform newParent = result.gameObject.transform.parent;
                        result.gameObject.GetComponent<InventoryItem>().SetParent(inventoryItem.originParent);
                        inventoryItem.originParent = newParent;
                        inventoryItem.SetParent(inventoryItem.originParent);
                        Clear();
                    }              
                }
                
                if (result.gameObject.CompareTag("SkillShotcutGrid")) //如果拖拽到了其他格子上
                {
                    MainShotcut mainShotcut = result.gameObject.GetComponent<MainShotcut>();
                    Swap(this, mainShotcut);                               
                }
            }
            image.color = Color.white;       
        }
    }

  
}

public enum ShotcutType
{
    NONE,
    SKILL,
    OBJECT
}


