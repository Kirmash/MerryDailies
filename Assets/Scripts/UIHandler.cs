using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {


    public GameObject profileResetApply;
    public GameObject tutorialCanvas;
    public GameObject ftueCanvas;
    public GameObject ftueFinishedCanvas;
    public GameObject profileHolderCanvas;

    public GameObject uiBlocker;
    public GameObject uiPopupBlocker;

    public GameObject activityManager;

    private Animator animator;
    private bool isDelayLaunched;
    private float delayTracker;
    private float delayTimeHolder;

    private float animationLength;

    public Animator onboardingAnimator;
    public Animator finishFTUEAnimator;
    public Animator profileAnimator;
    public Animator profileApplyAnimator;
    public Animator UIPopupBlockerAnimator;
    public Animator UIBlockerAnimator;
    public Animator selectorInfoPopupAnimator;

    //list of states { "Ftue", "StrengthApprove", "Main", "ProfileOpen", "ProfileFlushApprove", "SelectorInfoPopup" }

    [HideInInspector] public string currentScreenState;
    public OnboardingController onbController;
    private bool isQuittingStarted = false;
    private float quittingTime = 1.0f;
    public Image backButtonApply;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckForDelay();
        BackKeyCheckFlow();
    }

    public void OpenProfile ()
    {
        if (!isDelayLaunched)
        {
            uiBlocker.SetActive(true);
            profileAnimator.Play("AnimationAppear");
            currentScreenState = "ProfileOpen";
            delayTimeHolder = 0.5f;
            isDelayLaunched = true;
        }
    }

    public void CloseProfile()
    {
        if (!isDelayLaunched)
        {
            profileAnimator.Play("ProfileClose");
            if (uiBlocker.activeSelf)
            {
                UIBlockerAnimator.Play("UIBlockerGentleDisable");
                Invoke("SetUIBlockersDisable", 0.8f);
            }
            currentScreenState = "Main";
            delayTimeHolder = 0.5f;
            isDelayLaunched = true;
        }
        
    }

    public void ProfileResetOpen ()
    {
        if (!isDelayLaunched)
        {
            profileApplyAnimator.Play("ProfileResetActive");
            uiPopupBlocker.SetActive(true);
            currentScreenState = "ProfileFlushApprove";
            delayTimeHolder = 0.5f;
            isDelayLaunched = true;
        }
    }

    public void ProfileResetClose()
    {
        if (!isDelayLaunched)
        {
            profileApplyAnimator.Play("ProfileResetDoNotReset");
            if (uiPopupBlocker.activeSelf)
            {
                UIPopupBlockerAnimator.Play("GentleDisable");
                Invoke("SetUIPopupBlockersDisable", 0.8f);
            }
            currentScreenState = "ProfileOpen";
            delayTimeHolder = 0.8f;
            isDelayLaunched = true;
        }
    }

    public void SelectorInfoOpen()
    {
        if (!isDelayLaunched)
        {
            selectorInfoPopupAnimator.Play("SelectorInfoPopupSetActive");
            uiPopupBlocker.SetActive(true);
            currentScreenState = "SelectorInfoPopup";
            delayTimeHolder = 0.5f;
            isDelayLaunched = true;
        }
    }

    public void SelectorInfoClose()
    {
        if (!isDelayLaunched)
        {
            selectorInfoPopupAnimator.Play("SelectorInfoPopupDisable");
            if (uiPopupBlocker.activeSelf)
            {
                UIPopupBlockerAnimator.Play("GentleDisable");
                Invoke("SetUIPopupBlockersDisable", 0.8f);
            }
            currentScreenState = "Main";
            delayTimeHolder = 0.8f;
            isDelayLaunched = true;
        }
    }


    public void FlushStrengths()
    {
         if (!isDelayLaunched)
        {
            tutorialCanvas.SetActive(false);
            ftueCanvas.SetActive(true);
            uiBlocker.SetActive(true);

            if (uiPopupBlocker.activeSelf)
            {
                UIPopupBlockerAnimator.Play("GentleDisable");
                Invoke("SetUIPopupBlockersDisable", 0.8f);
            }

            if (finishFTUEAnimator.GetCurrentAnimatorStateInfo(0).IsName("FTUE_finished_appear_idle"))
            {
                // Debug.Log("Playing CorrectState");
                finishFTUEAnimator.Play("FTUE_finished_disappear");
                Invoke("SetStrengthSelectionApplyInactive", 0.8f);
            }

            if (profileApplyAnimator.GetCurrentAnimatorStateInfo(0).IsName("ProfileResetIdleActive"))
            {
                // Debug.Log("Playing CorrectState");
                profileApplyAnimator.Play("ProfileResetReset");
                profileAnimator.Play("ProfileReset");
            }
            currentScreenState = "Main";
            delayTimeHolder = 0.8f;
            isDelayLaunched = true;
        }    
    }

    public void LaunchFTUE()
    {
        if (!isDelayLaunched)
        {
            ftueCanvas.SetActive(true);
            onboardingAnimator.Play("OnboardingFinished");
            currentScreenState = "Main";
            delayTimeHolder = 0.5f;
            isDelayLaunched = true;
        }
    }

    public void SetOnboardingCanvasInactive ()
    {
        tutorialCanvas.SetActive(false);
    }

    private void SetStrengthSelectorInactive ()
    {
        ftueCanvas.SetActive(false);
    }

    private void SetStrengthSelectionApplyInactive ()
    {
        ftueFinishedCanvas.SetActive(false);
    }

    private void SetUIBlockersDisable()
    {
            uiBlocker.SetActive(false);
            uiBlocker.GetComponent<Image>().color = new Color(0, 0, 0, 0.7f);
    }

    private void SetUIPopupBlockersDisable()
    {
            uiPopupBlocker.SetActive(false);
            uiPopupBlocker.GetComponent<Image>().color = new Color(0, 0, 0, 0.7f);
    }

    public void FirstLaunch()
    {
        tutorialCanvas.SetActive(true);
        ftueCanvas.SetActive(false);
        uiBlocker.SetActive(true);
        currentScreenState = "Ftue";
    }

    public void FinishFTUE ()
    {
        ftueFinishedCanvas.SetActive(true);
        finishFTUEAnimator.Play("FTUE_finished_appear");
        Invoke("SetStrengthSelectorInactive", 0.5f);
        currentScreenState = "StrengthApprove";
    }

    public void ApplyFTUEResults()
    {
        if (!isDelayLaunched)
        {
            finishFTUEAnimator.Play("FTUE_finished_apply");
            Invoke("SetStrengthSelectionApplyInactive", 1f);
            if (uiBlocker.activeSelf)
            {
                UIBlockerAnimator.Play("UIBlockerGentleDisable");
                Invoke("SetUIBlockersDisable", 0.8f);
            }
            currentScreenState = "Main";
            isDelayLaunched = true;
        }    
    }

    public void LaunchSkippingAnimation ()
    {
        if (!isDelayLaunched)
        {
            animator.Play("TaskSkip");
            delayTimeHolder = 1.1f;
            isDelayLaunched = true;
        }
    }

    public void SkipActivity ()
    {
            activityManager.GetComponent<ActivityManager>().UpdateActivity();
    }

    public void LaunchCompletionAnimation()
    {
        if (!isDelayLaunched)
        {
            isDelayLaunched = true;
            animator.Play("TaskComplete");
            delayTimeHolder = 2f;
        }
  }

    public void CompleteActivity ()
    {  
            activityManager.GetComponent<ActivityManager>().CompleteActivity();
            activityManager.GetComponent<ActivityManager>().UpdateActivity();
    }

    public void GoToWebsite ()
    {
        Application.OpenURL("https://www.viacharacter.org/survey/account/register");
    }

    private void CheckForDelay()
    {
        if (isDelayLaunched && delayTracker <= delayTimeHolder)
        {
            delayTracker += Time.deltaTime;
        }

        if (isDelayLaunched && delayTracker > delayTimeHolder)
        {
            isDelayLaunched = false;
            delayTracker = 0f;
        }
    }

    private void BackKeyCheckFlow()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isDelayLaunched)
        {
            if (currentScreenState == "ProfileOpen")
            {
                CloseProfile();
            }
            else if (currentScreenState == "ProfileFlushApprove")
            {
                ProfileResetClose();
            }
            else if (currentScreenState == "SelectorInfoPopup")
            {
                SelectorInfoClose();
            }
            else if (currentScreenState == "StrengthApprove")
            {
                activityManager.GetComponent<ActivityManager>().FlushStrengths();
            }
            else if (currentScreenState == "Main" && !isQuittingStarted)
            {
                Debug.Log("Quitting start");
                isQuittingStarted = true;
                backButtonApply.GetComponent<Animator>().Play("BackApplyAppear");
                StartCoroutine(QuittingTimer());
            }

            else if (currentScreenState == "Ftue")
            {
                if (onbController.onbAnimator.GetInteger("OnbStep") == 1 && !isQuittingStarted)
                {
                    isQuittingStarted = true;
                    backButtonApply.GetComponent<Animator>().Play("BackApplyAppear");
                    StartCoroutine(QuittingTimer());
                }
                else
                {
                    onbController.AnimatorStepDown();
                }
            }
        }
    }

    IEnumerator QuittingTimer()
    {
        yield return null;
        float counter = 0f;
        while (counter < quittingTime)
        {
            counter += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            yield return null;
        }

        isQuittingStarted = false;
        backButtonApply.GetComponent<Animator>().Play("BackApplyDisappear");
    }

}
