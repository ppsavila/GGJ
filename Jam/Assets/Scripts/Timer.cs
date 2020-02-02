using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que comanda o Timer da UI deveria ser um manager mas assim fiz separado mesmo funciona bem 
/// </summary>
public class Timer : MonoBehaviour
{
    Text timer;
    float time = 15f;

    private void Awake()
    {
        timer = GetComponentInChildren<Text>();
    }

    private void FixedUpdate()
    {
        time -= Time.deltaTime;
        timer.text = time.ToString();
    }

    private void OnDisable()
    {
        time = 15f;
        timer.text = time.ToString();
    }
}
