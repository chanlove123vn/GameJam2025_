using UnityEngine;
using UnityEngine.Events;

public class EventOnTrgger : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Event On Trigger===")]
    [SerializeField] private UnityEvent onTrigger;

    //===========================================Unity============================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.onTrigger?.Invoke();
    }

}
