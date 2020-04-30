using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType {
  petitefille,
  autre
}

public class Ghost : MonoBehaviour {

  [SerializeField] Animator animator;
  [SerializeField] GhostType type;

  public void AnimationOver() {
    FindObjectOfType<InvocationManager>().AnimationOver.Invoke();

  }

  public void GoTo() {
    switch(type) {
      case GhostType.petitefille:
        //Start anim GoTo1
        break;
    }
  }

}
