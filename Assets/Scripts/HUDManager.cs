using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI talkText;
    [SerializeField] private Animator talkPanelAnimator;
    [SerializeField] private Image portraitImage;
    [SerializeField] private Animator portraitAniamtor;
    private Sprite lastPortraitSprite = null; // 마지막으로 사용된 초상화 Sprite

    [SerializeField] private GameObject pausePanel;

    private QuestManager questManager;
    [SerializeField] private TextMeshProUGUI questNameText;

    private TypeEffector typeEffector;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        typeEffector = FindObjectOfType<TypeEffector>();
    }

    public void OpenTalkPanel()
    {
        talkPanelAnimator.SetBool("isShow", true);
    }

    public void CloseTalkPanel()
    {
        talkPanelAnimator.SetBool("isShow", false);
        lastPortraitSprite = null;
    }

    public void SetTalk(string text, Sprite portrait)
    {
        typeEffector.StartTextEffect(text);
        portraitImage.sprite = portrait;
        if ((lastPortraitSprite != null) && (lastPortraitSprite != portrait)) // 표정이 바뀌었으면 Animation 활성화
        {
            portraitAniamtor.SetTrigger("doEffect");
        }
        lastPortraitSprite = portrait;
    }

    public void SkipTalk()
    {
        typeEffector.SkipTextEffect();
    }

    public void SetPortraitActive(bool isNPC) // NPC일때만 Portrait 활성화
    {
        portraitImage.color = new Color(1f, 1f, 1f, (isNPC ? 1f : 0f));
    }

    public void SetPausePanel()
    {
        SetQuestNameText();

        // 켜져있으면 Off, 꺼져있으면 On
        pausePanel.SetActive((pausePanel.activeSelf ? false : true));
    }

    private void SetQuestNameText()
    {
        // 현재 진행 중인 퀘스트 이름을 표시
        questNameText.SetText(questManager.GetCurrentQuestName());
    }
}
