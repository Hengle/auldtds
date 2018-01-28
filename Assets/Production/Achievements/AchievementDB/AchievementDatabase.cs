using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementDatabase : ScriptableObject
{

    
    public List<Achievement> database;

    private void OnEnable()
    {
        if (database == null)
        {
            database = new List<Achievement>();
        }
    }

    public void Add(Achievement achievement)
    {
        database.Add(achievement);
    }

    public void Remove (Achievement achievement)
    {
        database.Remove(achievement);
    }

    public void RemoveAt(int index)
    {
        database.RemoveAt(index);
    }

    public int COUNT
    {
        get
        {
            return database.Count;
        }
    }

    public Achievement achievement(int index)
    {
        return database.ElementAt(index);
    }

    public void SortAlphabeticallyAtoZ()
    {
        database.Sort((x, y) => string.Compare(x.achievementTitle, y.achievementTitle));
    }
}
