using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAnim : MonoBehaviour
{
    // 拖入你的动画剪辑
    public AnimationClip clickAnim;
    private Animation anim;

    void Start()
    {
        // 获取Animation组件
        anim = GetComponent<Animation>();
        // 如果没有Animation组件，自动添加
        if (anim == null) anim = gameObject.AddComponent<Animation>();
        // 将动画添加到Animation组件
        anim.AddClip(clickAnim, "ClickAnim");
        // 绑定按钮点击事件
        GetComponent<Button>().onClick.AddListener(PlayClickAnim);
    }

    // 播放动画的方法
    void PlayClickAnim()
    {
        anim.Play("ClickAnim");
    }
}