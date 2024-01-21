using Cinemachine;
using UnityEngine;

public class CinemachineTimedReprioritizer : CinemachineExtension
{
	/// <summary>
	/// When to apply the adjustment
	/// </summary>
	[Tooltip("When to apply the adjustment")]
	public CinemachineCore.Stage _applyAfter;

	[SerializeField, Min(0.1f)] private float _delay = 5;
	[SerializeField] private int _newPriority = 0;

	private float _timeElapsed;
	private CinemachineBrain _brain;

	private void Start()
	{
		_brain = CinemachineCore.Instance.GetActiveBrain(0);
	}

	private void Reset()
	{
		_applyAfter = CinemachineCore.Stage.Finalize;
		_delay = 5;
		_newPriority = 0;
	}

	private void Update()
	{
		if (_brain.IsBlending || _brain.ActiveVirtualCamera.VirtualCameraGameObject != gameObject)
			return;

		_timeElapsed += Time.deltaTime;
	}

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (_timeElapsed > _delay && stage == _applyAfter)
		{
			vcam.Priority = _newPriority;
			enabled = false;
		}
	}
}
