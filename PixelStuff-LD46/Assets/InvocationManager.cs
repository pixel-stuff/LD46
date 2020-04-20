using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InvocationManager : MonoBehaviour
{
    public List<InvocationConfig> Invocations = new List<InvocationConfig>();

    public AnimationClip[] MoveAnimation;

    public List<GameObject> InvocationGameObjects = new List<GameObject>();

    public SinusLine RefCurve;
    public UnityEvent InitInvocationEvent = new UnityEvent();
    public UnityEvent InvocationStarted = new UnityEvent();
    public UnityEvent AnimationOver = new UnityEvent();
    private int currentInvocationIndex = 0;

    public int TryBeforeDeath = 2;
    public int CollierZOffset = 10;
    public GameObject DeathGameObject;

    public string NextScene = "Menu";
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

    public void ConsumeTry()
    {
        TryBeforeDeath--;
        StartCoroutine(Move(3f));
        
    }

    public IEnumerator Move(float duration)
    {
        Vector3 originalPos = DeathGameObject.transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float newz = Mathf.Lerp(0, CollierZOffset, elapsed / duration);

            DeathGameObject.transform.position = originalPos + new Vector3(0, 0, newz);

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (TryBeforeDeath == 0)
        {
            SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
        }
    }


    void InitInvocation(InvocationConfig inv)
    {
        //init Curve
        RefCurve.Frequence = inv.Frequence;
        RefCurve.Dephasage = inv.Dephasage;
        RefCurve.Amplitude = inv.Amplitude;

        GameObject invoke = GameObject.Instantiate(Invocations[currentInvocationIndex].prefab, transform);
        InvocationGameObjects.Add(invoke);
        //resetMask
        invoke.GetComponentInChildren<MaskSlideComponent>().VisibleFactor = 0f;

        InitInvocationEvent.Invoke();
    }

    public void InvocationStart()
    {
        InvocationStarted.Invoke();
        GameObject invoke = InvocationGameObjects[InvocationGameObjects.Count - 1];
        invoke.GetComponent<Animation>().AddClip(MoveAnimation[currentInvocationIndex], MoveAnimation[currentInvocationIndex].name);
        invoke.GetComponent<Animation>().Play(MoveAnimation[currentInvocationIndex].name);
    }

    public void InvokeNext()
    {
        currentInvocationIndex++;
        if(currentInvocationIndex == Invocations.Count)
        {
            //GotoNext Level
            SceneManager.LoadScene(NextScene, LoadSceneMode.Single);
            return;
        }

        InitInvocation(Invocations[currentInvocationIndex]);

    }

    public void UpdateVisibleFactor(float factor)
    {
        InvocationGameObjects[InvocationGameObjects.Count - 1].GetComponentInChildren<MaskSlideComponent>().VisibleFactor = factor;
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