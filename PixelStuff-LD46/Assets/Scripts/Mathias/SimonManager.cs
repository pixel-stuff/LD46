﻿using System;
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

  [Header("Control")]
  [SerializeField] float step = 4f;

  [Header("Control")]
  [SerializeField] List<Iteration> currentSequence;

  [Header("Animation")]
  [SerializeField] GameObject[] countdown;
  [Space]
  [SerializeField] ParticleSystem GoodInvocation;
  [SerializeField] ParticleSystem BadInvocation;


  [Header("Event")]
  [SerializeField] UnityEvent SequenceAppeared;
  [SerializeField] UnityEvent StartCreateNewSequence;
  [SerializeField] MyFloatEvent Completion;
  [SerializeField] UnityEvent StartEndAnimation;
  [SerializeField] UnityEvent SimonSucceed;
  [SerializeField] UnityEvent SimonFailed;

  List<Receptacle> tmpReceptacle;
  List<SimonIngredient> tmpSimonIngredient;
  List<PlayerIteration> playerSequence;

  int ingredientReceived;
  float completion;

  private void Awake() {
    currentSequence = new List<Iteration>();
    playerSequence = new List<PlayerIteration>();
    foreach(var go in countdown) { go.SetActive(false); }
  }

  public void CreateSequence(int numberOfItemToPick = 3) {
    if(numberOfItemToPick > receptacles.Length) {
      numberOfItemToPick = receptacles.Length;
    }

    tmpSimonIngredient = new List<SimonIngredient>(simonIngredients);
    tmpReceptacle = new List<Receptacle>(receptacles);
    completion = 0.0f;
    StartCreateNewSequence.Invoke();
    currentSequence.Clear();
    playerSequence.Clear();
    RemoveIngredientFromBoard();

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

  public void RedoSameSequence() {
    CreateSequence(playerSequence.Count);
  }

  void RemoveIngredientFromBoard() {
    foreach(var o in receptacles) {
      o.spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
      o.DiscardIngredient();
    }
    ingredientReceived = 0;
  }

  IEnumerator SequenceApparition() {
    var index = 0;
    for(var i = 0; i < countdown.Count(); i++) {
      countdown[i].SetActive(true);
      yield return new WaitForSeconds(1.0f);
      foreach(var go in countdown) { go.SetActive(false); }
    }

    var currentColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    foreach(var iteration in currentSequence) {
      currentColor.a = 0.0f;

      do {
        currentColor.a += step / 100f;
        iteration.receptacle.spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.1f);
      } while(currentColor.a < 1.0f);

      do {
        currentColor.a -= step / 100f;
        iteration.receptacle.spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.1f);
      } while(currentColor.a > 0.0f);
    }

    SequenceAppeared.Invoke();
  }

  public void ReceptacleReceivedIngredient(Receptacle recep) {
    playerSequence.Add(new PlayerIteration() { receptacle = recep, isGoodanswer = false });

    var index = playerSequence.Count - 1;
    if(recep.IsIngredientTag(currentSequence[index].simonIngredient.tag) && recep == currentSequence[index].receptacle) {
      playerSequence[index].isGoodanswer = true;
    }

    if(playerSequence[index].isGoodanswer) {
      if(currentSequence.Count == playerSequence.Count) {
        completion = 1.0f;
      } else {
        completion += 1.0f / currentSequence.Count;
      }
      Completion.Invoke(completion);
    }
    if(currentSequence.Count == playerSequence.Count) {
      StartCoroutine(WaitBeforeCheckPlayerSequence());
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

  public void TimerEnd() => CheckPlayerSequence();

  IEnumerator WaitBeforeCheckPlayerSequence() {
    yield return new WaitForSeconds(1.0f);

    CheckPlayerSequence();
  }

  public void CheckPlayerSequence() {
    StartEndAnimation.Invoke();
    RemoveIngredientFromBoard();

    if(currentSequence.Count != playerSequence.Count) {
      StartCoroutine(EndAnimation(false));
      return;
    }

    for(var i = 0; i < playerSequence.Count; i++) {
      if(!playerSequence[i].isGoodanswer) {
        StartCoroutine(EndAnimation(false));
        return;
      }
    }
    StartCoroutine(EndAnimation(true));
  }

  IEnumerator EndAnimation(bool isGoodInvocation) {
    foreach(var it in currentSequence) {
      it.receptacle.DiscardIngredient();
      yield return new WaitForSeconds(0.8f);
    }

    if(isGoodInvocation) {
      GoodInvocation.Play();
    } else {
      BadInvocation.Play();
    }

    yield return new WaitForSeconds(2.5f);

    if(isGoodInvocation) {
      SimonSucceed.Invoke();
    } else {
      SimonFailed.Invoke();
    }
  }

}
