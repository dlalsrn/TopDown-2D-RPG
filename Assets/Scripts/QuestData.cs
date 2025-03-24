public class QuestData
{
    public string QuestName { get; private set; }
    public int[] NpcId { get; private set; }

    public QuestData(string questName, int[] npcId)
    {
        QuestName = questName;
        NpcId = npcId;
    }
}
