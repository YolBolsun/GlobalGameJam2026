using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
public class HitBox : MonoBehaviour
{
    public CharacterController forward;
    InputAction strike;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strike = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = forward.forward;
        if (strike.triggered)
        {
            
            Debug.Log("Attack performed - HitBox active");
        }
        else
        {

        }
    }
}
