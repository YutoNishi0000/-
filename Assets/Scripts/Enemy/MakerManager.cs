using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミニマップ内で自身を示す色を決めるためのクラス
public class MakerManager : MonoBehaviour
{
    private EnemyController _enemy;
    public GameObject[] MarkerObjcts;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<EnemyController>();

        for(int i = 0; i < MarkerObjcts.Length; i++)
        {
            MarkerObjcts[i].SetActive(false);
        }

        switch (_enemy._defenceState)
        {
            case EnemyController.DefenceState.red:
                MarkerObjcts[0].SetActive(true);
                break;

            case EnemyController.DefenceState.green:
                MarkerObjcts[1].SetActive(true);
                break;

            case EnemyController.DefenceState.blue:
                MarkerObjcts[2].SetActive(true);
                break;
        }
    }
}
