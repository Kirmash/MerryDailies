using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingController : MonoBehaviour {

    private int onbStepTracker = 1;

    private float delayTracker;
    private bool isDelayLaunched;

    private UIHandler uiHandler;
    public Animator onbAnimator;

	// Use this for initialization
	void Start () {
        uiHandler = GameObject.Find("Canvas").GetComponent<UIHandler>();
	}

    private void Update()
    {
        if (isDelayLaunched)
        {
            delayTracker += Time.deltaTime;
        }

        if (isDelayLaunched && delayTracker > 0.5f)
        {
            isDelayLaunched = false;
            delayTracker = 0f;
        }
    }

    public void AnimatorStepUp()
    {
        if (!isDelayLaunched)
        {
            onbStepTracker++;
            onbAnimator.SetInteger("OnbStep", onbStepTracker);
            isDelayLaunched = true;
        }
    }

    public void AnimatorStepDown()
    {
        if (!isDelayLaunched)
        {
        onbStepTracker--;
        onbAnimator.SetInteger("OnbStep", onbStepTracker);
        isDelayLaunched = true;
        }
    }

    public void SetOnboardingInactive()
    {
        uiHandler.SetOnboardingCanvasInactive();   
    }

    public void CleanUp()
    {
        onbStepTracker = 1;
        onbAnimator.SetInteger("OnbStep", onbStepTracker);
    }
}
