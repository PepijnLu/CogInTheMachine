using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear_rotation : MonoBehaviour
{
    public float speed;
    public bool rotate_x_axis;
    public bool rotate_y_axis;
    public bool rotate_z_axis;
    public int reverse = 1;
    bool audioOn;
    [SerializeField] AudioSource gearAudio;

    void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            gearAudio = GetComponent<AudioSource>();
        }
    }
    void FixedUpdate()
    {
        if (rotate_x_axis == true)
        {
            transform.Rotate(speed * reverse * Time.deltaTime, 0, 0);
        }
        if (rotate_y_axis == true)
        {
            transform.Rotate(0, speed * reverse * Time.deltaTime, 0);
        }
        if (rotate_z_axis == true)
        {
            transform.Rotate(0, 0, speed * reverse * Time.deltaTime);
        }

        if (!audioOn)
        {
            if(Player.instance.inBigRoom && gearAudio != null)
            {
                audioOn = true;
                gearAudio.Play();
            }
        }
    }
}