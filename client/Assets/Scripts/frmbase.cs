using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frmbase : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Transform gb
    {
        get
        {
            return transform.Find("gb");
        }
    }
    public void show()
    {
        gb.gameObject.SetActive(true);
        OnShow();
    }
    protected virtual void OnShow()
    {

    }
    protected virtual void OnHide()
    {

    }
    public void hide()
    {
        gb.gameObject.SetActive(false);
        OnHide();
    }
}
