using System;
using System.Data;
using System.IO;
using System.Xml;

namespace SiteGroupCms.DBUtility
{
    /// <summary>
    /// 提供对XML文档的操作接口。
    /// </summary>
    public class XmlControl
    {
        private string _strXmlFile;
        private XmlDocument _objXmlDoc = new XmlDocument();

        /// <summary>
        /// TODO: 在这里加入建构函式的程序代码 
        /// </summary>
        /// <param name="XmlFile"></param>
        public XmlControl(string _XmlFile)
        {
            try
            {
                _objXmlDoc.Load(_XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            _strXmlFile = _XmlFile;
        }
        /// <summary>
        /// 返回一个XmlNodeList
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <returns></returns>
        public XmlNodeList GetList(string _XmlPathNode)
        {
            XmlNodeList _NodeList = _objXmlDoc.SelectNodes(_XmlPathNode);
            return _NodeList;
        }
        public bool ExistNode(string _XmlPathNode)
        {
            XmlNode _Node = _objXmlDoc.SelectSingleNode(_XmlPathNode);
            if (_Node == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 查找数据。返回一个DataView
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <returns></returns>
        public DataView GetData(string _XmlPathNode)
        {
            DataSet _ds = new DataSet();
            StringReader _read = new StringReader(_objXmlDoc.SelectSingleNode(_XmlPathNode).OuterXml);
            _ds.ReadXml(_read);
            return _ds.Tables[0].DefaultView;
        }
        /// <summary>
        /// 查找数据。返回一个DataTable
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <returns></returns>
        public DataTable GetTable(string _XmlPathNode)
        {
            DataSet _ds = new DataSet();
            StringReader _read = new StringReader(_objXmlDoc.SelectSingleNode(_XmlPathNode).OuterXml);
            _ds.ReadXml(_read);
            return _ds.Tables[0];
        }
        /// <summary>
        /// 获得节点内容。
        /// </summary>
        /// <param name="_XmlPathNode">如:"Channels/Channel[ID=\"1\"]/Content"</param>
        public string GetText(string _XmlPathNode)
        {
            return GetText(_XmlPathNode, false);
        }
        public string GetText(string _XmlPathNode, bool isCDATA)
        {
            XmlNode _Node = _objXmlDoc.SelectSingleNode(_XmlPathNode);
            if (_Node != null)
                if (isCDATA)
                    return _Node.FirstChild.InnerText;
                else
                    return _Node.InnerText;
            else
                return "";
        }
        /// <summary>
        /// 更新节点内容。
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <param name="Content"></param>
        public void Update(string _XmlPathNode, string _Content)
        {
            Update(_XmlPathNode, _Content, false);
        }
        /// <summary>
        /// 更新节点内容。
        /// </summary>
        /// <param name="_XmlPathNode"></param>
        /// <param name="_Content">内容</param>
        /// <param name="isCDATA">是不是CDATA数据</param>
        public void Update(string _XmlPathNode, string _Content, bool isCDATA)
        {
            if (isCDATA)
                _objXmlDoc.SelectSingleNode(_XmlPathNode).FirstChild.InnerText = _Content;
            else
                _objXmlDoc.SelectSingleNode(_XmlPathNode).InnerText = _Content;
        }
        /// <summary>
        /// 删除一个节点.
        /// </summary>
        /// <param name="XmlPathNode"></param>
        public void Delete(string _XmlPathNode)
        {
            XmlNode _Node = _objXmlDoc.SelectSingleNode(_XmlPathNode);
            if (_Node != null)
                _Node.ParentNode.RemoveChild(_Node);
        }
        /// <summary>
        /// 删除所有节点。
        /// </summary>
        /// <param name="MainNode"></param>
        public void RemoveAll(string _MainNode)
        {
            XmlNode _objRootNode = _objXmlDoc.SelectSingleNode(_MainNode);
            if (_objRootNode != null)
                _objRootNode.RemoveAll();
        }
        /// <summary>
        /// 插入一节点和此节点的一子节点。
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="ChildNode"></param>
        /// <param name="Element"></param>
        /// <param name="Content"></param>
        public void InsertNode(string _MainNode, string _ChildNode, string _Element, string _Content)
        {
            XmlNode _objRootNode = _objXmlDoc.SelectSingleNode(_MainNode);
            XmlElement _objChildNode = _objXmlDoc.CreateElement(_ChildNode);
            if (_objRootNode != null)
            {
                _objRootNode.AppendChild(_objChildNode);
                XmlElement _objElement = _objXmlDoc.CreateElement(_Element);
                _objElement.InnerText = _Content;
                _objChildNode.AppendChild(_objElement);
            }
        }

        /// <summary>
        /// 插入一个节点，带一属性。
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="Element"></param>
        /// <param name="Attrib"></param>
        /// <param name="AttribContent"></param>
        /// <param name="Content"></param>
        public void InsertElement(string _MainNode, string _Element, string _Attrib, string _AttribContent, string _Content)
        {
            XmlNode _objNode = _objXmlDoc.SelectSingleNode(_MainNode);
            XmlElement _objElement = _objXmlDoc.CreateElement(_Element);
            _objElement.SetAttribute(_Attrib, _AttribContent);
            _objElement.InnerText = _Content;
            if (_objNode != null)
                _objNode.AppendChild(_objElement);
        }

        /// <summary>
        /// 插入一个节点，不带属性。
        /// </summary>
        /// <param name="MainNode">如：Channels/Channel[ID="12"]</param>
        /// <param name="Element">如：Title</param>
        /// <param name="Content">如：测试标题</param>
        /// <param name="isCDATA">是不是CDATA数据</param>
        public void InsertElement(string _MainNode, string _Element, string _Content, bool isCDATA)
        {
            XmlNode _objNode = _objXmlDoc.SelectSingleNode(_MainNode);
            XmlElement _objElement = _objXmlDoc.CreateElement(_Element);
            if (isCDATA)
                _objElement.InnerXml = "<![CDATA[" + _Content + "]]>";
            else
                _objElement.InnerText = _Content;
            if (_objNode != null)
                _objNode.AppendChild(_objElement);
        }

        /// <summary>
        /// 保存文档。
        /// </summary>
        public void Save()
        {
            try
            {
                _objXmlDoc.Save(_strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 销毁对象。
        /// </summary>
        public void Dispose()
        {
            _objXmlDoc = null;
        }
    }
}
