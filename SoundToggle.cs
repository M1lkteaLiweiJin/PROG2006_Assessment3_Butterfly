using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public bool isSoundOn = true;
    public Image soundIcon; // 需手动赋值：按钮上的Image组件（显示图标用）
    public UnityEngine.Sprite soundOnSprite;
    public UnityEngine.Sprite soundOffSprite;

    private Button soundButton; // 按钮组件

    void Start()
    {
        // 自动获取按钮组件
        soundButton = GetComponent<Button>();
        if (soundButton == null)
        {
            Debug.LogError("Btn_Sound上没有Button组件！");
            return;
        }

        // 强制绑定点击事件（无需手动在Inspector设置）
        soundButton.onClick.AddListener(ToggleSound);

        // 初始化图标状态
        UpdateSoundIcon();
    }

    // 切换音效开关状态
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        UpdateSoundIcon();

        // 调用AudioManager控制全局音效（确保场景中有AudioManager）
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetGlobalSound(isSoundOn);
        }
        else
        {
            Debug.LogWarning("场景中未找到AudioManager！");
        }
    }

    // 更新图标显示
    private void UpdateSoundIcon()
    {
        if (soundIcon != null)
        {
            soundIcon.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
        else
        {
            Debug.LogError("未赋值soundIcon！");
        }
    }
}