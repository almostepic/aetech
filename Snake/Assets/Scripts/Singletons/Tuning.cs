using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

namespace Singletons
{
	public class Tuning : Singleton<Tuning> {

		Dictionary<string, JSONNode> mTuningDict = new Dictionary<string, JSONNode>();

		public void LoadTuningFromDisk()
		{
			// these should be cached from a the web either in memory on disk
			LoadTuningFile("levels");
		}

		public JSONNode Get(string tuningName)
		{
			if(mTuningDict.ContainsKey(tuningName))
				return mTuningDict [tuningName];
			return null;
		}

		bool LoadTuningFile(string tuningName)
		{
			TextAsset targetFile = Resources.Load<TextAsset> (Path.Combine ("Tuning", tuningName));
			if (targetFile == null)
			{
				Debug.AssertFormat (false, "Tuning: " + tuningName + " could not be loaded.");
				return false;
			}

			JSONNode j = JSON.Parse (targetFile.text);
			if (j == null)
			{
				Debug.AssertFormat (false, "Tuning: " + tuningName + " could not be parsed as JSON.");
				return false;
			}

			mTuningDict [tuningName] = j;

			return true;
		}
	}
}
