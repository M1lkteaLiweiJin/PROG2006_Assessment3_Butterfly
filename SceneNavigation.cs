using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneNavigation : MonoBehaviour
{
    [Header("目标场景名称（必须与Build Settings中一致）")]
    public string targetSceneName; // 目标场景名称（需手动赋值）
    public Button targetButton; // 绑定当前按钮（用于控制启用/禁用）

    void Start()
    {
        // 初始禁用下一页按钮（符合评估“交互式导航事件”要求，需完成当前页交互后激活）
        if (targetButton != null && targetSceneName.Contains("Stage"))
        {
            targetButton.interactable = false;
        }
    }

    // 按钮点击触发跳转（需绑定到Button的OnClick事件）
    public void OnButtonClick()
    {
        // 校验场景名称是否有效
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("目标场景名称未赋值！请在Inspector中填写正确场景名");
            return;
        }

        // 异步加载场景（避免黑屏卡顿，提升用户体验）
        StartCoroutine(LoadSceneAsync());
    }

    // 异步加载场景函数
    IEnumerator LoadSceneAsync()
    {
        // 显示加载提示（可选，增强用户体验）
        GameObject loadingText = new GameObject("LoadingText");
        Text text = loadingText.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = "加载中...";
        text.fontSize = 36;
        text.alignment = TextAnchor.MiddleCenter;
        text.rectTransform.sizeDelta = new Vector2(200, 50);
        text.transform.SetParent(GameObject.Find("Canvas").transform, false);

        // 异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        asyncLoad.allowSceneActivation = false; // 先不激活场景

        // 等待加载进度达到90%
        while (asyncLoad.progress < 0.9f)
        {
            text.text = $"加载中...{Mathf.Round(asyncLoad.progress * 100)}%";
            yield return null;
        }

        // 进度达标后激活场景
        text.text = "即将进入...";
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
    }

    // 外部调用：激活按钮（如完成当前页交互后启用下一页）
    public void EnableButton()
    {
        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
    }
}