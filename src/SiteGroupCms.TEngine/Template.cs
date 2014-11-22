/*****************************************************
 * 本类库的核心系 AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *****************************************************/

using System;
using System.Collections.Generic;
using System.Text;

using SiteGroupCms.TEngine.Parser;
using SiteGroupCms.TEngine.Parser.AST;

namespace SiteGroupCms.TEngine
{
    public class Template
    {
        string name;
        List<Element> elements;
        Template parent;

        Dictionary<string, Template> templates;

        public Template(string name, List<Element> elements)
        {
            this.name = name;
            this.elements = elements;
            this.parent = null;

            InitTemplates();
        }

        public Template(string name, List<Element> elements, Template parent)
        {
            this.name = name;
            this.elements = elements;
            this.parent = parent;

            InitTemplates();
        }

        /// <summary>
        /// load template from file
        /// </summary>
        /// <param name="name">name of template</param>
        /// <param name="filename">file from which to load template</param>
        /// <returns></returns>
        public static Template FromFile(string name, string filename)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                string data = reader.ReadToEnd();
                return Template.FromString(name, data);
            }
        }

        /// <summary>
        /// load template from string
        /// </summary>
        /// <param name="name">name of template</param>
        /// <param name="data">string containg code for template</param>
        /// <returns></returns>
        public static Template FromString(string name, string data)
        {
            TemplateLexer lexer = new TemplateLexer(data);
            TemplateParser parser = new TemplateParser(lexer);
            List<Element> elems = parser.Parse();

            TagParser tagParser = new TagParser(elems);
            elems = tagParser.CreateHierarchy();

            return new Template(name, elems);
        }

        /// <summary>
        /// go thru all tags and see if they are template tags and add
        /// them to this.templates collection
        /// </summary>
        private void InitTemplates()
        {
            this.templates = new Dictionary<string, Template>(StringComparer.InvariantCultureIgnoreCase);

            foreach (Element elem in elements)
            {
                if (elem is Tag)
                {
                    Tag tag = (Tag)elem;
                    if (string.Compare(tag.Name, "template", true) == 0)
                    {
                        Expression ename = tag.AttributeValue("name");
                        string tname;
                        if (ename is StringLiteral)
                            tname = ((StringLiteral)ename).Content;
                        else
                            tname = "?";

                        Template template = new Template(tname, tag.InnerElements, this);
                        templates[tname] = template;
                    }
                }
            }
        }

        /// <summary>
        /// gets a list of elements for this template
        /// </summary>
        public List<Element> Elements
        {
            get { return this.elements; }
        }

        /// <summary>
        /// gets the name of this template
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// returns true if this template has parent template
        /// </summary>
        public bool HasParent
        {
            get { return parent != null; }
        }

        /// <summary>
        /// gets parent template of this template
        /// </summary>
        /// <value></value>
        public SiteGroupCms.TEngine.Template Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// finds template matching name. If this template does not
        /// contain template called name, and parent != null then
        /// FindTemplate is called on the parent
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Template FindTemplate(string name)
        {
            if (templates.ContainsKey(name))
                return templates[name];
            else if (parent != null)
                return parent.FindTemplate(name);
            else
                return null;
        }

        /// <summary>
        /// gets dictionary of templates defined in this template
        /// </summary>
        public System.Collections.Generic.Dictionary<string, SiteGroupCms.TEngine.Template> Templates
        {
            get { return this.templates; }
        }
    }
}
