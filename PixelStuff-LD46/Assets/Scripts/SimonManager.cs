using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[Serializable]
public class Iteration {
  public SimonIngredient simonIngredient;
  public Receptacle receptacle;
}

[Serializable]
public class PlayerIteration {
  public bool isGoodanswer;
  public Receptacle receptacle;
}


[Serializable]
public class SimonIngredient {
  public Sprite ingredientSprite;
  public EIngredientTag tag;
}

public class SimonManager : MonoBehaviour {
  [Header("Items available")]
  [SerializeField] SimonIngredient[] simonIngredients;

  [Header("Position to use")]
  [SerializeField] Receptacle[] receptacles;

  [Header("Event")]
  [SerializeField] MyFloatEvent Completion;
  [SerializeField] UnityEvent SimonSucceed;
  [SerializeField] UnityEvent SimonFailed;

  List<Receptacle> tmpReceptacle;
  List<SimonIngredient> tmpSimonIngredient;
  List<Iteration> currentSequence;
  List<PlayerIteration> playerSequence;

  int ingredientReceived;
  float completion;

  private void Awake() {
    currentSequence = new List<Iteration>();
    playerSequence = new List<PlayerIteration>();
  }

  public void CreateSequence(int numberOfItemToPick = 3) {
    if(numberOfItemToPick > receptacles.Length) {
      numberOfItemToPick = receptacles.Length;
    }

    tmpSimonIngredient = new List<SimonIngredient>(simonIngredients);
    tmpReceptacle = new List<Receptacle>(receptacles);
    completion = 0.0f;
    Completion.Invoke(completion);
    currentSequence.Clear();
    playerSequence.Clear();
    foreach(var o in receptacles) {
      o.spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
      o.DiscardIngredient();
    }
    ingredientReceived = 0;

    for(var i = 0; i < numberOfItemToPick; i++) {
      var randSprite = UnityEngine.Random.Range(0, tmpSimonIngredient.Count);
      var randReceptacle = UnityEngine.Random.Range(0, tmpReceptacle.Count);

      currentSequence.Add(new Iteration() { simonIngredient = tmpSimonIngredient[randSprite], receptacle = tmpReceptacle[randReceptacle] });

      tmpSimonIngredient.RemoveAt(randSprite);
      tmpReceptacle.RemoveAt(randReceptacle);
    }

    foreach(var iteration in currentSequence) {
      iteration.receptacle.spriteRenderer.sprite = iteration.simonIngredient.ingredientSprite;
    }

    StartCoroutine(SequenceApparition());
  }

  IEnumerator SequenceApparition() {

    var step = 0.02f;
    var currentColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    foreach(var iteration in currentSequence) {
      currentColor.a = 0.0f;

      do {
        currentColor.a += step;
        iteration.receptacle.spriteRenderer.color = currentColor;

        yield return new WaitForEndOfFrame();
      } while(currentColor.a < 1.0f);

      do {
        currentColor.a -= step;
        iteration.receptacle.spriteRenderer.color = currentColor;

        yield return new WaitForEndOfFrame();
      } while(currentColor.a > 0.0f);
    }
    //Debug.Log("End Of Apparition");
  }

  public void ReceptacleReceivedIngredient(Receptacle recep) {
    playerSequence.Add(new PlayerIteration() { receptacle = recep, isGoodanswer = false });

    var index = playerSequence.Count - 1; 
    if(recep.IsIngredientTag(currentSequence[index].simonIngredient.tag)) {
      playerSequence[index].isGoodanswer = true;
    }


   

    if(currentSequence.Count == playerSequence.Count) {
      CheckPlayerSequence();
    } else if(playerSequence[index].isGoodanswer) {
      completion += 1.0f / currentSequence.Count;
      Completion.Invoke(completion);
    }

  }

  public void ReceptacleRemovedIngredient(Receptacle recep) {
    var index = playerSequence.FindIndex(r => r.receptacle == recep);

    if(playerSequence[index].isGoodanswer) {
      completion -= 1.0f / currentSequence.Count;
      Completion.Invoke(completion);
    }
    playerSequence.RemoveAt(index);
  }

  public bool IsLastReceptacle(Receptacle recep) {
    if(playerSequence.Last().receptacle == recep) {
      return true;
    }
    return false;
  }

  public bool CheckPlayerSequence() {
    if(currentSequence.Count != playerSequence.Count) {
      Debug.Log("MATHIAS -> this shouldn't happen");
      SimonFailed.Invoke();
      return false;
    }

    for(var i = 0; i < playerSequence.Count; i++) {
      if(!playerSequence[i].receptacle.IsIngredientTag(currentSequence[i].simonIngredient.tag)) {
        SimonFailed.Invoke();
        return false;
      }
    }
    completion = 1.0f;
    Completion.Invoke(completion);
    SimonSucceed.Invoke();
    return true;
  }

}
