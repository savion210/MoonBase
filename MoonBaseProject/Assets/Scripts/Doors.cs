using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject doorPivot1;
    [SerializeField] private GameObject doorPivot2;

    Quaternion DoorClosedPivot1 = new Quaternion();
    Quaternion DoorClosedPivot2 = new Quaternion();
    Quaternion DoorOpenPivot1 = new Quaternion();
    Quaternion DoorOpenPivot2 = new Quaternion();
    

    public LayerMask layerMask;

    public bool isOpened = false;

    // this is the movement rate (if movemnt is applied to the door)
    public float moveSpeed = 3;
    // this is the rotation rate (if rotation is applied to the door)
    public float rotationSpeedPiv1 = 90;
    public float rotationSpeedPiv2 = -90;
    public float rotationSpeedClosed = 0;
    private void Start()
    {
        
        DoorClosedPivot1.Set(0, 0.7f, 0, 0.7f);
        DoorClosedPivot2.Set(0, 0.7f, 0, 0.7f);
        DoorOpenPivot1.Set(0, 1, 0,1);
        DoorOpenPivot2 .Set(0, 0, 0, 1);

        isOpened = false;
    }
    private void Update()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(gameObject.transform.position); //or whatever you're doing for your ray
        float distance = 1f; //however far your ray shoots
        int layerMask = 1 << 7; // "7" here needing to be replaced by whatever layer it is you're wanting to use
        layerMask = ~layerMask; //invert the mask so it targets all layers EXCEPT for this one

        if (Input.GetKeyDown(KeyCode.E))
        {
          
            
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                
                
                isOpened = !isOpened;

               

            }
            
        }
        //print(DoorClosedPivot1.ToString());

        // these actually do the moving/rotating
        //doorPivot1.transform.position = Vector3.MoveTowards(doorPivot1.transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        if (isOpened)
        {
            doorPivot1.transform.rotation = Quaternion.RotateTowards(doorPivot1.transform.rotation, DoorOpenPivot2, rotationSpeedPiv1 * Time.deltaTime);
            doorPivot2.transform.rotation = Quaternion.RotateTowards(doorPivot2.transform.rotation, DoorOpenPivot2, rotationSpeedPiv2 * Time.deltaTime);
        }
        else
        {
            doorPivot1.transform.rotation = Quaternion.RotateTowards(doorPivot1.transform.rotation, DoorClosedPivot1, rotationSpeedPiv1 * Time.deltaTime);
            doorPivot2.transform.rotation = Quaternion.RotateTowards(doorPivot2.transform.rotation, DoorClosedPivot2, rotationSpeedPiv1 * Time.deltaTime);
        }


    }
}