using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientContainer : MonoBehaviour
{
    SpriteRenderer rend;
    bool IsOver = false;

    public Camera OuijaCamera;

    public GameObject Ingredient2DPrefab;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject ingredien2D = GameObject.Instantiate(Ingredient2DPrefab);
            Vector3 mousePositionOnOuijaCamera = OuijaCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePositionOnOuijaCamera.z = -10;
            ingredien2D.transform.position = mousePositionOnOuijaCamera;
            ingredien2D.GetComponent<Ingredient>().OuijaCamera = OuijaCamera;
        }
    }

    // The mesh goes red when the mouse is over it...
    void OnMouseEnter()
    {
     //   rend.color = Color.red;
    }

    // ...the red fades out to cyan as the mouse is held over...
    void OnMouseOver()
    {
        //rend.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
        IsOver = true;
    }

    // ...and the mesh finally turns white when the mouse moves away.
    void OnMouseExit()
    {
       // rend.color = Color.white;
        IsOver = false;
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
