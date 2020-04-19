using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receptacle : MonoBehaviour
{

    public GameObject LinkedIngredient2D = null;
    public UnityEvent IsOveredByIngredient = new UnityEvent();
    public UnityEvent IsNotOveredAnymoreByIngredient = new UnityEvent();
    public UnityEvent IngredientDiscarded = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            IsOveredByIngredient.Invoke();
        }
    }

    void OnTriggerExist2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ingredient")
        {
            IsNotOveredAnymoreByIngredient.Invoke();
        }
    }

    void OnMouseOver()
    {
        if (LinkedIngredient2D != null && Input.GetKeyDown(KeyCode.Mouse1))
        {
            DiscardIngredient();
        }
    }

    public void DiscardIngredient()
    {
        Destroy(LinkedIngredient2D);
        LinkedIngredient2D = null;
        IngredientDiscarded.Invoke();
    }

    bool IsIngredientTag(EIngredientTag tag)
    {
        return LinkedIngredient2D.GetComponent<IngredientTag>().ingredientTag == tag;
    }
}
