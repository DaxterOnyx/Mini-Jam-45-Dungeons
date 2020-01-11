using UnityEngine;

public class PopupManager : MonoBehaviour
{
	public PopupData data;

	public void PopupValue(string value, PopupData.Style style)
	{
		Popup popup = Instantiate(data.PopupPrefab,transform.position,Quaternion.identity,transform).GetComponent<Popup>();
		popup.text.text = value;
		popup.text.color = data.styles[(int)style].Color;
		popup.text.fontSize = (int)data.styles[(int)style].Size * data.DefaultSize;
		Destroy(popup.gameObject, data.styles[(int)style].LifeTime * data.DefaultLifeTime) ;
	}
}
