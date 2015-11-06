using UnityEngine;
using System.Collections;
using Assets.Framework;
using System.Collections.Generic;

public class TokenFingers : MonoBehaviour {

    public GameObject fingerCircle;
    private GameObject[] fingerCircles = new GameObject[11];
    private List<FingerTouch> fingers;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < 11; i++)
        {
            GameObject c = Instantiate(fingerCircle);
            c.SetActive(false);
            fingerCircles[i] = c;
        }

    }
	
	// Update is called once per frame
	void Update () {
        int index = 0;
        foreach(FingerTouch f in InputManager.Touches)
        {
            Vector3 dstScreen = new Vector3(f.Position.x, f.Position.y, Camera.main.nearClipPlane);
            Vector3 dst = Camera.main.ScreenToWorldPoint(dstScreen);
            fingerCircles[index].transform.position = dst;
            fingerCircles[index].SetActive(true);
            index++;
        }

        for(int i=InputManager.FingersCount(); i < fingerCircles.Length; i++)
        {
            fingerCircles[i].SetActive(false);
        }
	}
}
