using UnityEngine;

public class puppet_controller : MonoBehaviour
{
    [SerializeField] private puppet_parts parts = new puppet_parts();
    [SerializeField] private finger_target_set targetSet = new finger_target_set();
    [SerializeField] private puppet_target_driver targetDriver = new puppet_target_driver();
    [SerializeField] private puppet_balance_assist balanceAssist = new puppet_balance_assist();
    [SerializeField] private puppet_limb_handle leftHandHandle = new puppet_limb_handle();
    [SerializeField] private puppet_limb_handle rightHandHandle = new puppet_limb_handle();
    [SerializeField] private puppet_limb_handle leftFootHandle = new puppet_limb_handle();
    [SerializeField] private puppet_limb_handle rightFootHandle = new puppet_limb_handle();
    [SerializeField] private puppet_limb_handle headHandle = new puppet_limb_handle();

    private void Awake()
    {
        targetDriver.initialize();
        leftHandHandle.initialize();
        rightHandHandle.initialize();
        leftFootHandle.initialize();
        rightFootHandle.initialize();
        headHandle.initialize();
    }

    private void FixedUpdate()
    {
        targetDriver.updateTargets(targetSet, Time.fixedDeltaTime);
        balanceAssist.apply(parts, headHandle);
        leftHandHandle.updateHandle(targetDriver.is_left_hand_target_active());
        rightHandHandle.updateHandle(targetDriver.is_right_hand_target_active());
        leftFootHandle.updateHandle(targetDriver.is_left_foot_target_active());
        rightFootHandle.updateHandle(targetDriver.is_right_foot_target_active());
        headHandle.updateHandle(targetDriver.is_head_target_active());
    }
}
