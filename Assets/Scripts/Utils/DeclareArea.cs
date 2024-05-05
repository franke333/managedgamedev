using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeclareArea : MonoBehaviour
{
    public float width, height;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(width, height, 0));
    }
}
