using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;

    [SerializeField] private float moveSpeed;
    private float xAxis;
    private float yAxis;
    private bool isHorizontalMove;

    private Vector3 currentDirVec;

    private GameObject scanObject;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 상호작용 중일 때는 움직이지 못함
        xAxis = GameManager.Instance.IsInteract ? 0 : Input.GetAxisRaw("Horizontal");
        yAxis = GameManager.Instance.IsInteract ? 0 : Input.GetAxisRaw("Vertical");

        if (xAxis != 0) // X축 키 입력이 존재하면 가로 이동
        {
            isHorizontalMove = true;
        } // X축, Y축 동시에 눌렀을 때 X축에 대해서 우선권이 있도록 else if로 처리
        else if (yAxis != 0) // Y축 키 입력이 존재하면 세로 이동
        {
            isHorizontalMove = false;
        }

        if (xAxis != 0)
        {
            currentDirVec = Vector3.right * xAxis;
        }
        else if (yAxis != 0)
        {
            currentDirVec = Vector3.up * yAxis;
        }

        // Animation 설정
        if (isHorizontalMove)
        {
            animator.SetInteger("XAxis", (int)xAxis); // 값이 다를 때만 설정
            animator.SetInteger("YAxis", 0); // 다른 축은 0으로 초기화
        }
        else
        {
            animator.SetInteger("YAxis", (int)yAxis); // 값이 다를 때만 설정
            animator.SetInteger("XAxis", 0); // 다른 축은 0으로 초기화
        }

        if (Input.GetKeyDown(KeyCode.Space) && scanObject != null)
        {
            GameManager.Instance.GetAction(scanObject);
            scanObject = null;
        }
    }

    private void FixedUpdate()
    {
        // 대각선 이동 제한.
        Vector2 moveVec = isHorizontalMove ? new Vector2(xAxis, 0) : new Vector2(0, yAxis);
        rigidbody2d.velocity = moveVec * moveSpeed;

        Debug.DrawRay(transform.position, currentDirVec * 0.7f, Color.red);
        // Player의 앞에 있는 Object의 정보를 가져옴
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, currentDirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }
}
