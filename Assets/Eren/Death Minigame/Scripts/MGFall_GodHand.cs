using UnityEngine;

public class MGFall_GodHand : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float roundSpeed = 2f;
    [SerializeField] private float roundRange = 3f;
    [SerializeField] private float slideSpeed = 10f;

    private Animator handAnim;
    private float startPosition;
    private bool isGrabbing = false;
    private bool deathSequenceStarted = false; // Animasyonun bir kez tetiklenmesi için
    private Transform caughtPlayerTransform;

    private void Awake()
    {
        handAnim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        startPosition = transform.position.x;
    }

    void Update()
    {
        if (!isGrabbing)
        {
            float newPosition = startPosition + Mathf.PingPong(Time.time * roundSpeed, roundRange) - (roundRange / 2f);
            transform.position = new Vector2(newPosition, transform.position.y);
        }
        else if (caughtPlayerTransform != null && !deathSequenceStarted)
        {
            float targetX = caughtPlayerTransform.position.x;
            Vector2 newPos = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), slideSpeed * Time.deltaTime);
            transform.position = newPos;

            if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
            {
                StartDeathSequence();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGrabbing) return; 

        MGFall_Player playerScript = collision.GetComponent<MGFall_Player>();
        if (playerScript != null)
        {
            isGrabbing = true;
            caughtPlayerTransform = collision.transform;
            playerScript.FreezePlayer();
        }
    }

    private void StartDeathSequence()
    {
        deathSequenceStarted = true;
        
        handAnim.SetTrigger("StartDeath");
        

        if (caughtPlayerTransform != null)
        {
            caughtPlayerTransform.gameObject.SetActive(false);
        }
        this.enabled = false;
    }
}