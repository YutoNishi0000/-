using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ƒvƒŒƒCƒ„[‚Ì–¡•û‚ª©w‚É“ü‚Á‚Ä‚«‚½‚Æ‚«‚Ì‹““®‚ğ§Œä‚·‚é
public class SearchFriends : Actor
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }
}