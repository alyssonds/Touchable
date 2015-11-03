using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using System;
using Assets.Scripts;

public class ApplicationToken1 : MonoBehaviour , IApplicationToken {

    public int tokenClass = 0;
    private TokenManager tm;

    private Transform myTransform;
    private SpriteRenderer spriteRenderer;

    private Vector3 tokenPosition;

    public int TokenClass
    {
        get
        {
            return tokenClass;
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    void Awake()
    {
        tm = TokenManager.Instance;
    }

	// Use this for initialization
	void Start () {

        tm.TokenPlacedOnScreen += OnTokenPlacedOnScreen;
        tm.TokenRemovedFromScreen += OnTokenRemovedFromScreen;
        tm.ScreenTokenUpdated += OnTokenUpdated;

        myTransform = transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public int GetTokenClass()
    {
        return this.tokenClass;
    }

    public void OnTokenPlacedOnScreen(object sender, ApplicationTokenEventArgs e)
    {
        tokenPosition = new Vector3(e.Token.Position.x, e.Token.Position.y, Camera.main.nearClipPlane);
        myTransform.position = Camera.main.ScreenToWorldPoint(tokenPosition);
        myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, myTransform.rotation.y, e.Token.Angle);
        spriteRenderer.enabled = true;
        
    }

    public void OnTokenRemovedFromScreen(object sender, ApplicationTokenEventArgs e)
    {
        spriteRenderer.enabled = false;
    }

    public void OnTokenUpdated(object sender, ApplicationTokenEventArgs e)
    {
        tokenPosition = new Vector3(e.Token.Position.x, e.Token.Position.y, Camera.main.nearClipPlane);
        myTransform.position = Camera.main.ScreenToWorldPoint(tokenPosition);
        myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, myTransform.rotation.y, e.Token.Angle);
    }
}
