using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Comment : MonoBehaviour
{
	[InfoBox("$comment")]
	[SerializeField]
	bool editComment = false;


	[TextArea(3,10)]
	[ShowIf("$editComment")]
	[HideLabel]
	public string comment;
}
