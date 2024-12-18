using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    Animator anim;
    bool doorOpen;
    [SerializeField] bool doorUnlocked;
    [SerializeField] bool doorCanBeUnlocked = true;

    [SerializeField] GameObject leftLightObject, rightLightObject;
    [SerializeField] Light leftPointLight, rightPointLight;
    [SerializeField] Material greenLightMaterial;

    [SerializeField] ScoreManager scoreScript;

    void Start()
    {
        doorOpen = false;
        anim = GetComponent<Animator>();      
    }

    void OnTriggerEnter(Collider other)
    {
        if (doorUnlocked) 
        {
            if (other.gameObject.CompareTag("Player"))
            {
                doorOpen = true;
                DoorControl("Open");
            }
        }       
    }

    void OnTriggerExit(Collider other)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorControl("Close");
        }
    }

    void DoorControl(string direction)
    {
        anim.SetTrigger(direction);
    }

    void Update()
    {
        if (scoreScript.targetScoreReached && doorCanBeUnlocked) // Door is unlocked if target score is reached.
        {
            doorUnlocked = true;
        }
            

        if (doorUnlocked)
        {
            leftLightObject.GetComponent<MeshRenderer>().material = greenLightMaterial;
            rightLightObject.GetComponent<MeshRenderer>().material = greenLightMaterial;

            leftPointLight.color = new Color32(0, 214, 17, 255);
            rightPointLight.color = new Color32(0, 214, 17, 255);

        }
    }
}
