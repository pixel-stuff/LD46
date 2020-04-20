using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
  [Header("Required")]
  [SerializeField] RectTransform rectTransform;
  [SerializeField] bool x;
  [SerializeField] bool y;

  [Header("Event")]
  [SerializeField] UnityEvent TimerStart;
  [SerializeField] UnityEvent TimerEnd;

  float timeCoroutStarted;
  Vector2 loadingStartScale;
  IEnumerator corout;

  public void Awake() {
    loadingStartScale = rectTransform.sizeDelta;
    rectTransform.gameObject.SetActive(false);
  }

  public void StartTimer(float waitForXSec = 10) {
    StopTimer(); // just to be sure ! 

    TimerStart.Invoke();
    rectTransform.gameObject.SetActive(true);
    timeCoroutStarted = Time.time;
    corout = TimerCorout(waitForXSec);
    StartCoroutine(corout);
  }

  public void StopTimer() {
    if(corout != null) {
      StopCoroutine(corout);
      corout = null;
    }
    rectTransform.localScale = Vector3.zero;
    rectTransform.gameObject.SetActive(false);
  }

  public void PauseTime() {
    if(corout != null) {
      StopCoroutine(corout);
      corout = null;
    }
  }

  IEnumerator TimerCorout(float waitForXSec) {
    Vector2 tmp = loadingStartScale;
    float currentTime;
    do {
      currentTime = Time.time - timeCoroutStarted;
      if(x) {
        tmp.x = loadingStartScale.x - loadingStartScale.x * currentTime / waitForXSec;
      }
      if(y) {
        tmp.y = loadingStartScale.y - loadingStartScale.y * currentTime / waitForXSec;
      }
      rectTransform.localScale = tmp;
      yield return new WaitForEndOfFrame();
      //Debug.Log("currenttime : " + currentTime + " / " + waitForXSec);
    } while(currentTime <= waitForXSec);

    rectTransform.localScale = Vector2.zero;
    rectTransform.gameObject.SetActive(false);
    corout = null;
    TimerEnd.Invoke();
  }

  public void DebugLog(string txt) {
    Debug.Log(txt);
  }
}
