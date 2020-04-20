using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
  [Header("Required")]
  [SerializeField] Transform transform;
  [SerializeField] bool x;
  [SerializeField] bool y;

  [Header("Event")]
  [SerializeField] UnityEvent TimerStart;
  [SerializeField] UnityEvent TimerEnd;

  float timeCoroutStarted;
  Vector3 loadingStartScale;
  IEnumerator corout;

  public void Awake() {
    loadingStartScale = transform.localScale;
    transform.gameObject.SetActive(false);
  }

  public void StartTimer(float waitForXSec = 10) {
    StopTimer(); // just to be sure ! 

    TimerStart.Invoke();
    transform.gameObject.SetActive(true);
    timeCoroutStarted = Time.time;
    corout = TimerCorout(waitForXSec);
    StartCoroutine(corout);
  }

  public void StopTimer() {
    if(corout != null) {
      StopCoroutine(corout);
      corout = null;
    }
    transform.localScale = Vector3.zero;
    transform.gameObject.SetActive(false);
  }

  IEnumerator TimerCorout(float waitForXSec) {
    Vector3 tmp = loadingStartScale;
    float currentTime;
    do {
      currentTime = Time.time - timeCoroutStarted;
      if(x) {
        tmp.x = loadingStartScale.x - loadingStartScale.x * currentTime / waitForXSec;
      }
      if(y) {
        tmp.y = loadingStartScale.y - loadingStartScale.y * currentTime / waitForXSec;
      }
      transform.localScale = tmp;
      yield return new WaitForEndOfFrame();
      //Debug.Log("currenttime : " + currentTime + " / " + waitForXSec);
    } while(currentTime <= waitForXSec);

    transform.localScale = Vector3.zero;
    transform.gameObject.SetActive(false);
    corout = null;
    TimerEnd.Invoke();
  }

  public void DebugLog(string txt) {
    Debug.Log(txt);
  }
}
