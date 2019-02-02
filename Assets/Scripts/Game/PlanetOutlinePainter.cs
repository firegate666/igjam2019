using UnityEngine;

namespace DefaultNamespace
{
	public class PlanetOutlinePainter : MonoBehaviour
	{
		[SerializeField] private GameObject lineRendererPrefab;
		[SerializeField] private Transform lineContainer;
		[SerializeField] private float size = 1;

		public void DrawOutLineForPiece(PlanetPiece planetPiece)
		{
			if (planetPiece.element == Elements.NotSet)
			{
				if (planetPiece.viewOutline != null)
				{
					Destroy(planetPiece.viewOutline);
					planetPiece.viewOutline = null;
				}

				return;
			}

			if (planetPiece.viewOutline != null)
			{
				return;
			}

			float leftEdgeAngle = planetPiece.angleSize * planetPiece.indexOnRing;
			float rightEdgeAngle = planetPiece.angleSize * (planetPiece.indexOnRing + 1f);

			Transform cycleSegment = DrawCycleSegment(planetPiece.level *size, leftEdgeAngle, rightEdgeAngle);
			
			Transform rightEdge = DrawRayEdge((planetPiece.level - 1) *size, planetPiece.level *size, rightEdgeAngle);
			rightEdge.parent = cycleSegment;
			
			if (planetPiece.HasLeftNeighbor() == false)
			{
				Transform leftEdge = DrawRayEdge((planetPiece.level - 1) *size, planetPiece.level *size, leftEdgeAngle);
				leftEdge.parent = cycleSegment;
			}

			planetPiece.viewOutline = cycleSegment.gameObject;
		}

		private Transform DrawCycleSegment(float radius, float startAngle, float endAngle)
		{
			Transform cycleSegment = Instantiate(lineRendererPrefab).transform;
			LineRenderer lineRenderer = cycleSegment.GetComponent<LineRenderer>();
			lineRenderer.transform.parent = lineContainer;

			lineRenderer.positionCount = GlobalConfig.CycleSegmentOutlineSoftness + 1;
			for (int i = 0; i <= GlobalConfig.CycleSegmentOutlineSoftness; i++)
			{
				float stepAngle = ((endAngle - startAngle) / GlobalConfig.CycleSegmentOutlineSoftness) * i + startAngle;
				lineRenderer.SetPosition(
					i,
					new Vector3(Mathf.Sin(stepAngle * Mathf.Deg2Rad), Mathf.Cos(stepAngle * Mathf.Deg2Rad), 0f) * radius
				);
			}

			return cycleSegment;
		}

		private Transform DrawRayEdge(float startRadius, float endRadius, float angle)
		{
			Transform rayEdge = Instantiate(lineRendererPrefab).transform;
			LineRenderer lineRenderer = rayEdge.GetComponent<LineRenderer>();
			lineRenderer.transform.parent = lineContainer;

			lineRenderer.positionCount = 4;

			Vector3 unitVectorOnAngle = new Vector3(
				Mathf.Sin(angle * Mathf.Deg2Rad),
				Mathf.Cos(angle * Mathf.Deg2Rad),
				0f
			);

			lineRenderer.SetPosition(
				0,
				unitVectorOnAngle * startRadius
			);
			lineRenderer.SetPosition(
				1,
				unitVectorOnAngle * (startRadius * .8f + endRadius * .2f)
			);
			lineRenderer.SetPosition(
				2,
				unitVectorOnAngle * (startRadius * .2f + endRadius * .8f)
			);
			lineRenderer.SetPosition(
				3,
				unitVectorOnAngle * endRadius
			);

			return rayEdge;
		}

		public void Reset()
		{
			StopAllCoroutines();
			foreach (Transform child in lineContainer.transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}