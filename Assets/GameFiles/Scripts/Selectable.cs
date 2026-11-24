using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using static Selectable;


public interface ISelectableCapabaility
{
    void OnSelect(SelectCtx ctx);
    void OnKeepSelect(SelectCtx ctx);
    void OnUnSelect(SelectCtx ctx);
}

public class Selectable : MonoBehaviour
{
    public ISelectableCapabaility[] Capabilities;
    [SerializeField, HideInInspector] MonoBehaviour[] _capabilities = [];
    [ShowInInspector, InlineProperty] public MonoBehaviour[] capabilities
    {
        // getter only here for unity-serialization, not to be used in code
        get => _capabilities;
        set
        {
            if (value.Any(v => !(v is ISelectableCapabaility)))
            {
                Debug.LogError("Tried to add a Selectable Capability that did not implement ISelectableCapability");
                return;
            }
            _capabilities = value;
        }
    }
    
    void Awake() => Init();
    public void Init() => Capabilities = _capabilities.Cast<ISelectableCapabaility>().ToArray();

    public struct SelectCtx
    {
        public readonly Selectable selected;
        public readonly ISelectableCapabaility[] capabilities;
        public Selector selector;
        public Vector3 mousePos;

        public SelectCtx(Selectable selected, ISelectableCapabaility[] capabilities, Selector selector)
        {
            this.selected = selected;
            this.capabilities = capabilities;
            this.selector = selector;
            mousePos = selector.mousePos;
        }
        
        public T Get<T>() where T : MonoBehaviour => capabilities.OfType<T>().FirstOrDefault();
        public bool Has<T>(out T capability) where T : MonoBehaviour => (capability = capabilities.OfType<T>().FirstOrDefault()) != null;
    }

    public void Select(Selector selector, out SelectCtx ctx)
    {
        ctx = new SelectCtx(this, Capabilities, selector);
        foreach (var capability in Capabilities)
            capability.OnSelect(ctx);
    }

    public void KeepSelect(SelectCtx ctx)
    {
        foreach (var capability in Capabilities)
            capability.OnKeepSelect(ctx);
    }

    public void UnSelect(Selector selector, out SelectCtx ctx)
    {
        ctx = new SelectCtx(this, Capabilities, selector);
        foreach (var capability in Capabilities)
            capability.OnUnSelect(ctx);
    }
    
    
}