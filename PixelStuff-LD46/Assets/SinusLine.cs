using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyStringEvent : UnityEvent<string>
{ }
[System.Serializable]
public class MyBoolEvent : UnityEvent<bool>
{ }
[System.Serializable]
public class MyFloatEvent : UnityEvent<float>
{ }

[ExecuteInEditMode]
public class SinusLine : MonoBehaviour
{
    public MyStringEvent MatchScoreChanged = new MyStringEvent();
    public UnityEvent CurveMatch = new UnityEvent();
    public MyFloatEvent MatchPercent = new MyFloatEvent();

    [SerializeField]
    public List<Line> Lines = new List<Line>();// = new List<Line>();
    public float ScreenSize = 2.0f;
    public int lengthOfLineRenderer = 20;

    public float Frequence = 1.0f;
    public float Dephasage = 0.0f;
    public float Amplitude = 1.0f;

    public float MinFrequence = 1.0f;
    public float MinDephasage = 0.0f;
    public float MinAmplitude = 1.0f;

    public float MaxFrequence = 1.0f;
    public float MaxDephasage = 0.0f;
    public float MaxAmplitude = 1.0f;

    public float FixSignal = 0.0f;
    public float TimeMultiplicator = 1.0f;
    protected float TimeMultiplicatorAlternate = 1.0f;

    public float ScoreSum = 1.0f;
    public float ScoreSnap = 150f;
    public float MaxScore = 1500f;
private bool IsAlreadySnap = false;


    public float MatchPercentValue = 0.0f;
    public float DebugPercent = 0f;
    private float comparDebugPercent = 0f;
    private bool onlyOnce = false;

    public SinusLine RefCurve = null;

    IEnumerator ChangeFreq(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Frequence = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Frequence = v_end;
    }

    IEnumerator ChangeAmpl(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Amplitude = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Amplitude = v_end;
    }

    IEnumerator ChangeDeph(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Dephasage = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Dephasage = v_end;
    }

    IEnumerator ChangeTimeMultiAl(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            TimeMultiplicatorAlternate = Mathf.Lerp(v_start, v_end, elapsed / duration);
            RefCurve.TimeMultiplicatorAlternate = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        TimeMultiplicatorAlternate = v_end;
        RefCurve.TimeMultiplicatorAlternate = v_end;
    }

    IEnumerator ChangeMatchPercent(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            MatchPercentValue = Mathf.Lerp(v_start, v_end, elapsed / duration);
            MatchPercent.Invoke(Mathf.Lerp(v_start, v_end, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        MatchPercentValue = v_end;
        MatchPercent.Invoke(v_end);
    }

    public void InvocationSucced()
    {
        if (MatchPercentValue == 1f)
        {
            CurveMatch.Invoke();
        }
    }

    public void ApplyPerfectMatchPercent()
    {
        ApplyMatchPercent(1f);
    }

    public void ApplyMatchPercent(float matchPercent)
    {
        StartCoroutine(ChangeFreq(Frequence, RefCurve.Frequence + (1 - matchPercent)* (MaxFrequence - MinFrequence), 2f));
        StartCoroutine(ChangeAmpl(Amplitude, RefCurve.Amplitude + (1 - matchPercent) * (MaxAmplitude - MinAmplitude), 2f));
        StartCoroutine(ChangeDeph(Dephasage, RefCurve.Dephasage + (1 - matchPercent) * (MaxDephasage - MinDephasage), 2f));

        //RecalculateAndSendMatchScore();
        StartCoroutine(ChangeTimeMultiAl(TimeMultiplicatorAlternate, TimeMultiplicator * (1 - matchPercent), 2f));

        StartCoroutine(ChangeMatchPercent(MatchPercentValue, matchPercent, 2f));

    }

    public void Apply0MatchPercent()
    {
        StartCoroutine(ChangeFreq(Frequence, RefCurve.Frequence + (1 - 0) * (MaxFrequence - MinFrequence), 2f));
        StartCoroutine(ChangeAmpl(Amplitude, RefCurve.Amplitude + (1 - 0) * (MaxAmplitude - MinAmplitude), 2f));
        StartCoroutine(ChangeDeph(Dephasage, RefCurve.Dephasage + (1 - 0) * (MaxDephasage - MinDephasage), 2f));

        //RecalculateAndSendMatchScore();
        StartCoroutine(ChangeTimeMultiAl(TimeMultiplicatorAlternate, TimeMultiplicator * (1 - 0), 2f));

        //StartCoroutine(ChangeMatchPercent(MatchPercentValue, 0, 2f));
        MatchPercentValue = 0f;
        MatchPercent.Invoke(0f);
    }

    public void ApplyOverAll(Vector3 data)
    {
        if (!IsAlreadySnap)
        {
            var amplFactor = data.x;
            var freqFactor = data.y;
            var dephFactor = data.z;

            Amplitude = MinAmplitude + (MaxAmplitude - MinAmplitude) * amplFactor;
            Frequence = MinFrequence + (MaxFrequence - MinFrequence) * freqFactor;
            Dephasage = MinDephasage + (MaxDephasage - MinDephasage) * dephFactor;

            RecalculateAndSendMatchScore();
        }
    }

    public void ApplyOverAllFrequence(float factor)
    {
        Frequence = MinFrequence + (MaxFrequence - MinFrequence) * factor;
        RecalculateAndSendMatchScore();
    }


    public void ApplyOverAllDephasage(float factor)
    {
        Dephasage = MinDephasage + (MaxDephasage - MinDephasage) * factor;
        RecalculateAndSendMatchScore();
    }

    public void ApplyOverAllAmplitude(float factor)
    {
        Amplitude = MinAmplitude + (MaxAmplitude - MinAmplitude) * factor;
        RecalculateAndSendMatchScore();
    }

    public void RecalculateAndSendMatchScore()
    {
        ScoreSum = 0.0f;
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            ScoreSum += Mathf.Abs(RefCurve.GetYCurvePosition(i) - GetYCurvePosition(i));
        }

        RefCurve.ScoreSum = ScoreSum;
        if(ScoreSum <= ScoreSnap)
        {
            if (!IsAlreadySnap)
            {
                IsAlreadySnap = true;
                CurveMatch.Invoke();
                //snapValue
                Frequence = RefCurve.Frequence;
                Amplitude = RefCurve.Amplitude;
                Dephasage = RefCurve.Dephasage;
            }
            
        }

        MatchScoreChanged.Invoke(ScoreSum.ToString());
        //Debug.Log(1f - (Mathf.Min(1f, ScoreSum / MaxScore)) + " " + ScoreSum + " " + MaxScore + " " + Mathf.Min(1f, ScoreSum / MaxScore));
        MatchPercent.Invoke(1f-(Mathf.Min(1f, ScoreSum/ MaxScore)));
    }

    public void ResetSnap()
    {
        IsAlreadySnap = false;
    }

    void Start()
    {
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
       /* lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(c1, c2);
        lineRenderer.SetWidth(0.2F, 0.2F);
        lineRenderer.SetVertexCount(lengthOfLineRenderer);*/
    }
    void Update()
    {
        if(RefCurve && DebugPercent != comparDebugPercent)
        {
            onlyOnce = true;
            comparDebugPercent = DebugPercent;
            ApplyMatchPercent(DebugPercent);
        }
       /* else if (RefCurve && DebugPercent == MatchPercentValue && onlyOnce)
        {
            onlyOnce = false;
            MatchPercent.Invoke(DebugPercent);
            if (MatchPercentValue == 1f)
            {
                CurveMatch.Invoke();
            }
        }*/


        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        int i = 0;
        float offset = ScreenSize / lengthOfLineRenderer;
        while (i < lengthOfLineRenderer)
        {
            Vector3 pos = new Vector3(this.gameObject.transform.position.x + offset * i, this.gameObject.transform.position.y + GetYCurvePosition(i), this.gameObject.transform.position.z);
            lineRenderer.SetPosition(i, pos);
            i++;
        }
    }

    public float GetYCurvePosition(int i)
    {
        float y = 0.0f;
        foreach (Line line in Lines)
        {
            y += line.GetValue(i, Frequence, Amplitude, Dephasage * 1000, FixSignal, TimeMultiplicator);
        }
        return y;
    }

    [System.Serializable]
    public class Line
    {
        public float Frequence = 1.0f;
        public float Dephasage = 0.0f;
        public float Amplitude = 1.0f;

        public float FixSignal = 0.0f;

        public float GetValue(float time, float superFrequence, float superAmplitude, float superDephasage,float superFixSignal, float timeMultiplicator)
        {
            return FixSignal + superFixSignal + Amplitude * superAmplitude * Mathf.Sin(Mathf.PI * 2 * Frequence * superFrequence / 1000 * (time+ superDephasage) + Dephasage +  Time.time* timeMultiplicator);
        }
    }
}

