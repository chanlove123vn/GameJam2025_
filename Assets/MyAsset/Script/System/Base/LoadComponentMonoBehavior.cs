using UnityEngine;


public class LoadComponentMonoBehavior : MonoBehaviour
{
    protected virtual void Awake()
    {
        LoadComponent();
        ResetValue();
    }

    protected virtual void Start()
    {
    }
    protected virtual void Reset()
    {
        LoadComponent();
        ResetValue();
    }
    protected virtual void LoadComponent()
    {
    }
    protected virtual void ResetValue()
    {

    }


}