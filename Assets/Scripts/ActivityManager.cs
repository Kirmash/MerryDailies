using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmartLocalization;
using UnityEngine.EventSystems;

public class ActivityManager : MonoBehaviour {

    public Text activityText; // text of the current activity

    //StrengthNames, should be of the same order as the buttons on FTUE sequence!
    private string[] strengthNamesArray = new string[] { "Appreciation of Beauty and Excellence", "Bravery", "Creativity", "Curiosity",
        "Fairness, Equity and Justice", "Forgiveness", "Gratitude", "Honesty", "Hope", "Humility / Modesty", "Humor", "Judgement", "Kindness", "Leadership", "Love", "Love of learning",
        "Perseverance", "Perspective", "Prudence", "Self-Regulation", "Social Intelligence", "Spirituality", "Teamwork", "Zest"};

    private int strengthInitializeCounter = 1;

    private int majorStrengthsArraySize = 5; //maximum number of major person's strengths
   
    [HideInInspector]
    public List<string> myStrengthList; //list of alrady selected player's strengths
    private string strenghtInit; //name of randomly selected strength
    private int randomStrengthIndex; //index for the random strengths generation

    [HideInInspector]
    public List<int> strengthIndexesList; //list of major strength indexes for 

    public Text[] strengthsNames;

    //Indexes of Strengths:
    private List<int> activityCountInStrengthList;
    private string activityInit;

    //ActivitiesCounterArrayHolder
    private int[] activitiesPerformedCounters;


    //activity Init parameters
    private int activtyCount = 7;
    private int activityTop1Count = 3;
    private int activity15Count = 2;

    [HideInInspector]
    public List<string> myActivityList;
    private List<int> myActivityIndexesList;


    //holds Strength indexes relative to the activity
    [HideInInspector] public List<int> activityToStrengthIndexHolder;
    private int currentActivityStrengthIndex;

    private int indexHolder;
   // public GameObject profileHolderCanvas;

    //FTUE parameters 
    const string FTUE_FINISHED = "FTUE_finished"; //marker for finishing the FTUE (selection of strengths)
    private bool isFTUEFinished;
    private PlayerPrefsManager playerPrefsManager;
    public Text fTUEHeader;
    private bool strengthButtonIsPressed = false;
    public Text[] FTUEStrengthsNames;
    public Image[] FTUEStrengthImagesHolders;
    public GameObject strengthSelectionHolder;

    private UIHandler uiHandler;

    public Text[] strengthCounterTexts;

    private Sprite[] strengthImagesArray;
    public Image[] strengthIcons;

    public Image currentActivityIcon;

    public Image[] strengthImagesHoldersForSelector;
    public GameObject[] strengthProfileCanvasHolder;

    // Use this for initialization
    void Start () {
        activityCountInStrengthList = new List<int>();
        myStrengthList = new List<string>();
        myActivityList = new List<string>();
        strengthIndexesList = new List<int>();
        activityToStrengthIndexHolder = new List<int>();

        playerPrefsManager = GetComponent<PlayerPrefsManager>();
        uiHandler = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<UIHandler>();

        myActivityIndexesList = new List<int>();
        activitiesPerformedCounters = new int[majorStrengthsArraySize];

        strengthImagesArray = Resources.LoadAll<Sprite>("StrengthsImages");

        //Initialize activity array counter
        for (int i = 0; i < strengthNamesArray.Length; i++)
        {
            while (LanguageManager.Instance.HasKey(strengthNamesArray[i] + "_" + (strengthInitializeCounter + 1).ToString()))
            {       
                strengthInitializeCounter++;
            }       
            activityCountInStrengthList.Add(strengthInitializeCounter);
            strengthInitializeCounter = 1;
        }

        //Initialize strength list
            if ( PlayerPrefs.GetString(FTUE_FINISHED) == "True")
        {
            LoadSavedStrengthsActivities();   
            
        } else
        {
         //   FlushStrengths();
            uiHandler.FirstLaunch();
        }   
    }

    private void Update()
    {
      //  Debug.Log(myActivityList.Count);
    }

    public void GenerateActivities ()
    {
        // Fill 3 top 3 activities
        //code not ready for Localization
        for (int j = 0; j < activityTop1Count; j++)
        {
            indexHolder = Random.Range(1, activityCountInStrengthList[strengthIndexesList[0]]+1);
           
            activityInit = LanguageManager.Instance.GetTextValue(myStrengthList[0] + "_" + (indexHolder).ToString());
            
            while (myActivityList.Contains(activityInit))
            {
                indexHolder = Random.Range(1, activityCountInStrengthList[strengthIndexesList[0]]+1);
                activityInit = LanguageManager.Instance.GetTextValue(myStrengthList[0] + "_" + (indexHolder).ToString());
            }
            myActivityList.Add(activityInit);
            activityToStrengthIndexHolder.Add(0);
        }

        // Fill 1 top 2-3 activities 
        indexHolder = Random.Range(1, 3);
        myActivityList.Add(LanguageManager.Instance.GetTextValue(myStrengthList[indexHolder] + "_" + (Random.Range(1, activityCountInStrengthList[strengthIndexesList[indexHolder]]+1)).ToString()));
        activityToStrengthIndexHolder.Add(indexHolder);

        // Fill 1 top 4-5 activities
        indexHolder = Random.Range(3, 5);
        myActivityList.Add(LanguageManager.Instance.GetTextValue(myStrengthList[indexHolder] + "_" + (Random.Range(1, activityCountInStrengthList[strengthIndexesList[indexHolder]]+1)).ToString()));
        activityToStrengthIndexHolder.Add(indexHolder);

        //Fill 2 top 1-5 activities
        for (int j = 0; j < activity15Count; j++)
        {
            int strengthRandomIndexHolder = Random.Range(0, 5);
            activityInit = LanguageManager.Instance.GetTextValue(myStrengthList[strengthRandomIndexHolder] + "_" + (Random.Range(1, activityCountInStrengthList[strengthIndexesList[strengthRandomIndexHolder]]+1)).ToString());
            while (myActivityList.Contains(activityInit))
            {
                strengthRandomIndexHolder = Random.Range(0, 5);
                activityInit = LanguageManager.Instance.GetTextValue(myStrengthList[strengthRandomIndexHolder] + "_" + (Random.Range(1, activityCountInStrengthList[strengthIndexesList[strengthRandomIndexHolder]]+1)).ToString());
            }
            myActivityList.Add(activityInit);
            activityToStrengthIndexHolder.Add(strengthRandomIndexHolder);
        }

        //save activities in playerprefs
        for (int i = 0; i < myActivityList.Count; i++)
        {
            playerPrefsManager.SetActivity(i);
        }

    }

    //update the currently selected activity on screen
    public void UpdateActivity ()
    {
        if (myActivityList.Count == 0) GenerateActivities();
        indexHolder = Random.Range(0, myActivityList.Count); 
        string currentActivity = myActivityList[indexHolder];
        activityText.text = currentActivity;
        currentActivityStrengthIndex = activityToStrengthIndexHolder[indexHolder];
        myActivityList.RemoveAt(indexHolder);
//     Debug.Log(myActivityList.Count);
        activityToStrengthIndexHolder.RemoveAt(indexHolder);
        playerPrefsManager.ClearActivity(indexHolder);
        playerPrefsManager.SetCurrentActivity(currentActivity, currentActivityStrengthIndex);
        currentActivityIcon.sprite = strengthImagesArray[strengthIndexesList[currentActivityStrengthIndex]];
    }

    public void CompleteActivity ()
    {
        activitiesPerformedCounters[currentActivityStrengthIndex]++;
        UpdateActivityCounterText(currentActivityStrengthIndex);
        playerPrefsManager.SetStrengthCounter(currentActivityStrengthIndex, activitiesPerformedCounters[currentActivityStrengthIndex]);
    }

    //public void GenerateStrengths()
    //{
    //    myStrengthList.Clear();
    //    strengthIndexesList.Clear();

    //    for (int j = 0; j < majorStrengthsArraySize; j++)
    //    {
    //        indexHolder = Random.Range(0, strengthNamesArray.Length); //pick random index of strengths in array
    //        strenghtInit = strengthNamesArray[indexHolder]; //find random strengths in Strength array
    //        while (myStrengthList.Contains(strenghtInit))  //if this strengths is already in Strength list - repeat search
    //        {
    //            indexHolder = Random.Range(0, strengthNamesArray.Length);
    //            strenghtInit = strengthNamesArray[indexHolder];
    //        }
    //        myStrengthList.Add(strenghtInit); //okay, found the new strength. Add it to the list.
    //        strengthsNames[j].text = strenghtInit;
    //        strengthIndexesList.Add(indexHolder);
    //    }

    //    playerPrefsManager.SetStrengths();
    //    GenerateActivities();
    //    UpdateActivity();
    //    PlayerPrefs.SetString("FTUE_finished", "True");
    //}

    public void FlushStrengths()
    {
        PlayerPrefs.SetString("FTUE_finished", "False");
        myStrengthList.Clear();
        strengthIndexesList.Clear();
        myActivityList.Clear();
        Debug.Log(myActivityList.Count);
        activityToStrengthIndexHolder.Clear();
        for (int i=0; i <strengthSelectionHolder.transform.childCount; i++)
        {
            if (!strengthSelectionHolder.transform.GetChild(i).gameObject.activeSelf)
                strengthSelectionHolder.transform.GetChild(i).gameObject.SetActive(true);
        }
        uiHandler.FlushStrengths();
        
        fTUEHeader.text = "Select strength # " + (myStrengthList.Count + 1).ToString();

        for (int i =0; i<activitiesPerformedCounters.Length; i++)
        {
            activitiesPerformedCounters[i] = 0;
            UpdateActivityCounterText(i);
            playerPrefsManager.SetStrengthCounter(i, activitiesPerformedCounters[i]);
        }
        for (int i = 0; i < strengthImagesHoldersForSelector.Length; i++)
        {
            strengthImagesHoldersForSelector[i].transform.parent.gameObject.GetComponent<Animator>().Play("Str" + (i+1).ToString() + "AnimationIdle");
        }
    }

    public void LaunchFTUE()
    {
        uiHandler.LaunchFTUE();
        fTUEHeader.text = "Select strength # " + (myStrengthList.Count + 1).ToString();
    }

    private void LoadSavedStrengthsActivities()
    {
        uiHandler.tutorialCanvas.SetActive(false);
        uiHandler.currentScreenState = "Main";
        for (int i = 0; i < majorStrengthsArraySize; i++)
        {
            myStrengthList.Add(playerPrefsManager.GetStrengths(i));
            strengthsNames[i].text = playerPrefsManager.GetStrengths(i);
            strengthIndexesList.Add(playerPrefsManager.GetStrengthsIndexes(i));
            activitiesPerformedCounters[i] = playerPrefsManager.GetStrengthCounter(i);  
            UpdateActivityCounterText(i);
            LayoutRebuilder.ForceRebuildLayoutImmediate(strengthProfileCanvasHolder[i].GetComponent<RectTransform>());

            for (int j = 0; j < strengthNamesArray.Length; j++)
            {
                if (strengthNamesArray[j] == strengthsNames[i].text)
                {
                    strengthIcons[i].sprite = strengthImagesArray[j];            
                }
            }
           
        }
        for (int i = 0; i <activtyCount; i++ )
        {
            if (playerPrefsManager.GetActivity(i) != "Empty")
            {
                myActivityList.Add(playerPrefsManager.GetActivity(i));
                activityToStrengthIndexHolder.Add(playerPrefsManager.GetActivityIndex(i));
            }
          //  Debug.Log(myActivityList.Count);
            activityText.text = playerPrefsManager.GetCurrentActivity();
            currentActivityStrengthIndex = playerPrefsManager.GetCurrentActivityIndex();
            currentActivityIcon.sprite = strengthImagesArray[strengthIndexesList[currentActivityStrengthIndex]];
        }


}


    public void PressStrenghtButtonInFTUE ()
    {
      //  fTUEHeader.text = "Select strength # " + (myStrengthList.Count).ToString();
        indexHolder = int.Parse(EventSystem.current.currentSelectedGameObject.name.Substring(0, 2)) - 1; //gets the index in strengthNamesArray of the pressed button
        myStrengthList.Add(strengthNamesArray[indexHolder]); //add this Strength to list
       // Debug.Log(myStrengthList.Count);
        strengthsNames[myStrengthList.Count-1].text = strengthNamesArray[indexHolder];
        strengthIndexesList.Add(indexHolder);

        strengthImagesHoldersForSelector[myStrengthList.Count - 1].sprite = strengthImagesArray[strengthIndexesList[myStrengthList.Count - 1]];
        strengthImagesHoldersForSelector[myStrengthList.Count - 1].transform.parent.gameObject.GetComponent<Animator>().Play("Str" + myStrengthList.Count.ToString() + "Animation");

      //  strengthImagesHoldersForSelector[myStrengthList.Count - 1].rectTransform.localScale = new Vector3(1f, 1f);
        //strengthImagesHoldersForSelector[myStrengthList.Count - 1].transform.parent.gameObject.GetComponent<Image>().enabled = false;

        EventSystem.current.currentSelectedGameObject.SetActive(false);
        if (myStrengthList.Count < 5)
        {
            fTUEHeader.text = "Select strength # " + (myStrengthList.Count + 1).ToString();
        } else
        {
            fTUEHeader.text = "Done!"; 
        }

        if (myStrengthList.Count == majorStrengthsArraySize)
        {
          //  LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformOfTheParentLayoutGroup);
            FinishFTUE();
        }
    }

    private void FinishFTUE()
    {
       // Debug.Log(strengthIcons.Length);
        for (int i = 0; i < myStrengthList.Count; i++)
        {           
            FTUEStrengthsNames[i].text = strengthsNames[i].text;                    
            FTUEStrengthImagesHolders[i].sprite = strengthImagesArray[strengthIndexesList[i]];
            strengthIcons[i].sprite = strengthImagesArray[strengthIndexesList[i]];
            UpdateActivityCounterText(i);
        }
        uiHandler.FinishFTUE();
    }

    public void ApplyFTUEResults()
    {
        PlayerPrefs.SetString("FTUE_finished", "True");
        playerPrefsManager.SetStrengths();
        GenerateActivities();
        UpdateActivity();
        uiHandler.ApplyFTUEResults();
    }

    private void UpdateActivityCounterText(int activityIndex)
    {
        if (activitiesPerformedCounters[activityIndex] == 0)
        {
            strengthCounterTexts[activityIndex].text = "You haven't done such activity yet.";
        } else
        {
            strengthCounterTexts[activityIndex].text = "Activities done: " + activitiesPerformedCounters[activityIndex].ToString();
        }
        
    }

    
}
