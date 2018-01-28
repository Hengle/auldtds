using UnityEngine;
using UnityEditor;
using System.Collections;
using System;



public class AchievementDatabaseEditor : EditorWindow
{
    private enum State
    {
        BLANK,
        EDIT,
        ADD
    }

    private enum Categories
    {
        GENERAL,
        SURVIVAL,
        SCENARIO,
        CAMPAIGN
    }

    private State state;
    private Categories achievementCategory;
    private int selectedAchievement;
    private string newAchievementTitle;
    private string newAchievementDescription;
    private int newAchievementScore;
    private bool newAchievementSingleGame;
    
    public achievementType newAchievementType;
    public float newAchievementTimeCounter;

    public string newAchievementMinionName;
    public int newAchievementMinionCount;


    private const string DATABASE_PATH = @""+"Assets/Production/Achievements/AchievementDB/AchievementDatabase.asset";

    private AchievementDatabase achievements;
    private Vector2 _scrollPos;

    [MenuItem("Triodin/Database/Achievement Database %#v")]
    public static void Init()
    {
        AchievementDatabaseEditor window = EditorWindow.GetWindow<AchievementDatabaseEditor>();
        window.minSize = new Vector2(800, 400);
        window.Show();
    }

    void OnEnable()
    {
        if (achievements == null)
            LoadDatabase();

        state = State.BLANK;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUI.skin.button.focused.textColor = Color.red;
        DisplayListArea();
        DisplayMainArea();
        EditorGUILayout.EndHorizontal();
    }

    void LoadDatabase()
    {
        achievements = (AchievementDatabase)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(AchievementDatabase));

        if (achievements == null)
            CreateDatabase();
    }

    void CreateDatabase()
    {
        achievements = ScriptableObject.CreateInstance<AchievementDatabase>();
        AssetDatabase.CreateAsset(achievements, DATABASE_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void DisplayListArea()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        EditorGUILayout.Space();

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, "box", GUILayout.ExpandHeight(true));

        for (int cnt = 0; cnt < achievements.COUNT; cnt++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                achievements.RemoveAt(cnt);
                achievements.SortAlphabeticallyAtoZ();
                EditorUtility.SetDirty(achievements);
                state = State.BLANK;
                return;
            }

            if (GUILayout.Button(achievements.achievement(cnt).achievementTitle +" - ("+
                achievements.achievement(cnt).achievementScore + ") " + "", GUILayout.ExpandWidth(true)))
            {
                selectedAchievement = cnt;
                state = State.EDIT;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Achievements: " + achievements.COUNT, GUILayout.Width(100));

        if (GUILayout.Button("New Achievement"))
            state = State.ADD;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    void DisplayMainArea()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        switch (state)
        {
            case State.ADD:
                DisplayAddMainArea();
                break;
            case State.EDIT:
                DisplayEditMainArea();
                break;
            default:
                DisplayBlankMainArea();
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }

    void DisplayBlankMainArea()
    {
        EditorGUILayout.LabelField(
            "There are 3 things that can be displayed here.\n" +
            "1) Achievement info for editing\n" +
            "2) Blanck fields for adding a new Achievement\n" +
            "3) Blank Area",
            GUILayout.ExpandHeight(true));
    }

    void DisplayEditMainArea()
    {
        achievements.achievement(selectedAchievement).achievementTitle = 
            EditorGUILayout.TextField(new GUIContent("Title: "), achievements.achievement(selectedAchievement).achievementTitle);
        achievements.achievement(selectedAchievement).achievementDescription = 
            EditorGUILayout.TextField(new GUIContent("Description: "), achievements.achievement(selectedAchievement).achievementDescription);
        achievements.achievement(selectedAchievement).achievementScore = 
            int.Parse(EditorGUILayout.TextField(new GUIContent("Score: "), achievements.achievement(selectedAchievement).achievementScore.ToString()));
   
        achievements.achievement(selectedAchievement).achieveType =
            (achievementType)EditorGUILayout.EnumPopup("Achievement Type: ", achievements.achievement(selectedAchievement).achieveType);
   
        EditorGUILayout.Space();
        achievements.achievement(selectedAchievement).achieveTimeSurvive =
            Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Time to survive (Minutes): "), achievements.achievement(selectedAchievement).achieveTimeSurvive.ToString()));
        achievements.achievement(selectedAchievement).achieveMinionToKill =
            EditorGUILayout.TextField(new GUIContent("Minion Name to monitor:"), achievements.achievement(selectedAchievement).achieveMinionToKill);
        achievements.achievement(selectedAchievement).achieveMinionToKillCount = 
            Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Minion Quantity: "), achievements.achievement(selectedAchievement).achieveMinionToKillCount.ToString()));
        achievements.achievement(selectedAchievement).achievementSingleGame =
            GUILayout.Toggle(achievements.achievement(selectedAchievement).achievementSingleGame, "Applicable in single game only? ");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            achievements.SortAlphabeticallyAtoZ();
            EditorUtility.SetDirty(achievements);
            state = State.BLANK;
        }
    }

    void DisplayAddMainArea()
    {
        newAchievementTitle = EditorGUILayout.TextField(new GUIContent("Title: "), newAchievementTitle);
        newAchievementDescription = EditorGUILayout.TextField(new GUIContent("Description: "), newAchievementDescription);
        newAchievementScore = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Score: "), newAchievementScore.ToString()));
        newAchievementType = (achievementType)EditorGUILayout.EnumPopup("Achievement Type: ", newAchievementType);
        //newAchievementSingleGame = EditorGUILayout.Toggle(true, "Available for a single Game");
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        newAchievementTimeCounter = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Time to survive(Minutes): "), newAchievementTimeCounter.ToString()));
        newAchievementMinionName = EditorGUILayout.TextField(new GUIContent("Minion Name to monitor:"), newAchievementMinionName);
        newAchievementMinionCount = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Minion Quantity: "), newAchievementMinionCount.ToString()));

        newAchievementSingleGame = GUILayout.Toggle(true, "Applicable in single game only? ");

        EditorGUILayout.Space();

        if (GUILayout.Button("Done", GUILayout.Width(100)))
        {
            achievements.Add(new Achievement(
                                newAchievementTitle,
                                newAchievementDescription,
                                newAchievementScore,
                                newAchievementSingleGame,
                                newAchievementType,
                                newAchievementTimeCounter,
                                newAchievementMinionName,
                                newAchievementMinionCount)
                );
            achievements.SortAlphabeticallyAtoZ();

            newAchievementTitle = string.Empty;
            newAchievementDescription = string.Empty;
            newAchievementScore = 0;
            EditorUtility.SetDirty(achievements);
            state = State.BLANK;
        }
    }

    void SetAchievementCases()
    {
        Debug.Log("Setting Achievements "+ achievements.achievement(selectedAchievement).achieveType);
        switch (achievements.achievement(selectedAchievement).achieveType)
        {
            case achievementType.TIME_SURVIVE:
                // EditorGUILayout.BeginVertical();
                //EditorGUILayout.Space();
                //newAchievementTimeCounter = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Time: "), newAchievementTimeCounter.ToString()));
                new GUIContent("TEST");
                Debug.Log("TIME_SURVIVE is SET -1");
                //EditorGUILayout.EndVertical();
                break;
            case achievementType.MINION_KILLS:
                break;
            case achievementType.NO_SPELL_USE:
                break;
            case achievementType.TOTAL_KILLS:
                break;
            case achievementType.NO_ITEM_USE:
                break;
            case achievementType.NO_UNIT_USE:
                break;
            default:
                break;
        }
    }
}