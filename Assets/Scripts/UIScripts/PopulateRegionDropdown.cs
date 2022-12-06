using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Slightly Modified Version taken out of the Photon Fusion Golf Example

namespace UIScripts
{
	public class PopulateRegionDropdown : MonoBehaviour
	{
		readonly string[] optionsKeys = { "USA East", "Europe", "Asia", "Japan", "South America", "South Korea" };
		readonly string[] optionsValues = { "us", "eu", "asia", "jp", "sa", "kr" };

		private void Awake()
		{
			if (TryGetComponent(out TMP_Dropdown dropdown))
			{
				dropdown.AddOptions(new List<string>(optionsKeys));
				dropdown.onValueChanged.AddListener((index) =>
				{
					Fusion.Photon.Realtime.PhotonAppSettings.Instance.AppSettings.FixedRegion = optionsValues[index];
				});

				string curRegion = Fusion.Photon.Realtime.PhotonAppSettings.Instance.AppSettings.FixedRegion;
				Debug.Log($"Initial region is {curRegion}");

				int curIndex = optionsValues.ToList().IndexOf(curRegion);
				dropdown.value = curIndex != -1 ? curIndex : 0;
			}
		}
	}
}
