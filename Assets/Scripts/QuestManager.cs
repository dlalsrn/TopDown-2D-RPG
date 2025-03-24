using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static GameConstants.Constants;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questList; // Quest ID, Quest Data

    [SerializeField] private int questId;
    private int questActionIndex = 0;

    [SerializeField] private GameObject[] questObject;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    private void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기.", new int[] { LUNA, LUDO }));
        questList.Add(20, new QuestData("루도의 동전 찾아주기", new int[] { COIN, LUDO }));
        questList.Add(30, new QuestData("퀘스트 올 클리어!", new int[] {}));
    }

    public int GetQuestTalkIndex() // Object ID
    {
        return questId + questActionIndex;
    }

    public void CheckQuest(int id = 0) // Object ID, id 값을 안 넣으면 0으로 세팅
    {
        if (questList[questId].NpcId.Length != 0)
        {
            if (id == questList[questId].NpcId[questActionIndex]) // 현재 상호작용한 Object의 ID가 퀘스트 진행과 알맞다면
            {
                questActionIndex++; // 다음 Object와 대화 가능
            }
            
            if (questActionIndex == questList[questId].NpcId.Length) // Quest가 끝났다면
            {
                NextQuest(); // 다음 Quest로 설정
            }

            ControlObejct(); // Control Qeust Object
        }
    }

    private void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObejct()
    {
        switch (questId)
        {
            case 10:
                
                break;
            case 20:
                if (questActionIndex == 0) // 퀘스트 시작 시
                {
                    questObject[0].SetActive(true);
                }
                if (questActionIndex == 1) // 동전을 먹었다면
                {
                    questObject[0].SetActive(false);
                }
                break;
        }
    }

    public string GetCurrentQuestName()
    {
        return questList[questId].QuestName;
    }

    public (int, int) GetQuestInfo() // 현재 퀘스트 정보 return (퀘스트 Id, 퀘스트 진행도)
    {
        return (questId, questActionIndex);
    }

    public void SetQuestInfo(int questId, int questActionIndex)
    {
        this.questId = questId;
        this.questActionIndex = questActionIndex;
    }
}
