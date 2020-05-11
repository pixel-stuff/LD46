using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MyIntEvent : UnityEvent<int> { }
public class InvocationManager : MonoBehaviour {

  [Header("Simon controller")]
  [SerializeField] int numberIngredientStart = 1;
  [SerializeField] int numberIngredientStep = 1;

  [Header("Ghosts controller")]
  public List<InvocationConfig> Invocations = new List<InvocationConfig>();
  [SerializeField] List<Ghost> ghosts;
  public SinusLine RefCurve;

  public MyIntEvent GhostInvocated;
  public UnityEvent FlickerEnd;
  public UnityEvent GoodToGoEnd;
  public UnityEvent GameWin;

  private int currentInvocationIndex = -1;
  public int TryBeforeDeath = 2;
  int CurrentTryBeforeDeath = 2;
  public int CollierZOffset = 10;
  public GameObject DeathGameObject;

  private GameObject lastGhostInvoked;

  public string GameOverScene = "GameOver";
  public string WinScene = "Win";
  // Start is called before the first frame update
  void Start() {
    CurrentTryBeforeDeath = TryBeforeDeath;
    if(Invocations.Count > 0) {
      InvokeNext();
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
    ghosts[ghosts.Count - 1].MakeBadGhostFlicker();

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
    if(currentInvocationIndex < Invocations.Count) {
      GoodToGoEnd.Invoke();
    } else {
      GameWin.Invoke();
    }
  }

  public void GoToWinScene() {
    SceneManager.LoadScene(WinScene);
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
    ghosts.Add(lastGhostInvoked.GetComponent<Ghost>());
    ghosts[ghosts.Count - 1].FlickerEnd.AddListener(EndFlicker);
    ghosts[ghosts.Count - 1].GoodGoToEnd.AddListener(EndGoodGoTo);
    //resetMask
    lastGhostInvoked.GetComponentInChildren<MaskSlideComponent>().VisibleFactor = 0f;

    GhostInvocated.Invoke(numberIngredientStart + Mathf.Max(0,(currentInvocationIndex-1))*numberIngredientStep);
  }

  public void GoodGoTo() {
    StartCoroutine(CoroutGoodGoTo());
  }

  IEnumerator CoroutGoodGoTo() {
    yield return new WaitForSeconds(2.0f);
    var invoke = ghosts[ghosts.Count - 1];
    invoke.GoodGoTo();
  }

  public void InvokeNext() {
    currentInvocationIndex++;
    GhostAppear(Invocations[currentInvocationIndex]);
  }

  public void UpdateVisibleFactor(float factor) => ghosts[ghosts.Count - 1].UpdateVisibleFactor(factor);

}

[System.Serializable]
public class InvocationConfig {
  public float Frequence = 1.0f;
  public float Dephasage = 0.0f;
  public float Amplitude = 1.0f;

  public GameObject prefab;
}