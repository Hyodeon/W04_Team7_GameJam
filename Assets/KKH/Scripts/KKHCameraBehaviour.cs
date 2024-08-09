using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKHCameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private LayerMask _layers;

    private void LateUpdate()
    {
        if (_player == null)
            return;

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, _layers);

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.GetComponent<KKHTransparentObject>().SetTransparent();
        }
    }

}
