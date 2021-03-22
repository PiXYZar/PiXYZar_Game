using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalCameraController : MonoBehaviour
{
    public float distanceFromPlayer;
    public float rotationalOffset;
    public GameObject player;
    public GameObject tower;
    public float smooth = 0.05f;
    public float changeOfViewSmooth = 0.5f;
    public Vector3 offSet;

    private Vector3 _camVel;
    private Vector3 _camRotVel;
    private ThirdPersonController _playerScript;
    private bool _insideTower;

    private float _initAngle;
    private float _currAngle;

    void Start()
    {
        _playerScript = player.GetComponent<ThirdPersonController>();
        _insideTower = _playerScript.InsideTower;

        _initAngle = _currAngle = GetAngle();
    }

    float GetAngle()
    {
        Vector3 playerLocal = tower.transform.InverseTransformPoint(player.transform.position);
        float hyp = new Vector2(playerLocal.x, playerLocal.z).magnitude;
        return Mathf.Atan2(playerLocal.z, playerLocal.x) * 180.0f / Mathf.PI;
    }

    void FixedUpdate()
    {
        // if changing view, transition time increases 
        float smoothTime = smooth;
        bool inside = true;

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
    }

    void MoveCamera(bool inside, float smoothTime)
    {      
        Transform _target = player.transform;

        // get target position by rotating offset based on position around tower 
        _currAngle = GetAngle();
        float angle = _initAngle - _currAngle;
        Quaternion rotateOffset = Quaternion.Euler(0.0f, angle, 0.0f);
        Vector3 _targetPosition = _target.position + rotateOffset * offSet;

        // move camera
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _camVel, 0.05f);

        // move camera to look at player
        Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 100 * Time.deltaTime);
    }
}
