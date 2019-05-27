using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string STRENGTH_1 = "strength1";
    const string STRENGTH_1_INDEX = "strength1index";
    const string STRENGTH_2 = "strength2";
    const string STRENGTH_2_INDEX = "strength2index";
    const string STRENGTH_3 = "strength3";
    const string STRENGTH_3_INDEX = "strength3index";
    const string STRENGTH_4 = "strength4";
    const string STRENGTH_4_INDEX = "strength4index";
    const string STRENGTH_5 = "strength5";
    const string STRENGTH_5_INDEX = "strength5index";

    const string ACTIVITY_1 = "activity1";
    const string ACTIVITY_2 = "activity2";
    const string ACTIVITY_3 = "activity3";
    const string ACTIVITY_4 = "activity4";
    const string ACTIVITY_5 = "activity5";
    const string ACTIVITY_6 = "activity6";
    const string ACTIVITY_7 = "activity7";
    const string CURRENT_ACTIVITY = "CurrentActivity";
    const string CURRENT_ACTIVITY_TO_STRENGTH_INDEX = "CurrentActivityStrengthIndex";

    const string ACTIVITY_1_STRENGTH_INDEX = "activity1StrengthIndex";
    const string ACTIVITY_2_STRENGTH_INDEX = "activity2StrengthIndex";
    const string ACTIVITY_3_STRENGTH_INDEX = "activity3StrengthIndex";
    const string ACTIVITY_4_STRENGTH_INDEX = "activity4StrengthIndex";
    const string ACTIVITY_5_STRENGTH_INDEX = "activity5StrengthIndex";
    const string ACTIVITY_6_STRENGTH_INDEX = "activity6StrengthIndex";
    const string ACTIVITY_7_STRENGTH_INDEX = "activity7StrengthIndex";

    const string STRENGTH_1_COUNTER = "strength1Counter";
    const string STRENGTH_2_COUNTER = "strength2Counter";
    const string STRENGTH_3_COUNTER = "strength3Counter";
    const string STRENGTH_4_COUNTER = "strength4Counter";
    const string STRENGTH_5_COUNTER = "strength5Counter";

    private ActivityManager activityManager;


    private void Start()
    {
        activityManager = GetComponent<ActivityManager>();
    }

    public void SetStrengths ()
    {
        for (int i = 0; i <5; i++)
        {
            PlayerPrefs.SetString("strength" + i.ToString(), activityManager.myStrengthList[i]);
            PlayerPrefs.SetInt("strength" + i.ToString() + "index", activityManager.strengthIndexesList[i]);
        }
    }

    public string GetStrengths (int strengthIndexInList)
    {
        return PlayerPrefs.GetString("strength" + strengthIndexInList.ToString());
    }

    public int GetStrengthsIndexes(int strengthIndexInList)
    {
        return PlayerPrefs.GetInt("strength" + strengthIndexInList.ToString() + "index");
    }

    public void SetActivity (int activityIndex)
    {
        PlayerPrefs.SetString("activity" + activityIndex.ToString(), activityManager.myActivityList[activityIndex]);
        PlayerPrefs.SetInt("activity" + activityIndex.ToString() + "StrengthIndex", activityManager.activityToStrengthIndexHolder[activityIndex]);
    }

    public void ClearActivity (int activityIndex)
    {
        PlayerPrefs.SetString("activity" + activityIndex.ToString(), "Empty");
        PlayerPrefs.SetString("activity" + activityIndex.ToString() + "StrengthIndex", "Empty");
    }

    public string GetActivity(int activityIndex)
    {
       return PlayerPrefs.GetString("activity" + activityIndex.ToString());
    }

    public int GetActivityIndex(int activityIndex)
    {
        return PlayerPrefs.GetInt("activity" + activityIndex.ToString() + "StrengthIndex");
    }

    public void SetCurrentActivity (string currentActivity, int currentActivityStrengthIndex)
    {
        PlayerPrefs.SetString("CurrentActivity", currentActivity);
        PlayerPrefs.SetInt("CurrentActivityStrengthIndex", currentActivityStrengthIndex);
    }

    public string GetCurrentActivity ()
    {
        return PlayerPrefs.GetString("CurrentActivity");
    }

    public int GetCurrentActivityIndex()
    {
        return PlayerPrefs.GetInt("CurrentActivityStrengthIndex");
    }


    public void SetStrengthCounter(int strengthIndex, int currentStrengthCounter)
    {
        PlayerPrefs.SetInt("strength" + strengthIndex.ToString() + "Counter", currentStrengthCounter);
    }

    public int GetStrengthCounter(int strengthIndex)
    {
        return PlayerPrefs.GetInt("strength" + strengthIndex.ToString() + "Counter");
    }


}
