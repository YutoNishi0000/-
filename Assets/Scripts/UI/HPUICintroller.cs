using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUICintroller : MonoBehaviour
{
    //HPUI‰ñ“]‚ğí‚ÉƒJƒƒ‰‚Ì‰ñ“]‚Æ“¯‚¶‚É‚·‚é
    private void LateUpdate()
    {
        //í‚ÉƒJƒƒ‰ƒg“¯‚¶‰ñ“]‚É‚·‚é‚±‚Æ‚ÅUI‚Ì‚¸‚ê‚ğ–h‚®
        transform.rotation = Camera.main.transform.rotation;
    }
}