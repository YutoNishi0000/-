using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUICintroller : MonoBehaviour
{
    //HPUI‰ñ“]‚ğí‚ÉƒJƒƒ‰‚Ì‰ñ“]‚Æ“¯‚¶‚É‚·‚é
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}