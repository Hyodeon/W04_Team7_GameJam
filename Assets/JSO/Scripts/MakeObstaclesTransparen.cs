using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObstaclesTransparen : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    GameObject player;

    private void LateUpdate()
    {
        Test();
    }

    void Test()
    {
        Vector3 dirction = (player.transform.position - transform.position).normalized;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, dirction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
        
        for(int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for(int j = 0; j < obj.Length; j++)
            {
                obj[j]?.BecomeTransparent();
            }
        }
    }
}
