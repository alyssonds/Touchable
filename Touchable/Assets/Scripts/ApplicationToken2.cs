using UnityEngine;
using System.Collections;
using Assets.Framework.TokenEngine;
using System;
using Assets.Scripts;

public class ApplicationToken2 : MonoBehaviour, IApplicationToken
{
    public int tokenClass;
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
    private TokenManager tm;

    void Awake()
    {
        tm = TokenManager.Instance;
    }

    // Use this for initialization
    void Start()
    {

        tm.TokenPlacedOnScreen += OnTokenPlacedOnScreen;
        tm.TokenRemovedFromScreen += OnTokenRemovedFromScreen;
    }

    public void OnTokenPlacedOnScreen(object sender, ApplicationTokenEventArgs e)
    {
        //This is Token2
    }

    public void OnTokenRemovedFromScreen(object sender, ApplicationTokenEventArgs e)
    {
        //This is Token2
    }

    public void OnTokenUpdated(object sender, ApplicationTokenEventArgs e)
    {
        throw new NotImplementedException();
    }
}
