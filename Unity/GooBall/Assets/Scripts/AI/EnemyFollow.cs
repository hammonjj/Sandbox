using UnityEngine;
using Pathfinding;

public class EnemyFollow : MonoBehaviourBase
{
	[Header("Enemy Follow")]
	public float ChaseSpeed = 200f;
	public float NextWaypointDistance = 3f;

	[HideInInspector]
	public Transform TransformToFollow;

	private Path _path;
	private int _currentWaypoint = 0;
	private bool _reachedEndOfPath;

	private Seeker _seeker;
	private Rigidbody2D _rigidbody;
	

	private void Start()
	{
		_seeker = GetComponent<Seeker>();
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	public void EnableFollow(bool enable)
	{
		if(enable)
		{
			InvokeRepeating(nameof(_UpdatePath), 0f, 0.5f);
		}
		else
		{
			CancelInvoke(nameof(_UpdatePath));
		}
	}
	private void FixedUpdate()
	{
		if(_path == null)
		{
			return;
		}

		if(_currentWaypoint >= _path.vectorPath.Count)
		{
			_reachedEndOfPath = true;
			return;
		}

		_reachedEndOfPath = false;

		var direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody.position).normalized;
		_rigidbody.velocity = new Vector2(direction.x * ChaseSpeed, direction.y * ChaseSpeed);

		float distance = Vector2.Distance(_rigidbody.position, _path.vectorPath[_currentWaypoint]);
		if(distance < NextWaypointDistance)
		{
			_currentWaypoint++;
		}
	}

	private void _UpdatePath()
	{
		if(_seeker.IsDone())
		{
			_seeker.StartPath(_rigidbody.position, TransformToFollow.position, _OnPathComplete);
		}
	}

	private void _OnPathComplete(Path path)
	{
		if(!path.error)
		{
			_path = path;
			_currentWaypoint = 0;
		}
	}
}
