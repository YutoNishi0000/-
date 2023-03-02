using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : Actor
{
    void Update()
    {
        //ƒvƒŒƒCƒ„[‚ğí‚É’Ç”ö‚³‚¹‚é
        transform.position = new Vector3(Instance.transform.position.x, transform.position.y, Instance.transform.position.z);
    }
}
