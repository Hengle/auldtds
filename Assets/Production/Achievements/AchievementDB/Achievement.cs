using UnityEngine;

public enum achievementType
{
    TIME_SURVIVE,
    MINION_KILLS,
    NO_SPELL_USE,
    TOTAL_KILLS,
    NO_ITEM_USE,
    NO_UNIT_USE
};

[System.Serializable]
public class Achievement
{
    
    [SerializeField]
    public string achievementTitle;
    [SerializeField]
    public string achievementDescription;
    [SerializeField]
    public int achievementScore;
    [SerializeField]
    public bool achievementSingleGame;
    [SerializeField]
    public achievementType achieveType;

    [SerializeField]
    public float achieveTimeSurvive;

    [SerializeField]
    public string achieveMinionToKill;
    [SerializeField]
    public int achieveMinionToKillCount;


    public Achievement (string title, string description, int score,
        bool singleGame, achievementType achType, float achTimeSurvive, string achMinionToKill, int achMinionCount)
    {
        achievementTitle = title;
        achievementDescription = description;
        achievementScore = score;
        achievementSingleGame = singleGame;
        achieveType = achType;
        achieveTimeSurvive = achTimeSurvive;
        achieveMinionToKill = achMinionToKill;
        achieveMinionToKillCount = achMinionCount;
        /*
        achType = achieveType;
        achTimeSurvive = achieveTimeSurvive;
        achMinionToKill = achieveMinionToKill;
        achMinionCount = achieveMinionToKillCount;*/
    }

}
