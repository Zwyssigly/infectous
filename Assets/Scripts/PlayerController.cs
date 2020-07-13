using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState state = PlayerState.OnAir;
    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D body;
    private WheelVisual visual;
    private Infectable infectable;

    private WheelController wheelController;
    private Infectable wheelInfectable;
    private DecayController[] decayControllers;
    private float wheelDistance;
    public float wheelAngle { get; private set; }

    private float floorLevel;
    private float countdown;

    private float playTime;
    private float movement;

    public PlayButtons playMenu;

    public BoxVisual floor;

    public float gravity = .5f;
    public float jump = 1f;
    public float roll = 1f;

    public AudioClip landSound;
    public AudioClip jumpSound;
    public AudioClip bounceSound;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        visual = GetComponent<WheelVisual>();
        infectable = GetComponent<Infectable>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playMenu.Fail(playTime);
        } 

        movement = Input.GetAxis("Horizontal");
        switch (state)
        {
            case PlayerState.OnFloor:
                playTime += Time.deltaTime;
                CheckJump(Vector2.up);
                var x = transform.position.x + movement * roll * Time.deltaTime;
                x = Math.Max(x, floor.transform.position.x - (floor.width - visual.scale) / 2f);
                x = Math.Min(x, floor.transform.position.x + (floor.width - visual.scale) / 2f);
                transform.position = new Vector3(x, floorLevel, 0);
                break;

            case PlayerState.OnWheel:
                playTime += Time.deltaTime;
                CheckJump(MathUtility.AngleAsNormal(wheelAngle));

                wheelAngle += wheelController.rotation * Time.deltaTime;

                if (movement != 0) {
                    var newAngle = wheelAngle + movement * roll / (wheelDistance * Mathf.PI * 2) * -360 * Time.deltaTime;
                    if (decayControllers.All(d => !d.IsWithinDecay(newAngle)))
                        wheelAngle = newAngle;
                }

                break;

            case PlayerState.OnAir:
                playTime += Time.deltaTime;
                FlipVelocity();
                break;

            case PlayerState.OnTarget:
                FlipVelocity();
                Countdown(() => playMenu.Win(playTime));
                break;

            case PlayerState.OnDead:
                FlipVelocity();
                Countdown(() => playMenu.Fail(playTime));
                break;
        }
    }

    private void Countdown(Action onFinish)
    {
        countdown -= Time.deltaTime;
        if (countdown > 0)
        {
            visual.outerInstances.ForEach(i =>
            {
                i.transform.localPosition += i.transform.localPosition.normalized * countdown * Time.deltaTime;
            });
        }
        else
        {
            enabled = false;
            onFinish();
        }
    }

    private void FlipVelocity()
    {
        var dx = transform.position.x - floor.transform.position.x + (floor.width - visual.scale) / 2f;
        if (dx <= 0f)
        {
            velocity = new Vector2(Math.Abs(velocity.x), velocity.y);
            AudioSource.PlayClipAtPoint(bounceSound, transform.position);
        }
        else if (dx >= floor.width - visual.scale)
        {
            velocity = new Vector2(-Math.Abs(velocity.x), velocity.y);
            AudioSource.PlayClipAtPoint(bounceSound, transform.position);
        }
    }

    private void CheckJump(Vector2 direction)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInternal(direction);
        }
    }

    public void Jump()
    {
        JumpInternal(MathUtility.AngleAsNormal(wheelAngle));
    }

    private void JumpInternal(Vector2 direction)
    {
        velocity = direction * jump;
        state = PlayerState.OnAir;

        if (wheelInfectable != null) wheelInfectable.AbortInfection();

        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    private void OnEndInfection()
    {
        if (wheelInfectable != null) wheelInfectable.AbortInfection();

        state = PlayerState.OnDead;
        countdown = 2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vel = movement * roll / (visual.scale * Mathf.PI) * -360;
        switch (state)
        {
            case PlayerState.OnWheel:
                var position = (Vector3)MathUtility.AngleAsNormal(wheelAngle) * wheelDistance;
                transform.position = wheelController.transform.position + position;
                vel += wheelController.rotation / (wheelDistance * 2f - visual.scale) * (visual.scale);
                break;

            case PlayerState.OnAir:
            case PlayerState.OnDead:
                velocity += (Vector2.down * gravity * Time.deltaTime);
                body.MovePosition(transform.position + (Vector3)velocity * Time.deltaTime);
                break;
            case PlayerState.OnTarget:
                body.MovePosition(transform.position + (Vector3)velocity * Time.deltaTime);
                break;
        }
        body.MoveRotation(transform.rotation.eulerAngles.z + vel * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponentInParent<BoxVisual>() != null)
        {
            state = PlayerState.OnTarget;
            countdown = 2f;
        }
    }

    public void UpdateDecay()
    {
        decayControllers = wheelController.GetComponents<DecayController>();
        if (decayControllers.Any(d => d.IsWithinDecay(wheelAngle)))
        {
            Jump();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (state == PlayerState.OnAir)
        {
            wheelController = col.gameObject.GetComponent<WheelController>();
            if (wheelController != null)
            {
                decayControllers = col.gameObject.GetComponents<DecayController>();
                wheelAngle = MathUtility.AngleBetween(transform.position, col.transform.position);

                if (decayControllers.Any(d => d.IsWithinDecay(wheelAngle)))
                {
                    velocity = Vector2.Reflect(velocity, (transform.position - col.transform.position).normalized);
                    AudioSource.PlayClipAtPoint(bounceSound, transform.position);
                }
                else
                {
                    state = PlayerState.OnWheel;

                    wheelInfectable = col.gameObject.GetComponent<Infectable>();
                    wheelInfectable.BeginInfection(infectable);

                    var wheelVisual = col.gameObject.GetComponent<WheelVisual>();
                    wheelDistance = (wheelVisual.scale + visual.scale) / 2f;

                    AudioSource.PlayClipAtPoint(landSound, transform.position);
                }
            }
            if (col.gameObject.GetComponent<BoxVisual>() == floor)
            {
                state = PlayerState.OnFloor;
                floorLevel = floor.transform.position.y + (visual.scale + floor.height) / 2f;
                AudioSource.PlayClipAtPoint(landSound, transform.position);
            }
        }        
    }
}

public enum PlayerState
{
    OnAir,
    OnFloor,
    OnWheel,
    OnTarget,
    OnDead,
}
