using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Loadzinho visual que fiz ele comanda a UI e  faz com que tenha um timer decrescente
/// </summary>
public class Load : MonoBehaviour
{

    Text timer;
    float time = 3f;

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
        time = 3f;
        timer.text = time.ToString();
    }
}
