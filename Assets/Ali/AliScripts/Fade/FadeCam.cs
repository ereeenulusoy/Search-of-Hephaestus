using UnityEngine;

public class FadeCam : MonoBehaviour
{
    ObjectFading objectFader;

    GameObject player;
    [SerializeField] private Renderer _playerRenderer;
    private void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {

        if (player != null)
        {
            Vector3 bounds = _playerRenderer.bounds.center;
            Vector3 dir = bounds - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == null)
                    return;

                if (hit.collider.gameObject == player)
                {
                    if (objectFader != null)
                        objectFader.DoFade = false;
                }
                else
                {
                    objectFader = hit.collider.gameObject.GetComponent<ObjectFading>();
                    if (objectFader != null)
                    {
                        objectFader.DoFade = true;
                    }
                }
            }
        }
    }
}
