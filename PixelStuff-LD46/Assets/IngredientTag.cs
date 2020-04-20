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
    crane,
    os,
    intestin,
    cyboulette,
    carotte,
    oeil,
    toto,
    poudre,
    cristal,
    papillon,
    cheveux,
    lezard,
    eprouvette,
    fossile, 
    citron,
    none
}
