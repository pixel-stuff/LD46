﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyGameObjectEvent : UnityEvent<GameObject> { }
public class ReceptacleEvent : UnityEvent<Receptacle> { }

public class Receptacle : MonoBehaviour
{

    public GameObject LinkedIngredient2D = null;
    public UnityEvent IsOveredByIngredient = new UnityEvent();
    public UnityEvent IsNotOveredAnymoreByIngredient = new UnityEvent();
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public UnityEvent IngredientDiscarded = new UnityEvent();
    
    public ReceptacleEvent IngredientReceived;
    public ReceptacleEvent IngredientRemoved;


  private void Awake() {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }


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

    void OnTriggerExit2D(Collider2D col)
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

    public void OnIngredientLinked(GameObject ingredient)
    {
        LinkedIngredient2D = ingredient;
        IngredientReceived.Invoke(this);
    }

    public void DiscardIngredient()
    {
        Destroy(LinkedIngredient2D);
        LinkedIngredient2D = null;
        IngredientDiscarded.Invoke();
        IngredientRemoved.Invoke(this);
    }

    bool IsIngredientTag(EIngredientTag tag)
    {
        if (LinkedIngredient2D)
        {
            return LinkedIngredient2D.GetComponent<IngredientTag>().ingredientTag == tag;
        }
        return false;
    }
}
