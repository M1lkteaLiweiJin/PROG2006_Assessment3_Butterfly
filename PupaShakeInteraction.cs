using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class PupaShakeInteraction : MonoBehaviour
{
    [Header("晃动参数")]
    [Tooltip("左右晃动的最大角度（度）")]
    [Range(5f, 30f)] public float shakeAngle = 10f;

    [Tooltip("晃动一次的总时长（秒）")]
    [Range(0.5f, 2f)] public float shakeDuration = 1f;

    [Tooltip("晃动的频率（次数/秒）")]
    [Range(2f, 10f)] public float shakeFrequency = 5f;

    [Header("音效设置")]
    [Tooltip("晃动时的音效（如蛹壳摩擦声）")]
    public AudioClip shakeSound;

    // 组件引用
    private Image pupaImage;
    private Button interactionButton;
    private AudioSource audioSource;
    private RectTransform pupaRect; // 用于控制位置旋转
    private bool isShaking = false; // 晃动状态标记

    void Awake()
    {
        // 获取组件（使用完整命名空间避免冲突）
        pupaImage = GetComponent<UnityEngine.UI.Image>();
        interactionButton = GetComponent<UnityEngine.UI.Button>();
        pupaRect = GetComponent<RectTransform>();

        // 添加音频组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        // 绑定点击事件
        interactionButton.onClick.AddListener(OnPupaClicked);
    }

    /// <summary>
    /// 点击蛹触发晃动
    /// </summary>
    public void OnPupaClicked()
    {
        if (isShaking) return; // 正在晃动时忽略点击

        // 播放晃动音效
        if (shakeSound != null)
        {
            audioSource.PlayOneShot(shakeSound);
        }

        // 开始晃动协程
        StartCoroutine(ShakePupa());
    }

    /// <summary>
    /// 蛹左右晃动的协程（实现平滑动画）
    /// </summary>
    private System.Collections.IEnumerator ShakePupa()
    {
        isShaking = true;
        float elapsedTime = 0f;
        float startRotation = pupaRect.localEulerAngles.z; // 初始旋转角度

        while (elapsedTime < shakeDuration)
        {
            // 计算当前时间比例（0-1）
            float t = elapsedTime / shakeDuration;

            // 使用正弦函数计算旋转角度（实现左右摇摆）
            float rotation = Mathf.Sin(t * Mathf.PI * 2 * shakeFrequency) * shakeAngle;

            // 应用旋转
            pupaRect.localEulerAngles = new Vector3(0, 0, startRotation + rotation);

            // 累加时间
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 晃动结束后恢复初始角度
        pupaRect.localEulerAngles = new Vector3(0, 0, startRotation);
        isShaking = false;
    }

    // 编辑器模式下验证参数
    private void OnValidate()
    {
        // 确保参数在合理范围内
        if (shakeAngle < 5f) shakeAngle = 5f;
        if (shakeDuration < 0.5f) shakeDuration = 0.5f;
        if (shakeFrequency < 2f) shakeFrequency = 2f;
    }
}