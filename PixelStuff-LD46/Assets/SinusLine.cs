using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyStringEvent : UnityEvent<string>
{ }
public class MyBoolEvent : UnityEvent<bool>
{ }

[ExecuteInEditMode]
public class SinusLine : MonoBehaviour
{
    public MyStringEvent MatchScoreChanged = new MyStringEvent();
    public UnityEvent CurveMatch = new UnityEvent();

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

    public float ScoreSum = 1.0f;
    public float ScoreSnap = 150f;
    private bool IsAlreadySnap = false;

    public SinusLine RefCurve;

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

