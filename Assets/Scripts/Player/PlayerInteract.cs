using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : BaseBehaviour
{
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private Camera _camera;

    private HashSet<Interactable> _interactables;


    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000, _interactLayer))
            {

                if (hit.collider == null)
                    return;
                else if (hit.transform.gameObject == this.gameObject)
                {
                    return;
                }
                else
                {
                    hit.collider.GetComponent<Interactable>().Interact();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        col.GetComponent<Interactable>().ActivateInteractable(transform);
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _camera = Camera.main;
    }

#endif
}
