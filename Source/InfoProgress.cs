// Copyright (C) 2016    Chang Spivey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software Foundation,
// Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
//
using System;
using System.Collections.Generic;

namespace subtitleMemorize
{
	/// <summary>
	/// Information for progress dialog. This is independent of any toolkit because it a handler. Progress
	/// information is divided into two types:
	///
	/// a)	the section, which describes what is currently done.
	/// 	The section is a string like "Episode 01 - Extracting subtitle"
	/// 	Every section has...
	/// b)	... a number of steps. This is the information how much of the section is handled.
	/// 	An example is the number of processed lines when matching. Each section is finished
	/// 	after the number of processed steps is the same as the number of given steps for
	/// 	this section.
	///
	///
	/// Every section has the same share of the total 100%. There are some section fully completed, then there is the "active" section and then
	/// sections, that are 0% complete.
	/// Every of the n sections has 1/n part in the progress bar. In-between steps are determined by the sub-steps in the active section.
	/// </summary>
	public class InfoProgress
	{

		/// <summary>
		/// A section in the whole process. See documentation of InfoProgress for more detail.
		/// </summary>
		private class ProgressSection
		{
			private String name; // "Episode 01 - Extracting subtitle"
			private int numberOfStepsProcessed; // between 0 and numberOfStepsGiven
			private int numberOfStepsGiven;

			public String Name {
				get { return name; }
			}

			public int NumbersOfStepsProcessed {
				get { return numberOfStepsProcessed; }
			}

			public int NumbersOfStepsGiven {
				get { return numberOfStepsGiven; }
				set {
					numberOfStepsGiven = value;
					if (numberOfStepsProcessed > numberOfStepsGiven)
						numberOfStepsProcessed = numberOfStepsGiven;
				}
			}

			public ProgressSection(String name, int numberOfSteps) {
				this.name = name;
				this.numberOfStepsGiven = numberOfSteps;
				this.numberOfStepsProcessed = 0;
			}

			/// <summary>
			/// Increase counter for processed steps. Returns the number of
			/// steps that were really increased (in case "new steps + saved steps > max steps").
			/// </summary>
			/// <returns>The processed steps.</returns>
			/// <param name="steps">Steps.</param>
			public int AddProcessedSteps(int steps) {
				numberOfStepsProcessed += steps;

				int restSteps = 0;
				if (numberOfStepsProcessed > numberOfStepsGiven) {
					restSteps = numberOfStepsProcessed - numberOfStepsGiven;
					numberOfStepsProcessed = numberOfStepsGiven;
				}
				return restSteps;
			}
		}

		private int m_currentSection = 0;
		private readonly List<ProgressSection> m_progressSections = new List<ProgressSection>();
		private readonly Action<String, double> m_setProgressHandler = null;
		private bool m_cancelled = false;

		public bool Cancelled {
			get { return m_cancelled; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="subtitleMemorize.InfoProgress"/> class. The handler takes
		/// an information string and a value between 0 and 1, indicating the progress.
		/// </summary>
		/// <param name="setProgressHandler">Set progress handler.</param>
		public InfoProgress (Action<String, double> setProgressHandler)
		{
			m_setProgressHandler = setProgressHandler;
		}

		public void AddSection(String name, int numberOfSteps) {
			m_progressSections.Add (new ProgressSection (name, numberOfSteps));
		}

		/// <summary>
		/// Show information for 0%.
		/// </summary>
		public void StartProgressing() {
			UpdateHandler ();
		}

		/// <summary>
		/// Increase the number of processed steps by "steps".
		/// </summary>
		/// <param name="steps">Steps.</param>
		public void ProcessedSteps(int steps) {

			// update "current" section
			int restSteps = m_progressSections [m_currentSection].AddProcessedSteps(steps);

			if (m_progressSections [m_currentSection].NumbersOfStepsProcessed >= m_progressSections [m_currentSection].NumbersOfStepsGiven) {
				m_currentSection++;
				if (m_currentSection >= m_progressSections.Count)
					m_currentSection = m_progressSections.Count - 1;
			}


			while (restSteps > 0) {
				m_currentSection++;
				if (m_currentSection >= m_progressSections.Count)
					m_currentSection = m_progressSections.Count - 1;

				restSteps = m_progressSections [m_currentSection].AddProcessedSteps (restSteps);
			}


			UpdateHandler ();
		}

		public void Update() {
			UpdateHandler();
		}

		private void UpdateHandler() {
			ProgressSection currentSec = m_progressSections [m_currentSection];
			double inSectionProgress = (double)currentSec.NumbersOfStepsProcessed / (double)currentSec.NumbersOfStepsGiven;

			double sectionPercents = 1.0 / (double) m_progressSections.Count;
			double totalPercent = sectionPercents * (m_currentSection + inSectionProgress);

			m_setProgressHandler ((int)(totalPercent * 100) + "% - " + currentSec.Name, totalPercent);
		}

		public void Cancel() {
			m_cancelled = true;
		}
	}
}
