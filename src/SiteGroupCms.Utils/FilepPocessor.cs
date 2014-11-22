/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序版本: 5.2.X
 * 
 * 程序作者: 高伟 ( 合作请联系：254860396#qq.com)
 * 
 * 
 * 
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SiteGroupCms.Utils
{
    /// <summary>
    /// FileProcessor is used to process byte data from a multi-part form and save it to the disk.
    /// </summary>
    public class FileProcessor : IDisposable
    {

        #region "Class Vars"

        /// <summary>
        /// Default folder to where the files are to be uploaded.
        /// </summary>
        private string _currentFilePath = "";

        /// <summary>
        /// Form post id is used in finding field seperators in the multi-part form post
        /// </summary>
        private string _formPostID = "";
        /// <summary>
        /// Used to find the start of a file
        /// </summary>
        private string _fieldSeperator = "";

        /// <summary>
        /// Used to note what buffer index we are on in the collection
        /// </summary>
        private long _currentBufferIndex = 0;

        /// <summary>
        /// Used to flag each byte process if we have already found the start of a file or not
        /// </summary>
        private bool _startFound = false;

        /// <summary>
        /// Used to flag each byte process if we have already found the end of a file.
        /// </summary>
        private bool _endFound = false;

        /// <summary>
        /// Default file name
        /// </summary>
        public string _currentFileName = Guid.NewGuid().ToString() + ".bin";

        /// <summary>
        /// FileStream object that is left open while a file is beting written to.  Each file will open and 
        /// close its filestream automaticly
        /// </summary>
        private FileStream _fileStream = null;

        /// <summary>
        /// The following fields are used in the byte searching of buffer datas
        /// </summary>
        private long _startIndexBufferID = -1;
        private int _startLocationInBufferID = -1;

        private long _endIndexBufferID = -1;
        private int _endLocationInBufferID = -1;


        /// <summary>
        /// Dictionary array used to store byte chunks.
        /// Should store a max of 2 items.  2 history chunks are kept.
        /// </summary>
        private Dictionary<long, byte[]> _bufferHistory = new Dictionary<long, byte[]>();

        /// <summary>
        /// Object to hold all the finished filenames.
        /// </summary>
        private List<string> _finishedFiles = new List<string>();

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor with the path of where the files should be uploaded.
        /// </summary>
        public FileProcessor(string uploadLocation)
        {
            // This is the path where the files will be uploaded too.
            _currentFilePath = uploadLocation;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Property used to get the finished files.
        /// </summary>
        public List<string> FinishedFiles
        {
            get { return _finishedFiles; }
        }

        #endregion

        #region "Methods

        /// <summary>
        /// Method that is used to process the buffer.  
        /// </summary>
        /// <param name="bufferData">Byte data to scan</param>
        /// <param name="addToBufferHistory">If true, it will add it to the buffer history. If false, it will not.</param>
        public void ProcessBuffer(ref byte[] bufferData, bool addToBufferHistory)
        {
            int byteLocation = -1;

            // If the start has not been found, search for it.
            if (!_startFound)
            {
                // Search for start location
                byteLocation = GetStartBytePosition(ref bufferData);
                if (byteLocation != -1)
                {
                    // Set the start position to this current index
                    _startIndexBufferID = _currentBufferIndex + 1;
                    // Set the start location in the index
                    _startLocationInBufferID = byteLocation;

                    _startFound = true;

                }
            }

            // If the start was found, we can start to store the data into a file.
            if (_startFound)
            {
                // Save this data to a file
                // Have to find the end point.  Makes sure the end is not in the same buffer

                int startLocation = 0;
                if (byteLocation != -1)
                {
                    startLocation = byteLocation;
                }

                // Write the data from the start point to the end point to the file.

                int writeBytes = (bufferData.Length - startLocation);
                int tempEndByte = GetEndBytePosition(ref bufferData);
                if (tempEndByte != -1)
                {
                    writeBytes = (tempEndByte - startLocation);
                    // not that the end was found.
                    _endFound = true;
                    // Set the current index the file was found
                    _endIndexBufferID = (_currentBufferIndex + 1);
                    // Set the current byte location for the assoicated index the file was found
                    _endLocationInBufferID = tempEndByte;
                }

                // Make sure we have something to write.
                if (writeBytes > 0)
                {
                    if (_fileStream == null)
                    {
                        // create a new file stream to be used.
                        _fileStream = new FileStream(_currentFilePath + _currentFileName, FileMode.OpenOrCreate);

                        // this will create a time to live for the file so it will automaticly be removed
                        int fileTimeToLive = 3600;


                        // if the form were not to handle the file and remove it, this is an automatic removal of the file
                        // the timer object will execute in x number of seconds
                        Timer t = new Timer(new TimerCallback(DeleteFile), _currentFilePath + _currentFileName, (fileTimeToLive * 1000), 0);

                    }
                    // Write the datat to the file and flush it.
                    _fileStream.Write(bufferData, startLocation, writeBytes);
                    _fileStream.Flush();
                }
            }

            // If the end was found, then we need to close the stream now that we are done with it.
            // We will also re-process this buffer to make sure the start of another file 
            // is not located within it.
            if (_endFound)
            {
                CloseStreams();
                _startFound = false;
                _endFound = false;

                // Research the current buffer for a new start location.  
                ProcessBuffer(ref bufferData, false);
            }

            // Add to buffer history
            if (addToBufferHistory)
            {
                // Add to history object.
                _bufferHistory.Add(_currentBufferIndex, bufferData);
                _currentBufferIndex++;
                // Cleanup old buffer references
                RemoveOldBufferData();
            }
        }

        /// <summary>
        /// Method used to clean up the internal buffer array.  
        /// Older elements are not needed, only a history of one is needed.
        /// </summary>
        private void RemoveOldBufferData()
        {
            for (long bufferIndex = _currentBufferIndex; bufferIndex >= 0; bufferIndex--)
            {
                if (bufferIndex > (_currentBufferIndex - 3))
                {
                    // Dont touch. preserving the last 2 items.
                }
                else
                {
                    if (_bufferHistory.ContainsKey(bufferIndex))
                    {
                        _bufferHistory.Remove(bufferIndex);
                    }
                    else
                    {
                        // no more previous buffers. 
                        bufferIndex = 0;
                    }
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// Close the stream, and reset the filename.
        /// </summary>
        public void CloseStreams()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream.Close();
                _fileStream = null;

                // add the file name to the finished list.
                _finishedFiles.Add(_currentFileName);

                // Reset the filename.
                _currentFileName = Guid.NewGuid().ToString() + ".bin";
            }
        }

        /// <summary>
        /// Method should be ran on the bytes that are returned on GetPreloadedEntityBody().
        /// </summary>
        /// <param name="bufferData"></param>
        public void GetFieldSeperators(ref byte[] bufferData)
        {
            try
            {
                _formPostID = Encoding.UTF8.GetString(bufferData).Substring(29, 13);
                _fieldSeperator = "-----------------------------" + _formPostID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetFieldSeperators(): " + ex.Message);
            }
        }

        /// <summary>
        /// Method used for searching buffer data, and if needed previous buffer data.
        /// </summary>
        /// <param name="bufferData">current buffer data need to be processed.</param>
        /// <returns>Returns byte location of data to start</returns>
        private int GetStartBytePosition(ref byte[] bufferData)
        {

            int byteOffset = 0;
            // Check to see if the current bufferIndex is the same as any previous index found.
            // If it is, offset the searching by the previous location
            if (_startIndexBufferID == (_currentBufferIndex + 1))
            {
                byteOffset = _startLocationInBufferID;
            }

            // Check to see if the end index was found before this start index.  That way we keep moving ahead
            if (_endIndexBufferID == (_currentBufferIndex + 1))
            {
                byteOffset = _endLocationInBufferID;
            }

            int tempContentTypeStart = -1;


            byte[] searchString = Encoding.UTF8.GetBytes("Content-Type: ");
            tempContentTypeStart = FindBytePattern(ref bufferData, ref searchString, byteOffset);

            if (tempContentTypeStart != -1)
            {
                // Found content type start location
                // Next search for \r\n\r\n at this substring

                searchString = Encoding.UTF8.GetBytes("\r\n\r\n");
                int tempSearchStringLocation = FindBytePattern(ref bufferData, ref searchString, tempContentTypeStart);

                if (tempSearchStringLocation != -1)
                {
                    // Found this.  Can get start of data here
                    // Add 4 to it. That is the number of positions before it gets to the start of the data
                    int byteStart = tempSearchStringLocation + 4;
                    return byteStart;
                }
            }
            else if ((byteOffset - searchString.Length) > 0)
            {

                return -1;
            }

            else
            {
                // Did not find it. Add this and previous buffer together to see if it exists.
                // Check to see if the buffer index is at the start. 
                if (_currentBufferIndex > 0)
                {
                    // Get the previous buffer
                    byte[] previousBuffer = _bufferHistory[_currentBufferIndex - 1];
                    byte[] mergedBytes = MergeArrays(ref previousBuffer, ref bufferData);
                    // Get the byte array for the text
                    byte[] searchString2 = Encoding.UTF8.GetBytes("Content-Type: ");
                    // Search the bytes for the searchString
                    tempContentTypeStart = FindBytePattern(ref mergedBytes, ref searchString2, previousBuffer.Length - searchString2.Length);

                    //tempContentTypeStart = combinedUTF8Data.IndexOf("Content-Type: ");
                    if (tempContentTypeStart != -1)
                    {
                        // Found content type start location
                        // Next search for \r\n\r\n at this substring

                        searchString2 = Encoding.UTF8.GetBytes("Content-Type: ");
                        // because we are searching part of the previosu buffer, we only need to go back the length of the search 
                        // array.  Any further, and our normal if statement would have picked it up when it first was processed.
                        int tempSearchStringLocation = FindBytePattern(ref mergedBytes, ref searchString2, (previousBuffer.Length - searchString2.Length));

                        if (tempSearchStringLocation != -1)
                        {
                            // Found this.  Can get start of data here
                            // It is possible for some of this to be located in the previous buffer.
                            // Find out where the excape chars are located.
                            if (tempSearchStringLocation > previousBuffer.Length)
                            {
                                // Located in the second object.  
                                // If we used the previous buffer, we shoudl not have to worry about going out of
                                // range unless the buffer was set to some really low number.  So not going to check for
                                // out of range issues.
                                int currentBufferByteLocation = (tempSearchStringLocation - previousBuffer.Length);
                                return currentBufferByteLocation;
                            }
                            else
                            {
                                // Located in the first object.  The only reason this could happen is if
                                // the escape chars ended right at the end of the buffer.  This would mean
                                // that that the next buffer would start data at offset 0
                                return 0;
                            }
                        }
                    }
                }
            }
            // indicate not found.
            return -1;
        }

        /// <summary>
        /// Method used for searching buffer data for end byte location, and if needed previous buffer data.
        /// </summary>
        /// <param name="bufferData">current buffer data needing to be processed.</param>
        /// <returns>Returns byte location of data to start</returns>
        private int GetEndBytePosition(ref byte[] bufferData)
        {

            int byteOffset = 0;
            // Check to see if the current bufferIndex is the same as any previous index found.
            // If it is, offset the searching by the previous location.  This will allow us to find the next leading
            // Stop location so we do not return a byte offset that may have happened before the start index.
            if (_startIndexBufferID == (_currentBufferIndex + 1))
            {
                byteOffset = _startLocationInBufferID;
            }

            int tempFieldSeperator = -1;

            // First see if we can find it in the current buffer batch.
            byte[] searchString = Encoding.UTF8.GetBytes(_fieldSeperator);
            tempFieldSeperator = FindBytePattern(ref bufferData, ref searchString, byteOffset);

            if (tempFieldSeperator != -1)
            {
                // Found field ending. Depending on where the field seperator is located on this, we may have to move back into
                // the prevoius buffer to return its offset.
                if (tempFieldSeperator - 2 < 0)
                {
                    //TODO: Have to get the previous buffer data.

                }
                else
                {
                    return (tempFieldSeperator - 2);
                }
            }
            else if ((byteOffset - searchString.Length) > 0)
            {

                return -1;
            }
            else
            {
                // Did not find it. Add this and previous buffer together to see if it exists.
                // Check to see if the buffer index is at the start. 
                if (_currentBufferIndex > 0)
                {

                    // Get the previous buffer
                    byte[] previousBuffer = _bufferHistory[_currentBufferIndex - 1];
                    byte[] mergedBytes = MergeArrays(ref previousBuffer, ref bufferData);
                    // Get the byte array for the text
                    byte[] searchString2 = Encoding.UTF8.GetBytes(_fieldSeperator);
                    // Search the bytes for the searchString
                    tempFieldSeperator = FindBytePattern(ref mergedBytes, ref searchString2, previousBuffer.Length - searchString2.Length + byteOffset);

                    if (tempFieldSeperator != -1)
                    {
                        // Found content type start location
                        // Next search for \r\n\r\n at this substring

                        searchString2 = Encoding.UTF8.GetBytes("\r\n\r\n");
                        int tempSearchStringLocation = FindBytePattern(ref mergedBytes, ref searchString2, tempFieldSeperator);

                        if (tempSearchStringLocation != -1)
                        {
                            // Found this.  Can get start of data here
                            // It is possible for some of this to be located in the previous buffer.
                            // Find out where the excape chars are located.
                            if (tempSearchStringLocation > previousBuffer.Length)
                            {
                                // Located in the second object.  
                                // If we used the previous buffer, we shoudl not have to worry about going out of
                                // range unless the buffer was set to some really low number.  So not going to check for
                                // out of range issues.
                                int currentBufferByteLocation = (tempSearchStringLocation - previousBuffer.Length);
                                return currentBufferByteLocation;
                            }
                            else
                            {
                                // Located in the first object.  The only reason this could happen is if
                                // the escape chars ended right at the end of the buffer.  This would mean
                                // that that the next buffer would start data at offset 0
                                return -1;
                            }
                        }
                    }
                }
            }
            // indicate not found.
            return -1;
        }

        /// <summary>
        /// Method created to search for byte array patterns inside a byte array.
        /// </summary>
        /// <param name="containerBytes">byte array to search</param>
        /// <param name="searchBytes">byte array with pattern to search with</param>
        /// <param name="startAtIndex">byte offset to start searching at a specified location</param>
        /// <returns>-1 if not found or index of starting location of pattern</returns>
        private static int FindBytePattern(ref byte[] containerBytes, ref byte[] searchBytes, int startAtIndex)
        {
            int returnValue = -1;
            for (int byteIndex = startAtIndex; byteIndex < containerBytes.Length; byteIndex++)
            {

                // Make sure the searchBytes lenght does not exceed the containerbytes
                if (byteIndex + searchBytes.Length > containerBytes.Length)
                {
                    return -1;
                }

                // First the first reference of the bytes to search
                if (containerBytes[byteIndex] == searchBytes[0])
                {
                    bool found = true;
                    int tempStartIndex = byteIndex;
                    for (int searchBytesIndex = 1; searchBytesIndex < searchBytes.Length; searchBytesIndex++)
                    {
                        // Set next index
                        tempStartIndex++;
                        if (!(searchBytes[searchBytesIndex] == containerBytes[tempStartIndex]))
                        {
                            found = false;
                            // break out of the loop and continue searching.
                            break;
                        }
                    }
                    if (found)
                    {
                        // Indicates that the byte array has been found. Return this index.
                        return byteIndex;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Used to merge two byte arrays into one.  
        /// </summary>
        /// <param name="arrayOne">First byte array to go to the start of the new array</param>
        /// <param name="arrayTwo">Second byte array to go to right after the first array that was passed</param>
        /// <returns>new byte array of all the new bytes</returns>
        private static byte[] MergeArrays(ref byte[] arrayOne, ref byte[] arrayTwo)
        {
            System.Type elementType = arrayOne.GetType().GetElementType();
            byte[] newArray = new byte[arrayOne.Length + arrayTwo.Length];
            arrayOne.CopyTo(newArray, 0);
            arrayTwo.CopyTo(newArray, arrayOne.Length);

            return newArray;
        }

        /// <summary>
        /// This is used as part of the clean up procedures. the Timer object will execute this function.
        /// </summary>
        /// <param name="filePath"></param>
        private static void DeleteFile(object filePath)
        {
            // File may have already been removed from the main appliation.
            try
            {
                if (System.IO.File.Exists((string)filePath))
                {
                    System.IO.File.Delete((string)filePath);
                }
            }
            catch { }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Clean up method.
        /// </summary>
        public void Dispose()
        {
            // Clear the buffer history
            _bufferHistory.Clear();
            GC.Collect();
        }

        #endregion
    }
}
