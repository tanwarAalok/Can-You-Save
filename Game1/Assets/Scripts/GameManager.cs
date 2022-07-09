using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isPlayerDead = false;
    public void PlayerDeadState(bool check)
    {
        isPlayerDead = check;
    }
    public bool GetPlayerState()
    {
        return isPlayerDead;
    }
}
