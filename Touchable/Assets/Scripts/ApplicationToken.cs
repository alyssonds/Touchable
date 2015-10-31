using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using System;

public class ApplicationToken : MonoBehaviour {

    public int tokenClass = 0;
    private TokenManager tm;

    void Awake()
    {
        tm = TokenManager.Instance;
    }

	// Use this for initialization
	void Start () {

        tm.TokenPlacedOnScreen += OnTokenPlacedOnScreen; 
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTokenPlacedOnScreen(object sender, ApplicationTokenEventArgs e)
    {
        Debug.Log("Token" + tokenClass + " On Screen");
    }

    public int GetTokenClass()
    {
        return this.tokenClass;
    }
}
