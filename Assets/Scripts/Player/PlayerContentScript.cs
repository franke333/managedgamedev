using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContentScript : PersistentSingletonClass<PlayerContentScript>
{
    public int gamesPlayed;
    public int gamesWon;
    public int gamesLost => gamesPlayed - gamesWon;

    public int gold;
}
