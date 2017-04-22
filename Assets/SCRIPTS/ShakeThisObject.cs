using UnityEngine;
using System.Collections;

public class ShakeThisObject : MonoBehaviour
{
    /*  
     *  USE: StartShake(10);
     */

    private Vector3 originalPosition;
	
	//remember to set it in inspector
	public int shakeDivisor;	//50
	public int shakeCounter;	//10
	public float shakeTempo;	//0.01f
	
	void Awake ()
    {
		originalPosition = transform.localPosition;
	}


	public void StartShake(float shakeAmt)
    {	

		if (shakeDivisor ==0 || shakeCounter==0 || shakeTempo==0)
        {
			shakeDivisor = 50;
			shakeCounter = 10;
			shakeTempo = 0.01f;
		}

		shakeAmt = shakeAmt/shakeDivisor;
		
		StartCoroutine(ShakeCoroutine(shakeAmt));
	}
	
	IEnumerator ShakeCoroutine (float shakeAmt)
    {
		float quakeAmtX;
		float quakeAmtY;
		Vector3 pp;
		
		int amplify; amplify = 3;

		if (shakeAmt > 0)
        {
			int counter = shakeCounter;	//counter or deltatime?
			while (true)
            {
				quakeAmtX = Random.value * shakeAmt * amplify - shakeAmt;
				quakeAmtY = Random.value * shakeAmt * amplify - shakeAmt;

                pp = originalPosition;
				pp.y += quakeAmtX;
				pp.x += quakeAmtY;
				pp.z = originalPosition.z;

				transform.localPosition = pp;
				counter--;

				if (counter <= 0) {	break; }
				yield return new WaitForSeconds (shakeTempo);
			}
		}

		transform.localPosition = originalPosition;
	}

    #if UNITY_EDITOR
        // TEST
        void Update()
        {
            if (Input.GetButton("Submit"))
            {
                StartShake(10);
            }
        }
    #endif

}
