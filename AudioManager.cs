using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 拖拽背景音乐源到这里（在场景中创建AudioSource播放背景音乐）
    public AudioSource backgroundMusic;

    // 控制全局音效开关
    public void SetGlobalSound(bool isOn)
    {
        // 控制背景音乐
        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isOn;
        }

        // 如果有其他音效（如按钮点击音），在这里统一控制
        // 例如：所有音效的AudioSource静音状态同步为!isOn
    }
    void Start()
    {
        // 启动时自动播放音乐（默认开启）
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }
}