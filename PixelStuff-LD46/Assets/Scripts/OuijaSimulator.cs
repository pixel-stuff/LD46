using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class Vector3Event : UnityEvent<Vector3> { }

public class OuijaSimulator : MonoBehaviour {

  [SerializeField] Transform frequencePlus;
  [SerializeField] Transform frequenceLess;
  [SerializeField] Transform amplitudePlus;
  [SerializeField] Transform amplitudeLess;
  [SerializeField] Transform offsetPlus;
  [SerializeField] Transform offsetLess;


  [SerializeField] Vector3Event data;

  List<Vector3> tab;
  private void Start() {
    tab = new List<Vector3>();
    tab.Add(frequenceLess.position);
    tab.Add(frequencePlus.position);
  }

  void Update() {

    if(Input.GetKey(KeyCode.Mouse0)) {

      data.Invoke(new Vector3(
        MousePositionProjectionIntoLessPlus(amplitudeLess.position, amplitudePlus.position),
        MousePositionProjectionIntoLessPlus(frequenceLess.position, frequencePlus.position),
       MousePositionProjectionIntoLessPlus(offsetLess.position, offsetPlus.position))
     );
    }
  }

  //bool IsMouseInside(Vector3 less, Vector3 plus) {
  //  if(Input.mousePosition.x < Mathf.Min(less.x, plus.x) || Input.mousePosition.x > Mathf.Max(less.x, plus.x)
  //    || Input.mousePosition.y < Mathf.Min(less.y, plus.y) || Input.mousePosition.y > Mathf.Max(less.y, plus.y)) {
  //    return false;
  //  }
  //  return true;
  //}

  float MousePositionProjectionIntoLessPlus(Vector3 less, Vector3 plus) {
    var lessPlusVector = plus - less;
    var v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    v3.z = 0.0f;
    var lessMouseVector = v3 - less;
    var val = Vector3.Dot(lessPlusVector.normalized, lessMouseVector) / Vector3.Distance(less, plus);
    if(val > 1.0f) {
      return 1.0f;
    }
    if(val < 0.0f) {
      return 0.0f;
    }
    return val;
  }
}
