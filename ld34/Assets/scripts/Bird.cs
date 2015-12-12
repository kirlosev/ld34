using UnityEngine;
using System.Collections;

public class Bird : Character {
    public Transform wings;
    
    public float defMoveSpeed = 3f;
    public float maxMoveSpeed = 4f;
    public float viewRadius = 2f;
    public Vector3 velocity;
    public float moveSpeed;

    public bool showDebug = false;
    public bool moveToMouse = false;

    public LayerMask birdLayer = 1 << 8;
    public LayerMask obstacleLayer = 1 << 9;
    public LayerMask playerLayer = 1 << 10;

    public bool infiniteSpace = false;

    protected void Start() {
        base.Start();
        BirdManager.instance.dangerDistance = size.x * 4f;
        moveSpeed = defMoveSpeed + defMoveSpeed * Random.Range(0.1f, 0.4f);
        StartCoroutine(beSwarm());
    }

    void Update() {
        if (velocity.magnitude > maxMoveSpeed)
            velocity = velocity.normalized * maxMoveSpeed;

        if (float.IsNaN(velocity.x) && float.IsNaN(velocity.y) && float.IsNaN(velocity.z)) {
            velocity = Random.insideUnitCircle * viewRadius; // TODO : remove debug
        }

        if (showDebug)
            Debug.DrawRay(transform.position, velocity, Color.blue);

        if (!float.IsNaN(velocity.x) && !float.IsNaN(velocity.y) && !float.IsNaN(velocity.z)) {
            wings.localScale = Vector3.right + Vector3.up * Mathf.Clamp(Mathf.Abs(transform.right.y), 0, 1);
            
            transform.position += velocity * moveSpeed * Time.deltaTime;

            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
/*
        if (infiniteSpace) {
            if (transform.position.x < Level.inst.lbCorner.position.x ||
                transform.position.x > Level.inst.rtCorner.position.x ||
                transform.position.y < Level.inst.lbCorner.position.y ||
                transform.position.y > Level.inst.rtCorner.position.y) {
                transform.position *= -1;
                transform.position += velocity * 0.3f;
            }
        } else {
            if (transform.position.x < Level.inst.lbCorner.position.x ||
                transform.position.x > Level.inst.rtCorner.position.x) {
                velocity.x *= -1 * BirdManager.instance.reactionSpeed * 10;
            }
            if (transform.position.y < Level.inst.lbCorner.position.y ||
                transform.position.y > Level.inst.rtCorner.position.y) {
                velocity.y *= -1 * BirdManager.instance.reactionSpeed * 10;
            }
        }
*/        
    }

    IEnumerator beSwarm() {
        while (true) {
            Vector3 playerDir = Player.inst.transform.position - transform.position;
            velocity += playerDir * BirdManager.instance.foodWeight * Time.deltaTime;
            
            if (transform.position.y < 0) {
                velocity += Vector3.up *velocity.magnitude *10* Time.deltaTime;
            }
            
            var birdsAround = Physics2D.OverlapCircleAll(transform.position, viewRadius, birdLayer);
            if (birdsAround.Length > 1) {
                var centerMassDirection = MoveToCenterMass(birdsAround);
                velocity += centerMassDirection * BirdManager.instance.reactionSpeed * Time.deltaTime;
                yield return null;

                var avoidNeighs = AvoidNeighs(birdsAround);
                velocity += avoidNeighs * BirdManager.instance.reactionSpeed * Time.deltaTime;
                yield return null;

                var affectedVelocity = CalcVelocity(birdsAround);
                velocity += affectedVelocity * BirdManager.instance.reactionSpeed * Time.deltaTime;
                yield return null;

                var avoidObstacles = AvoidObstacles();
                velocity += avoidObstacles * BirdManager.instance.reactionSpeed * Time.deltaTime;
                yield return null;
/*
                var foodDirection = CheckPlayer();
                velocity += foodDirection * BirdManager.instance.reactionSpeed * Time.deltaTime;
                yield return null;
*/                
            }
            yield return null;
        }
    }

    Vector3 MoveToCenterMass(Collider2D[] neighs) {
        var centerMass = Vector3.zero;
        for (var i = 0; i < neighs.Length; ++i) {
            if (neighs[i].gameObject == this.gameObject)
                continue;

            centerMass += neighs[i].transform.position;

            if (showDebug)
                Debug.DrawLine(transform.position, neighs[i].transform.position, Color.magenta);
        }
        centerMass /= (neighs.Length - 1);

        if (showDebug)
            Debug.DrawLine(transform.position, centerMass, Color.yellow);

        return (centerMass - transform.position) * BirdManager.instance.centerMassWeight;
    }

    Vector3 AvoidNeighs(Collider2D[] neighs) {
        var sumBirdDir = Vector3.zero;
        for (var i = 0; i < neighs.Length; ++i) {
            if (neighs[i].gameObject == this.gameObject)
                continue;

            var neighDir = neighs[i].transform.position - transform.position;
            if (neighDir.magnitude < 0.7f) {
                sumBirdDir += neighDir;
            }
        }

        if (showDebug)
            Debug.DrawRay(transform.position, -sumBirdDir, Color.cyan);

        return -sumBirdDir * BirdManager.instance.avoidBirdsWeight;
    }

    Vector3 CalcVelocity(Collider2D[] neighs) {
        var targetVelocity = Vector3.zero;
        for (var i = 0; i < neighs.Length; ++i) {
            if (neighs[i].gameObject == this.gameObject)
                continue;

            var bird = neighs[i].GetComponent<Bird>();
            targetVelocity += bird.velocity;
        }
        if (targetVelocity.magnitude != 0)
            targetVelocity /= (neighs.Length - 1);

        if (showDebug)
            Debug.DrawRay(transform.position, targetVelocity, Color.red);

        return targetVelocity * BirdManager.instance.velocityWeight;
    }

    Vector3 AvoidObstacles() {
        var obstaclesAround = Physics2D.OverlapCircleAll(transform.position, BirdManager.instance.dangerDistance, obstacleLayer | enemyMask);
        var sumBirdDir = Vector3.zero;
        for (var i = 0; i < obstaclesAround.Length; ++i) {
            if (obstaclesAround[i].gameObject == this.gameObject)
                continue;

            var neighDir = obstaclesAround[i].transform.position - transform.position;
            if (neighDir.magnitude - obstaclesAround[i].bounds.extents.x < BirdManager.instance.dangerDistance) {
                var rayObstacle = Physics2D.Raycast(transform.position, neighDir, BirdManager.instance.dangerDistance, obstacleLayer | enemyMask);
                var reflection = -2 * Vector3.Dot(velocity, rayObstacle.normal) * (Vector3)rayObstacle.normal + velocity;
                sumBirdDir += reflection;
            }
        }

        if (showDebug)
            Debug.DrawRay(transform.position, -sumBirdDir, Color.white);

        return sumBirdDir * BirdManager.instance.aboidObstacleWeight;
    }

    Vector3 CheckPlayer() {
        var foodAround = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerLayer);
        for (var i = 0; i < foodAround.Length; ++i) {
            if (foodAround[i].gameObject == this.gameObject)
                continue;

            var foodDir = foodAround[i].transform.position - transform.position;
            if (showDebug)
                Debug.DrawRay(transform.position, foodDir, Color.white);
            return foodDir * BirdManager.instance.foodWeight;
        }
        return Vector3.zero;
    }

    public void init(Vector3 initPos, Vector3 initDir) {
        moveSpeed = defMoveSpeed;
        transform.position = initPos;
        velocity = initDir * moveSpeed;
    }

    public override void Damage(float value) {
        health -= value;
        if (health <= 0) {
            var scaryCircle = ObjPool.inst.getScaryCircle();
            scaryCircle.gameObject.SetActive(true);
            scaryCircle.init(transform.position, Mathf.Clamp(Random.value * 10, 1, 1.5f));
            
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos() {
        if (showDebug) {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
        }
    }
}
