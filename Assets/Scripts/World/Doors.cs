using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void CloseDoor()
    {
        _animator.SetTrigger("Close");
        this.GetComponent<BoxCollider>().enabled = true;
    }

    private void DeactiveObject()
    {
        this.gameObject.SetActive(false);
    }
}
