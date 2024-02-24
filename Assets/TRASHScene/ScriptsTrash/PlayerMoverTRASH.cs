using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoverTRASH : MonoBehaviour
{
    [SerializeField] private float _moveForce = 1f;

    [SerializeField] private float _rotationForce = 0.05f; // если не умножать ввод на дельту тайм, то значения вот такие мизерные, это нормально?

    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rigidbody;
    private float _forwardInput;
    private float _sideInput;

    private void OnValidate()
    {
        _rigidbody ??= GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //ReadInput();
        ReadInput2();
    }

    private void FixedUpdate()
    {
        MoveLikePlane(); // все ок, летит нормально

        //MoveLikePerson(); // Вариант 2 с видео, которое я скинул в комментах к домаше 3 недели
        
        //MoveLikePerson2(); // Вариант 1 с видео, которое я скинул в комментах к домаше 3 недели
    }

    private void MoveLikePlane()
    {
        _rigidbody.AddRelativeForce(0f, 0f, _forwardInput * _moveForce, ForceMode.VelocityChange);
        _rigidbody.AddRelativeTorque(_sideInput * _rotationForce, 0f, 0f, ForceMode.VelocityChange);
    }

    private void MoveLikePerson() 
    {
        //При такой записи, как мы делали с игроком, который ходит по земле - ничего не работает, голову сломал, почему так
        //управление искажено и полет совершенно некорректен

        Vector3 offcet = new Vector3(0f, 0f, _forwardInput) * _moveSpeed; 

        offcet.y = _rigidbody.velocity.y;

        Vector3 worldVelocity = transform.TransformVector(offcet);

        _rigidbody.velocity = worldVelocity;
        
        Vector3 rotation = new Vector3(_sideInput * _rotationSpeed, 0f, 0f);

        _rigidbody.angularVelocity = rotation; // вот здесь полный неадекват
    }
    
    private void MoveLikePerson2() 
    {
        //Здесь мы вообще не летим вперед

        float zCoord = Mathf.Clamp(_forwardInput, 0f, 1f);// Mathf.Clamp - и с ним и без него проблемы

        Vector3 offcet = new Vector3(0f, 0f, zCoord) * _moveSpeed; 

        offcet.y = _rigidbody.velocity.y;

        Vector3 worldVelocity = transform.TransformVector(offcet); //перевели лок в глоб, все ок

        _rigidbody.velocity = worldVelocity;
        
        Vector3 rotation = new Vector3(_sideInput * _rotationSpeed, 0f, 0f);
        
        Vector3 worldRotation = transform.TransformVector(rotation);

        _rigidbody.angularVelocity = worldRotation; // вот здесь полный неадекват
    }
    
    private void ReadInput2()
    {
        _forwardInput = Input.GetAxisRaw("Slope(Vertical)");
        _sideInput = Input.GetAxisRaw("Spin(Horizontal)");
    }
}