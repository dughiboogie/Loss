using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public void DisableEnemy()
    {
        GetComponent<Seeker>().enabled = false;
        GetComponent<EnemyPathfinding>().StopFollowingPlayer();
        // GetComponent<EnemyAI>().enabled = false;
        GetComponent<EnemyCombat>().enabled = false;
        this.enabled = false;
    }

}
