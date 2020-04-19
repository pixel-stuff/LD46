using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
  [Header("Required")]
  [SerializeField] Transform transform;

  [Header("Event")]
  [SerializeField] UnityEvent TimerStart;
  [SerializeField] UnityEvent TimerEnd;

  float timeCoroutStarted;
  Vector3 loadingStartScale;

  public void Awake() {
    loadingStartScale = transform.localScale;
  }

  public void StartTimer(float waitForXSec = 10) {
    TimerStart.Invoke();
    timeCoroutStarted = Time.time;
    StartCoroutine(TimerCorout(waitForXSec));
  }

  IEnumerator TimerCorout(float waitForXSec) {
    Vector3 tmp = new Vector3(0.0f, loadingStartScale.y, loadingStartScale.z);
    float currentTime;
    do {
      currentTime = Time.time - timeCoroutStarted;
      tmp.x = loadingStartScale.x * currentTime / waitForXSec;
      transform.localScale = tmp;
      yield return new WaitForEndOfFrame();
      //Debug.Log("currenttime : " + currentTime + " / " + waitForXSec);
    } while(currentTime <= waitForXSec);

    transform.localScale = loadingStartScale;
    TimerEnd.Invoke();
  }

  public void DebugLog(string txt) {
    Debug.Log(txt);
  }
}
