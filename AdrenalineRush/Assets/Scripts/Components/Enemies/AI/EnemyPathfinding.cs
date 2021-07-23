using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinding : MonoBehaviour
{
    // Context variables
    private Transform target;
    private Seeker seeker;
    private Path path;
    private new Rigidbody2D rigidbody;
    private EnemyMovement enemyMovement;

    // Variables to find path to player with A*
    public float waypointSwitchDistance = 2f;
    private int currentWaypoint = 0;
    private LayerMask playerLayer;
    [SerializeField] private Transform endOfPathCheck;
    private float endOfPathCheckRadius;
    private bool reachedEndOfPath = false;

    private void Awake()
    {
        target = GameManager.instance.player;
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();

        playerLayer = LayerMask.GetMask("Player");
        endOfPathCheckRadius = endOfPathCheck.GetComponent<CircleCollider2D>().radius;
    }

    /*
     * Method called when current enemy enters player's range.
     * Invoke repeating the UpdatePath function to start calculating the path to the player.
     */
    public void FollowPlayer()
    {
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if(seeker.IsDone()) {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    public void StopFollowingPlayer()
    {
        CancelInvoke("UpdatePath");
        seeker.CancelCurrentPathRequest();

        path = null;
    }

    /*
     * Callback function that checks for errors on the path calculated.
     * If there are no errors set the path to follow and reset currentWaypoint.
     */
    private void OnPathComplete(Path p)
    {
        // When a new path is calculated, reset currentWaypoint and reachedEndOfPath values
        if(!p.error) {
            path = p;
            currentWaypoint = 0;
            reachedEndOfPath = false;
        }
        else {
            Debug.LogWarning("Path calculation was canceled");
            path = null;
        }
    }

    private void Update()
    {
        // No path to follow yet
        if(path == null) {
            return;
        }

        // Check if enemy has reached the destination
        if(!reachedEndOfPath) {
            Collider2D playerIsMelee = Physics2D.OverlapCircle(endOfPathCheck.position, endOfPathCheckRadius, playerLayer);

            if(playerIsMelee || currentWaypoint >= path.vectorPath.Count) {
                reachedEndOfPath = true;
                return;
            }
        }
        else {
            return;
        }

        /*
         * Cast a Vector2 from the current rigidbody position to the next waypoint position, normalized (length of 1) to always maintain the right speed.
         * Move the enemy in the direction of the Vector just calculated.
         */
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        enemyMovement.TryMove(direction);

        /*
         * Calculate distance of enemy from current waypoint.
         * If enemy is too close to the current waypoint, go to the next.
         */
        float nextWaypointDistance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);
        if(nextWaypointDistance < waypointSwitchDistance) {
            currentWaypoint++;
        }
    }
}
