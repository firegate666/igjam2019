using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class PlanetOutlinePainter : MonoBehaviour
	{
		[SerializeField] private GameObject lineRendererPrefab;
		[SerializeField] private Transform lineContainer;
		private readonly List<LineRenderer> _lineRenderers = new List<LineRenderer>();

		public void DrawOutLineForPiece(PlanetPiece planetPiece)
		{
			float leftEdgeAngle = planetPiece.angleSize * planetPiece.indexOnRing;
			float rightEdgeAngle = planetPiece.angleSize * (planetPiece.indexOnRing + 1f);

			DrawCycleSegment(planetPiece.level, leftEdgeAngle, rightEdgeAngle);
			DrawRayEdge(planetPiece.level - 1, planetPiece.level, rightEdgeAngle);
			if (planetPiece.HasLeftNeighbor() == false)
			{
				DrawRayEdge(planetPiece.level - 1, planetPiece.level, leftEdgeAngle);
			}
		}

		private void DrawCycleSegment(float radius, float startAngle, float endAngle)
		{
			LineRenderer lineRenderer = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
			_lineRenderers.Add(lineRenderer);
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
		}

		private void DrawRayEdge(float startRadius, float endRadius, float angle)
		{
			LineRenderer lineRenderer = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
			_lineRenderers.Add(lineRenderer);
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
		}
	}
}