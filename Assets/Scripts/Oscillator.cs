using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(1,10)]
    [SerializeField] float period = 2f;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float cycles = Time.time / period;  // grows continuesly from start
        const float tau = Mathf.PI * 2;
        float rawSin = Mathf.Sin(cycles * tau);
        float movementFactor = rawSin / 2f + .5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
