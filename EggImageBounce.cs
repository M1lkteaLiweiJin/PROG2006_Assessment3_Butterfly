using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EggImageBounce : MonoBehaviour
{
    [Header("交互资源配置（需手动赋值）")]
    public AudioClip bounceSound; // 弹起音效
    public Button nextButton; // 下一页按钮

    private bool isInteracted = false;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        if (nextButton != null) nextButton.interactable = false;
    }

    public void OnEggImageClick()
    {
        if (!isInteracted)
        {
            StartCoroutine(PlayBounceAnimation());
            PlayBounceSoundEffect();
            ActivateNextButton();
            isInteracted = true;
        }
    }

    IEnumerator PlayBounceAnimation()
    {
        transform.localPosition = originalPos + new Vector3(0, 50, 0);
        yield return new WaitForSeconds(0.2f);
        transform.localPosition = originalPos;
    }

    void PlayBounceSoundEffect()
    {
        if (bounceSound != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = bounceSound;
            audioSource.volume = 0.5f;
            audioSource.Play();
            Destroy(audioSource, bounceSound.length);
        }
    }

    void ActivateNextButton()
    {
        if (nextButton != null)
            nextButton.interactable = true;
    }
}