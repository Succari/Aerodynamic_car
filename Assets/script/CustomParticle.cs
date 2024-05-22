using UnityEngine;

public class CustomParticle : MonoBehaviour
{
    public Vector3 velocity;
    public float lifetime = 10f;

    public bool IsDead(){
        return lifetime <= 0;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
        transform.position += 10 * Time.deltaTime * velocity;
        
        
        lifetime -= Time.deltaTime;
        if (IsDead()) Destroy(gameObject);
        
    }
}
