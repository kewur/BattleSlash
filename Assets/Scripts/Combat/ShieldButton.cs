using UnityEngine;
using System.Collections;

public class ShieldButton : MonoBehaviour {



	// Use this for initialization
    void OnPress(bool flag)
    {

        
        if (!flag)
        {
            
            PlayerScript.mSingleton.UnBlock();
        }

        else
        {
            
            PlayerScript.mSingleton.Block();
        }

    }


}
