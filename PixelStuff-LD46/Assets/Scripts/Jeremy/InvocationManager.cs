using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MyIntEvent : UnityEvent<int> { }
public class InvocationManager : MonoBehaviour {

  public List<InvocationConfig> Invocations = new List<InvocationConfig>();
  public List<Ghost> ghostInvocated;
  public SinusLine RefCurve;

  public MyIntEvent GhostInvocated;
  public UnityEvent FlickerEnd;
  public UnityEvent GoodToGoEnd;
  public UnityEvent GameWin;

  private int currentInvocationIndex = 0;
  public int TryBeforeDeath = 2;
  int CurrentTryBeforeDeath = 2;
  public int CollierZOffset = 10;
  public GameObject DeathGameObject;

  private GameObject lastGhostInvoked;

  public string GameOverScene = "GameOver";
  // Start is called before the first frame update
  void Start() {
    CurrentTryBeforeDeath = TryBeforeDeath;
    if(Invocations.Count > 0) {
      GhostAppear(Invocations[currentInvocationIndex]);
    }
  }

  public void ConsumeTry() {
    CurrentTryBeforeDeath--;
    StartCoroutine(FlickerGhost(0.2f, 1));
  }

  public void RestaureTry() {
    if(CurrentTryBeforeDeath < TryBeforeDeath) {
      CurrentTryBeforeDeath++;
      //StartCoroutine(FailedAnimation(1.5f, -1));
    }
  }

  public IEnumerator FlickerGhost(float duration, int signe) {
    Vector3 originalPos = DeathGameObject.transform.position;

    float elapsed = 0.0f;
    ghostInvocated[ghostInvocated.Count - 1].MakeBadGhostFlicker();

    while(elapsed < duration) {
      float newz = Mathf.Lerp(0, CollierZOffset, elapsed / duration);

      DeathGameObject.transform.position = originalPos + new Vector3(0, 0, signe * newz);

      elapsed += Time.deltaTime;

      yield return new WaitForEndOfFrame();
    }

    if(CurrentTryBeforeDeath <= 0) {
      SceneManager.LoadScene(GameOverScene, LoadSceneMode.Single);
    }
  }

  void EndFlicker() {
    HideFilter();
    FlickerEnd.Invoke();
  }

  void EndGoodGoTo() {
    GoodToGoEnd.Invoke();
  }

  void HideFilter() {
    Vector3 tmpPos = DeathGameObject.transform.position;
    tmpPos.z -= CollierZOffset;
    DeathGameObject.transform.position = tmpPos;
  }

  void GhostAppear(InvocationConfig inv) {
    //init Curve
    RefCurve.Frequence = inv.Frequence;
    RefCurve.Dephasage = inv.Dephasage;
    RefCurve.Amplitude = inv.Amplitude;

    lastGhostInvoked = GameObject.Instantiate(Invocations[currentInvocationIndex].prefab, transform);
    ghostInvocated.Add(lastGhostInvoked.GetComponent<Ghost>());
    ghostInvocated[ghostInvocated.Count - 1].FlickerEnd.AddListener(EndFlicker);
    ghostInvocated[ghostInvocated.Count - 1].GoodGoToEnd.AddListener(EndGoodGoTo);
    //resetMask
    lastGhostInvoked.GetComponentInChildren<MaskSlideComponent>().VisibleFactor = 0f;

    GhostInvocated.Invoke(2 + currentInvocationIndex);
  }

  public void GoodGoTo() {
    currentInvocationIndex++;
    StartCoroutine(CoroutGoodGoTo());
  }

  IEnumerator CoroutGoodGoTo() {
    yield return new WaitForSeconds(2.0f);
    var invoke = ghostInvocated[ghostInvocated.Count - 1];
    invoke.GoodGoTo();

    yield return new WaitForSeconds(2.0f);
    //NextSequence.Invoke(2 + currentInvocationIndex);
    if(currentInvocationIndex < Invocations.Count) {
      GhostAppear(Invocations[currentInvocationIndex]);
      GhostInvocated.Invoke(2 + currentInvocationIndex);
    } else {
      GameWin.Invoke();
    }
  }

  public void InvokeNext() {
    currentInvocationIndex++;
    if(currentInvocationIndex == Invocations.Count) {
      //GotoNext Level
      SceneManager.LoadScene(GameOverScene, LoadSceneMode.Single);
      return;
    }

    GhostAppear(Invocations[currentInvocationIndex]);
  }

  public void UpdateVisibleFactor(float factor) => ghostInvocated[ghostInvocated.Count - 1].UpdateVisibleFactor(factor);

}

[System.Serializable]
public class InvocationConfig {
  public float Frequence = 1.0f;
  public float Dephasage = 0.0f;
  public float Amplitude = 1.0f;

  public GameObject prefab;
}