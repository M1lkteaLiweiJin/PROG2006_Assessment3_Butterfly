using UnityEngine;
using UnityEngine.UI;

// 确保没有与Sprite重名的类或命名空间冲突
[RequireComponent(typeof(Button), typeof(Image))]
public class ButterflyFlyInteraction : MonoBehaviour
{
    [Header("图片配置")]
    [Tooltip("默认静止状态的蝴蝶图片（必须是Sprite类型）")]
    public UnityEngine.Sprite idleSprite;       // 明确指定UnityEngine命名空间的Sprite

    [Tooltip("点击后显示的飞舞状态图片（必须是Sprite类型）")]
    public UnityEngine.Sprite flySprite;        // 明确指定UnityEngine命名空间的Sprite

    [Tooltip("飞舞状态持续时间（秒）")]
    [Range(0.5f, 3f)] public float flyDuration = 1.5f;

    [Header("音效配置")]
    [Tooltip("翅膀扇动的音效")]
    public AudioClip flapSound;     // 翅膀音效

    [Header("飞舞动画参数")]
    [Tooltip("飞舞时的上下浮动幅度")]
    [Range(10f, 50f)] public float floatRange = 20f;

    // 组件引用
    private Image butterflyImage;
    private Button interactionButton;
    private AudioSource audioSource;
    private RectTransform butterflyRect;
    private Vector2 originalPosition;  // 初始位置（用于浮动动画）
    private bool isFlying = false;     // 飞舞状态标记

    void Awake()
    {
        // 获取UI组件（使用完整命名空间避免歧义）
        butterflyImage = GetComponent<UnityEngine.UI.Image>();
        interactionButton = GetComponent<UnityEngine.UI.Button>();
        butterflyRect = GetComponent<RectTransform>();

        // 记录初始位置
        originalPosition = butterflyRect.anchoredPosition;

        // 添加音频组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        // 初始化显示静止图片（添加空值判断）
        if (idleSprite != null)
        {
            butterflyImage.sprite = idleSprite;
        }
        else
        {
            Debug.LogError("请为idleSprite赋值（必须是Sprite类型素材）", this);
        }

        // 绑定点击事件
        interactionButton.onClick.AddListener(OnButterflyClicked);
    }

    /// <summary>
    /// 点击蝴蝶触发飞舞效果
    /// </summary>
    public void OnButterflyClicked()
    {
        if (isFlying) return;  // 正在飞舞时忽略点击

        // 播放翅膀音效
        if (flapSound != null)
        {
            audioSource.PlayOneShot(flapSound);
        }
        else
        {
            Debug.LogWarning("未赋值翅膀音效，可忽略此警告", this);
        }

        // 开始飞舞协程（切换图片+浮动动画）
        StartCoroutine(FlyAnimation());
    }

    /// <summary>
    /// 蝴蝶飞舞动画协程
    /// </summary>
    private System.Collections.IEnumerator FlyAnimation()
    {
        isFlying = true;
        float elapsedTime = 0f;

        // 切换到飞舞图片（添加空值判断）
        if (flySprite != null)
        {
            butterflyImage.sprite = flySprite;
        }
        else
        {
            Debug.LogError("请为flySprite赋值（必须是Sprite类型素材）", this);
            yield break; // 若素材缺失，终止协程
        }

        // 飞舞期间的浮动动画
        while (elapsedTime < flyDuration)
        {
            // 计算时间比例（0-1）
            float t = elapsedTime / flyDuration;

            // 用正弦函数实现上下浮动效果
            float yOffset = Mathf.Sin(t * Mathf.PI * 2) * floatRange;

            // 应用位置偏移
            butterflyRect.anchoredPosition = new Vector2(
                originalPosition.x,
                originalPosition.y + yOffset
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 恢复初始状态
        butterflyImage.sprite = idleSprite;
        butterflyRect.anchoredPosition = originalPosition;
        isFlying = false;
    }

    // 编辑器参数验证
    private void OnValidate()
    {
        if (flyDuration < 0.5f) flyDuration = 0.5f;
        if (floatRange < 10f) floatRange = 10f;
    }
}