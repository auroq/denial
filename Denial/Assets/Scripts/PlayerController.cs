using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private float MaxSpeed => agent.speed;
    private float Acc => agent.acceleration;
    private float CurrentSpeed;
    
//    private Animator anim;
    private NavMeshAgent agent;

    public EventVector3 OnclickEnvironment;

    private readonly Dictionary<KeyCode, Vector3> KeyVectorMapping = new Dictionary<KeyCode, Vector3>()
    {
        [KeyCode.W] = Vector3.forward,
        [KeyCode.A] = Vector3.left,
        [KeyCode.S] = Vector3.back,
        [KeyCode.D] = Vector3.right,
    };
    
    // Start is called before the first frame update
    void Start()
    {
//        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleMovement();
        HandleLooking();
    }

    void HandleLooking()
    {
    }

    void HandleMovement()
    {
        var codes = Helpers.Input.GetAnyKeys(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
        if (codes.Any())
        {
            foreach (var code in codes)
                OnclickEnvironment.Invoke(CalculateVector(code));
        }
        else
            CurrentSpeed = CurrentSpeed > 0 ? CurrentSpeed - Acc : 0;
    }

    Vector3 CalculateVector(KeyCode key)
    {
        CurrentSpeed += Acc;
        if (CurrentSpeed >= MaxSpeed)
            CurrentSpeed = MaxSpeed;
        
        return KeyVectorMapping[key] * CurrentSpeed;
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> {}
