using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatSlot : MonoBehaviour
{
	public int FeatCode;
	public bool isClear;

	public Image CheckImage;
	public Text ExplanText;
	public Text CountText;

	public void Init(int code)
	{
		FeatCode = code;
		GameData.FeatData feat = Managers.GData.PlayerFeat[FeatCode];

		if (feat.CurCount >= feat.MaxCount)
		{
			isClear = true;
			CheckImage.enabled = true;
		}
		else
		{
			isClear = false;
			CheckImage.enabled = false;
		}

		ExplanText.text = feat.FeatExplan;
		CountText.text = $"{feat.CurCount}/{feat.MaxCount}";
	}



}
