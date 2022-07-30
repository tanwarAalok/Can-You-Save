using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCapsule : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        PlayerController.currHealth += 10;
        Debug.Log("Player health increase by 10");
        Destroy(gameObject);
    }


}
