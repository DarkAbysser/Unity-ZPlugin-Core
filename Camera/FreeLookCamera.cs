/**********************************************************************
* 第三人称视角相机
* 该脚本需要挂在摄像机上
* 
* 基本操作：
* 1.设置目标跟随
* 2.鼠标移动来控制摄像机围绕目标的旋转
* 3.可设置是否隐藏鼠标
* 
* 防止画面抖动：
* 在 Update 计算 Target 位置
* PivotAction: 控制 Pivot 点的旋转,其子物体 Target 会绕着Pivot 旋转
* TargetAction: 控制 Target 相对 Pivot 的偏移(半径),实现视角缩放
* 
* 在 FixedUpdate 设置 Transform 为 Target 坐标和旋转(或插值到)
* 
* 视角限制:
* Pivot 控制摄像机旋转, 
* 其 EularAngle.X 的范围在 90~0,360~270 ,需要限制(不能从头顶,脚底看)
* 如果 旋转x轴后x不在限定区域,则回退旋转x的操作。
***********************************************************************/
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeLookCamera : MonoBehaviour
{

    [SerializeField] public Transform Target;

    [SerializeField] float _cameraRotateSpeed = 3;
    [SerializeField] Vector3 _pivotOffset = new Vector3(0, 2, 0);
    [SerializeField] Vector3 _targetOffset = new Vector3(0, 0.2f, -3);

    [SerializeField] float MaxAngleX = 60;
    [SerializeField] float MinAngleX = -60;

    public bool Workable = true;
    public bool LockCursor = true;

    Transform cameraTarget;//摄像机最终的位移坐标
    Transform cameraPivot;//摄像机面朝的点,并且这个点随着目标移动
    void Start()
    {
        SetTarget(Target);
    }

    void Update()
    {
        if (Target != null && Workable)
        {
            PivotAction();
            TargetAction();
        }
        
        Cursor.lockState = LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !LockCursor;
    }

    public void SetTarget(Transform target)
    {
        if (target == null)
            return;

        cameraPivot = new GameObject("CameraPivot").transform;
        cameraPivot.position = Target.position + _pivotOffset;

        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.SetParent(cameraPivot);
    }

    public void RemoveTarget()
    {
        if (Target == null)
            return;

        Destroy(cameraPivot.gameObject);
        cameraPivot = null;

        Destroy(cameraTarget.gameObject);
        cameraTarget = null;
    }

    private void TargetAction()
    {
        float scrollWheel = -Input.GetAxis("Mouse ScrollWheel");
        _targetOffset += _targetOffset * scrollWheel;
        cameraTarget.localPosition = _targetOffset;
    }

    private void PivotAction()
    {
        float m_x = Input.GetAxis("Mouse X");
        float m_y = Input.GetAxis("Mouse Y");

        cameraPivot.position = Target.position + _pivotOffset;

        Quaternion rotateBefor = cameraPivot.rotation;
        cameraPivot.Rotate(Vector3.left, m_y * _cameraRotateSpeed);

        //X = 90~0,360~270 
        //Check X no legal back to before
        Vector3 angle = cameraPivot.eulerAngles;
        if ((angle.x > MaxAngleX && angle.x < 180) ||
            (angle.x < 360 + MinAngleX && angle.x > 180))
            cameraPivot.rotation = rotateBefor;
        cameraPivot.Rotate(Vector3.up, m_x * _cameraRotateSpeed);
        cameraPivot.eulerAngles = Vector3.Scale(cameraPivot.rotation.eulerAngles, Vector3.right + Vector3.up);
    }

    private void MoveToTarget()
    {
        //transform.position = cameraTarget.position;
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, 0.3f);
    }

    private void FixedUpdate()
    {
        if (Target != null && Workable)
        {
            MoveToTarget();
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraTarget.rotation, 0.5f);
    }
}
