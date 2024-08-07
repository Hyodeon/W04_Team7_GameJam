using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushBox : Interactable
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private GameObject _boxFragment;

    protected override void DestroyInteractable()
    {
        GameObject go = Instantiate(_boxFragment, transform.position, Quaternion.identity);
        go.transform.localScale = transform.localScale;
        Destroy(this.gameObject);
    }

}
