using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static GameConstants.Constants;

public class TalkData : MonoBehaviour
{
    private Dictionary<int, string[]> talkData; // (id, 대사)
    private Dictionary<int, Sprite> portraitData; // 초상화 Data
    [SerializeField] private Sprite[] portraits;
    
    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    private void GenerateData()
    {
        GenerateCommonData();
        GenerateQuestData();
        GeneratePortratitData();
    }

    private void GenerateCommonData() // 일반 대화 생성
    {
        // LUNA
        talkData.Add(LUNA, new string[] { $"안녕?:{IDLE}",
                                          $"나는 Luna라고해.:{TALK}",
                                          $"처음 보는 얼굴인데 이 곳에 처음 왔구나?:{SMILE}" });

        // LUDO
        talkData.Add(LUDO, new string[] { $"여어 처음 보는 얼굴이네.:{IDLE}",
                                          $"이 호수 정말 아름답지?:{TALK}",
                                          $"사실 이 호수에는 숨겨진 비밀이 있다고:{TALK}" });

        // BOX
        talkData.Add(BOX, new string[] { "평범한 나무상자다." });

        // DESK
        talkData.Add(DESK, new string[] { "누군가 사용했던 흔적이 있다." });
    }

    private void GenerateQuestData() // 퀘스트 대화 생성
    {
        // LUNA
        talkData.Add(LUNA + 10, new string[] { $"어서 와.:{IDLE}",
                                               $"이 마을에 놀라운 전설이 있다는데 혹시 알고 있어?:{TALK}",
                                               $"오른쪽 호수로 가면 루도가 알려줄거야.:{IDLE}" });
        
        talkData.Add(LUNA + 20, new string[] { $"루도의 동전?:{IDLE}",
                                               $"루도는 또 동전을 흘리고 다닌단 말이야?:{ANGRY}",
                                               $"나중에 한마디 해줘야겠어.:{ANGRY}" });
        // LUDO
        talkData.Add(LUDO + 11, new string[] { $"여어.:{IDLE}",
                                               $"호수의 비밀을 들으러 온거야?:{TALK}",
                                               $"공짜로 들을 수는 없고, 일 좀 하나 해주면 좋을텐데..:{TALK}",
                                               $"혹시 내 집 근처에 떨어진 동전 좀 주워서 갖다줄 수 있어?:{TALK}",
                                               $"참고로 내 집은 분홍색이야:{SMILE}" });

        talkData.Add(LUDO + 20, new string[] { $"찾으면 꼭 좀 가져다 줘..:{IDLE}" });

        talkData.Add(LUDO + 21, new string[] { $"찾아줘서 정말 고마워!:{SMILE}" });

        // COIN
        talkData.Add(COIN + 20, new string[] { "누군가 흘린 동전 같다." });
    }   

    private void GeneratePortratitData()
    {
        portraitData.Add(LUDO + ANGRY, portraits[0]); // Angry
        portraitData.Add(LUDO + IDLE, portraits[1]); // Idle
        portraitData.Add(LUDO + SMILE, portraits[2]); // Smile
        portraitData.Add(LUDO + TALK, portraits[3]); // Talk

        portraitData.Add(LUNA + ANGRY, portraits[4]); // Angry
        portraitData.Add(LUNA + IDLE, portraits[5]); // Idle
        portraitData.Add(LUNA + SMILE, portraits[6]); // Smile
        portraitData.Add(LUNA + TALK, portraits[7]); // Talk
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id)) // 만약 id에 해당하는 대화가 존재하지 않는다면
        {
            if (talkData.ContainsKey(id - id % 10)) // Quest Line에 해당 Object가 존재하면
            {
                // 퀘스트 맨 처음 대사 return
                id -= id % 10;
            }
            else
            {
                // 퀘스트에 포함되지 않은 Object면 기본 대사
                id -= id % 100;
            }
        }

        if (talkIndex == talkData[id].Length) // 대화가 끝났다면
        {
            return null;
        }
        else // 대화가 끝나지 않았다면
        {
            return talkData[id][talkIndex];
        }
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
