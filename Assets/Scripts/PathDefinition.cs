using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefinition : MonoBehaviour {

public Transform[] Points;
    private bool loopPath = true;

	public IEnumerator<Transform> GetPathEnumerator(){
		if (Points == null || Points.Length < 1)
			yield break;

		var direction = 1;
		var index = 0;
		while (loopPath) {
            yield return Points[index];
			if(Points.Length == 1)
				continue;
            if (index <= 0)
                direction = 1;
            else if (index >= Points.Length - 1)
            {
				direction = -1;
            }
			index = index + direction;

		}
	}
}
