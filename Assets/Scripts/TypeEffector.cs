using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeEffector : MonoBehaviour
{
    private TextMeshProUGUI textField;
    [SerializeField] private GameObject endCursor;
    [SerializeField] private int charPerSeconds; // 초당 문자열 생성 개수

    private Coroutine textEffectCoroutine;

    private AudioSource audioSource;

    private string targetText;

    private void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SkipTextEffect()
    {
        StopCoroutine(textEffectCoroutine); // Effect Routine 종료
        textField.SetText(targetText);
        EndTextEffect();
    }

    public void StartTextEffect(string text)
    {
        GameManager.Instance.SetIsTalking(true);
        targetText = text;
        endCursor.SetActive(false);
        textField.SetText(""); // Text Field 초기화
        textEffectCoroutine = StartCoroutine(TextEffectRoutine(text));
    }

    IEnumerator TextEffectRoutine(string text)
    {
        yield return new WaitForSeconds(0.2f);

        for (int textIndex = 0; textIndex < text.Length; textIndex++)
        {
            textField.text += text[textIndex];

            // 글자마다 효과음 재생
            if (text[textIndex] != ' ' && text[textIndex] != '.')
            {
                audioSource.Play();
            }

            // 추가한 문자가 공백일 시 다음 문자로, 공백이 아니면 기다림
            yield return new WaitForSeconds((text[textIndex] == ' ' ? 0f : (1f / charPerSeconds)));
        }

        EndTextEffect();
    }

    private void EndTextEffect()
    {
        endCursor.SetActive(true);
        GameManager.Instance.SetIsTalking(false);
    }
}
