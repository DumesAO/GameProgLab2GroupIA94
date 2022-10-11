using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCamera : MonoBehaviour
{
    public float lookSpeed=60f;

    // Update is called once per frame
    void Update()
    {
        ProcessRotation();
    }

    void ProcessRotation()
    {
        float verticalLookInput = Input.GetAxis("Mouse Y");
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x - verticalLookInput * lookSpeed * Time.deltaTime, playerRotation.y , playerRotation.z));
    }
}
