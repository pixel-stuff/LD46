using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientContainer : MonoBehaviour {
 
  bool IsOver = false;
  public Camera OuijaCamera;
  public GameObject Ingredient2DPrefab;

  private void Update() {
    if(IsOver && Input.GetKeyDown(KeyCode.Mouse0)) {
      GameObject ingredien2D = GameObject.Instantiate(Ingredient2DPrefab);
      Vector3 mousePositionOnOuijaCamera = OuijaCamera.ScreenToWorldPoint(Input.mousePosition);
      mousePositionOnOuijaCamera.z = -10;
      ingredien2D.transform.position = mousePositionOnOuijaCamera;
      ingredien2D.GetComponent<Ingredient>().OuijaCamera = OuijaCamera;
    }
  }

  void OnMouseOver() {
    IsOver = true;
  }

  void OnMouseExit() {
    IsOver = false;
  }

  public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
    Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
    float distance;
    xy.Raycast(ray, out distance);
    return ray.GetPoint(distance);
  }
}
