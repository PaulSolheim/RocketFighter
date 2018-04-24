using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {


    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 rotationVector = new Vector3(0f, 1f, 0f);

    [SerializeField][Range(0,1)]
    float movementFactor;

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave / 2f + 0.5f;
        print(rawSinWave);

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;

        transform.Rotate(rotationVector);
	}
}
