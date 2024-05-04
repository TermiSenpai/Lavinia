using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//Object type Enum
public enum InteractiveType
{
    Magic,
    Bush
}



public class BushBehaviour : MonoBehaviour
{
    //Properties
    private bool mousePressed = false;
    private bool mouseOver = false;
    private float count = 0;
    private bool clickeable = true;

    //References
    [SerializeField] private InteractiveType interactiveType = InteractiveType.Bush;
    [SerializeField] private float timer = 2;
    [SerializeField] private GameObject Hunger;
    [SerializeField] private AudioSource m_audiosource;
    [SerializeField] private AudioClip clipArbusto, clipMagia;

    //Delegates
    public delegate void InteractiveDelegate();
    public static InteractiveDelegate OnMagicRelease;
    public static InteractiveDelegate OnBushRelease;

    [SerializeField] PlayerStats player;

    void FixedUpdate()
    {
        ///TODO: RECOLECTABLE?


        //If the object is clickeable and the timer is done the recolection process starts
        if (clickeable)
        {
            //Them moment the player moves the mouse or stop clicking, the timer restarts
            if ((!mouseOver) || (!mousePressed))
            {
                count = 0;
                return;
            }
            else
            if (count >= timer)
            {
                //Recolection procces depending of object type
                clickeable = false;
                switch (interactiveType)
                {
                    case InteractiveType.Magic:

                        if (player.currentMagic >= player.maxMagicQuantity)
                        {
                            player.currentMagic = player.maxMagicQuantity;
                            break;
                        }
                        player.currentMagic += 5;
                        
                        OnMagicRelease?.Invoke();
                        Hunger.SetActive(false);
                        break;

                    case InteractiveType.Bush:
                        if (player.currentFood >= player.maxFoodQuantity)
                        {
                            player.currentFood = player.maxFoodQuantity;
                            break;
                        }
                        player.currentFood += 1;
                        
                        OnBushRelease?.Invoke();
                        Hunger.SetActive(false);
                        break;
                }
                //Debug.Log("Donete");
            }
            else
            {
                StartCoroutine(Tremble());
            }
        }

    }

    // Couroutine to tremble the object
    IEnumerator Tremble()
    {
        count += Time.deltaTime;
        transform.localPosition += new Vector3(0.1f, 0, 0);
        yield return new WaitForSeconds(0.01f);
        transform.localPosition -= new Vector3(0.1f, 0, 0);
        yield return new WaitForSeconds(0.01f);
    }

    void OnMouseDown()
    {
        mousePressed = true;
        switch (interactiveType)
        {
            case InteractiveType.Magic:
                if (clickeable!=false)
                {

                m_audiosource.PlayOneShot(clipMagia);
                }
                break;
            case InteractiveType.Bush:
                if (clickeable!=false)
                {

                m_audiosource.PlayOneShot(clipArbusto);
                }
                break;

        }
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        mouseOver = false;
    }

    void OnMouseUp()
    {
        mousePressed = false;
        m_audiosource.Stop();
    }


    private void OnEnable()
    {
        DayCycle.DayStartRelease += RestartBush;
    }

    private void OnDisable()
    {
        DayCycle.DayStartRelease -= RestartBush;
    }

    void RestartBush()
    {
        Hunger.SetActive(true);
    }

}
