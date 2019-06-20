using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firefly : MonoBehaviour {
	// speed of z movement and timer (to keep track of sinusoidal and linear movement)
	[SerializeField]
	private float speed;
	[SerializeField]
	private float time;
	// start position and time limit
	[SerializeField]
	private Vector3 startPos;
	[SerializeField]
	private float timeLimit;
	// max amplitudes on x and y
	[SerializeField]
	private float yAmp;
	[SerializeField]
	private float xAmp;
	// current amplitudes on x and y - to increase gradually to seem continuous
	private float curXAmp;
	private float curYAmp;
	// to see if the light is left or right, time to switch between linear and sinusoidal
	[SerializeField]
	private bool oppositeLight;
	[SerializeField]
	private float lerpTime;
	// oscillation speeds on x and y
	[SerializeField]
	private float xOscSpeed;
	[SerializeField]
	private float yOscSpeed;
	// to keep track of whether the oscillation has begun
	private bool oscStarted = false;

	
	void Start () {
		startPos = transform.position;
	}
	
	
	void Update () {
		time += Time.deltaTime;

		float x = startPos.x, y = startPos.y, z;
		z = startPos.z + time * speed;
		if (time > timeLimit) // oscillate only if time is more than time limit
		{
			if (!oscStarted) // oscillation is just beginning
			{	// to gradually increase the amplitude to make it seem continuous
				float t = time - timeLimit;
				t /= lerpTime;
				
				if (t > 1)
					oscStarted = true; // normal oscillation with max amplitude can begin now
				else 
				{	// set current amplitudes based on how much time has passed
					curXAmp = Mathf.Lerp (0, xAmp, t);
					curYAmp = Mathf.Lerp (0, yAmp, t);
				}
			}
			// do the actual calculation of x and y
			float factor = (oppositeLight) ? Mathf.Deg2Rad * 90f : 0f;
			x = Mathf.Sin (factor + time * xOscSpeed) * curXAmp + startPos.x;
			y = Mathf.Cos (factor + time * yOscSpeed) * curYAmp + startPos.y;

		}
		// set position
		transform.position = new Vector3 (x, y, z);
	}
}
