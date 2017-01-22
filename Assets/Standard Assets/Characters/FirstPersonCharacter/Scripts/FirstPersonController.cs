using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : NetworkBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        //[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
		private AudioSource m_AudioSourcePew;
		private AudioSource m_AudioSourceHit;
		private AudioSource m_AudioSourceDeath;
		private AudioSource m_AudioSourceJump;
		private AudioSource m_AudioSourceDash;
		
		public GameObject bulletPrefab;
		public Transform bulletSpawn;
		public float handSpeed = 25.0f;

        [SyncVar]
        public int playerID; //1=blue 2=green 3=yellow 4=red
		
		[SyncVar]
		public Color m_color;
		
		public AudioClip[] m_PewSounds1;
		public AudioClip[] m_PewSounds2;
		public AudioClip[] m_PewSounds3;
		public AudioClip[] m_PewSounds4;
		
		public AudioClip[] m_HitSounds1;
		public AudioClip[] m_HitSounds2;
		public AudioClip[] m_HitSounds3;
		public AudioClip[] m_HitSounds4;
		
		public AudioClip[] m_DeathSounds1;
		public AudioClip[] m_DeathSounds2;
		public AudioClip[] m_DeathSounds3;
		public AudioClip[] m_DeathSounds4;
	
		public AudioClip[] m_JumpSounds1;
		public AudioClip[] m_JumpSounds2;
		public AudioClip[] m_JumpSounds3;
		public AudioClip[] m_JumpSounds4;
		
		public AudioClip[] m_DashSounds1;
		public AudioClip[] m_DashSounds2;
		public AudioClip[] m_DashSounds3;
		public AudioClip[] m_DashSounds4;
		
		
		public void PlayCustomAudio(String sound)
        {
			int n;
			AudioClip[] sounds1 = m_HitSounds1;
			AudioClip[] sounds2 = m_HitSounds2;
			AudioClip[] sounds3 = m_HitSounds3;
			AudioClip[] sounds4 = m_HitSounds4;
			AudioSource audSource = m_AudioSource;
			if(sound == "pew") {
				sounds1 = m_PewSounds1;
				sounds2 = m_PewSounds2;
				sounds3 = m_PewSounds3;
				sounds4 = m_PewSounds4;
				audSource = m_AudioSourcePew;
			} else if (sound == "hit") {
				sounds1 = m_HitSounds1;
				sounds2 = m_HitSounds2;
				sounds3 = m_HitSounds3;
				sounds4 = m_HitSounds4;
				audSource = m_AudioSourceHit;
			} else if (sound == "death") {
				sounds1 = m_DeathSounds1;
				sounds2 = m_DeathSounds2;
				sounds3 = m_DeathSounds3;
				sounds4 = m_DeathSounds4;
				audSource = m_AudioSourceDeath;
			} else if (sound == "jump") {
				sounds1 = m_JumpSounds1;
				sounds2 = m_JumpSounds2;
				sounds3 = m_JumpSounds3;
				sounds4 = m_JumpSounds4;
				audSource = m_AudioSourceJump;
			} else if (sound == "dash") {
				sounds1 = m_DashSounds1;
				sounds2 = m_DashSounds2;
				sounds3 = m_DashSounds3;
				sounds4 = m_DashSounds4;
				audSource = m_AudioSourceDash;
			}
			switch(playerID) 
			{
				case 1:
					n = Random.Range(1, sounds1.Length);
					audSource.clip = sounds1[n];
					audSource.PlayOneShot(audSource.clip);
					// move picked sound to index 0 so it's not picked next time
					sounds1[n] = sounds1[0];
					sounds1[0] = audSource.clip;
				break;
				case 2:
					n = Random.Range(1, sounds2.Length);
					audSource.clip = sounds2[n];
					audSource.PlayOneShot(audSource.clip);
					// move picked sound to index 0 so it's not picked next time
					sounds2[n] = sounds2[0];
					sounds2[0] = audSource.clip;
				break;
				case 3:
					n = Random.Range(1, sounds3.Length);
					audSource.clip = sounds3[n];
					audSource.PlayOneShot(audSource.clip);
					// move picked sound to index 0 so it's not picked next time
					sounds3[n] = sounds3[0];
					sounds3[0] = audSource.clip;
				break;
				case 4:
					n = Random.Range(1, sounds4.Length);
					audSource.clip = sounds4[n];
					audSource.PlayOneShot(audSource.clip);
					// move picked sound to index 0 so it's not picked next time
					sounds4[n] = sounds4[0];
					sounds4[0] = audSource.clip;
				break;
			}
        }
		

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
			AudioSource[] audioSources = GetComponents<AudioSource>();
			m_AudioSource = audioSources[0];
			m_AudioSourcePew = audioSources[1];
			m_AudioSourceHit = audioSources[2];
			m_AudioSourceDeath = audioSources[3];
			m_AudioSourceJump = audioSources[4];
			m_AudioSourceDash = audioSources[5];
			
			
			
			m_MouseLook.Init(transform , m_Camera.transform);
			
			if(isLocalPlayer) 
			{
				switch (playerID)
				{
					case 1:
						m_color = Color.blue;
						gameObject.GetComponentInChildren<MeshRenderer>().material.color = m_color;
						CmdSetPlayerColor(m_color);
						break;
					case 2:
						m_color = Color.green;
						gameObject.GetComponentInChildren<MeshRenderer>().material.color = m_color;
						CmdSetPlayerColor(m_color);
						break;
					case 3:
						m_color = Color.yellow;
						gameObject.GetComponentInChildren<MeshRenderer>().material.color = m_color;
						CmdSetPlayerColor(m_color);
						break;
					case 4:
						m_color = Color.red;
						gameObject.GetComponentInChildren<MeshRenderer>().material.color = m_color;
						CmdSetPlayerColor(m_color);
						break;
				}
			}
        }
		
		[Command]
		public void CmdSetPlayerColor(Color c)
		{
			m_color = c; 
		}
		
		[ClientCallback]
		void TransmitColor(){
			if(isLocalPlayer)
			{
				CmdSetPlayerColor(m_color);
			}
		}
		
		 public override void OnStartClient ()
     {
         StartCoroutine (UpdateColor (1.5f));
 
     }
 
     IEnumerator UpdateColor(float time){
     
         float timer = time;
 
         while (timer > 0) {
             timer -= Time.deltaTime;
 
             TransmitColor();
             if(!isLocalPlayer)
                 gameObject.GetComponentInChildren<MeshRenderer>().material.color = m_color;
 
         
             yield return null;
         }
	 }


        // Update is called once per frame
        double cooldown = 0;
        private void Update()
        {    
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
			
			
			if (Input.GetMouseButtonDown(0))
			{
				CmdFire();
			}
			else if (Input.GetMouseButtonDown(1))
			{
				
			}

            if (Input.GetKey(KeyCode.E))
            {
            //    transform.localScale.x += 100;
            }
            
            if (cooldown < Time.time)
            {
                if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.W))
                {
                    transform.Translate(Vector3.forward * 500 * Time.deltaTime);
                    cooldown = Time.time + 3;
					PlayCustomAudio("dash");
                }
                else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.left * 500 * Time.deltaTime);
                    cooldown = Time.time + 3;
					PlayCustomAudio("dash");
                }
                else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.right * 500 * Time.deltaTime);
                    cooldown = Time.time + 3;
					PlayCustomAudio("dash");
                }
                else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.S))
                {
                    transform.Translate(Vector3.back * 500 * Time.deltaTime);
                    cooldown = Time.time + 3;
					PlayCustomAudio("dash");
                }
            } 
           
        }
		
		
		[Command]
		void CmdFire() 
		{
			// Create Bullet
			var bullet = (GameObject) Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
			//add velocity
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * handSpeed;
			NetworkServer.Spawn(bullet);
			PlayCustomAudio("pew");
			// destroy after 2.8 seconds TODO: Make object pool for better efficiency?
			Destroy(bullet, 2.8f);
		}
		
		
        private void PlayLandingSound()
        {
                m_AudioSource.clip = m_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayCustomAudio("jump");
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            //PlayFootStepAudio();
        }


        // private void PlayFootStepAudio()
        // {
            // if (!m_CharacterController.isGrounded)
            // {
                // return;
            // }
            // // pick & play a random footstep sound from the array,
            // // excluding sound at index 0
            // int n = Random.Range(1, m_FootstepSounds.Length);
            // m_AudioSource.clip = m_FootstepSounds[n];
            // m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // // move picked sound to index 0 so it's not picked next time
            // m_FootstepSounds[n] = m_FootstepSounds[0];
            // m_FootstepSounds[0] = m_AudioSource.clip;
        // }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
