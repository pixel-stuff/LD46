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
  Vector2 loadingStartSize;
  IEnumerator corout;

  public void Awake() {
    loadingStartSize = rectTransform.sizeDelta;
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
    rectTransform.sizeDelta = Vector2.zero;
    rectTransform.gameObject.SetActive(false);
  }

  public void PauseTime() {
    if(corout != null) {
      StopCoroutine(corout);
      corout = null;
    }
  }

  IEnumerator TimerCorout(float waitForXSec) {
    Vector2 tmp = loadingStartSize;
    float currentTime;
    do {
      currentTime = Time.time - timeCoroutStarted;
      if(x) {
        tmp.x = loadingStartSize.x - loadingStartSize.x * currentTime / waitForXSec;
      }
      if(y) {
        tmp.y = loadingStartSize.y - loadingStartSize.y * currentTime / waitForXSec;
      }
      rectTransform.sizeDelta = tmp;
      yield return new WaitForSeconds(Time.deltaTime);
      //Debug.Log("currenttime : " + currentTime + " / " + waitForXSec);
    } while(currentTime <= waitForXSec);

    rectTransform.sizeDelta = Vector2.zero;
    rectTransform.gameObject.SetActive(false);
    corout = null;
    TimerEnd.Invoke();
  }

  public void DebugLog(string txt) {
    Debug.Log(txt);
  }
}
