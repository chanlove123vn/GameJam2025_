using System.Collections;
using UnityEngine;

public class SmugEnemy : MonsterAbstract
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private float arriveDist = 0.05f;
    [SerializeField] private Animator anim;
    [SerializeField] private string dieAnim = "Die";
    [SerializeField] private string dieStateName = "Die";
    private bool goingToTarget = true;

    private void Update()
    {
        Moving();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!target) target = GameObject.Find("Target").transform;
        if (!anim) anim = GetComponent<Animator>();
        Init();
    }
    private void Init()
    {
        startPos = transform.position;
        goingToTarget = true;
        moveSpeed = 5;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        Init();

    }
    protected override void Moving()
    {
        if (!rb || !target) return;
        if (currentState == MonsterState.DEAD)
        {
            if (rb) rb.linearVelocity = Vector2.zero;
            return;
        }

        float destX = goingToTarget ? target.position.x : startPos.x;
        float dx = destX - rb.position.x;

        // Tới đích theo X thì đảo hướng
        if (Mathf.Abs(dx) <= arriveDist)
        {
            goingToTarget = !goingToTarget;
            destX = goingToTarget ? target.position.x : startPos.x;
            dx = destX - rb.position.x;
        }

        float dir = Mathf.Sign(dx);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // lật sprite
        if (Mathf.Abs(rb.linearVelocity.x) > 0.001f)
        {
            var s = transform.localScale;
            s.x = Mathf.Sign(rb.linearVelocity.x) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

}
