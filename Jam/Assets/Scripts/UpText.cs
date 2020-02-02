using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// codigozinho meio lixo que fiz baseado num video do youtube https://www.youtube.com/watch?v=NXq8ADMCq9U
/// </summary>
public class UpText : MonoBehaviour
{
    GameObject text;
    float speed ;


    private void Awake()
    {
        text = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        text.transform.Translate(0f, speed, 0f);
    }

    private void OnEnable()
    {
        speed = 0.02f;
    }

    private void OnDisable()
    {
        text.transform.position = new Vector3(0, -16, -6);
    }


}
