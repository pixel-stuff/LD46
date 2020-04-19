using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public SpriteRenderer rend;
    bool IsOver = true;
    bool IsSelected = true;

    public GameObject Receptacle = null;

    public Camera OuijaCamera;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(IsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsSelected = true;
            rend.color = Color.blue;
        }


        if(IsSelected && Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsSelected = false;
            rend.color = Color.white;
            if (Receptacle != null)
            {
                this.transform.position = Receptacle.transform.position;
                Receptacle.GetComponent<Receptacle>().OnIngredientLinked(this.gameObject);
                Destroy(this.GetComponent<BoxCollider2D>());
                Destroy(this);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        //Follow cursor
        if (IsSelected && Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mousePositionOnOuijaCamera = OuijaCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePositionOnOuijaCamera.z =-10;
            this.transform.position = mousePositionOnOuijaCamera;
        }
    }

    // The mesh goes red when the mouse is over it...
    void OnMouseEnter()
    {
        rend.color = Color.red;
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
        rend.color = Color.white;
        IsOver = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Receptacle")
        {
            Debug.Log("CollideWithReceptacle");
            Receptacle = col.gameObject;
            rend.color = Color.yellow;
        }
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
