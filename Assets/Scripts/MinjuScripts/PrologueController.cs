using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrologueController : MonoBehaviour
{
    public Image prologueImage; // 이미지 표시할 Image 컴포넌트
    public TextMeshProUGUI dialogText; // 자막 표시할 TextMeshPro - Text 컴포넌트
    public CanvasGroup fadeGroup; // 페이드 인/아웃 효과를 위한 Canvas Group 컴포넌트
    public Sprite[] prologueImages; // 프롤로그에 사용될 이미지 배열
    public string[] prologueDialogs; // 프롤로그에 사용될 자막 배열
    public float typingSpeed = 0.05f; // 타이핑 속도

    private int currentIndex = 0; // 현재 인덱스

    void Start()
    {
        StartCoroutine(InitialDelayAndFadeIn()); // 검정 화면 유지 후 페이드 인 시작
    }

    IEnumerator InitialDelayAndFadeIn()
    {
        fadeGroup.alpha = 0; // 검정 화면 설정
        yield return new WaitForSeconds(1f); // 검정 화면을 2초 동안 유지

        prologueImage.sprite = prologueImages[0]; // 첫 번째 이미지 설정
        dialogText.text = ""; // 자막 초기화
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 0); // 자막을 투명하게 설정
        yield return StartCoroutine(FadeIn()); // 페이드 인 효과
        StartCoroutine(PlayPrologue()); // 페이드 인 후 프롤로그 시작
    }

    IEnumerator PlayPrologue()
    {
        for (currentIndex = 0; currentIndex < prologueImages.Length; currentIndex++)
        {
            prologueImage.sprite = prologueImages[currentIndex]; // 현재 이미지 설정
            yield return StartCoroutine(TypeSentence(prologueDialogs[currentIndex])); // 현재 자막 타이핑 효과로 표시
            yield return new WaitForSeconds(2); // 대사 끝나고 잠시 대기

        }

        // 프롤로그 종료 후 메인 게임 씬으로 전환
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = ""; // 자막 초기화
        dialogText.color = new Color(dialogText.color.r, dialogText.color.g, dialogText.color.b, 1); // 자막을 보이게 설정
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도 만큼 대기
        }
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 4f; // 페이드 인 지속 시간 (길게 조정)
        for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration)
        {
            fadeGroup.alpha = i; // 알파값 증가
            yield return null;
        }
        fadeGroup.alpha = 1;
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            fadeGroup.alpha = i; // 알파값 감소
            yield return null;
        }
        fadeGroup.alpha = 0;
    }
}
