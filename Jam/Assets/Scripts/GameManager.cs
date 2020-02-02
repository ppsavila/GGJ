using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
/// Classe que comanda a porra toda, esse script ta na camera, lugar merda porem é que a movimentação é a mecanica principal e meio que é o melhor lugar pra afertar ela
/// esse vou comentar os metodos certinhos 
/// </summary>
public class GameManager : MonoBehaviour
{
    //https://kylewbanks.com/blog/unity3d-panning-and-pinch-to-zoom-camera-with-touch-and-mouse-input Tutorial do pitch zoom

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }

    [SerializeField]
    Camera cam;
    Vector3 defaultPos = new Vector3(0f, 0f, -30f);
    float size = 10f;

    float PanSpeed = 10f;
    float ZoomSpeedTouch = 0.5f;
    float ZoomSpeedMouse = 0.5f;
    float[] BoundsX = new float[] { -9f, 9f };
    float[] BoundsZ = new float[] { -3.5f, 3.5f };
    float[] ZoomBounds = new float[] { 1f, 60f };
    Vector3 lastPanPosition;
    int panFingerId; // Touch mode only
    bool wasZoomingLastFrame; // Touch mode only
    Vector2[] lastZoomPositions; // Touch mode only



    float time = 0f;
    float faseTime = 15f;
    [SerializeField]
    List<Obj> objs1 = new List<Obj>();
    [SerializeField]
    List<Obj> objs2 = new List<Obj>();
    [SerializeField]
    List<Obj> objs3 = new List<Obj>();
    [SerializeField]
    List<Obj> objs4 = new List<Obj>();
    [SerializeField]
    List<Obj> objs5 = new List<Obj>();
    Obj actual;
    int index;

    bool asStart= false;
    [SerializeField]
    GameObject startPanel;
    [SerializeField]
    GameObject loadPanel;
    [SerializeField]
    GameObject timerPanel;
    [SerializeField]
    GameObject qntPanel;

    public Text atual;
    public Text objetivo;


    /// <summary>
    /// Awake so pro " singleton" 
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Start para incialização dos valores como padrao por que eu faço quando eu zerar ele voltar pra tela de start dando um reload na scnena
    /// </summary>
    void Start()
    {
       
        index = 0;
        actual = selectObj(index);
        asStart = false;
        startPanel.SetActive(true);
        time = 0f;
        
    }

    /// <summary>
    /// O fixed aqui ta pra so travar o start pra começar so quando o player apertar o botao, ai eu dou true na variavel bool asStart, e faço uns tremzinhos com coroutine 
    /// ai eu feço o painel de start, ativo o painel de timer que é resetado por rodada e ativo o painel de quantidade de peças que o jogador pegou
    /// </summary>
    void FixedUpdate()
    {
        if(asStart)
        {
            StartCoroutine(startRound());
            startPanel.SetActive(false);
            timerPanel.SetActive(true);
            qntPanel.SetActive(true);
        }  
    }


    /// <summary>
    /// O update ta so para os inputs que funciona tanto para mobile qunato para pc, eu sinceramente nao entendo muito bem como ele funciona, o tutorial é o primerio lin
    /// link do script e tambem o mais importante
    /// </summary>
    private void Update()
    {

        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }


    /// <summary>
    /// metodo que trata os inputs do celular 
    /// </summary>
    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                AudioManager.getInstance().playSong(1);
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                AudioManager.getInstance().playSong(2);
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch * Time.deltaTime);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    /// <summary>
    /// Metodo que trata dos inputs do mouse 
    /// </summary>
    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }


    /// <summary>
    /// Movimentação da camera idenpendente do modo da mesma
    /// </summary>
    /// <param name="newPanPosition"> a nova posição que vai fazer interpolação com a anterior </param>
    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed  , offset.y * PanSpeed,0);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }
    /// <summary>
    /// Metodo de de zoom mesmo ne 
    /// </summary>
    /// <param name="offset"> offest parametro tanto para o mouse quanto para o touch</param>
    /// <param name="speed">Velocidade do zoom é um pouco exagerado </param>
    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }
        // Camera em 3d 
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
        // Camera em 2d
       // cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }


    /// <summary>
    /// Seleciona o objeto que vai ser escondido pelo mata em umas listas, fiz isso para gerar uma "aleatoriedade" 
    /// </summary>
    /// <param name="list"> o id da lsita que vai buscar, cada lista é um nivel </param>
    /// <returns> Retorna o objeto a ser encontrado </returns>
    Obj selectObj(int list)
    {
        int rnd;
        switch (list)
        {
            case 0:
                rnd = Random.Range(0, objs1.Count - 1);
                objs1[rnd].gameObject.SetActive(true);
                return objs1[rnd];
                break;
            case 1:
                rnd = Random.Range(0, objs2.Count - 1);
                objs2[rnd].gameObject.SetActive(true);
                return objs2[rnd];
                break;
            case 2:
                rnd = Random.Range(0, objs3.Count - 1);
                objs3[rnd].gameObject.SetActive(true);
                return objs3[rnd];
                break;
            case 3:
                rnd = Random.Range(0, objs4.Count - 1);
                objs4[rnd].gameObject.SetActive(true);
                return objs4[rnd];
                break;
            case 4:
                rnd = Random.Range(0, objs5.Count - 1);
                objs5[rnd].gameObject.SetActive(true);
                return objs5[rnd];
                break;
            case 5:
                SceneManager.LoadScene(0);
                break;

        }
        return null;
    }
 
    /// <summary>
    /// Volta a camera para a pos e o zoom original, nao sei se eu to usando isso ainda 
    /// </summary>
    void resetCam()
    {
        cam.transform.position = defaultPos;
        cam.orthographicSize = size;
    }

    /// <summary>
    /// Metodo que eu chamo no botao 
    /// </summary>
    public void startGame()
    {
        asStart = true;
    }

    //Agora começa a confusao dos Enumerator
    
    /// <summary>
    /// Okay esse é o de final do jogo caso o jogador tenha encontrado todas as peças 
    /// eu reseto qual é o objeto atual, abro o painel de load, reseto o timer do load, dps de 3segundos 
    /// eu fecho o load, sub mais um no index de listas, e busco a aleatoriedade baseado nesse index
    /// reseto a camera tambem so de zoas 
    /// Paro todas as Courotines
    /// </summary>
    /// <returns> Nada nao, so tem 3 segundos de delay</returns>
    IEnumerator endRound()
    {
        actual.gameObject.SetActive(false);
        loadPanel.SetActive(true);
        timerPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        loadPanel.SetActive(false);
        index++;
        actual = selectObj(index);
        resetCam();
        time = Time.time + faseTime;
        StopAllCoroutines();


    }

    /// <summary>
    /// Esse é o final do jogo caso o tempo tenha acabado e o jogador nao tenha encontrado todas as peças 
    /// eu reseto o objeto atual, abro o painel de load e o timer, depois de 3sgundos eu 
    /// reseto o tempo de partida, recomeço o atual porem sem somar o index, e desligo o painel de load, 
    /// reseto a camera so de zoas
    /// </summary>
    /// <returns>retorna nada tambem nao so tem um delay de 3segundos</returns>
    IEnumerator resetRound()
    {
        actual.gameObject.SetActive(false);
        loadPanel.SetActive(true);
        timerPanel.SetActive(false);
        yield return new WaitForSeconds(3f);


        timerPanel.SetActive(true);
        actual = selectObj(index);
        loadPanel.SetActive(false);
        resetCam();
        time = Time.time + faseTime;
    }


    /// <summary>
    /// Essa e a courotine da rodada, ele atualiza o texto de rodada na UI, tanto a quantidade atual e quanto tem que ser pego, baseado no objeto atual
    /// checo se o obj atual esta completo,  se estiver eu ativo a routine de endRound
    /// se nao ele continua ate o final dos 15segundos
    /// ai ele para todas as routines e da o reset rounnmd
    /// </summary>
    /// <returns>Retorna nada tambem nao so tem um delauy de 15 segundos memso</returns>
    IEnumerator startRound()
    {
        objetivo.text = (actual.parts.Count ).ToString();
        
        List<Part> part = actual.parts.FindAll(x => x.asClicked);
        atual.text = (part.Count ).ToString();

        if (actual.complete())
        {
            timerPanel.SetActive(false);
            qntPanel.SetActive(false);
            StartCoroutine(endRound());
        }
        yield return new WaitForSeconds(15f);
        
        StopAllCoroutines();
        StartCoroutine(resetRound());

    }

}


