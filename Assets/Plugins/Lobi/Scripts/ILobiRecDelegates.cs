using UnityEngine;
using System.Collections;

namespace Kayac.Lobi.SDK
{
	public interface ILobiRecDelegates
	{
		void BeforeStartCapturingDelegate();

		void AfterStartCapturingDelegate();

		void BeforeStopCapturingDelegate();

		void AfterStopCapturingDelegate();
	}
}
