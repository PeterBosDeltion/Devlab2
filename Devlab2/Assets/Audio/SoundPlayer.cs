using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    public List<AudioClip> clips = new List<AudioClip>();
    public int clipToPlayIndex = 0;

    public enum WhenToPlay
    {
        Other,
        InputE,
        InputI,
        InputT,
        OnMouseDown,
        InputAxis

    }

    public WhenToPlay whenToPlay;

    public bool isResource;

    public AudioSource source;
    private bool waiting;

    private Resource resource;
    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();

        if (isResource)
        {
            resource = GetComponent<Resource>();
        }
	}


    private void FixedUpdate()
    {
        if (whenToPlay == WhenToPlay.InputAxis)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0 || y != 0)
            {
                if (!waiting)
                {
                    PlayAudio();
                    StartCoroutine(WaitForAudio(source.clip));
                }

            }
            else
            {
                source.Stop();
            }
        }
    }
    // Update is called once per frame
    void Update () {
		

        if (whenToPlay == WhenToPlay.InputE)
        {
            if (Input.GetKeyDown("e"))
            {
                if (!waiting)
                {
                    PlayAudio();
                    StartCoroutine(WaitForAudio(source.clip));
                }
            }
        }
        else if (whenToPlay == WhenToPlay.InputI)
        {
            if (Input.GetKeyDown("i"))
            {
                if (!waiting)
                {
                    PlayAudio();
                    StartCoroutine(WaitForAudio(source.clip));
                }
            }
        }
        else if (whenToPlay == WhenToPlay.InputT)
        {
            if (Input.GetKeyDown("t"))
            {
                if (!waiting)
                {
                    PlayAudio();
                    StartCoroutine(WaitForAudio(source.clip));
                }
            }
        }

    }

    public void OnMouseDown()
    {
        if(whenToPlay == WhenToPlay.OnMouseDown)
        {
            if (!waiting)
            {
                PlayAudio();
                StartCoroutine(WaitForAudio(source.clip));
            }
        }
    }

    public void ResourcePlay()
    {
        if (isResource)
        {
            if (!waiting)
            {
                PlayAudio();
                StartCoroutine(WaitForAudio(source.clip));
            }
        }
    }

    private void PlayAudio()
    {
        source.clip = clips[clipToPlayIndex];
        source.Play();
    }

    private IEnumerator WaitForAudio(AudioClip clip)
    {
        waiting = true;
        yield return new WaitForSeconds(clip.length);
        waiting = false;
    }
}
