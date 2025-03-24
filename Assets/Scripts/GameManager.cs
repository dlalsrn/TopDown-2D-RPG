using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject scanObject;
    private ObjectData objData;

    [SerializeField] private TalkData talkData;
    private int talkIndex = 0;
    public bool IsInteract { get; private set; } // Object와 상호작용 중인가?
    public bool IsTalking { get; private set; } = false; // 말하는 중인가?

    private QuestManager questManager;

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
        //PlayerPrefs.DeleteAll(); // 게임 진행도 초기화
        questManager = FindObjectOfType<QuestManager>();
        GameLoad();
    }

    private void Update()
    {
        // Esc 버튼 누르면 Pasue Menu Control
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HUDManager.Instance.SetPausePanel();
        }
    }

    public void GetAction(GameObject scanObj)
    {
        if (!IsInteract) // 상호작용 중이 아니라면 대화창 On
        {
            IsInteract = true;
            talkIndex = 0;
            scanObject = scanObj;
            objData = scanObject.GetComponent<ObjectData>();
            HUDManager.Instance.OpenTalkPanel();
            HUDManager.Instance.SetPortraitActive(objData.IsNPC); // Image 활성화 유무
        }
        
        Talk(objData.Id, objData.IsNPC);
    }

    private void Talk(int id, bool isNPC)
    {
        if (IsTalking) // 대화 중에 한 번 더 누르면 대화 스킵
        {
            HUDManager.Instance.SkipTalk();
            return;
        }

        // Set Talk Data
        int questTalkIndex = questManager.GetQuestTalkIndex(); // 현재 진행 중인 Quest ID를 return
        string data = talkData.GetTalk(id + questTalkIndex, talkIndex); // "대화 Text : 초상화 Index"

        if (data == null) // 대화가 끝났다면
        {
            IsInteract = false;
            HUDManager.Instance.CloseTalkPanel();
            questManager.CheckQuest(id); // Quest 진행도 확인
            return;
        }
        else
        {
            if (isNPC)
            {
                // "대화 Text:초상화 Index" Split
                string talkText = data.Split(':')[0];
                int portraitIndex = int.Parse(data.Split(':')[1]);
                Sprite portrait = talkData.GetPortrait(id, portraitIndex);
                HUDManager.Instance.SetTalk(talkText, portrait);
            }
            else
            {
                HUDManager.Instance.SetTalk(data, null);
            }

            talkIndex++; // 다음 대화 Text
        }
    }

    public void SetIsTalking(bool isTalking)
    {
        IsTalking = isTalking;
    }

    public void GameContinue() // 계속하기 버튼
    {
        HUDManager.Instance.SetPausePanel();
    }

    public void GameSave() // 저장하기 버튼
    {
        // Player x, y 좌표
        Player player = FindObjectOfType<Player>();
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);

        // Quest Id
        // Quest Action Index
        (int, int) questInfo = questManager.GetQuestInfo(); // [quest Id, questActionIndex]
        int questId = questInfo.Item1;
        int questActionIndex = questInfo.Item2;
        PlayerPrefs.SetInt("QuestId", questId);
        PlayerPrefs.SetInt("QuestActionIndex", questActionIndex);

        PlayerPrefs.Save();
    }

    public void GameLoad()
    {
        // 세이브를 한 적이 없으면 Load X
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }

        Player player = FindObjectOfType<Player>();
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        player.transform.position = new Vector3(x, y, player.transform.position.z);

        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");
        questManager.SetQuestInfo(questId, questActionIndex);
        questManager.ControlObejct();
    }

    public void GameQuit() // 종료하기 버튼
    {
        Application.Quit();
    }
}
