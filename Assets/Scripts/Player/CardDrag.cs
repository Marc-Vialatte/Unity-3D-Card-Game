using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrag : MonoBehaviour
{
	private Vector3 screenPoint;
	private Vector3 offset;

	private Ray ray;
	private RaycastHit hitData;

	private void Update()
    {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hitData, 1000) && Input.GetMouseButton(1))
		{
			OnMouseRightDown(hitData.transform.gameObject);
			OnMouseRightDrag();
		}
    }

    void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseRightDown(GameObject gameObject)
    {
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag()
	{
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}

	void OnMouseRightDrag()
    {
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		//transform.rotation = Quaternion.LookRotation(cursorPosition - transform.position);
		transform.LookAt(cursorPosition);
	}

    void OnMouseUp()
    {
		GameManager.Instance.MoveChampion();
    }
}
