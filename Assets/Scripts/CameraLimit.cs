using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLimit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject.FindGameObjectWithTag("VCAM").GetComponent<CinemachineConfiner>().m_BoundingShape2D = transform.GetComponent<PolygonCollider2D>();
    }
}
