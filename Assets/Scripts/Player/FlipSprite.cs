using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator animator;

    // Update is called once per frame
    void LateUpdate()
    {
        sr.flipX = animator.GetBool("FlipX");
    }
}
