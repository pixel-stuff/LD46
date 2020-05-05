using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType {
  PetiteFille,
  Sage,
  autre
}

public class Ghost : MonoBehaviour {
  [Header("Required")]
  [SerializeField] Animator animator;
  [SerializeField] GhostType type;
  [SerializeField] MaskSlideComponent mask;

  [Header("Sprite")]
  [SerializeField] SpriteRenderer renderer;
  [SerializeField] Sprite goodGhost;
  [SerializeField] Sprite badGhost;

  public void AnimationOver() {
    FindObjectOfType<InvocationManager>().AnimationOver.Invoke();
  }

  public void GoodGoTo() {
    switch(type) {
      case GhostType.PetiteFille:
        animator.Play("GoodGoToA");
        break;
      case GhostType.Sage:
        animator.Play("GoodGoToB");
        break;
    }
  }

  public void UpdateVisibleFactor(float factor) => mask.VisibleFactor = factor;

  public void MakeBadGhostFlicker() => StartCoroutine(Flicker());

  IEnumerator Flicker() {
    mask.VisibleFactor = 1.0f;
    for(var i = 0; i < 3; i++) {
      renderer.sprite = badGhost;
      yield return new WaitForSeconds(Random.value);
      renderer.sprite = goodGhost;
      yield return new WaitForSeconds(Random.value);
    }
  }

  public void InvoqueBadGhost() {

  }

}
