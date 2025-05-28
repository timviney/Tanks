using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour 
{
    void Start() {
        var anim = GetComponent<Animator>();
        var animLength = anim.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animLength); 
    }
}