using UnityEngine;
using System.Collections;

public class Unmoving : MonoBehaviour
{
    Vector3 position;

    // Use this for initialization
    void Start()
    {
        position = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = position;
    }
}
