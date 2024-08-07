using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : BaseBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Outline _outline;



    [Header("Destroyed")]
    [SerializeField] private float _scaleRatio;
    [SerializeField] private GameObject _destroyedParticle;
    [SerializeField] private Vector3 _offSet;
    protected override void Awake()
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        _outline.enabled = false;
    }


    protected virtual void DestroyInteractable()
    {
        GameObject go = Instantiate(_destroyedParticle, transform.position + _offSet, Quaternion.identity);
        go.transform.localScale = transform.localScale * _scaleRatio;
        Destroy(this.gameObject);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _outline = GetComponent<Outline>();
    }

    protected override void OnButtonField()
    {
        base.OnButtonField();
        DestroyInteractable();
    }
#endif
}
