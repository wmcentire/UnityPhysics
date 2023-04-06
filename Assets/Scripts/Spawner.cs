using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform target;
    private Component collider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(prefab, target.transform.position, target.transform.rotation);
        }
    }

    public void spawn()
    {
        Instantiate(prefab, target.transform.position, target.transform.rotation);
    }
}
