using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainManager : MonoBehaviour
{

    private static GameMainManager instance;
    public static GameMainManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameMainManager>();
            }
            return instance;
        }
    }

    public enum gameDifficulty { Easy, Normal, Nightmare };
    [Header("Game Basic Settings")]
    public gameDifficulty _gameDifficulty;
    public int _gameMaxLevels;
    public int _gameMaxHeroUnits;
    [Space]
    [Header("Player Info")]
    public string _profileName;
    public string _profileEmail;
    public string _profilePUID;
    public int _playerCurrentLevel = 1;
    public int _playerCurrentLevelXP;
    public int _playerNextLevelXP;
    public List<int> _gameXPLevels = new List<int>();
    [Space]
    [Header("Player Scores")]
    public int _minionsKilled;
    public int _bossesDefeated;
    public int _totalTimePlayed;
    public float _levelTimePlayed;
    [Space]
    [Header("Player Treasury")]
    public int _treasureGold;
    public int _treasureMithril;
    public int _treasureIlluminum;
    public float _totalRoomTreasure;
    [Space]
    [Header("Game Variables")]
    public bool showHealthBars;


    private void Start()
    {
        SetLevelLadder();
    }

    private void Update()
    {
        SetXPtoLevel();
    }

    public void SetLevelLadder()
    {
        for (int i = 0; i < _gameMaxLevels; i++)
        {
            if (i == 0)
            {
                _playerCurrentLevelXP = 0;
                int xpToLevelUp = 1000;
                _gameXPLevels.Add(xpToLevelUp);
            }
            else
            {
                int currentXP = _gameXPLevels[i - 1];
                int xpToLevelUp = (currentXP + (currentXP / 2));
                _gameXPLevels.Add(xpToLevelUp);
            }
        }
        _playerNextLevelXP = _gameXPLevels[_playerCurrentLevel - 1];
        XPMan.Instance.currentXP = _playerCurrentLevelXP;
        XPMan.Instance.nextLevelXP = _playerNextLevelXP;
    }

    private void SetXPtoLevel()
    {
        _playerNextLevelXP = _gameXPLevels[_playerCurrentLevel - 1];
    }
}
