using UnityEngine;
using System.Collections;

public class TouchInputs : MonoBehaviour {

    public GameObject touchCircle;
    private GameObject[] touchCircles = new GameObject[11];

	// Use this for initialization
	void Start () {

        for(int i=0; i < 11; i++)
        {
            GameObject c = Instantiate(touchCircle);
            c.SetActive(false);
            touchCircles[i] = c;
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        int index = 0;
        foreach(Touch t in Input.touches)
        {   
            Vector3 fingerPosScreen = new Vector3(t.position.x, t.position.y, Camera.main.nearClipPlane);
            Vector3 dst = Camera.main.ScreenToWorldPoint(fingerPosScreen);
            touchCircles[index].transform.position = dst;
            touchCircles[index].SetActive(true);
            index++;
        }

        for(int i = Input.touchCount; i < touchCircles.Length; i++)
        {
            touchCircles[i].SetActive(false);
        }
	
	}
}
