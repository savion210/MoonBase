using UnityEngine;

public class RequiredPiece : MonoBehaviour
{
    [SerializeField]private TaskMaster associatedTask = null;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag is "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                associatedTask.requiredGameObject = true;
                Destroy(gameObject);
            }
        }
    }
}
