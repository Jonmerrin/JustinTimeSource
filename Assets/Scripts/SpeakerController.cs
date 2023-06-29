using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerController : MonoBehaviour
{

    public bool enterOnStart;
    Animator anim;
    bool isActiveSpeaker;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (enterOnStart)
        {
            anim.SetTrigger("enter");
        }
    }

    public void Speak()
    {
        if (!isActiveSpeaker)
        {
            anim.SetTrigger("in");
        }
    }

    public void Listen()
    {
        if (isActiveSpeaker)
        {
            anim.SetTrigger("out");
        }
    }

    public void Exit()
    {
        anim.SetTrigger("exit");
    }

    public void Enter()
    {
        anim.SetTrigger("enter");
    }
}
