using UnityEngine;

public class RoomarEnemy : MonsterAbstract
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private float arriveDist = 0.05f;
    [SerializeField] private Animator anim;
    [SerializeField] private string turnRightanim = "TurnRight";
    [SerializeField] private string turnLeftanim = "TurnLeft";
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
        moveSpeed = 3;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        Init();

    }
    protected override void Moving()
    {
        if (!rb || !target) return;

        float destX = goingToTarget ? target.position.x : startPos.x;
        float dx = destX - rb.position.x;

        if (Mathf.Abs(dx) <= arriveDist)
        {
            goingToTarget = !goingToTarget;
            destX = goingToTarget ? target.position.x : startPos.x;
            dx = destX - rb.position.x;
        }

        float dir = Mathf.Sign(dx);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // lật sprite
        float vx = rb.linearVelocity.x;
        bool movingRight = vx > 0.001f;
        bool movingLeft = vx <= 0.001f;

        anim.SetBool(turnRightanim, movingRight);
        anim.SetBool(turnLeftanim, movingLeft);

    }
}