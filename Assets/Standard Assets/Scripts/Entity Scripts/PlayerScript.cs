using UnityEngine;
using System.Collections;


public class PlayerScript : MonoBehaviour {
    public delegate void AudioFinished();
    private BlockPosition _position;

    //Player object singleton.
    public static PlayerScript Instance = null;
    public AudioClip mario;
    public AudioClip monster;
    public AudioClip victorySound;
    public AudioClip lossSound;

    public GameObject FlashLight;
    private bool inGame = true;
	void Start () 
    {
        Instance = this;
        gameObject.SetActive(false);

	}
	
    public void Activate(bool MonsterMode)
    {
        inGame = true;

        if (MonsterMode)
        {
            audio.clip = monster;
            FlashLight.SetActive(true);
        }

        else
        {
            audio.clip = mario;
            FlashLight.SetActive(false);

        }

        audio.Play();
    }
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (inGame)
        {
            Ray DownRay = new Ray(transform.position, Vector3.down);
            RaycastHit hitinfo;
            if (Physics.Raycast(DownRay, out hitinfo, LayerMask.NameToLayer("Block")))
            {
                string CellName = hitinfo.collider.gameObject.name;
                string[] cellCoordinates = CellName.Split('x');
                BlockPosition CellPosition;
                CellPosition.row = 0; CellPosition.column = 0;
                try
                {
                    CellPosition.row = int.Parse(cellCoordinates[0]); CellPosition.column = int.Parse(cellCoordinates[1]);
                }
                catch { }

                 
                
                GameManager.Instance.UpdatePlayerPosition(CellPosition);

            }
        }
        

	}

    public void PlayerKill()
    {
        audio.Stop();        
        audio.PlayOneShot(lossSound);
        GameManager.Instance.CurrentMode = GameMode.EndFail;
        inGame = false;
        StartCoroutine(AudioCallBack(lossSound.length, DeactivatePlayer));

    }

    IEnumerator AudioCallBack(float time, AudioFinished callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public void PlayerWin()
    {
        audio.Stop();
        audio.PlayOneShot(victorySound);
        GameManager.Instance.CurrentMode = GameMode.EndSuccess;
        inGame = false;
        StartCoroutine(AudioCallBack(victorySound.length, DeactivatePlayer));
    }

    void DeactivatePlayer()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayerStartingPosition(Vector3 pos)
    {
        transform.position = pos;

    }

    BlockPosition getPlayerCoordinates()
    {
        BlockPosition coords;
        coords.row = 0;
        coords.column = 0;





        return coords;

    }


    public BlockPosition PlayerPosition
    {
        get { return getPlayerCoordinates(); }
    }

}
