using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Ingredient : MonoBehaviour
{
    public SpriteRenderer rend;
    bool IsSelected = true;

    public UnityEvent IsSelectedEvent = new UnityEvent();
    public UnityEvent IsDropEvent = new UnityEvent();
    public UnityEvent IsDropOnReceptacleEvent = new UnityEvent();

    public GameObject Receptacle = null;

    public Camera OuijaCamera;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        IsSelectedEvent.Invoke();
    }

    private void Update()
    {
        if(IsSelected && Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsSelected = false;
            rend.color = Color.white;
            if (Receptacle != null)
            {
                this.transform.position = Receptacle.transform.position;
                Receptacle.GetComponent<Receptacle>().OnIngredientLinked(this.gameObject);
                Destroy(this.GetComponent<BoxCollider2D>());
                IsDropOnReceptacleEvent.Invoke();
                this.enabled = false; //Destroy(this);
            }
            else
            {
                IsDropEvent.Invoke();
                Destroy(this.GetComponent<SpriteRenderer>());
                this.enabled = false;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Receptacle")
        {
            Debug.Log("CollideWithReceptacle");
            if (col.gameObject.GetComponent<Receptacle>().LinkedIngredient2D == null)
            {
                Receptacle = col.gameObject;
                rend.color = Color.yellow;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Receptacle")
        {
            Debug.Log("ENDCollideWithReceptacle");
            if (col.gameObject.GetComponent<Receptacle>().LinkedIngredient2D == null)
            {
                Receptacle = null;
                rend.color = Color.red;
            }
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
