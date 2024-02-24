using TMPro;
using UnityEngine;

public class AirplaneMover : MonoBehaviour
{
    [SerializeField] private float _powerIncrement = 0.1f;
    [SerializeField] private float _maxPower = 45f;
    [SerializeField] private float _slopeSensetivity = 3f;
    [SerializeField] private float _turnSensetivity = 3f;
    [SerializeField] private float _spinSensetivity = 1.5f;
    [SerializeField] private float _liftPower = 0.3f;

    [SerializeField] private TextMeshProUGUI _stats;

    private Rigidbody _rigidbody;

    private float _targetEnginePower;
    private float _noseSpin; // вращение по своей оси
    private float _noseSlope; // уклон носа  самолета 
    private float _bodyTurn; // поворот корпуса

    public float EnginePower => _targetEnginePower;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadInput();
        
        StatsUpdate();

        //Debug.Log(_rigidbody.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        ApplyForces(); // дельты тайм не нужны для addForce & Torque
    }

    private void ReadInput()
    {
        _noseSpin = Input.GetAxis("Spin(Horizontal)");
        _noseSlope = Input.GetAxis("Slope(Vertical)");
        _bodyTurn = Input.GetAxis("Turn");

        if (Input.GetKey(KeyCode.Space))
        {
            _targetEnginePower += _powerIncrement;
        }

        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _targetEnginePower -= _powerIncrement;
        }

        _targetEnginePower = Mathf.Clamp(_targetEnginePower, 0f, _maxPower);
    }

    private void ApplyForces()
    {
        _rigidbody.AddTorque(transform.up * (_bodyTurn * _turnSensetivity), ForceMode.Acceleration); // не зависим от массы, но зависим от времени рассчета физики
        _rigidbody.AddTorque(transform.right * (_noseSlope * _slopeSensetivity), ForceMode.Acceleration);
        _rigidbody.AddTorque(-transform.forward * (_noseSpin * _spinSensetivity), ForceMode.Acceleration);
        
        _rigidbody.AddForce(transform.forward * _targetEnginePower, ForceMode.Acceleration); // движение вперед

        _rigidbody.AddForce(Vector3.up * (_rigidbody.velocity.magnitude * _liftPower), ForceMode.Acceleration); // добавляем небольшую подъемную силу
    }

    private void StatsUpdate()
    {
        _stats.text = $"Engine Power:  {(_targetEnginePower / _maxPower * 100).ToString("0")} % \n";
        _stats.text += $"Airspeed:  {(_rigidbody.velocity.magnitude * 3.6f).ToString("0")} km/h \n";
        _stats.text += $"Altitude:  {transform.position.y.ToString("0")} m \n";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.2f);
    }
}