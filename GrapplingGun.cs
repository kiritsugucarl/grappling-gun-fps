using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    [Header("Grappling Points and References")]
    public LayerMask whatIsGrappeable;
    public Transform gunTip;
    public Transform mainCamera;
    public Transform player;
    private float maxDistance = 200f;
    private SpringJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit; //To make a ray to grapple 
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, maxDistance, whatIsGrappeable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false; //so that its upon you
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f; // smoothness
            joint.massScale = 4.5f; // the mass of the line

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        //if there is no rope, then do nothing
        if (!joint)
        {
            return;
        }

        lr.SetPosition(0, gunTip.position); //from gunTip
        lr.SetPosition(1, grapplePoint); // to the grapple point
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint); // destory the line if stopped
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
