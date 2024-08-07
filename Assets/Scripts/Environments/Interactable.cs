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


    [Header("Interactable")]
    [SerializeField] private Transform _playerTrs;
    [SerializeField] private float _distance = 7;

    protected bool _isActive;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _outline.enabled = false;
        _isActive = false;
        _playerTrs = null;
    }




    private void Update()
    {
        if (_playerTrs == null || !_isActive)
        {
            _playerTrs = null;
            _isActive = false;
            return;
        }

        CheckDistance();
    }

    private void CheckDistance()
    {
        if (_distance <= Vector3.Distance(_playerTrs.position, transform.position))
        {
            DeActivateInteractable();
        }
    }

    protected virtual void DestroyInteractable()
    {
        GameObject go = Instantiate(_destroyedParticle, transform.position + _offSet, Quaternion.identity);
        go.transform.localScale = transform.localScale * _scaleRatio;
        Destroy(this.gameObject);
    }



    public void ActivateInteractable(Transform trs)
    {
        if (_isActive)
            return;

        _playerTrs = trs;
        _outline.enabled = true;
        _isActive = true;
    }

    public virtual void Interact()
    {
        if (!_isActive)
            return;
        DestroyInteractable();
    }

    public void DeActivateInteractable()
    {
        if (!_isActive)
            return;

        _playerTrs = null;
        _outline.enabled = false;
        _isActive = false;
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
