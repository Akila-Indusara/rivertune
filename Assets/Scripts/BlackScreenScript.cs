using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenScript : MonoBehaviour
{
    public GameObject dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void endFadeIn_Out()
    {
        dialog.GetComponent<DialogManager>().setIsWaitingFalse();
    }
}
