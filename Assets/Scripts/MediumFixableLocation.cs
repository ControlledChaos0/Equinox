using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumFixableLocation : MonoBehaviour
{
    [SerializeField]
    private GameObject targetGO;
    [SerializeField]
    private float distanceToLock = 10.0f;
    [SerializeField]
    private float moveTime = 1.5f;

    private Transform _targetTransform;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private LineRenderer _lineRenderer;
    private Quaternion _baseTargetRot;
    private Vector3 _baseTargetTrans;

    private bool locked;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
        if (targetGO != null) {
            transform.localScale = targetGO.transform.localScale;
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = targetGO.GetComponent<MeshFilter>().mesh;
        }
        #endif
        _targetTransform = targetGO.transform;
        _meshRenderer = targetGO.GetComponent<MeshRenderer>();
        //_lineRenderer = GetComponent<LineRenderer>();
        Vector3[] positions = {transform.position, targetGO.transform.position};
        _lineRenderer.SetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && distanceToLock > Vector3.Distance(transform.position, _meshRenderer.gameObject.transform.position)) {
            locked = true;
            _baseTargetRot = _targetTransform.rotation;
            _baseTargetTrans = _targetTransform.position;
            startTime = 0.0f;
        }
        if (locked) {
            _targetTransform.position = Vector3.Lerp(_baseTargetTrans, transform.position, startTime / moveTime);
            _targetTransform.rotation = Quaternion.Lerp(_baseTargetRot, transform.rotation, startTime / moveTime);
            startTime += Time.deltaTime;
        }
        if (startTime > moveTime) {
            Destroy(gameObject);
        }
    }
}
