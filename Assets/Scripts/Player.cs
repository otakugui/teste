using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Câmera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Áudio")]
    public AudioSource audiosource;
    public AudioClip dogAudio, catAudio, ratAudio;
    public float songLength;

    [Header("Game Objects")]
    public GameObject dog;
    public GameObject cat;
    public GameObject rat;
    public GameObject currentAnimal;

    [Header("Movimentação")]
    public float dogSpeed;
    public float catSpeed;
    public float ratSpeed;
    private float speed;
    public float step;

    // Start is called before the first frame update
    void Start()
    {
        currentAnimal = dog;
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {  
        transform.position = new Vector3(currentAnimal.transform.position.x, 0, currentAnimal.transform.position.z);
        ChooseAnimal();
        CameraTarget();
        ControlAnimal(currentAnimal.transform, currentAnimal.GetComponent<Rigidbody>(), 0.2f);
    }

    void ChooseAnimal(){
        if(Input.GetKeyDown(KeyCode.Z)){
            currentAnimal = dog;
            speed = dogSpeed;
            songLength = audiosource.time;
            audiosource.clip = dogAudio;
            audiosource.time = songLength;
            audiosource.Play();
        }
        if(Input.GetKeyDown(KeyCode.X)){
            currentAnimal = cat;
            speed = catSpeed;
            songLength = audiosource.time;
            audiosource.clip = catAudio;
            audiosource.time = songLength;
            audiosource.Play();
        }
        if(Input.GetKeyDown(KeyCode.C)){
            currentAnimal = rat;
            speed = ratSpeed;
            songLength = audiosource.time;
            audiosource.clip = ratAudio;
            audiosource.time = songLength;
            audiosource.Play();
        }
    }

    void CameraTarget(){
        virtualCamera.LookAt = currentAnimal.transform;
        virtualCamera.Follow = currentAnimal.transform;
    }

    void ControlAnimal(Transform animal, Rigidbody rb, float speed){
        float inputx = Input.GetAxis("Horizontal");
        float inputz = Input.GetAxis("Vertical");
        float axisCombined = inputx / inputz;

        Vector3 lookDir = new Vector3(inputx, 0, inputz);

        if(rb.velocity.magnitude > 0.1f || lookDir == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        animal.rotation = Quaternion.Slerp(animal.rotation, lookRotation, Time.deltaTime * step);
        transform.rotation = animal.rotation;

        animal.Translate(Vector3.forward * speed);
    }
}
