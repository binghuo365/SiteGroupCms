/*
 * FCKeditor - The text editor for Internet - http://www.fckeditor.net
 * Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
 *
 * == BEGIN LICENSE ==
 *
 * Licensed under the terms of any of the following licenses at your
 * choice:
 *
 *  - GNU General Public License Version 2 or later (the "GPL")
 *    http://www.gnu.org/licenses/gpl.html
 *
 *  - GNU Lesser General Public License Version 2.1 or later (the "LGPL")
 *    http://www.gnu.org/licenses/lgpl.html
 *
 *  - Mozilla Public License Version 1.1 or later (the "MPL")
 *    http://www.mozilla.org/MPL/MPL-1.1.html
 *
 * == END LICENSE ==
 *
 * Useful tools.
 */

using System ;
using System.Runtime.InteropServices ;
using System.IO ;
using System.Collections ;

namespace JumboTCMS.FredFCKeditorV2
{
	internal sealed class Util
	{
		// The "_mkdir" function is used by the "CreateDirectory" method.
		[DllImport("msvcrt.dll", SetLastError=true)]
		private static extern int _mkdir(string path) ;

		private Util()
		{}

		/// <summary>
		/// This method should provide safe substitude for Directory.CreateDirectory().
		/// </summary>
		/// <param name="path">The directory path to be created.</param>
		/// <returns>A <see cref="System.IO.DirectoryInfo"/> object for the created directory.</returns>
		/// <remarks>
		///		<para>
		///		This method creates all the directory structure if needed.
		///		</para>
		///		<para>
		///		The System.IO.Directory.CreateDirectory() method has a bug that gives an
		///		error when trying to create a directory and the user has no rights defined
		///		in one of its parent directories. The CreateDirectory() should be a good 
		///		replacement to solve this problem.
		///		</para>
		/// </remarks>
		public static DirectoryInfo CreateDirectory( string path )
		{
			ArrayList oDirsToCreate = new ArrayList() ;

			// Create the directory info object for that dir (normalized to its absolute representation).
			DirectoryInfo oDir = new DirectoryInfo( Path.GetFullPath( path ) ) ;

			// Check the entire path structure to find directories that must be created.
			while ( oDir != null && !oDir.Exists )
			{
				oDirsToCreate.Add( oDir.FullName ) ;
				oDir = oDir.Parent ;
			}

			// "oDir == null" means that the check arrives in the root and it doesn't exist too.
			if ( oDir == null )
				throw( new System.IO.DirectoryNotFoundException( "Directory \"" + oDirsToCreate[ oDirsToCreate.Count-1 ] + "\" not found." ) ) ;

			// Create all directories that must be created (from bottom to top).
			for( int i = oDirsToCreate.Count - 1 ; i >= 0 ; i-- )
			{
				string sPath = (string)oDirsToCreate[i] ;
				int iReturn = _mkdir( sPath ) ;

				if ( iReturn != 0 )
					throw new ApplicationException("Error calling [msvcrt.dll]:_wmkdir(" + sPath + "), error code: " + iReturn );
			}

			return new DirectoryInfo( path ) ;
		}
	}
}
