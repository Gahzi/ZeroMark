using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for objects used in effects that auto-return to objectpool
/// </summary>
public class ObjectPoolEffect : MonoBehaviour
{

    bool fadeOut;
    Color originalColor;
    protected int lifetime;

    private void Start()
    {
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }
    }

    public virtual void Init(int _lifetime)
    {
        StartCoroutine(Reset(lifetime = _lifetime));
    }

    public virtual void Init()
    {
        Init(KBConstants.GameConstants.decalLifetime);
    }

    protected virtual IEnumerator Reset(int _lifetime)
    {
        fadeOut = true;
        yield return new WaitForSeconds(_lifetime);
        ObjectPool.Recycle(this);
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
        fadeOut = false;
    }
    
    void FixedUpdate()
    {
        if (fadeOut && renderer != null)
        {
            renderer.material.color = Color.Lerp(renderer.material.color, Color.clear, 1.0f * Time.deltaTime);
        }
    }
}