// Copyright 2013 Gnonthgol
// see LICENSE.txt

using System.Collections.Generic;
using UnityEngine;

namespace NameReplacer
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class NameReplacer : MonoBehaviour
	{
		List<string> names = null;

		public void Update ()
		{
			if (names == null) {
				LoadNames ();
			}

			if (HighLogic.CurrentGame != null) {
				CrewRoster roster = HighLogic.CurrentGame.CrewRoster;
				KerbalApplicants applicants = roster.Applicants;

				foreach (ProtoCrewMember crew in applicants) {
					if (names.IndexOf (crew.name) == -1) {
						string name = GetRandomName (roster);
						print ("Changed name of " + crew.name + " to " + name);
						crew.name = name;
					}
				}
			}
		}

		public string GetRandomName (CrewRoster roster)
		{
			int i = 10;
			string name;
			do {
				name = names [Random.Range (0, names.Count)];
				i--;
			} while (i > 0 && NameInRoster (name, roster));

			return name;
		}

		private bool NameInRoster (string name, CrewRoster roster)
		{
			bool found = false;
			foreach (ProtoCrewMember crew in roster) {
				if (crew.name == name) {
					found = true;
				}
			}
			foreach (ProtoCrewMember crew in roster.Applicants) {
				if (crew.name == name) {
					found = true;
				}
			}
			return found;
		}

		public void LoadNames ()
		{
			names = new List<string> ();

			KSP.IO.TextReader reader = KSP.IO.TextReader.CreateForType<NameReplacer> ("names.txt");
			while (!reader.EndOfStream) {
				string name = reader.ReadLine ().Trim ();
				if (name != "") {
					names.Add (name);
				}
			}
			reader.Close ();
		}
	}
}

