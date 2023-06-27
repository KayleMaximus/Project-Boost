using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 _momentVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    [SerializeField] float movementOffset = 0.5f;

    [Range(0, 1)] [SerializeField] float _momentFactor;
    Vector3 _startingPos;

    void Start()
    {
        _startingPos = GetComponent<Transform>().position;
    }

    void Update()
    {
        //set movement factor
        if (period <= Mathf.Epsilon) return;//if (period == 0) return;
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2; // 6.28
        float rawSineWave = Mathf.Sin(cycles * tau);    //Sin fluctuate between -1 and 1

        _momentFactor = rawSineWave / 2f + movementOffset;
        Vector3 offset = _momentFactor * _momentVector;
        GetComponent<Transform>().position = _startingPos + offset;
    }
}
