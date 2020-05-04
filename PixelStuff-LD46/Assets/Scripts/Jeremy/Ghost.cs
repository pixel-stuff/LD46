using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType {
  PetiteFille,
  Sage,
  autre
}

public class Ghost : MonoBehaviour {

  [SerializeField] Animator animator;
  [SerializeField] GhostType type;

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

}
