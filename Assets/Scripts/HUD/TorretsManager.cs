using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretsManager : MonoBehaviour
{
    public InteractiveWorld IW;

    // Start is called before the first frame update
    private void Start()
    {
        //IW = GetComponent<InteractiveWorld>();
        TorretsHUD[] HUDS= GetComponentsInChildren<TorretsHUD>();
        foreach (TorretsHUD h in HUDS)
        {
            //Debug.Log("hey");
            h.world = IW;
        }
    }
}
