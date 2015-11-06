using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    public float deltaPostion;

    private Vector2 originalPosition;
    private Vector2 currentScreenPosition;
    private Transform myTransform;
    private Rigidbody2D rigidBody;


	// Use this for initialization
	void Start () {

        myTransform = transform;
        originalPosition = myTransform.position;
        rigidBody = GetComponent<Rigidbody2D>();
	
	}
	
	// Update is called once per frame
	void Update () {

        currentScreenPosition = Camera.main.WorldToScreenPoint(myTransform.position);

        if (currentScreenPosition.y + deltaPostion < 0.0f || currentScreenPosition.x < deltaPostion || currentScreenPosition.x > Screen.height + deltaPostion)
        {
            myTransform.position = originalPosition;
            rigidBody.velocity = Vector2.zero;
            rigidBody.angularVelocity = 0.0f;
        }
	
	}
}
