using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }//get; private set; means read-only -->you can read but you cant control
    public PlayerAim aim { get; private set; } 
    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>(); 
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
