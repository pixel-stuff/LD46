using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientTag : MonoBehaviour
{
    public EIngredientTag ingredientTag = EIngredientTag.toto;
}


[System.Serializable]
public enum EIngredientTag
{
    Flamme,
    tete,
    os,
    intestin,
    cyboulette,
    carotte,
    oeil,
    toto,
    none
}
