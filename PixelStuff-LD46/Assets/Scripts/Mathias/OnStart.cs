using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour {

  [SerializeField] UnityEvent start;

  void Start() => start.Invoke();
}
