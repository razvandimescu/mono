// cs1570-11.cs: XML comment on `P:Testing.Test.PublicProperty2' has non-well-formed XML ('summary' is expected  Line 3, position 4.)
// Line: 32
// Compiler options: -doc:dummy.xml -warn:1 -warnaserror

using System;

namespace Testing
{
	public class Test
	{
		public static void Main ()
		{
		}

		/// <summary>
		/// comment for public property
		/// </summary>
		public string PublicProperty {
			/// <summary>
			/// On public getter - no effect
			/// </summary>
			get { return null; }
			/// <summary>
			/// On public setter - no effect
			/// </summary>
			set { }
		}

		/// <summary>
		/// incorrect comment for public property
		/// </incorrect>
		public string PublicProperty2 {
			get { return null; }
		}

		/**
		<summary>
		Javadoc comment for public property
		</summary>
		*/
		public string PublicProperty3 {
			/**
			<summary>
			On public getter - no effect
			</summary>
			*/
			get { return null; }
			/**
			<summary>
			On public setter - no effect
			</summary>
			*/
			set { }
		}
	}
}

