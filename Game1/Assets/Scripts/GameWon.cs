using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        if(PlayerController.deathCount <= 3)
        {
            text.text = "Congratulations!! You are the Savior. \n You avoided the Zombie Apocalypse";
        }
        else if(PlayerController.deathCount <= 5)
        {
            text.text = "You ran out in time ! Only one antitode was left. \n You saved yourself.";
        }

    }
}
