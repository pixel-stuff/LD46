using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvocationManager : MonoBehaviour
{
    public List<InvocationConfig> Invocations = new List<InvocationConfig>();

    public AnimationClip[] MoveAnimation;

    public SinusLine RefCurve;
    public UnityEvent InvocationStarted = new UnityEvent();
    public UnityEvent AnimationOver = new UnityEvent();
    private int currentInvocationIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(Invocations.Count > 0)
        {
            InitInvocation(Invocations[currentInvocationIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitInvocation(InvocationConfig inv)
    {
        //init Curve
        RefCurve.Frequence = inv.Frequence;
        RefCurve.Dephasage = inv.Dephasage;
        RefCurve.Amplitude = inv.Amplitude;
    }

    public void InvocationStart()
    {
        InvocationStarted.Invoke();
        GameObject invoke = GameObject.Instantiate(Invocations[currentInvocationIndex].prefab,transform);
        invoke.GetComponent<Animation>().AddClip(MoveAnimation[currentInvocationIndex], MoveAnimation[currentInvocationIndex].name);
        invoke.GetComponent<Animation>().Play(MoveAnimation[currentInvocationIndex].name);
        //invoke.GetComponent<Animation>().Play()
    }

    public void InvokeNext()
    {
        currentInvocationIndex++;
        if(currentInvocationIndex == Invocations.Count)
        {
            //GotoNext Level
            return;
        }

        InitInvocation(Invocations[currentInvocationIndex]);

    }
}

[System.Serializable]
public class InvocationConfig
{
    public float Frequence = 1.0f;
    public float Dephasage = 0.0f;
    public float Amplitude = 1.0f;

    public GameObject prefab;
}