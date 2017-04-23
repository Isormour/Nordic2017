using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{

    public Transform current_mount;
    public float speed_factor;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, current_mount.position, speed_factor);
        transform.rotation = Quaternion.Slerp(transform.rotation, current_mount.rotation, speed_factor);
    }

    public void set_mount(Transform new_mount)
    {
        current_mount = new_mount;
    }
    
}
