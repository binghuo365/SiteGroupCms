#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using SiteGroupCms.TEngine.Parser.AST;

namespace SiteGroupCms.TEngine
{
    /// <summary>
    /// ITagHandler is used to execute custom tags.
    /// You register handler with TemplateManager with RegisterTagHandler(string tagName, ITagHandler handler) method.
    /// A handler is called twice. Once before the content of the tag is executed,
    /// and once after. 
    /// </summary>
    public interface ITagHandler
    {
        /// <summary>
        ///		This method is called at the beginning of processing
        ///	of the tag.
        /// </summary>
        /// <param name="manager">manager executing the tag</param>
        /// <param name="tag">tag being executed</param>
        /// <param name="processInnerElements">instructs manager if it should process
        ///		inner elements of the tag. If this value will be set to false,
        ///		then manager will not execute inner content. 
        ///		Default value is true.
        /// </param>
        /// <param name="captureInnerContent">
        ///		instructs manager if inner content should be sent to default
        ///		output, or custom output. If this value is set to false, all
        ///		output will be sent to current writer. If set to true
        ///		then output will be called and passed as string to TagEndProcess.
        ///		Default value is false.
        /// </param>
        void TagBeginProcess(TemplateManager manager, Tag tag, ref bool processInnerElements, ref bool captureInnerContent);

        /// <summary>
        /// this tag is called at the end of processing the content.
        /// </summary>
        /// <param name="innerContent">If captureinnerContent was set true, 
        ///		then this is the output that was generated when inside of this tag.
        /// </param>
        void TagEndProcess(TemplateManager manager, Tag tag, string innerContent);
    }
}
