using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PrologueDialog
{
    public Sprite image; // 이미지
    [TextArea(3, 10)]
    public string[] dialogs; // 자막 배열
}

public class PrologueController : MonoBehaviour
{
    public Image prologueImage; // 이미지 표시할 Image 컴포넌트
    public TextMeshProUGUI dialogText; // 자막 표시할 TextMeshPro - Text 컴포넌트
    public CanvasGroup fadeGroup; // 페이드 인/아웃 효과를 위한 Canvas Group 컴포넌트
    public PrologueDialog[] prologueDialogs; // 프롤로그에 사용될 이미지와 자막 배열
    public float typingSpeed = 0.05f; // 타이핑 속도
    public float fadeDuration = 1f; // 페이드 인/아웃 지속 시간

    private int currentImageIndex = 0; // 현재 이미지 인덱스
    private int currentDialogIndex = 0; // 현재 자막 인덱스
    private bool isTyping = false; // 자막 타이핑 중 여부
    private bool canProceed = false; // 다음 자막으로 진행할 수 있는지 여부

    void Start()
    {
        StartCoroutine(InitialDelayAndFadeIn()); // 검정 화면 유지 후 페이드 인 시작
    }

    void Update()
    {
        // 클릭 이벤트 감지
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            canProceed = true;
        }
    }

    IEnumerator InitialDelayAndFadeIn()
    {
        fadeGroup.alpha = 1; // 검정 화면 설정
        yield return new WaitForSeconds(1f); // 검정 화면을 1초 동안 유지

        prologueImage.sprite = prologueDialogs[currentImageIndex].image; // 첫 번째 이미지 설정
        dialogText.text = ""; // 자막 초기화
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 0); // 자막을 투명하게 설정
        yield return StartCoroutine(FadeIn()); // 페이드 인 효과
        StartCoroutine(PlayPrologue()); // 페이드 인 후 프롤로그 시작
    }

    IEnumerator PlayPrologue()
    {
        while (currentImageIndex < prologueDialogs.Length)
        {
            yield return StartCoroutine(DisplayDialogsForCurrentImage()); // 현재 이미지에 대한 자막 표시
            yield return new WaitForSeconds(1f); // 대사 끝나고 잠시 대기
            yield return StartCoroutine(FadeOut()); // 페이드 아웃 효과
            currentImageIndex++; // 다음 이미지로 이동
            if (currentImageIndex < prologueDialogs.Length)
            {
                prologueImage.sprite = prologueDialogs[currentImageIndex].image; // 다음 이미지 설정
                yield return StartCoroutine(FadeIn()); // 페이드 인 효과
            }
        }

        // 프롤로그 종료 후 메인 게임 씬으로 전환
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    IEnumerator DisplayDialogsForCurrentImage()
    {
        string[] dialogs = prologueDialogs[currentImageIndex].dialogs;
        for (currentDialogIndex = 0; currentDialogIndex < dialogs.Length; currentDialogIndex++)
        {
            canProceed = false;
            yield return StartCoroutine(TypeSentence(dialogs[currentDialogIndex])); // 현재 자막 타이핑 효과로 표시

            // 다음 자막으로 넘어가기 전 클릭을 기다림
            yield return new WaitUntil(() => canProceed);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = ""; // 자막 초기화
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 1); // 자막을 보이게 설정
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도 만큼 대기
        }
        isTyping = false;
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration)
        {
            fadeGroup.alpha = i; // 알파값 증가
            yield return null;
        }
        fadeGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeDuration)
        {
            fadeGroup.alpha = i; // 알파값 감소
            yield return null;
        }
        fadeGroup.alpha = 0;
    }
}
