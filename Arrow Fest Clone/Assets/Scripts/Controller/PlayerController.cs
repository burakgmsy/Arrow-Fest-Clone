using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Veriables
    public Transform target;
    public float _sensitivity = 0.005f;
    private bool _isSwerving;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _transfom = Vector3.zero;
    #endregion

    void FixedUpdate()
    {
        Swerving();
    }

    private void Swerving()
    {
        if (_isSwerving)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply move x left & right
            _transfom.x = (_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // move
            target.transform.Translate(_transfom);

            // store new mouse positionn
            _mouseReference = Input.mousePosition;
        }
    }
    
    void OnMouseDown()
    {
        // moving flag
        _isSwerving = true;

        // store mouse position
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // moving flag
        _isSwerving = false;
    }

}
