using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D collider;
    private CollisionInfo collisionInfo;
    [SerializeField]
    private LayerMask collisionMask;
    const float SKINWIDTH = .5f;
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector2 velocity) {
        //detect collisions
        DetectCollisions(ref velocity);
        transform.Translate(velocity);
    }

    public void Update() {
        collisionInfo.GetBounds(collider);
    }

    public void  DetectCollisions(ref Vector2 velocity) {
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        int numHorizontalRays = 5;
        //Vertical
        if(directionY != 0) {
            float rayLength = Mathf.Abs(velocity.y) + SKINWIDTH;

            Vector2 raycastOrigin = directionY > 0 ? collisionInfo.topLeft : collisionInfo.bottomLeft;
            float raySpacing = (collisionInfo.topRight.x - collisionInfo.topLeft.x)/ numHorizontalRays;
            for(int i = 0; i < numHorizontalRays; i++) {
                Vector2 start = raycastOrigin + (Vector2.right * i * raySpacing);
                RaycastHit2D hit = Physics2D.Raycast(start,Vector2.up * directionY, rayLength, collisionMask);
                Debug.DrawRay(start, Vector2.up * directionY * rayLength, Color.red);
                if(hit) {
                    velocity.y = (hit.distance - SKINWIDTH) * directionY;
                }
            }
        }

        //Horizontal
        if(directionX != 0) {
            float rayLength = Mathf.Abs(velocity.x) + SKINWIDTH;

            Vector2 raycastOrigin = directionY > 0 ? collisionInfo.bottomRight : collisionInfo.bottomLeft;
            float raySpacing = (collisionInfo.topLeft.y - collisionInfo.bottomLeft.y)/ numHorizontalRays;
            for(int i = 0; i < numHorizontalRays; i++) {
                Vector2 start = raycastOrigin + (Vector2.up * i * raySpacing);
                RaycastHit2D hit = Physics2D.Raycast(start,Vector2.right * directionX, rayLength, collisionMask);
                Debug.DrawRay(start, Vector2.right * directionX * rayLength, Color.red);
                if(hit) {
                    velocity.x = (hit.distance - SKINWIDTH) * directionX;
                }
            }        }
    }

    struct CollisionInfo {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

        private void Reset() {
            this.topLeft = Vector2.zero;
            this.topRight = Vector2.zero;
            this.bottomLeft = Vector2.zero;
            this.bottomRight = Vector2.zero;
        }

        public void GetBounds(BoxCollider2D collider) {
            Bounds bounds = collider.bounds;
            bounds.Expand(-2*SKINWIDTH);

            topLeft = new Vector2(bounds.min.x, bounds.max.y);
            topRight = new Vector2(bounds.max.x, bounds.max.y);
            bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        } 
    }
}
