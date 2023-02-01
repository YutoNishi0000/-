using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : Actor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Instance.transform.position.x, transform.position.y, Instance.transform.position.z);
    }
}
