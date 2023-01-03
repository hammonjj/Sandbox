using UnityEngine;

namespace EditorExtensions
{
    public class DrawAttackRange : MonoBehaviourBase
    {
        public EnemyBaseObj EnemyObj;

        private void OnDrawGizmos()
        {
            var ArcPos = new Vector3(
                gameObject.transform.position.x, 
                gameObject.transform.position.y, 
                gameObject.transform.position.z);

            DrawAttackRangeArc(gameObject.transform.position, ArcPos, 90, 1);
        }

        private void DrawAttackRangeArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
        {
            var srcAngles = GetAnglesFromDir(position, dir);
            var initialPos = position;
            var posA = initialPos;
            var stepAngles = anglesRange / maxSteps;
            var angle = srcAngles - anglesRange / 2;
            for(var i = 0; i <= maxSteps; i++)
            {
                var rad = Mathf.Deg2Rad * angle;
                var posB = initialPos;
                posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));

                Gizmos.DrawLine(posA, posB);

                angle += stepAngles;
                posA = posB;
            }

            Gizmos.DrawLine(posA, initialPos);
        }

        private float GetAnglesFromDir(Vector3 position, Vector3 dir)
        {
            var forwardLimitPos = position + dir;
            var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

            return srcAngles;
        }
    }
}

