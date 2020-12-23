using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerPuzle : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    private Vector2 m_Move;
    private bool canMove = true;
    private Vector2 normalToCheck = Vector2.zero;
    private Vector2 currentColNormal = Vector2.zero;
    SimpleController_UsingPlayerInput mainPlayer;


    private Rigidbody2D rb;
    private PlayerInput pInput;
    private Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public AudioClip walking;
    private AudioSource audioPlayer;


    //---CALLBACKS---
    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();

    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact();
    }


    //---UNITY METHODS---
    void Start()
    {
        mainPlayer = FindObjectOfType<SimpleController_UsingPlayerInput>();
        disableMainPlayerController();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        pInput = Input.Instance.GetComponent<PlayerInput>();

        //Subscribe the input callback functions to the corresponding input events
        string actionMap = pInput.currentActionMap.name;
        pInput.SwitchCurrentActionMap("Player");

        pInput.currentActionMap.FindAction("Move").performed += ctx => OnMove(ctx);
        pInput.currentActionMap.FindAction("Move").canceled += ctx => OnMove(ctx);


        pInput.currentActionMap.FindAction("Interact").performed += ctx => OnInteract(ctx);

        DontDestroyOnLoad(gameObject);
        pInput.SwitchCurrentActionMap(actionMap);

        audioPlayer = GetComponent<AudioSource>();


    }

    public void FixedUpdate()
    {
        if (canMove)
            Move(m_Move);
    }

    public void Update()
    {
        float horizontal = m_Move.x;
        float vertical = m_Move.y;

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("MoveX", lookDirection.x);
        animator.SetFloat("MoveY", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

    }


    //---METHODS---
    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, direction.y,0 );

        rb.MovePosition(transform.position + move * scaledMoveSpeed);
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.clip = walking;
            audioPlayer.Play();
        }
    }
    private void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC")); //Tercer Parametro es la longitud del Rayo
        if (hit.collider != null)
        {
            print(hit.collider.gameObject.name);
            foreach (Transform child in hit.collider.transform)
                if (child.CompareTag("Interactable"))
                    DialogueManager.Instance.StartDialogue(child);
        }
        else {
        //    Debug.Log("Donde estas mirando?");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PushingFloor pf = collision.transform.GetComponent<PushingFloor>();
        if (pf != null)
        {
            if (!canMove)
                rb.velocity = Vector2.zero;
            else if (pf.pushForce.normalized * -1 != currentColNormal)
            {
                canMove = false;
                rb.AddForce(pf.pushForce);
                normalToCheck = pf.pushForce.normalized * -1;
                StartCoroutine(CanMoveAgainAfterWait(pf.pushTime));
            }
        }
        else
        {

            Teleporter tp = collision.transform.GetComponent<Teleporter>();
            if (tp != null)
                transform.position = tp.destination.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentColNormal = collision.GetContact(0).normal;

        if (collision.GetContact(0).normal == normalToCheck)
        {
            canMove = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentColNormal = Vector2.zero;
    }

    IEnumerator CanMoveAgainAfterWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canMove = true;
        rb.velocity = Vector2.zero;
    }

    private void disableMainPlayerController()
    {
        mainPlayer?.gameObject.SetActive(false);
        SceneManager.sceneUnloaded += enableMainPlayerController;
        GameObject.FindGameObjectWithTag("VCAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    }

    private void enableMainPlayerController(Scene s)
    {
        mainPlayer?.gameObject.SetActive(true);
        SceneManager.sceneUnloaded -= enableMainPlayerController;
        GameObject.FindGameObjectWithTag("VCAM").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = mainPlayer.transform;
        gameObject.SetActive(false);
    }
}
