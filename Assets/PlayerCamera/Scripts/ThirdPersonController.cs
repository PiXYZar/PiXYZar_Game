using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : PortalTraveller
{
    public GameObject tower;

    public GameObject invertedTower;

    public float userInputDelay = 0.1f;

    public float walkingSpeed = 3;
    public float runningSpeed = 6;

    public float rotationSpeed = 2.5f;

    public float gravity = 0.75f;
    public float jumpingSpeed = 50f;

    public float portalRadiusCheck = 3.0f;

    public bool lockCursor = false;

    public bool invertControls = false;

    public LayerMask mask;

    private Vector3 _towerCentre;

    private Rigidbody _rb;
    private CapsuleCollider _collider;
    private BoxCollider _boxCollider;

    private float _inputX, _inputY, _inputZ;

    private Vector3 _playerVel;
    private float _verticalVel;

    public Vector3 PlayerVelocity { get { return _playerVel; } }

    private int _layerMask;
    private int _portalLayerMask;

    private bool _insideTower;
    public bool InsideTower { get { return _insideTower; } }

    Animator animator;

    private bool _enteredPortal = false;
    private bool _exitedPortal = false;
    private bool _insidePortal = false;

    public bool EnteredPortal { get { return _enteredPortal; } set { _enteredPortal = value; } }
    public bool ExitedPortal { get { return _exitedPortal; } set { _exitedPortal = value; } }
    public bool InsidePortal { get { return _insidePortal; } set { _insidePortal = value; } }
    
    private Transform _modelTransform;

    private bool _facingPortal;
    public bool FacingPortal { get { return _facingPortal; } }

    private Camera _portalCamera;
    private Camera _mainCamera;

    private float _radius;
    private GameObject _triggerField;
    private Vector3 _center;

    private bool _frozen;
    private FreezeTrap _freezeTrap;

    private bool _hasKey;
    public bool HasKey { get { return _hasKey; } set { _hasKey = value; } }
   

    void Awake()
    {
        // check if asset has rigid body, if yes, store it 
        if (GetComponent<Rigidbody>())
            _rb = GetComponent<Rigidbody>();
        else
            Debug.LogError("Player asset requires a rigid body component.");

        // check if component has collider, if yes, store y bounds 
        if (GetComponentInChildren<CapsuleCollider>())
            _collider = GetComponentInChildren<CapsuleCollider>();
        else
            Debug.LogError("Player asset requires a capsule collider component.");

        if (GetComponentInChildren<BoxCollider>())
            _boxCollider = GetComponentInChildren<BoxCollider>();
        else
            Debug.LogError("Player asset requires a capsule collider component.");
    }

    void Start()
    {
        // lock the cursor 
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // initialise animator
        animator = GetComponentInChildren<Animator>();

        // initialize global velocities and inputs 
        _playerVel = Vector3.zero;
        _verticalVel = _playerVel.y;
        _inputX = _inputZ = _inputY = 0;

        // ignore player, detect anything else 
        _layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        _portalLayerMask = 1 << LayerMask.NameToLayer("Portal");

        _towerCentre = tower.GetComponent<Renderer>().bounds.center;

        _insideTower = false;

        _modelTransform = gameObject.transform.GetChild(0);

        _facingPortal = false;

        _mainCamera = transform.Find("Main Camera").GetComponent<Camera>();

        _triggerField = tower.transform.Find("Trigger Field").gameObject;
        Vector3 scale = _triggerField.transform.parent.localScale;
        scale.y = 0.0f;
        _radius = scale.magnitude;
        _center = _triggerField.GetComponent<CapsuleCollider>().center;

        _hasKey = false;
    }

    bool IsFacingPortal()
    {
        //Debug.Log(InsidePortal);
        //Debug.Log(EnteredPortal);
        if (!InsidePortal && !EnteredPortal)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, portalRadiusCheck, _portalLayerMask);

            foreach (Collider collider in colliders)
            {
                // if the angle between the current agent and the detected agent is above a threshold, add it 
                Vector3 distance = collider.transform.position - transform.position;
                distance.y = 0.0f;
                //Debug.Log(collider.name);
                //Debug.DrawRay(transform.position, _playerVel.normalized, Color.red, 2.0f);
                //Debug.DrawRay(transform.position, distance.normalized, Color.blue, 2.0f);
                //Debug.Log("dot product : " + Vector3.Dot(distance, _playerVel));
                if (Vector3.Dot(distance, transform.forward) > Mathf.Cos(90.0f * Mathf.PI / 180.0f))
                    return true;
            }
            //Debug.DrawRay(transform.position, transform.forward, Color.green, 5.0f);
            //if (Physics.Raycast(transform.position, transform.forward, portalRadiusCheck, _portalLayerMask))
            //    return true;
        }
        return false;
    }

    bool IsGrounded()
    {
        /*
        // distance from center of collider to each of the centres of the spheres that make up the capsule 
        float distToSpheres = _collider.height / 2 - _collider.radius;

        // sphere centres in world coordinates 
        Vector3 centre1 = transform.position + _collider.center + Vector3.up * distToSpheres;
        Vector3 centre2 = transform.position + _collider.center + Vector3.up * distToSpheres;

        // capsule collider excluding its own collider and with smaller radius to avoid hitting walls 
        RaycastHit[] hits = Physics.CapsuleCastAll(centre1, centre2, _collider.radius * 0.95f, Vector3.down, 1.1f, _layerMask);

        bool jumpableObjects = false;
        foreach(RaycastHit hit in hits)
        {

            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.isTrigger == false)
            {
                //Debug.Log("trigger false: " + hit.collider.gameObject.name);
                jumpableObjects = true;
            }
        }
        */
        bool jumpableObjects = false;
        RaycastHit hit;
        bool grounded = Physics.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.extents / 2f, Vector3.down,
            out hit, _modelTransform.rotation, 0.5f);

        if (grounded && hit.collider.isTrigger == false)
        {
            jumpableObjects = true;
        }

        // if there is a hit, it is grounded 
        //if (hits.Length > 0 && jumpableObjects)
        if (grounded && jumpableObjects)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsInsideTower()
    {
        // local position of player wrt tower centre 
        Vector3 playerLocal = tower.transform.InverseTransformPoint(transform.position);

        Vector3 difference = tower.GetComponent<Renderer>().bounds.center-  transform.position;
        Vector3 extents = tower.GetComponent<Renderer>().bounds.extents;

        //Debug.Log("diff: " + difference);
        //Debug.Log("extents: " + extents);

        //Debug.Log("local x=> " + (tower.GetComponent<Renderer>().bounds.center.x - transform.position.x) + 
        //    ", z=> " + (tower.GetComponent<Renderer>().bounds.center.z - transform.position.z));
        //Debug.Log("tower extents x=> " + tower.GetComponent<Renderer>().bounds.extents.x + ", z=> " + tower.GetComponent<Renderer>().bounds.extents.z);
        // check if player is inside tower 
        //if (playerLocal.x < tower.GetComponent<MeshFilter>().mesh.bounds.extents.x &&
        //    playerLocal.z < tower.GetComponent<MeshFilter>().mesh.bounds.extents.z)
        //    return true;
        if (Mathf.Abs(difference.x) < extents.x && Mathf.Abs(difference.z) < extents.z)
            return true;

        return false;
    }

    void Translate()
    {
        // check whether player is running or walking 
        float speed = (Input.GetKey(KeyCode.LeftShift)) ? runningSpeed : walkingSpeed;

        // Determine walking or running animation
        animator.SetFloat("Speed", speed);

        // local position of player wrt tower centre 
        Vector3 playerLocal = tower.transform.InverseTransformPoint(transform.position);

        if (_insideTower)
        {
            playerLocal = invertedTower.transform.InverseTransformPoint(transform.position);
            playerLocal.z = -playerLocal.z;
        }

        //if (tower.gameObject.name == "Inverted Tower")
        //{
        //    playerLocal.z = -playerLocal.z;
        //}

        // calculate x and z values for player movement 
        float hypotenuse = new Vector2(playerLocal.x, playerLocal.z).magnitude;
        float xDir = -playerLocal.z / hypotenuse;
        float zDir = playerLocal.x / hypotenuse;

        float xVel, zVel;
        if (Mathf.Abs(_inputX) > userInputDelay)
        {
            xVel = _inputX;
        }
        else
        {
            xVel = 0.0f;
        }

        if (Mathf.Abs(_inputZ) > userInputDelay)
        {
            zVel = _inputZ;
        }
        else
        {
            zVel = 0.0f;
        }

        // if any of buttons WASD pressed, move, else don't move
        if (Mathf.Abs(_inputX) > userInputDelay || Mathf.Abs(_inputZ) > userInputDelay)
        {
            // create sideways and forward movement and combine them 
            Quaternion targetRotation = Quaternion.Euler(0.0f, Mathf.Abs(xVel) * 90.0f, 0.0f);
            Vector3 forward = new Vector3(xDir * zVel, 0.0f, zDir * zVel);
            Vector3 sideways = targetRotation * new Vector3(xDir * xVel, 0.0f, zDir * xVel);

            if (invertControls)
            {                
                if (_insideTower && _exitedPortal)
                {
                    targetRotation = Quaternion.Euler(0.0f, Mathf.Abs(zVel) * -90.0f, 0.0f);
                    forward = new Vector3(xDir * -xVel, 0.0f, zDir * -xVel);
                    sideways = targetRotation * new Vector3(xDir * zVel, 0.0f, zDir * zVel);

                }
                else
                {
                    targetRotation = Quaternion.Euler(0.0f, Mathf.Abs(zVel) * -90.0f, 0.0f);
                    forward = new Vector3(xDir * xVel, 0.0f, zDir * xVel);
                    sideways = targetRotation * new Vector3(xDir * zVel, 0.0f, zDir * zVel);
                }
               
            }
            //Debug.DrawRay(transform.position, forward, Color.red, 2.0f);
            //Debug.DrawRay(transform.position, sideways, Color.green, 2.0f);
            Debug.DrawRay(tower.transform.position, playerLocal, Color.blue, 2.0f);
            _playerVel = (forward + sideways).normalized * speed;



           
        } 
        else
        {
            _playerVel = Vector3.zero;
            animator.SetFloat("Speed", 0);
        }
    }

    void Jump()
    {
        // collider edge needs to lower than the origin of the player asset 
        // don't put gravity, modify it from script 
        // freeze rotation x y z so character doesn't stumble on its own collider

        // player is grounded and jump button pressed -> jump
        if (_inputY > 0 && IsGrounded())
        {
            _playerVel.y = _verticalVel = jumpingSpeed;
        }
        // player is grounded and jump button not pressed -> don't jump
        else if (_inputY == 0 && IsGrounded())
        {
            _playerVel.y = _verticalVel = 0.0f;
        }
        // player is in the air -> decrease vertical velocity
        else
        {
            _verticalVel -= gravity;
            _playerVel.y = _verticalVel;
        }

        animator.SetFloat("Vertical Speed", _verticalVel);
    }

    void Update()
    {
        _inputX = Input.GetAxis("Horizontal"); // left and right arrow keys or A/D
        _inputY = Input.GetAxisRaw("Jump"); // no need for interpolation, either -1, 0 or 1
        _inputZ = Input.GetAxis("Vertical"); // up and down arrow keys or W/S

        //_insideTower = IsInsideTower();
        //Debug.Log("inside tower: " + _insideTower);
        _facingPortal = IsFacingPortal();
        //Debug.Log("facing portal: " + _facingPortal);
        //Debug.Log("inside tower: " + _insideTower);
    }

    void FixedUpdate()
    {
        Translate();
        Jump();

        if (_freezeTrap != null && _freezeTrap.IsFrozen == true)
            _rb.velocity = Vector3.zero;
        else
        {
            _rb.velocity = _playerVel;

            if (_playerVel != Vector3.zero && _verticalVel == 0.0f)
            {
                //Debug.DrawRay(transform.position, _playerVel, Color.blue, 2.0f);
                Quaternion targetRot = Quaternion.LookRotation(_playerVel, Vector3.up);
                targetRot = Quaternion.Euler(0.0f, 90.0f, 0.0f) * targetRot;
                _modelTransform.rotation = Quaternion.Lerp(_modelTransform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
        
        /*
        if ((transform.position - _triggerField.transform.TransformPoint(_center)).magnitude < _radius)
            _insideTower = true;
        else
            _insideTower = false;*/
        //Debug.Log(_insideTower);
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("entering " + collider.gameObject.name);

        if (collider.gameObject.tag == "Freeze Trap")
        {
            _freezeTrap = (FreezeTrap)collider.gameObject.GetComponent<FreezeTrap>();
            _freezeTrap.IsFrozen = true;
        }

        /*if (collider.gameObject.layer.Equals("Portal"))
        {
            _enteredPortal = true;
            Debug.Log("check");
            if (!_insidePortal)
            {
                Debug.Log("check4");
                _portalCamera = collider.transform.Find("Portal Camera").GetComponent<Camera>();
                Debug.Log(collider.gameObject.name);
                _mainCamera.enabled = false;
                _portalCamera.enabled = true;
                
            }
        }*/

     
    }

    void OnTriggerExit(Collider collider)
    {
        //Debug.Log("exiting  " + collider.gameObject.name);
        
        if (collider.gameObject.layer.Equals("Portal"))
        {
            Debug.Log("check5");
            _exitedPortal = true;
            //_portalCamera.enabled = false;
            //_mainCamera.enabled = true;
        }
        
    }

    void OnTriggerStay(Collider collider)
    {
        //Debug.Log("inside  " + collider.gameObject.name);
        /*
        if (collider.gameObject.layer.Equals("Portal"))
        {
            _insidePortal = true;
        }
        */

        if (collider.gameObject.name.Equals("Trigger Field"))
        {
            _insideTower = true;
        }
        else
        {
            _insideTower = false;
        }
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(transform.rotation.y, eulerRot.y);
        float yaw = transform.rotation.y + delta;
        transform.eulerAngles = Vector3.up * yaw;
        _rb.velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(_rb.velocity));
        Physics.SyncTransforms();
    }
}
