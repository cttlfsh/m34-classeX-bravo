using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Streamer : MonoBehaviour
{
    protected Vector2 inputMovement;
    [SerializeField] private float speed;
    private float turnSmoothTime = 0.01f;
    protected CharacterController cc;

    public IIModifiers currentModifier;
    public List<GameObject> modifiersPool = new List<GameObject>();
    public int selectedModifierIndex;
    public bool isNearFloor;
    public Floor currentSelectedFloorInstance;

    public void GetCurrentModifier(int modifierIndex)
    {
        currentModifier = modifiersPool[modifierIndex].GetComponent<IIModifiers>();
        print($"Current modifier is {modifiersPool[modifierIndex].GetComponent<IIModifiers>()}");
    }

    private void OnTriggerEnter(Collider other)
    {
        isNearFloor = true;
        print($"Collided with {other.gameObject.name}");
        print(currentModifier);
        currentSelectedFloorInstance = other.GetComponent<Floor>();
    }

    private void OnTriggerExit(Collider other)
    {
        isNearFloor = false;
    }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public void Update()
    {
        
        Vector3 finalMovement = new Vector3(Input.GetAxis("Horizontal")*-1, Input.GetAxis("Vertical"), 0f).normalized;
        float turnSmoothVelocity = 0f;

        if (finalMovement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(finalMovement.x*-1, finalMovement.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            cc.Move(finalMovement * speed * Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedModifierIndex = 0;
            GetCurrentModifier(selectedModifierIndex);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            selectedModifierIndex = 1;
            GetCurrentModifier(selectedModifierIndex);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(isNearFloor)
            {
                if (currentModifier != null)
                {
                    print("prova");
                    currentModifier.ApplyEffect(currentSelectedFloorInstance);
                    currentModifier = null;
                } else
                {
                    print("First select a modifier to apply");
                }
            }
        }



    }
}
