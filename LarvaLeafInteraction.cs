using UnityEngine;
using UnityEngine.UI;

// 强制要求组件，避免缺失依赖
[RequireComponent(typeof(Button), typeof(AudioSource), typeof(Image))]
public class LarvaLeafInteraction : MonoBehaviour
{
    [Header("图片设置")]
    [Tooltip("默认显示的完整叶子图片（必须赋值）")]
    public UnityEngine.Sprite defaultLeafSprite;  // 明确指定Unity的Sprite类型

    [Tooltip("点击后显示的带咬痕叶子图片（必须赋值）")]
    public UnityEngine.Sprite eatenLeafSprite;   // 明确指定Unity的Sprite类型

    [Header("音效设置")]
    [Tooltip("咀嚼叶子的音效文件")]
    public AudioClip chewSoundEffect;

    [Header("交互参数")]
    [Tooltip("咬痕图片显示的持续时间（秒）")]
    [Range(0.5f, 3f)] public float displayDuration = 1.5f;

    // 组件引用
    private Image leafImageComponent;
    private Button interactionButtonComponent;
    private AudioSource audioSourceComponent;
    private bool isInteractionActive = true;  // 控制点击有效性

    void Awake()
    {
        // 获取组件（使用完整命名空间避免冲突）
        leafImageComponent = GetComponent<UnityEngine.UI.Image>();
        interactionButtonComponent = GetComponent<UnityEngine.UI.Button>();
        audioSourceComponent = GetComponent<UnityEngine.AudioSource>();

        // 初始化音频设置
        audioSourceComponent.playOnAwake = false;
        audioSourceComponent.loop = false;
    }

    void Start()
    {
        // 初始显示完整叶子
        if (defaultLeafSprite != null)
        {
            leafImageComponent.sprite = defaultLeafSprite;
        }
        else
        {
            Debug.LogError("请在Inspector中为defaultLeafSprite赋值！", this);
        }

        // 绑定点击事件
        interactionButtonComponent.onClick.AddListener(OnLeafClicked);
    }

    /// <summary>
    /// 叶子被点击时的交互逻辑
    /// </summary>
    public void OnLeafClicked()
    {
        // 防止重复点击
        if (!isInteractionActive) return;

        // 播放咀嚼音效
        if (chewSoundEffect != null)
        {
            audioSourceComponent.PlayOneShot(chewSoundEffect);
        }
        else
        {
            Debug.LogWarning("未赋值咀嚼音效，请在Inspector中添加！", this);
        }

        // 显示咬痕图片
        if (eatenLeafSprite != null)
        {
            leafImageComponent.sprite = eatenLeafSprite;
        }
        else
        {
            Debug.LogError("请在Inspector中为eatenLeafSprite赋值！", this);
        }

        // 暂时禁用交互，延迟恢复
        isInteractionActive = false;
        Invoke(nameof(RestoreLeafState), displayDuration);
    }

    /// <summary>
    /// 恢复叶子默认状态
    /// </summary>
    private void RestoreLeafState()
    {
        leafImageComponent.sprite = defaultLeafSprite;
        isInteractionActive = true;  // 重新激活交互
    }

    // 编辑器模式下验证参数有效性
    private void OnValidate()
    {
        // 确保显示时间为有效值
        if (displayDuration < 0.5f) displayDuration = 0.5f;
    }
}