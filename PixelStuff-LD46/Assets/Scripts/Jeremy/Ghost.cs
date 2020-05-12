using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GhostType {
  PetiteFille,
  Sage,
  Femme,
  Samourai,
  Autre
}

public class Ghost : MonoBehaviour {
  [Header("Required")]
  [SerializeField] Animator animator;
  [SerializeField] GhostType type;
  [SerializeField] MaskSlideComponent mask;

  [Header("Sprite")]
  [SerializeField] SpriteRenderer renderer1;
  [SerializeField] SpriteRenderer renderer2;
  [SerializeField] Sprite goodGhost;
  [SerializeField] Sprite badGhost;

  [Header("Event")]
  [SerializeField] public UnityEvent GoodGoToEnd;
  [SerializeField] public UnityEvent FlickerEnd;
  [SerializeField] public UnityEvent KillPlayerEnd;

  public void GoodGoTo() {
    animator.Play("GoodGoTo" + type.ToString());
    StartCoroutine(WaitForGoodGoToEnd());
  }

  IEnumerator WaitForGoodGoToEnd() {
    yield return new WaitForSeconds(1.0f);
    GoodGoToEnd.Invoke();
  }

  public void StartKillPlayer() {
    StartCoroutine(WaitKillPlayerEnd());
  }

  IEnumerator WaitKillPlayerEnd() {
    animator.Play("KillPlayer");
    yield return new WaitForSeconds(0.2f);
    mask.VisibleFactor = 1.0f;
    for(var i = 0; i < 4; i++) {
      renderer1.sprite = badGhost;
      renderer2.sprite = badGhost;
      yield return new WaitForSeconds(UnityEngine.Random.value);
      renderer1.sprite = goodGhost;
      renderer2.sprite = goodGhost;
      yield return new WaitForSeconds(UnityEngine.Random.value / 2);
    }
    mask.VisibleFactor = 0.0f;

    KillPlayerEnd.Invoke();
  }

  public void UpdateVisibleFactor(float factor) => mask.VisibleFactor = factor;

  public void MakeBadGhostFlicker() => StartCoroutine(Flicker());

  IEnumerator Flicker() {
    yield return new WaitForSeconds(0.2f);
    mask.VisibleFactor = 1.0f;
    for(var i = 0; i < 4; i++) {
      renderer1.sprite = badGhost;
      renderer2.sprite = badGhost;
      yield return new WaitForSeconds(UnityEngine.Random.value);
      renderer1.sprite = goodGhost;
      renderer2.sprite = goodGhost;
      yield return new WaitForSeconds(UnityEngine.Random.value / 2);
    }
    mask.VisibleFactor = 0.0f;
    FlickerEnd.Invoke();
  }

  public void InvoqueBadGhost() {

  }

}
