using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe da parte que vai ser escondida pelo mapa, cada um item tem que ter uma dessa se nao nao funciona, tem que ter um colider mesmo qualquer um 
/// se nao nao funciona, o codigo do update de mudar a cor ali é so pra tester mesmo. pensei em fazer um shader de dissolve na part ou sei la o que
/// </summary>
public class Part : MonoBehaviour
{
    public bool asClicked;


    private void Update()
    {
        if(asClicked)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material.color = Color.white;
    }

    /// <summary>
    /// CLica no objeto na tela com o eventsystem e magia negra
    /// </summary>
    void OnMouseDown()
    {
        AudioManager.getInstance().playSong(0);
        asClicked = true;
    }
    

}
