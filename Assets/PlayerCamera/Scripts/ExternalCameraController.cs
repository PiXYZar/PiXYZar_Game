using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalCameraController : MonoBehaviour
{
    public float distanceFromPlayer;
    public float rotationalOffset;
    public GameObject player;
    public GameObject tower;
    public GameObject invertedTower;
    public float smooth = 0.05f;
    public float changeOfViewSmooth = 0.5f;
    public Vector3 offSet;
    public Transform firstPersonView;
    public float insideRadius = 5.0f;

    private Vector3 _camVel;
    private Vector3 _camRotVel;
    private ThirdPersonController _playerScript;
    private bool _insideTower;

    private float _initAngle;
    private float _currAngle;

    private Vector3 _targetPosition;

    private MeshRenderer _headRenderer;

    void Start()
    {
        _playerScript = player.GetComponent<ThirdPersonController>();
        _insideTower = _playerScript.InsideTower;

        _initAngle = _currAngle = GetAngle();

        _headRenderer = firstPersonView.GetComponent<MeshRenderer>();
        _targetPosition = Vector3.zero;
    }

    float GetAngle()
    {
        Vector3 playerLocal = tower.transform.InverseTransformPoint(player.transform.position);

        if (_playerScript.InsideTower)
        {
            playerLocal = invertedTower.transform.InverseTransformPoint(transform.position);
            //playerLocal.z = -playerLocal.z;
        }

        float hyp = new Vector2(playerLocal.x, playerLocal.z).magnitude;
        return Mathf.Atan2(playerLocal.z, playerLocal.x) * 180.0f / Mathf.PI;
    }

    void FixedUpdate()
    {
        // if changing view, transition time increases 
        float smoothTime = smooth;
        bool inside = false;        
                
        if (_playerScript.FacingPortal)
        {
            //Debug.Log(_playerScript.EnteredPortal);
            //Debug.Log(_playerScript.ExitedPortal);

            if (_playerScript.EnteredPortal && _playerScript.ExitedPortal)
            {
                _playerScript.EnteredPortal = false;
                _playerScript.InsidePortal = false;
                _playerScript.ExitedPortal = false;
                _insideTower = _playerScript.InsideTower;

                _headRenderer.enabled = true;
            }
            else
            {
                //Debug.Log("check3");
                FirstPersonCamera();
            }
        }
        else
        {
            _headRenderer.enabled = true;
            if (!_playerScript.InsideTower)
            {
                //if (_insideTower)
                //{
                    smoothTime = changeOfViewSmooth;
                    inside = false;
                    _insideTower = false;
                //}
            }
            else
            {
                //if (!_insideTower)
                //{
                    smoothTime = changeOfViewSmooth;
                    inside = true;
                    _insideTower = true;
                //}
            }

            MoveCamera(inside, smoothTime);
        }

       /*
       if (_playerScript.FacingPortal)
       {
           if (_playerScript.EnteredPortal && _playerScript.ExitedPortal)
           {
               _playerScript.EnteredPortal = false;
               _playerScript.InsidePortal = false;
               _playerScript.ExitedPortal = false;
           }
           else
           {
               //FirstPersonCamera();
           }
       }
       else
       {
           Renderer rend = player.GetComponentInChildren<MeshRenderer>();
           rend.enabled = true;
           MoveCamera(smooth);
           /*
           if (!_playerScript.InsideTower)
           {
               if (_insideTower)
               {
                   smoothTime = changeOfViewSmooth;
                   inside = false;
                   _insideTower = false;
               }
           }
           else
           {
               if (!_insideTower)
               {
                   smoothTime = changeOfViewSmooth;
                   inside = true;
                   _insideTower = true;
               }
           }

           MoveCamera(false, smoothTime);
       }*/

    }

    void MoveCamera(bool inside, float smoothTime)
    {      
        Transform _target = player.transform;

        // get target position by rotating offset based on position around tower 
        _currAngle = GetAngle();
        float angle = _initAngle - _currAngle;
        Quaternion rotateOffset = Quaternion.Euler(0.0f, angle, 0.0f);

        Vector3 _targetPosition = Vector3.zero;

        if (inside)
        {
            //Vector3 local = player.transform.position - tower.transform.position;
            //Vector3 updated = local.normalized * insideRadius;

            _targetPosition = invertedTower.GetComponentInChildren<Collider>().bounds.center;            
            
            //_targetPosition = player.transform.position - updated;
            _targetPosition.y = player.transform.position.y + offSet.y;
        }
        else
            _targetPosition = _target.position + rotateOffset * offSet;
        

        // move camera
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _camVel, 0.05f);

        // move camera to look at player
        Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 100 * Time.deltaTime);
    }

    void FirstPersonCamera()
    {
        // move camera 
        transform.position = Vector3.SmoothDamp(transform.position, firstPersonView.position, ref _camVel, changeOfViewSmooth);

        // rotate player 
        Vector3 target = _playerScript.PlayerVelocity;
        target.y = transform.position.y;

        Quaternion targetRotation = Quaternion.LookRotation(player.transform.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 100 * Time.deltaTime);

        // make player invisible if distance to player too low 
        float dist = (firstPersonView.position - transform.position).sqrMagnitude;

        if (dist < 0.5)
        {
            _headRenderer.enabled = false;
        }
        else
        {
            _headRenderer.enabled = true;
        }
    }
}
