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

  public void GoodGoTo() {
    //switch(type) {
    //  case GhostType.PetiteFille:
    //    animator.Play("GoodGoToPetiteFille");
    //    break;
    //  case GhostType.Sage:
    //    animator.Play("GoodGoToSage");
    //    break;
    //  case GhostType.Femme:
    //    animator.Play("GoodGoToFemme");
    //    break;
    //}
    Debug.Log("Math - " + "GoodGoTo" + type.ToString());
    animator.Play("GoodGoTo" + type.ToString());
    StartCoroutine(WaitBeforeGoodToGo());
  }

  IEnumerator WaitBeforeGoodToGo() {
    yield return new WaitForSeconds(1.0f);
    GoodGoToEnd.Invoke();
  }

  public void UpdateVisibleFactor(float factor) => mask.VisibleFactor = factor;

  public void MakeBadGhostFlicker() => StartCoroutine(Flicker());

  IEnumerator Flicker() {
    yield return new WaitForSeconds(0.2f);
    mask.VisibleFactor = 1.0f;
    for(var i = 0; i < 4; i++) {
      renderer1.sprite = badGhost;
      renderer2.sprite = badGhost;
      yield return new WaitForSeconds(UnityEngine.Random.value / 2);
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
