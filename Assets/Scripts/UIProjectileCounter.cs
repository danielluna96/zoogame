using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProjectileCounter : MonoBehaviour
{
    public static UIProjectileCounter instance { get; private set; }

    public Text texto;
    //float originalSize;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
    }

    public void SetValue(int value)
    {
        texto.text = value.ToString();
    }
}