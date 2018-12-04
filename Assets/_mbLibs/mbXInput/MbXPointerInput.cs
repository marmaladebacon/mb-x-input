using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MbXPointerInput {
	
	public const int PointerStateDown = 1;
	public const int PointerStateUp = 2;

	public static Vector3 V2toV3(Vector2 p){
		return new Vector3(p.x, p.y, 0f);
	}
	
	public static bool GetComponentOnPointer<T>(int pointerState, out Vector2 pos, out T result){
		result = default(T);
		if(MbXPointerInput.isPointerState(pointerState, out pos)){			
			if(MbXPointerInput.GetRaycastHitAtPosition<T>(MbXPointerInput.V2toV3(pos), out result)){
				return true;
			}
		}
		return false;
	}

	public static bool GetGenericComponent<T>(Transform target, out T result){
		result = target.GetComponent<T>();
		if(result == null){
			return false;
		}
		return true;
	}

	public static bool GetRaycastHitAtPosition<T>(Vector3 pos, out T result){
		result = default(T);
		var worldPoint = Camera.main.ScreenToWorldPoint(pos);
			RaycastHit2D hit = Physics2D.Raycast(
				new Vector2(worldPoint.x, worldPoint.y),
				Vector2.zero, 
				0f);
					
			if(hit.collider!=null && 
				MbXPointerInput.GetGenericComponent<T>(hit.collider.transform, out result)){
				return true;
			}
			
			return false;
	}
	public static bool isPointerState(int pointerState, out Vector2 pointerPos){
		pointerPos = Vector2.zero;
		switch(Application.platform){
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WebGLPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				if(PointerToMouse(pointerState, out pointerPos)){
					return true;
				}
				return false;
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				if(PointerToMobile(pointerState, out pointerPos)){
					Debug.Log(pointerPos);
					return true;
				}
				return false;
		}
		return false;
	}

	//Mouse btn 0 is left mouse btn
	public static bool PointerToMouse(int p, out Vector2 outPointerPos, int mouseBtn = 0){
		outPointerPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		switch(p){
			case PointerStateDown: return Input.GetMouseButtonDown(0);
			case PointerStateUp: return Input.GetMouseButtonUp(0);
			default:
			return false;
		}
	}
	public static bool PointerToMobile(int p, out Vector2 outPointerPos, int touchIndex = 0){
		outPointerPos = Input.GetTouch(touchIndex).position;		
		switch(p){
			case PointerStateDown: return Input.GetTouch(touchIndex).phase == TouchPhase.Began;
			case PointerStateUp: return Input.GetTouch(touchIndex).phase == TouchPhase.Ended;
			default:
			return false;
		}
	}
}