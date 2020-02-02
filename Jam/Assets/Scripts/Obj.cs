using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classe do objeto que é a junção de cada parte do objeto que vai ser espalhado na cena, pode ser um objeto vazio mesmo so pra organizar 
/// </summary>
public class Obj : MonoBehaviour
{
    public List<Part> parts = new List<Part>();
    [SerializeField] 
    bool asComplete = false;

    /// <summary>
    /// Estraguei a merda do teclado, porem assim eu aqui fassosdajsdiashdas uma funcasduhasudhasud lambda com a lista de partes 
    /// verificando se nela nao existe nenhuma que tenha uma com o falso ativo 
    /// </summary>
    public bool complete()
    {
        if (!parts.Exists(x => x.asClicked == false))
        {
            return true;
        }

        return false;
    }

  
}
