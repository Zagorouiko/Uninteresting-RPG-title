using UnityEngine;
using UnityEngine.EventSystems;
using Dragon.Character;

namespace Dragon.CameraUI {
	public class CameraRaycaster : MonoBehaviour
	{
		float maxRaycastDepth = 100f; // Hard coded value
		const int WALKABLE_LAYER = 8;

		public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverWalkable;

		public delegate void OnMouseOverEnemy(Enemy enemy);
		public event OnMouseOverEnemy onMouseOverEnemy;

		[SerializeField] Texture2D enemyCursor = null;
		[SerializeField] Texture2D walkCursor = null;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

		Rect screenRectOnConstruction = new Rect();

		void Update()
		{
			screenRectOnConstruction = new Rect(0, 0, Screen.width, Screen.height);
			// Check if pointer is over an interactable UI element
			if (EventSystem.current.IsPointerOverGameObject())
			{
				// Impliment UI interaction
			} else
			{
				PerformRaycasts();
			}			
		}

		void PerformRaycasts()
		{
			if (screenRectOnConstruction.Contains(Input.mousePosition))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (RaycastforEnemy(ray)) { return; }
				if (RaycastforWalkable(ray)) { return; }
			}		
		}

		bool RaycastforEnemy(Ray ray)
		{
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
			if (hitInfo.collider.gameObject == null)
			{
				return false;
			}
			var gameObjectHit = hitInfo.collider.gameObject;
			var enemyHit = gameObjectHit.GetComponent<Enemy>();
			if (enemyHit)
			{
				Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverEnemy(enemyHit);
				return true;
			}
			return false;
		}

		private bool RaycastforWalkable(Ray ray)
		{
			RaycastHit hitInfo;
			LayerMask walkableLayer = 1 << WALKABLE_LAYER;
			bool walkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, walkableLayer);
			if (walkableHit)
			{
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverWalkable(hitInfo.point);
				return true;
			}
			return false;			
		}
	}
}
