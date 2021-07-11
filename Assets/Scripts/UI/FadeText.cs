using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    private float fadeTime = 0.5f;
    private Text text;
    private float alpha = 1;
    private Transform followTarget;
    private float moveSpeed = 0.5f;
    private float offset = 10;
    private float timer = 0;
    void Awake()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        Fade();
        Follow();
    }

    public void Initial(Transform target,string text,Color color,float fadeTime)
    {
        this.text.text = text;
        this.text.color = color;
        this.fadeTime = fadeTime;
        this.followTarget = target;
    }
    private void Fade()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        timer += Time.deltaTime;
        alpha = 1 - timer / fadeTime;
        if (alpha < 0)
            Destroy(this.gameObject);
    }

    private void Follow()
    {
        if (followTarget == null)
        {
            Destroy(this.gameObject);
            return;
        }          

        Vector3 target = Camera.main.WorldToScreenPoint(followTarget.position);
        transform.position = new Vector3(target.x, target.y + offset, target.z);
        offset += moveSpeed;

    }
}
