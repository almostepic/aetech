using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons
{
	public class PlayerStats : Singleton<PlayerStats> {
		// this should just be a JSON doc from dynamo
		public int level {get; set;}
		public int highestScore {get; set;}

		protected PlayerStats() {
			ResetStats ();
		}
			
		public void LoadStats() {
			Debug.Log ("LoadStats");
		}

		public void ResetStats() {
			level = 0;
			highestScore = 0;
		}
	}
}
