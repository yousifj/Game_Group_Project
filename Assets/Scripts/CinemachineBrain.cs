using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CinemachineBrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        Transform followTarget = FindAnyObjectByType<PlayerController>().transform;
       vcam.Follow = followTarget;

}

// Update is called once per frame
void Update()
    {
        
    }
}
