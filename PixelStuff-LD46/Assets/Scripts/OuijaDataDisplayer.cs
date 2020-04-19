using UnityEngine;
using UnityEngine.UI;

public class OuijaDataDisplayer : MonoBehaviour {

  [SerializeField] Text textA;
  [SerializeField] Text textF;
  [SerializeField] Text textO;


  public void Display(Vector3 data) {
    //Debug.Log("data : " + data.x + " / " + data.y + " / " + data.z);
    textA.text = "Amplitude: " + data.x;
    textF.text = "Frequence: " + data.y;
    textO.text = "Offset: " + data.z;
  }
}
