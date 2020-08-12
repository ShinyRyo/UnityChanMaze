using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
   public Animator animator;
   public float speed = 0.9f;
   public float speedDampTime = 0.1f;

   private readonly int hashSpeedPara = Animator.StringToHash("Speed");

   // Start is called before the first frame update
   void Start()
   {
       
   }
   　
   // Update is called once per frame
   private void Update()
   {
       transform.position += transform.forward * speed * Time.deltaTime;
       animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
   }
}