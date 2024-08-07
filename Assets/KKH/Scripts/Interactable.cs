using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactable : BaseBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Outline _outline;
    [SerializeField] private Outline.Mode _outlineMode;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private float _outlineWidth;

    protected override void Awake()
    {
        SetOutline();
    }

    private void SetOutline()
    {
        _outline.OutlineMode = _outlineMode;
        _outline.OutlineColor = _outlineColor;
        _outline.OutlineWidth = _outlineWidth;
        _outline.enabled = false;
    }



    public void IsInteractable()
    {

    }





#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _outline = GetComponent<Outline>();

    }
#endif
}
