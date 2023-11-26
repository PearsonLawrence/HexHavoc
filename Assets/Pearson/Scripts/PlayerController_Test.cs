using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerController_Test : NetworkBehaviour
{
    private Camera _mainCamera;
    private Vector3 _mouseInput;

    [SerializeField]
    private Animator animController;
    [SerializeField]
    private float speed = 3f;
    private void Initialize()
    {
        _mainCamera = Camera.main;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }
    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return;

        //movement
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);
        Vector3 movePos = raycastHit.point;
        movePos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(current: transform.position, target: movePos, Time.deltaTime * speed);
        //Rotate
        if(movePos != transform.position)
        {
            animController.SetBool("isMoving", false);
            Vector3 targetDir = movePos - transform.position;
            transform.forward = targetDir;
        }
        else
        {
            animController.SetBool("isMoving", true);
        }
    }
}
