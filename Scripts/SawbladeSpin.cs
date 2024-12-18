using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawbladeSpin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 360f;

    void Update()
    {
        transform.Rotate(Vector3.up * -spinSpeed * Time.deltaTime);
    }
}
