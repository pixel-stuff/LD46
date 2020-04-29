using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvocationAnimationReaction : MonoBehaviour {


  public void AnimationOver() {
    FindObjectOfType<InvocationManager>().AnimationOver.Invoke();

  }


  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
