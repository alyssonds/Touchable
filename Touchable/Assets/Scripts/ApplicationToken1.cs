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
    private BoxCollider2D collider2d;

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

    void OnEnable()
    {
        tm.TokenPlacedOnScreen += OnTokenPlacedOnScreen;
        tm.TokenRemovedFromScreen += OnTokenRemovedFromScreen;
        tm.ScreenTokenUpdated += OnTokenUpdated;
    }

	// Use this for initialization
	void Start () {

        myTransform = transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        collider2d = GetComponent<BoxCollider2D>();
        if (collider2d != null)
            collider2d.enabled = false;
    }

    void OnDisable()
    {
        tm.TokenPlacedOnScreen -= OnTokenPlacedOnScreen;
        tm.TokenRemovedFromScreen -= OnTokenRemovedFromScreen;
        tm.ScreenTokenUpdated -= OnTokenUpdated;
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

        if (collider2d != null)
            collider2d.enabled = true;

    }

    public void OnTokenRemovedFromScreen(object sender, ApplicationTokenEventArgs e)
    {
        spriteRenderer.enabled = false;
        if (collider2d != null)
            collider2d.enabled = false;
    }

    public void OnTokenUpdated(object sender, ApplicationTokenEventArgs e)
    {
        tokenPosition = new Vector3(e.Token.Position.x, e.Token.Position.y, Camera.main.nearClipPlane);
        myTransform.position = Camera.main.ScreenToWorldPoint(tokenPosition);
        myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, myTransform.rotation.y, e.Token.Angle);
    }
}
